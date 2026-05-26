using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DataManager.ICE;
using static DataManager.ICE.RemoteExecutor;

namespace DataManager
{
    public partial class Form1 : Form
    {
        // ==========================================
        // 전역 변수 선언
        // ==========================================
        private ICE.ICommandExecutor _executor;
        private string modelPath = "";
        private string testImagePath = "";
        private string loggedInUser = "";
        private System.Windows.Forms.Timer playTimer;
        private bool _isUpdatingRadio = false;
        private ToolStripMenuItem menuProfile;

        private string configPath = "";
        private string tubPath = "";
        private readonly List<TubFrame> tubFrames = new();
        private readonly HashSet<int> missingImageFrames = new();
        private readonly ImageList timelineImages = new();
        private const int TimelineVisibleCount = 20;
        private int currentTimelineStart = -1;
        private bool isUpdatingTimelineSelection;

        // 데이터 정리(필터/삭제) 대상 범위. -1은 미지정.
        private int rangeStart = -1;
        private int rangeEnd = -1;

        // ==========================================
        // 1. 초기화 및 생성자
        // ==========================================
        public Form1()
        {
            InitializeComponent();

            // 프로그램 시작 시 기본 실행기를 로컬로 세팅
            _executor = new ICE.LocalExecutor();

            // 라디오 버튼 및 타이머 연결 (디자이너와 겹치지 않는 특수 이벤트만 유지)
            rdoLocal.CheckedChanged += rdoLocal_CheckedChanged;
            rdoRemote.CheckedChanged += rdoRemote_CheckedChanged;

            playTimer = new System.Windows.Forms.Timer();
            playTimer.Interval = 100; // 0.1초 간격
            playTimer.Tick += PlayTimer_Tick;

            // 상단 메뉴바 동적 생성
            CreateTopMenu();

            // 나머지 탐색 바 및 리스트 이벤트 제어
            btnReloadTub.Click += btnReloadTub_Click;
            lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged;
            lvTimeline.SelectedIndexChanged += lvTimeline_SelectedIndexChanged;
            trackFrame.Scroll += trackFrame_Scroll;
            btnFirst.Click += btnFirst_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnLast.Click += btnLast_Click;

            // 데이터 정리(범위 지정/필터/삭제/휴지통) 이벤트 연결
            btnSetLeft.Click += btnSetLeft_Click;
            btnSetRight.Click += btnSetRight_Click;
            btnFilter.Click += btnFilter_Click;
            btnDelete.Click += btnDelete_Click;
            btnRestore.Click += btnRestore_Click;
            btnEmptyTrash.Click += btnEmptyTrash_Click;
            lstTrash.SelectionMode = SelectionMode.MultiExtended;

            // 타임라인 썸네일 이미지 리스트 설정
            timelineImages.ImageSize = new Size(36, 27);
            timelineImages.ColorDepth = ColorDepth.Depth32Bit;
            lvTimeline.LargeImageList = timelineImages;
            lvTimeline.View = View.LargeIcon;
            lvTimeline.HideSelection = false;
            lvTimeline.MultiSelect = false;
            lvTimeline.ShowItemToolTips = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(
                "Donkeycar 데이터 관리 프로그램에 오신 것을 환영합니다!\n\n프로그램 사용이 처음이신가요?\n'예'를 누르시면 사용 설명서가 팝업으로 뜹니다.",
                "환영합니다!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (res == DialogResult.Yes)
            {
                MessageBox.Show("[기본 사용 순서]\n\n1. 좌측 [서버 연결 설정]에서 [원격] 선택 후 로그인\n2. [설정 파일 열기] 클릭하여 서버 경로 동기화\n3. [Tub 데이터 열기]로 윈도우로 다운받은 주행기록 폴더 열기\n4. 이상한 데이터 필터링 및 삭제\n5. [학습] 버튼을 눌러 AI 훈련시키기\n6. 훈련된 모델로 [모델 테스트] 진행\n\n이 내용은 상단 메뉴바에서도 언제든 확인 가능합니다.", "초보자 가이드");
            }
        }

        // 상단 메뉴바 자동 생성기 (UI/UX 패치)
        private void CreateTopMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.WhiteSmoke; // 배경색을 약간 회색으로 주어 메뉴바 영역이 잘 보임
            menuStrip.Padding = new Padding(5, 5, 5, 5); // 여백
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            menuStrip.BringToFront(); // 다른 UI(프레임 목록 등) 뒤에 가려지지 않고 무조건 맨 위로 오게 강제 설정!

            // 1. Donkeycar 사용 설명서 메뉴
            ToolStripMenuItem menuManual = new ToolStripMenuItem("📖 Donkeycar 사용 설명서");
            menuManual.DropDownItems.Add("1. 좌측 [서버 연결 설정]에서 [원격] 선택 후 로그인");
            menuManual.DropDownItems.Add("2. [설정 파일 열기] 클릭 후 기준 폴더 자동 세팅");
            menuManual.DropDownItems.Add("3. [Tub 데이터 열기]로 윈도우로 다운받은 주행 데이터 불러오기");
            menuManual.DropDownItems.Add("4. [학습] 버튼을 클릭하여 AI 모델 생성");
            menuManual.DropDownItems.Add("5. 생성된 모델과 테스트 폴더를 선택해 [모델 테스트] 진행");
            menuManual.MouseEnter += (s, e) => menuManual.ShowDropDown();

            // 2. 단축키 메뉴
            ToolStripMenuItem menuHotkeys = new ToolStripMenuItem("⌨️ 단축키 안내");
            menuHotkeys.DropDownItems.Add("Space Bar : 자동 재생 / 정지 토글");
            menuHotkeys.DropDownItems.Add("← / → 방향키 : 프레임 1칸씩 이동");
            menuHotkeys.MouseEnter += (s, e) => menuHotkeys.ShowDropDown();

            // 3. 우측 상단 로그인 프로필 메뉴 (잘림 방지 및 디테일 추가)
            menuProfile = new ToolStripMenuItem("👤 로컬 모드 (로그아웃 상태)");
            menuProfile.Alignment = ToolStripItemAlignment.Right;
            menuProfile.Font = new Font("맑은 고딕", 9, FontStyle.Bold);

            // 화면 잘림 방지: 오른쪽 끝에 있으므로 메뉴가 왼쪽 아래로 펼쳐지도록 방향 강제
            menuProfile.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            // 프로필 하위 메뉴 (인성님 아이디어 적용)
            ToolStripMenuItem infoRole = new ToolStripMenuItem("상태: 로컬 환경 대기 중");
            infoRole.Enabled = false; // 클릭 안되는 정보 표시용

            ToolStripMenuItem infoTrain = new ToolStripMenuItem("오늘 학습 시도: 0회");
            infoTrain.Enabled = false;

            ToolStripSeparator separator = new ToolStripSeparator(); // 구분선

            ToolStripMenuItem menuLogout = new ToolStripMenuItem("🛑 원격 서버 로그아웃");
            menuLogout.Click += menuLogout_Click;

            menuProfile.DropDownItems.Add(infoRole);
            menuProfile.DropDownItems.Add(infoTrain);
            menuProfile.DropDownItems.Add(separator);
            menuProfile.DropDownItems.Add(menuLogout);

            menuProfile.MouseEnter += (s, e) => menuProfile.ShowDropDown();

            menuStrip.Items.Add(menuManual);
            menuStrip.Items.Add(menuHotkeys);
            menuStrip.Items.Add(menuProfile);
        }

        // ==========================================
        // 2. 서버 연결 및 가상환경 설정
        // ==========================================
        private void rdoRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingRadio || !rdoRemote.Checked) return;

            while (true)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            _executor = new ICE.RemoteExecutor(loginForm.Host, loginForm.User, loginForm.Pass);
                            loggedInUser = loginForm.User;

                            // lblProfile 찌꺼기 싹 지우고 menuProfile로 대체
                            menuProfile.Text = $"👤 {loginForm.User} 접속중";
                            menuProfile.ForeColor = Color.Blue; // 글씨를 파란색으로!
                            menuProfile.Tag = loginForm.Host;   // 나중에 로그아웃할 때 쓰려고 IP를 살짝 숨겨둠

                            // 하위 메뉴 첫 번째 줄에 접속 IP 띄우기
                            if (menuProfile.DropDownItems.Count > 0 && menuProfile.DropDownItems[0] is ToolStripMenuItem ipMenuItem)
                            {
                                ipMenuItem.Text = $"접속 IP: {loginForm.Host}";
                            }

                            return;
                        }
                        catch (Exception ex)
                        {
                            DialogResult retry = MessageBox.Show($"서버 연결에 실패했습니다.\n아이디와 비밀번호를 확인하세요.\n\n[오류내용]: {ex.Message}\n\n다시 시도하시겠습니까?", "접속 실패", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (retry == DialogResult.Cancel) break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _isUpdatingRadio = true;
            rdoLocal.Checked = true;
            _executor = new ICE.LocalExecutor();
            _isUpdatingRadio = false;
        }


        private void rdoLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (_isUpdatingRadio || !rdoLocal.Checked) return;

            if (_executor is ICE.RemoteExecutor)
            {
                DialogResult res = MessageBox.Show("원격 서버 연결을 종료하고 로컬 모드로 전환하시겠습니까?", "연결 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    _executor.Stop();
                    _executor = new ICE.LocalExecutor();

                    // 로컬 전환 시 파란색을 다시 검은색
                    menuProfile.Text = "👤 로컬 모드 (로그아웃 상태)";
                    menuProfile.ForeColor = Color.Black;

                    if (menuProfile.DropDownItems.Count > 0 && menuProfile.DropDownItems[0] is ToolStripMenuItem ipMenuItem)
                    {
                        ipMenuItem.Text = "상태: 로컬 환경 대기 중";
                    }
                }
                else
                {
                    _isUpdatingRadio = true;
                    rdoRemote.Checked = true;
                    _isUpdatingRadio = false;
                }
            }
            else
            {
                _executor = new ICE.LocalExecutor();
            }
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                string ip = menuProfile.Tag?.ToString();
                DialogResult result = MessageBox.Show($"현재 접속 정보\n- IP: {ip}\n\n로그아웃하고 로컬 모드로 전환하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _isUpdatingRadio = true;
                    rdoLocal.Checked = true;

                    // 로그아웃 시 파란색을 다시 검은색
                    menuProfile.Text = "👤 로컬 모드 (로그아웃 상태)";
                    menuProfile.ForeColor = Color.Black;

                    if (menuProfile.DropDownItems.Count > 0 && menuProfile.DropDownItems[0] is ToolStripMenuItem ipMenuItem)
                    {
                        ipMenuItem.Text = "상태: 로컬 환경 대기 중";
                    }

                    _executor.Stop();
                    _executor = new ICE.LocalExecutor();
                    _isUpdatingRadio = false;
                }
            }
        }

        // 디자인 창에서 체크박스 연결
        private void chkUseVenv_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseVenv.Checked)
            {
                MessageBox.Show("✅ 가상환경(venv) 모드가 켜졌습니다.\n\n[설명]\n서버 환경 충돌을 막기 위해 독립된 공간에서 안전하게 AI를 실행합니다. (권장)", "가상환경 켬", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult res = MessageBox.Show("⚠️ 가상환경(venv) 모드를 끄시겠습니까?\n\n[주의]\n서버에 설치된 기본 파이썬으로 실행되며, 패키지 충돌 오류가 발생할 수 있습니다.", "가상환경 끔 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No) chkUseVenv.Checked = true;
            }
        }

        // ==========================================
        // 3. 파이썬 연동: 설정, 학습, 테스트
        // ==========================================
        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                configPath = $"/home/{loggedInUser}/mycar";
                lblConfigPath.Text = configPath;
                MessageBox.Show($"[원격 모드 설정 완료]\n\n{loggedInUser}님의 서버 폴더({configPath})로\n작업 기준점이 똑똑하게 자동 설정되었습니다!\n\n이제 다음 단계인 'Tub 데이터 열기'를 진행해 주세요.", "설정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("로컬(윈도우) 환경에서 작업할 준비를 합니다.\n\n이어서 뜨는 폴더 선택 창에서\n'manage.py' 파일이 들어있는 동키카 기본 폴더(mycar)를 선택해 주세요.", "작업 폴더 선택 안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "manage.py 파일이 있는 mycar 폴더를 찾아 선택해주세요.";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    configPath = dlg.SelectedPath;
                    lblConfigPath.Text = configPath;
                }
            }
        }

        // 학습/테스트 강제 중지 버튼 클릭 이벤트
        private void btnStopTask_Click(object sender, EventArgs e)
        {
            if (_executor != null)
            {
                DialogResult res = MessageBox.Show("현재 진행 중인 학습/테스트를 강제로 멈추시겠습니까?\n(진행 중이던 학습 데이터는 저장되지 않습니다.)", "작업 강제 중지", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    _executor.Cancel(); // 파이썬에 강제 종료 신호 전송

                    txtLog.AppendText(Environment.NewLine + "🛑 [알림] 사용자에 의해 작업이 강제 중지되었습니다." + Environment.NewLine);

                    // 진행률 바 초기화
                    if (progressBarTrain != null) progressBarTrain.Value = 0;
                }
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (_executor == null)
            {
                MessageBox.Show("먼저 로컬/원격 연결 설정을 완료해주세요!", "설정 필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(configPath))
            {
                MessageBox.Show("Donkeycar 프로젝트 폴더(Load Config)를 먼저 로드해주세요!", "폴더 누락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult res = MessageBox.Show($"원격 서버({configPath})에서 AI 모델 학습을 시작하시겠습니까?\n(학습에는 시간이 오래 걸릴 수 있습니다.)", "학습 시작 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No) return;

            txtLog.AppendText(Environment.NewLine + "[Train] AI 모델 학습을 시작합니다...");

            bool useVenv = chkUseVenv != null ? chkUseVenv.Checked : true;
            _executor.ExecuteTrain(configPath, useVenv, (log) =>
            {
                this.Invoke(new Action(() => { UpdateChartRealTime(log); }));
            });
        }

        private void UpdateChartRealTime(string logText)
        {
            if (string.IsNullOrEmpty(logText)) return;
            txtLog.AppendText(logText + Environment.NewLine);

            // 1. 에러 발생 시 알림
            if (logText.Contains("[Errno 2]") || logText.Contains("Error") || logText.Contains("Exception"))
            {
                MessageBox.Show($"학습 중 파이썬 오류가 발생했습니다.\n로그 창을 확인해주세요.\n\n내용: {logText}", "학습 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  2. 로그 정리 및 안내 팝업
            if (logText.Contains("Saved model") || logText.Contains("Finished") || logText.Contains("Stopping early") || logText.ToLower().Contains("model saved"))
            {
                // UI 정리 (진행률 바 0으로 초기화)
                if (progressBarTrain != null)
                {
                    progressBarTrain.Value = 0;
                }

                // 로그 창에 구분선과 안내 멘트 추가
                txtLog.AppendText(Environment.NewLine + "--------------------------------------------------");
                txtLog.AppendText(Environment.NewLine + "✅ [학습 완료] AI 모델 생성이 끝났습니다.");
                txtLog.AppendText(Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);

                // 팝업 알림 (다음 행동 지침 포함)
                MessageBox.Show("🎉 AI 모델 학습이 무사히 완료되었습니다!\n\n[다음 단계]\n1. 하단의 '모델 선택'을 눌러 방금 학습된 모델(mypilot.h5)을 선택하세요.\n2. '테스트 이미지 선택'을 누르고 주행 데이터 폴더를 고르세요.\n3. '모델 테스트' 버튼을 눌러 AI가 예측 조향각을 잘 뽑아내는지 확인해보세요!", "학습 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 3. Loss 값 추출 (그래프용 - 현재는 주석 처리됨)
            Match matchLoss = Regex.Match(logText, @"loss:\s*([0-9]*\.?[0-9]+)");
            if (matchLoss.Success)
            {
                double lossValue = Convert.ToDouble(matchLoss.Groups[1].Value);
                // chartLoss.Series["Loss"].Points.AddY(lossValue); 
            }

            // 진행률 바 순간이동 버그 수정
            // 앞쪽에 정확히 'Epoch'라는 단어가 있는 진짜 진행률만 캐치
            Match epochMatch = Regex.Match(logText, @"Epoch\s+(\d+)/(\d+)");
            if (epochMatch.Success && progressBarTrain != null)
            {
                int current = int.Parse(epochMatch.Groups[1].Value);
                int total = int.Parse(epochMatch.Groups[2].Value);
                progressBarTrain.Maximum = total;
                progressBarTrain.Value = current <= total ? current : total;
            }
        }

        private void btnSelectModel_Click(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                modelPath = "models/mypilot.h5";
                MessageBox.Show($"[원격 모드]\n서버의 기본 학습 모델로 지정되었습니다:\n{modelPath}", "모델 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLog.AppendText(Environment.NewLine + $"[Info] 선택된 모델: {modelPath}");
                return;
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "테스트할 Donkeycar 모델(.h5) 선택";
                dlg.Filter = "Keras Models (*.h5)|*.h5|All files (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    modelPath = dlg.FileName;
                    MessageBox.Show($"모델이 선택되었습니다:\n{modelPath}");
                }
            }
        }

        private void btnSelectTestImage_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "테스트할 이미지들이 들어있는 폴더(예: data3/images)를 선택하세요.";
                if (!string.IsNullOrEmpty(tubPath)) dlg.SelectedPath = tubPath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    testImagePath = dlg.SelectedPath;
                    MessageBox.Show($"테스트 이미지 폴더가 선택되었습니다:\n{testImagePath}", "선택 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLog.AppendText(Environment.NewLine + $"[Info] 선택된 테스트 폴더: {testImagePath}");
                }
            }
        }

        private void btnModelTest_Click(object sender, EventArgs e)
        {
            if (_executor == null)
            {
                MessageBox.Show("먼저 로컬/원격 연결을 완료해주세요!", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(testImagePath))
            {
                MessageBox.Show("먼저 [테스트 이미지 선택] 버튼을 눌러 AI에게 보여줄 폴더를 골라주세요!", "이미지 누락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(modelPath))
            {
                DialogResult res = MessageBox.Show($"선택된 모델 파일(.h5)이 없습니다.\n기본 모델(mypilot.h5)을 사용하여 테스트를 진행하시겠습니까?", "기본 모델 사용 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes) modelPath = "models/mypilot.h5";
                else return;
            }
            else
            {
                DialogResult res = MessageBox.Show($"선택하신 모델 [{Path.GetFileName(modelPath)}] (으)로\n테스트를 진행하시겠습니까?", "테스트 진행 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No) return;
            }

            txtLog.AppendText(Environment.NewLine + $"[Test] {Path.GetFileName(modelPath)} 예측 시작...");

            bool useVenv = chkUseVenv != null ? chkUseVenv.Checked : true;
            _executor.ExecuteTest(configPath, modelPath, useVenv, (log) =>
            {
                this.Invoke(new Action(() => { if (!string.IsNullOrEmpty(log)) txtLog.AppendText(Environment.NewLine + log); }));
            });
        }

        // ==========================================
        // 4. 데이터(Tub) 탐색 및 이미지 로딩 로직
        // ==========================================
        private void btnPlayStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (tubFrames.Count == 0) return;
                if (playTimer.Enabled) playTimer.Stop();
                else
                {
                    if (trackFrame.Value == trackFrame.Maximum) trackFrame.Value = 0;
                    playTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"재생 중 오류가 발생했습니다.\n\n[오류내용]: {ex.Message}", "재생 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (trackFrame.Value < trackFrame.Maximum) trackFrame.Value++;
            else playTimer.Stop();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_executor != null) _executor.Stop();
            base.OnFormClosing(e);
        }

        private async void btnLoadTub_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Donkeycar tub 데이터 폴더를 선택하세요.";
                if (dlg.ShowDialog() == DialogResult.OK) await LoadTubAsync(dlg.SelectedPath);
            }
        }

        private async void btnReloadTub_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tubPath))
            {
                MessageBox.Show("먼저 Tub 폴더를 선택하세요.", "Load Tub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            await LoadTubAsync(tubPath);
        }

        private async Task LoadTubAsync(string selectedTubPath)
        {
            string[] catalogFiles = Directory.GetFiles(selectedTubPath, "catalog_*.catalog").OrderBy(file => file).ToArray();
            if (catalogFiles.Length == 0)
            {
                MessageBox.Show("catalog_*.catalog 파일을 찾을 수 없습니다.", "Load Tub", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tubPath = selectedTubPath;
            lblTubPath.Text = tubPath;

            btnLoadTub.Enabled = false;
            btnReloadTub.Enabled = false;
            UseWaitCursor = true;
            AddLog("Load Tub 시작...");

            tubFrames.Clear();
            missingImageFrames.Clear();
            currentTimelineStart = -1;
            lstTrash.Items.Clear();
            lstFrames.Items.Clear();
            lvTimeline.Items.Clear();
            picFrame.Image?.Dispose();
            picFrame.Image = null;
            rangeStart = -1;
            rangeEnd = -1;
            UpdateRangeLabel();

            try
            {
                TubLoadResult result = await Task.Run(() => ReadTubFrames(selectedTubPath, catalogFiles));
                tubFrames.AddRange(result.Frames);
                ResetTubView();

                if (tubFrames.Count > 0) ShowFrame(0);
                foreach (string error in result.Errors) AddLog(error);
                AddLog($"Load Tub 완료: {tubFrames.Count}개 프레임");

                // 순차적 안내 팝업창
                MessageBox.Show($"주행 데이터 {tubFrames.Count}장을 성공적으로 불러왔습니다!\n\n[다음 단계 안내]\n1. 화면 하단의 슬라이더를 움직여 비정상적인 주행 사진이 있는지 확인하세요.\n2. 필요하다면 데이터 필터링/삭제 기능을 이용해 정리하세요.\n3. 정리가 완료되었다면 좌측 하단의 [학습] 버튼을 눌러 AI 훈련을 시작하세요.", "데이터 로드 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Tub 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog($"Load Tub 오류: {ex.Message}");
            }
            finally
            {
                UseWaitCursor = false;
                btnLoadTub.Enabled = true;
                btnReloadTub.Enabled = true;
            }
        }

        private static TubLoadResult ReadTubFrames(string selectedTubPath, string[] catalogFiles)
        {
            TubLoadResult result = new TubLoadResult();
            string imageBasePath = GetImageBasePath(selectedTubPath);

            foreach (string catalogFile in catalogFiles)
            {
                foreach (string line in File.ReadLines(catalogFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        using JsonDocument document = JsonDocument.Parse(line);
                        JsonElement root = document.RootElement;
                        string imageFileName = GetStringValue(root, "cam/image_array");

                        if (string.IsNullOrWhiteSpace(imageFileName)) continue;

                        TubFrame frame = new TubFrame
                        {
                            FrameNumber = GetIntValue(root, "_index", result.Frames.Count),
                            ImageFileName = imageFileName,
                            ImagePath = FindImagePath(selectedTubPath, imageBasePath, imageFileName),
                            Angle = GetDoubleValue(root, "user/angle"),
                            Throttle = GetDoubleValue(root, "user/throttle"),
                            SourceCatalog = catalogFile
                        };
                        result.Frames.Add(frame);
                    }
                    catch (JsonException ex)
                    {
                        result.Errors.Add($"catalog 파싱 오류: {Path.GetFileName(catalogFile)} - {ex.Message}");
                    }
                }
            }
            return result;
        }

        private void ResetTubView()
        {
            lstFrames.BeginUpdate();
            lstFrames.Items.Clear();
            lvTimeline.Items.Clear();
            timelineImages.Images.Clear();
            currentTimelineStart = -1;
            trackFrame.Minimum = 0;
            trackFrame.Maximum = Math.Max(0, tubFrames.Count - 1);
            trackFrame.Value = 0;

            try
            {
                object[] frameItems = new object[tubFrames.Count];
                for (int i = 0; i < tubFrames.Count; i++) frameItems[i] = tubFrames[i];
                lstFrames.Items.AddRange(frameItems);
            }
            finally { lstFrames.EndUpdate(); }
        }

        private void UpdateTimelineForFrame(int frameIndex)
        {
            int timelineStart = (frameIndex / TimelineVisibleCount) * TimelineVisibleCount;
            if (timelineStart == currentTimelineStart) return;

            currentTimelineStart = timelineStart;
            lvTimeline.BeginUpdate();
            lvTimeline.Items.Clear();
            timelineImages.Images.Clear();

            try
            {
                int timelineEnd = Math.Min(tubFrames.Count, timelineStart + TimelineVisibleCount);
                for (int i = timelineStart; i < timelineEnd; i++)
                {
                    TubFrame frame = tubFrames[i];
                    string imageKey = i.ToString();
                    timelineImages.Images.Add(imageKey, CreateTimelineThumbnail(frame.ImagePath));
                    lvTimeline.Items.Add(new ListViewItem("", imageKey) { Tag = i, ToolTipText = frame.ToString() });
                }
            }
            finally { lvTimeline.EndUpdate(); }
        }

        private void ShowFrame(int index)
        {
            if (index < 0 || index >= tubFrames.Count) return;

            TubFrame frame = tubFrames[index];

            lstFrames.SelectedIndexChanged -= lstFrames_SelectedIndexChanged;
            lstFrames.SelectedIndex = index;
            lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged;

            trackFrame.Value = index;
            UpdateTimelineForFrame(index);

            int timelineItemIndex = index - currentTimelineStart;
            if (timelineItemIndex >= 0 && timelineItemIndex < lvTimeline.Items.Count)
            {
                isUpdatingTimelineSelection = true;
                lvTimeline.Items[timelineItemIndex].Selected = true;
                lvTimeline.Items[timelineItemIndex].Focused = true;
                lvTimeline.Items[timelineItemIndex].EnsureVisible();
                isUpdatingTimelineSelection = false;
            }

            picFrame.Image?.Dispose();
            picFrame.Image = LoadImage(frame.ImagePath);

            if (!File.Exists(frame.ImagePath)) missingImageFrames.Add(index);

            lblFrame.Text = $"프레임: {frame.FrameNumber:D6}";
            lblAngle.Text = $"조향각: {frame.Angle:0.00}";
            lblThrottle.Text = $"속도: {frame.Throttle:0.00}";
        }

        private static string GetImageBasePath(string selectedTubPath)
        {
            string imagesPath = Path.Combine(selectedTubPath, "images");
            if (Directory.Exists(imagesPath)) return imagesPath;
            string imageArrayPath = Path.Combine(selectedTubPath, "image_array");
            if (Directory.Exists(imageArrayPath)) return imageArrayPath;
            return selectedTubPath;
        }

        private static string FindImagePath(string selectedTubPath, string imageBasePath, string imageFileName)
        {
            if (Path.IsPathRooted(imageFileName)) return imageFileName;
            string normalizedImageFileName = imageFileName.Replace('/', Path.DirectorySeparatorChar);
            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(normalizedImageFileName))) return Path.Combine(selectedTubPath, normalizedImageFileName);
            return Path.Combine(imageBasePath, normalizedImageFileName);
        }

        private static Image LoadImage(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Bitmap missing = new Bitmap(160, 120);
                using Graphics graphics = Graphics.FromImage(missing);
                graphics.Clear(Color.Black);
                using Brush brush = new SolidBrush(Color.White);
                graphics.DrawString("Missing image", SystemFonts.DefaultFont, brush, new PointF(28, 52));
                return missing;
            }
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using Image source = Image.FromStream(stream);
            return new Bitmap(source);
        }

        private static Image CreateTimelineThumbnail(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Bitmap missing = new Bitmap(36, 27);
                using Graphics graphics = Graphics.FromImage(missing);
                graphics.Clear(Color.Black);
                using Pen pen = new Pen(Color.DarkGray);
                graphics.DrawRectangle(pen, 0, 0, 35, 26);
                return missing;
            }
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using Image source = Image.FromStream(stream);
            return new Bitmap(source, new Size(36, 27));
        }

        private static string GetStringValue(JsonElement root, string propertyName)
        {
            if (!root.TryGetProperty(propertyName, out JsonElement value)) return "";
            return value.ValueKind == JsonValueKind.String ? value.GetString() ?? "" : value.ToString();
        }

        private static int GetIntValue(JsonElement root, string propertyName, int defaultValue)
        {
            if (!root.TryGetProperty(propertyName, out JsonElement value)) return defaultValue;
            if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out int result)) return result;
            return int.TryParse(value.ToString(), out result) ? result : defaultValue;
        }

        private static double GetDoubleValue(JsonElement root, string propertyName)
        {
            if (!root.TryGetProperty(propertyName, out JsonElement value)) return 0;
            if (value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out double result)) return result;
            return double.TryParse(value.ToString(), out result) ? result : 0;
        }

        private void AddLog(string message) => txtLog.AppendText(Environment.NewLine + message);
        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e) => ShowFrame(lstFrames.SelectedIndex);
        private void lvTimeline_SelectedIndexChanged(object sender, EventArgs e) { if (!isUpdatingTimelineSelection && lvTimeline.SelectedItems.Count > 0 && lvTimeline.SelectedItems[0].Tag is int index) ShowFrame(index); }
        private void trackFrame_Scroll(object sender, EventArgs e) => ShowFrame(trackFrame.Value);
        private void btnFirst_Click(object sender, EventArgs e) => ShowFrame(0);
        private void btnPrev_Click(object sender, EventArgs e) => ShowFrame(Math.Max(0, trackFrame.Value - 1));
        private void btnNext_Click(object sender, EventArgs e) => ShowFrame(Math.Min(tubFrames.Count - 1, trackFrame.Value + 1));
        private void btnLast_Click(object sender, EventArgs e) => ShowFrame(tubFrames.Count - 1);

        // 안 쓰는 빈 이벤트 모음 (에러 방지용)
        private void lblConfigPath_Click(object sender, EventArgs e) { }
        private void groupTubNavigator_Enter(object sender, EventArgs e) { }
        private void lvTimeline_SelectedIndexChanged_1(object sender, EventArgs e) { }

        // ==========================================
        // 5. 데이터 정리: 휴지통(삭제 상태) 관리
        // ==========================================
        // 프레임을 휴지통으로 이동(소프트 삭제). 실제 파일은 휴지통 비우기 시점까지 유지한다.
        private bool MoveToTrash(int index, string reason)
        {
            if (index < 0 || index >= tubFrames.Count) return false;
            TubFrame frame = tubFrames[index];
            if (frame.Deleted) return false;
            frame.Deleted = true;
            frame.DeleteReason = reason;
            RefreshFrameListItem(index);
            return true;
        }

        // 휴지통에서 프레임을 되살린다(삭제 상태 해제).
        private void RestoreFromTrash(TubFrame frame)
        {
            if (!frame.Deleted) return;
            frame.Deleted = false;
            frame.DeleteReason = "";
            RefreshFrameListItem(tubFrames.IndexOf(frame));
        }

        // 현재 삭제 상태인 프레임들로 휴지통 목록을 다시 구성한다.
        private void RebuildTrashList()
        {
            lstTrash.BeginUpdate();
            lstTrash.Items.Clear();
            foreach (TubFrame frame in tubFrames)
            {
                if (frame.Deleted) lstTrash.Items.Add(new TrashEntry(frame));
            }
            lstTrash.EndUpdate();
        }

        // ListBox 항목의 표시 문자열(삭제 표시)을 갱신하기 위해 동일 항목을 재대입한다.
        // 항목 값만 교체하므로 선택 인덱스는 바뀌지 않아 SelectedIndexChanged가 발생하지 않는다.
        private void RefreshFrameListItem(int index)
        {
            if (index < 0 || index >= lstFrames.Items.Count) return;
            lstFrames.Items[index] = tubFrames[index];
        }

        // ==========================================
        // 6. 데이터 필터링 (범위 지정 + 조건 필터)
        // ==========================================
        private void btnSetLeft_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0) return;
            rangeStart = trackFrame.Value;
            UpdateRangeLabel();
        }

        private void btnSetRight_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0) return;
            rangeEnd = trackFrame.Value;
            UpdateRangeLabel();
        }

        private void UpdateRangeLabel()
        {
            string left = rangeStart >= 0 ? rangeStart.ToString() : "-";
            string right = rangeEnd >= 0 ? rangeEnd.ToString() : "-";
            lblRange.Text = $"범위: {left} ~ {right}";
        }

        // 지정된 범위를 [lo, hi]로 정규화한다. 범위가 전혀 지정되지 않았으면 전체 구간을 반환한다.
        private (int lo, int hi) GetEffectiveRange()
        {
            if (rangeStart < 0 && rangeEnd < 0) return (0, tubFrames.Count - 1);
            int a = rangeStart < 0 ? rangeEnd : rangeStart;
            int b = rangeEnd < 0 ? rangeStart : rangeEnd;
            int lo = Math.Max(0, Math.Min(a, b));
            int hi = Math.Min(tubFrames.Count - 1, Math.Max(a, b));
            return (lo, hi);
        }

        private void btnFilter_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0)
            {
                MessageBox.Show("먼저 Tub 데이터를 불러오세요.", "필터 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!chkThrottleZero.Checked && !chkMissingImage.Checked && !chkAbnormalAngle.Checked)
            {
                MessageBox.Show("적용할 필터 조건을 하나 이상 선택하세요.", "필터 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            (int lo, int hi) = GetEffectiveRange();

            // 비정상 조향각은 이상치 탐지 알고리즘으로 후보를 미리 산출한다.
            HashSet<int> abnormal = chkAbnormalAngle.Checked ? DetectAbnormalAngleFrames(lo, hi) : new HashSet<int>();

            int moved = 0;
            for (int i = lo; i <= hi; i++)
            {
                TubFrame frame = tubFrames[i];
                if (frame.Deleted) continue;

                string? reason = null;
                if (chkThrottleZero.Checked && Math.Abs(frame.Throttle) < 1e-6) reason = "속도 0";
                else if (chkMissingImage.Checked && !File.Exists(frame.ImagePath)) reason = "이미지 누락";
                else if (chkAbnormalAngle.Checked && abnormal.Contains(i)) reason = "비정상 조향각";

                if (reason != null && MoveToTrash(i, reason)) moved++;
            }

            RebuildTrashList();
            AddLog($"필터 적용: 범위 {lo}~{hi}에서 {moved}개 프레임을 휴지통으로 이동했습니다.");
            if (moved == 0)
            {
                MessageBox.Show("조건에 해당하는 프레임이 없습니다.", "필터 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 비정상 조향각(이상치) 프레임 인덱스를 반환한다.
        // 판정 기준(셋 중 하나라도 해당 시 이상치):
        //   (1) 정규화 범위 [-1, 1] 초과
        //   (2) 통계적 이상치: 대상 구간 평균/표준편차 기준 z-score > 3.0
        //   (3) 급변: 직전 유효 프레임 대비 조향각 변화량 |Δ| > 0.8
        private HashSet<int> DetectAbnormalAngleFrames(int lo, int hi)
        {
            const double rangeLimit = 1.0;
            const double zThreshold = 3.0;
            const double jumpThreshold = 0.8;

            HashSet<int> result = new HashSet<int>();

            // 대상 구간의 미삭제 프레임만 수집
            List<int> active = new List<int>();
            for (int i = lo; i <= hi; i++)
            {
                if (!tubFrames[i].Deleted) active.Add(i);
            }
            if (active.Count == 0) return result;

            // 평균/표준편차(표본) 계산
            double sum = 0;
            foreach (int i in active) sum += tubFrames[i].Angle;
            double mean = sum / active.Count;

            double sqSum = 0;
            foreach (int i in active)
            {
                double d = tubFrames[i].Angle - mean;
                sqSum += d * d;
            }
            double std = active.Count > 1 ? Math.Sqrt(sqSum / (active.Count - 1)) : 0;

            // 이상치 판정
            for (int k = 0; k < active.Count; k++)
            {
                int idx = active[k];
                double angle = tubFrames[idx].Angle;

                bool outlier = Math.Abs(angle) > rangeLimit;
                if (!outlier && std > 1e-9 && Math.Abs(angle - mean) / std > zThreshold) outlier = true;
                if (!outlier && k > 0 && Math.Abs(angle - tubFrames[active[k - 1]].Angle) > jumpThreshold) outlier = true;

                if (outlier) result.Add(idx);
            }
            return result;
        }

        // ==========================================
        // 7. 데이터 삭제 / 복원 / 휴지통 비우기
        // ==========================================
        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0) return;

            int moved = 0;
            if (rangeStart >= 0 || rangeEnd >= 0)
            {
                (int lo, int hi) = GetEffectiveRange();
                DialogResult res = MessageBox.Show($"범위 {lo}~{hi}의 프레임을 휴지통으로 이동하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No) return;
                for (int i = lo; i <= hi; i++)
                {
                    if (MoveToTrash(i, "수동 삭제")) moved++;
                }
            }
            else
            {
                int index = lstFrames.SelectedIndex;
                if (index < 0)
                {
                    MessageBox.Show("삭제할 프레임을 목록에서 선택하거나 [시작 지정]/[끝 지정]으로 범위를 정하세요.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MoveToTrash(index, "수동 삭제")) moved++;
            }

            RebuildTrashList();
            AddLog($"{moved}개 프레임을 휴지통으로 이동했습니다.");
        }

        private void btnRestore_Click(object? sender, EventArgs e)
        {
            if (lstTrash.SelectedItems.Count == 0)
            {
                MessageBox.Show("복원할 항목을 휴지통 목록에서 선택하세요.", "복원", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<TrashEntry> selected = lstTrash.SelectedItems.Cast<TrashEntry>().ToList();
            foreach (TrashEntry entry in selected) RestoreFromTrash(entry.Frame);
            RebuildTrashList();
            AddLog($"{selected.Count}개 프레임을 복원했습니다.");
        }

        // 안전 모드: 이미지는 deleted 폴더로 이동하고 catalog는 백업 후 해당 기록만 제거(되돌릴 수 있도록 보존).
        private void btnEmptyTrash_Click(object? sender, EventArgs e)
        {
            List<TubFrame> deleted = tubFrames.Where(f => f.Deleted).ToList();
            if (deleted.Count == 0)
            {
                MessageBox.Show("휴지통이 비어 있습니다.", "휴지통 비우기", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(tubPath))
            {
                MessageBox.Show("Tub 폴더 정보가 없습니다.", "휴지통 비우기", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult res = MessageBox.Show(
                $"휴지통의 {deleted.Count}개 프레임을 최종 적용합니다.\n\n" +
                "· 이미지는 tub 폴더 하위 [deleted] 폴더로 이동합니다.\n" +
                "· catalog 파일은 백업 후 해당 기록을 제거합니다.\n\n계속하시겠습니까?",
                "휴지통 비우기 (안전 모드)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.No) return;

            try
            {
                string deletedDir = Path.Combine(tubPath, "deleted");
                Directory.CreateDirectory(deletedDir);

                // 1) catalog 파일별로 삭제 대상 기록을 제외하고 재작성 (원본은 1회 백업)
                foreach (IGrouping<string, TubFrame> group in deleted.Where(f => !string.IsNullOrEmpty(f.SourceCatalog)).GroupBy(f => f.SourceCatalog))
                {
                    string catalogFile = group.Key;
                    if (!File.Exists(catalogFile)) continue;

                    HashSet<string> removeImages = new HashSet<string>(group.Select(f => f.ImageFileName));

                    string backupPath = Path.Combine(deletedDir, Path.GetFileName(catalogFile) + ".backup");
                    if (!File.Exists(backupPath)) File.Copy(catalogFile, backupPath);

                    List<string> keptLines = new List<string>();
                    foreach (string line in File.ReadLines(catalogFile))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string? imageName = TryGetImageName(line);
                        if (imageName != null && removeImages.Contains(imageName)) continue;
                        keptLines.Add(line);
                    }
                    File.WriteAllLines(catalogFile, keptLines);
                }

                // 2) 이미지 파일을 deleted 폴더로 이동
                int movedImages = 0;
                foreach (TubFrame f in deleted)
                {
                    if (File.Exists(f.ImagePath))
                    {
                        string dest = Path.Combine(deletedDir, Path.GetFileName(f.ImagePath));
                        File.Move(f.ImagePath, dest, true);
                        movedImages++;
                    }
                }

                // 3) 메모리 목록에서 제거 후 뷰 재구성
                tubFrames.RemoveAll(f => f.Deleted);
                missingImageFrames.Clear();
                ResetTubView();
                lstTrash.Items.Clear();
                if (tubFrames.Count > 0) ShowFrame(0);
                else { picFrame.Image?.Dispose(); picFrame.Image = null; }

                AddLog($"휴지통 비우기 완료: 기록 {deleted.Count}건 제거, 이미지 {movedImages}개 이동 (백업 위치: {deletedDir}).");
                MessageBox.Show($"휴지통을 비웠습니다.\n\n제거된 기록: {deleted.Count}건\n이동된 이미지: {movedImages}개\n백업 위치: {deletedDir}", "휴지통 비우기 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"휴지통 비우기 중 오류가 발생했습니다.\n\n[오류내용]: {ex.Message}", "휴지통 비우기 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog($"휴지통 비우기 오류: {ex.Message}");
            }
        }

        // catalog 한 줄(JSON)에서 cam/image_array 값을 추출한다. 실패 시 null.
        private static string? TryGetImageName(string line)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(line);
                string name = GetStringValue(document.RootElement, "cam/image_array");
                return string.IsNullOrWhiteSpace(name) ? null : name;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        // ==========================================
        // 내부 클래스
        // ==========================================
        private sealed class TubFrame
        {
            public int FrameNumber { get; set; }
            public string ImageFileName { get; set; } = "";
            public string ImagePath { get; set; } = "";
            public double Angle { get; set; }
            public double Throttle { get; set; }
            public bool Deleted { get; set; }
            public string DeleteReason { get; set; } = "";
            public string SourceCatalog { get; set; } = "";
            public override string ToString() => Deleted ? $"Frame {FrameNumber:D6} [삭제됨]" : $"Frame {FrameNumber:D6}";
        }

        private sealed class TubLoadResult
        {
            public List<TubFrame> Frames { get; } = new();
            public List<string> Errors { get; } = new();
        }

        // 휴지통 목록(lstTrash)에 표시되는 삭제 프레임 항목. 복원 시 원본 프레임으로 역매핑한다.
        private sealed class TrashEntry
        {
            public TubFrame Frame { get; }
            public TrashEntry(TubFrame frame) => Frame = frame;
            public override string ToString() => $"Frame {Frame.FrameNumber:D6} · {Frame.DeleteReason}";
        }
    }
}