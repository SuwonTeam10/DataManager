using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
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
        private string latestTestImagePath = "";
        private double? latestTestRealAngle;
        private double? latestTestPredictAngle;
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
        private const int PlaybackBaseIntervalMs = 100;
        private const int PlaybackMinimumIntervalMs = 50;
        private int currentTimelineStart = -1;
        private bool isUpdatingTimelineSelection;
        private int playbackFrameStep = 1;

        // 데이터 정리(필터/삭제) 대상 범위. -1은 미지정.
        private int rangeStart = -1;
        private int rangeEnd = -1;

        // 자동 재생 성능을 위한 표시용 이미지 LRU 캐시(메모리 상한 적용). 캐시가 Bitmap 소유권을 가진다.
        private readonly FrameImageCache frameImageCache;

        // 그래프 탭 컨트롤은 디자이너 충돌을 줄이기 위해 런타임에 생성한다.
        private readonly PictureBox picDataGraph = new();
        private readonly Button btnReloadGraph = new();
        private readonly CheckBox chkGraphAngle = new();
        private readonly CheckBox chkGraphThrottle = new();
        private readonly Label lblGraphSummary = new();
        private readonly Label lblGraphHover = new();
        private Rectangle graphPlotBounds = Rectangle.Empty;
        private List<TubFrame> graphVisibleFrames = new();
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

            // 표시용 이미지 캐시 초기화 (최근 64프레임 유지)
            frameImageCache = new FrameImageCache(64, LoadImage);

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
            cmbPlaySpeed.SelectedIndexChanged += cmbPlaySpeed_SelectedIndexChanged;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            Resize += (_, _) => RedrawGraphAfterLayout();

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

            InitializeGraphControls();
            picTestImage.SizeMode = PictureBoxSizeMode.Zoom;
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

            // 기본 재생속도 1x로 설정
            if (cmbPlaySpeed.SelectedIndex < 0) cmbPlaySpeed.SelectedIndex = 0;
            ApplyPlaybackSpeed();
            UpdatePlaybackControlsVisual(false);
            UpdateAutoPlayLoopVisual();
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
                    ShowTestImagePreview(FindFirstTestImagePath());
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
            ResetModelTestResult();
            ShowTestImagePreview(FindFirstTestImagePath());

            bool useVenv = chkUseVenv != null ? chkUseVenv.Checked : true;
            _executor.ExecuteTest(configPath, modelPath, useVenv, (log) =>
            {
                this.Invoke(new Action(() => HandleModelTestLog(log)));
            });
        }

        private void ResetModelTestResult()
        {
            // 모델 테스트 시작 전 이전 결과를 지워서 새 로그 값만 보이게 한다.
            latestTestImagePath = "";
            latestTestRealAngle = null;
            latestTestPredictAngle = null;
            lblRealAngle2.Text = "-";
            lblPredictAngle2.Text = "-";
            lblErrorValue2.Text = "-";
            lblTrainStatus2.Text = "테스트 중";
            lblTrainStatus2.ForeColor = Color.DarkOrange;
        }

        private void HandleModelTestLog(string logText)
        {
            if (string.IsNullOrWhiteSpace(logText)) return;

            txtLog.AppendText(Environment.NewLine + logText);

            string? imagePath = TryFindImagePathFromLog(logText);
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                ShowTestImagePreview(imagePath);
            }

            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:real|actual|label|target|user/angle|실제\s*조향각|실제)\s*[:=]\s*(-?\d+(?:\.\d+)?)",
                    @"^\s*angle\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double realAngle))
            {
                latestTestRealAngle = realAngle;
                lblRealAngle2.Text = realAngle.ToString("0.000");
            }

            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:predict(?:ed)?|prediction|pred|pilot/angle|예측\s*조향각|예측)\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double predictAngle))
            {
                latestTestPredictAngle = predictAngle;
                lblPredictAngle2.Text = predictAngle.ToString("0.000");
            }

            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:error|loss|diff|오차)\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double errorValue))
            {
                lblErrorValue2.Text = Math.Abs(errorValue).ToString("0.000");
            }
            else if (latestTestRealAngle.HasValue && latestTestPredictAngle.HasValue)
            {
                lblErrorValue2.Text = Math.Abs(latestTestRealAngle.Value - latestTestPredictAngle.Value).ToString("0.000");
            }

            if (logText.Contains("Finished", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("complete", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("완료", StringComparison.OrdinalIgnoreCase))
            {
                lblTrainStatus2.Text = "테스트 완료";
                lblTrainStatus2.ForeColor = Color.Green;
            }
            else if (logText.Contains("Error", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("Exception", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("Traceback", StringComparison.OrdinalIgnoreCase))
            {
                lblTrainStatus2.Text = "오류";
                lblTrainStatus2.ForeColor = Color.Red;
            }
        }

        private string? FindFirstTestImagePath()
        {
            if (string.IsNullOrWhiteSpace(testImagePath) || !Directory.Exists(testImagePath)) return null;

            try
            {
                return Directory.EnumerateFiles(testImagePath, "*.*", SearchOption.AllDirectories)
                    .FirstOrDefault(IsImageFile);
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        private string? TryFindImagePathFromLog(string logText)
        {
            Match match = Regex.Match(logText, @"(?<path>[^\s""']+\.(?:jpg|jpeg|png|bmp))", RegexOptions.IgnoreCase);
            if (!match.Success) return null;

            string rawPath = match.Groups["path"].Value.Trim();
            if (File.Exists(rawPath)) return rawPath;

            string normalizedPath = rawPath.Replace('/', Path.DirectorySeparatorChar);
            if (File.Exists(normalizedPath)) return normalizedPath;

            if (!string.IsNullOrWhiteSpace(testImagePath))
            {
                string byName = Path.Combine(testImagePath, Path.GetFileName(normalizedPath));
                if (File.Exists(byName)) return byName;

                if (Directory.Exists(testImagePath))
                {
                    try
                    {
                        return Directory.EnumerateFiles(testImagePath, Path.GetFileName(normalizedPath), SearchOption.AllDirectories)
                            .FirstOrDefault();
                    }
                    catch (IOException)
                    {
                        return null;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        private void ShowTestImagePreview(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath)) return;
            if (string.Equals(latestTestImagePath, imagePath, StringComparison.OrdinalIgnoreCase)) return;

            Image? oldImage = picTestImage.Image;
            picTestImage.Image = LoadImage(imagePath);
            oldImage?.Dispose();
            latestTestImagePath = imagePath;
        }

        private static bool TryExtractLogValue(string logText, IEnumerable<string> patterns, out double value)
        {
            foreach (string pattern in patterns)
            {
                Match match = Regex.Match(logText, pattern, RegexOptions.IgnoreCase);
                if (match.Success && TryParseLogDouble(match.Groups[1].Value, out value))
                {
                    return true;
                }
            }

            value = 0;
            return false;
        }

        private static bool TryParseLogDouble(string text, out double value)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                || double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        private static bool IsImageFile(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            return extension is ".jpg" or ".jpeg" or ".png" or ".bmp";
        }

        // ==========================================
        // 4. 데이터(Tub) 탐색 및 이미지 로딩 로직
        // ==========================================
        private void btnPlayStop_Click(object sender, EventArgs e)
        {
            try
            {
                SetPlaybackState(!playTimer.Enabled);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"재생 중 오류가 발생했습니다.\n\n[오류내용]: {ex.Message}", "재생 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlayTimer_Tick(object? sender, EventArgs e)
        {
            // 재생 중에는 트랙바 값만 바꾸지 않고 ShowFrame을 호출해서 이미지/라벨/목록 선택까지 같이 갱신한다.
            int next = AdvanceActiveIndex(trackFrame.Value, playbackFrameStep);
            if (next < 0)
            {
                if (chkAutoPlay.Checked)
                {
                    int firstActive = FirstActiveIndex();
                    if (firstActive >= 0)
                    {
                        ShowFrame(firstActive);
                        return;
                    }
                }

                SetPlaybackState(false);
                return;
            }

            ShowFrame(next);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_executor != null) _executor.Stop();
            frameImageCache.Clear();
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
            SetPlaybackState(false);

            // 신버전 Tub은 catalog_*.catalog, 구버전 Tub은 record_*.json 파일을 사용한다.
            // 사용자가 data 폴더를 선택해도 내부 tub 폴더를 찾을 수 있도록 하위 폴더까지 검색한다.
            string[] catalogFiles = Directory.GetFiles(selectedTubPath, "catalog_*.catalog", SearchOption.AllDirectories)
                .OrderBy(GetFileOrderNumber)
                .ThenBy(file => file)
                .ToArray();
            string[] recordFiles = Array.Empty<string>();
            bool isOldRecordTub = catalogFiles.Length == 0;

            if (isOldRecordTub)
            {
                // catalog 파일이 없으면 구버전 record JSON 형식으로 판단하고 record_*.json을 찾는다.
                recordFiles = Directory.GetFiles(selectedTubPath, "record_*.json", SearchOption.AllDirectories)
                    .OrderBy(GetFileOrderNumber)
                    .ThenBy(file => file)
                    .ToArray();
            }

            if (catalogFiles.Length == 0 && recordFiles.Length == 0)
            {
                MessageBox.Show("catalog_*.catalog 또는 record_*.json 파일을 찾을 수 없습니다.", "Load Tub", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            picFrame.Image = null;
            frameImageCache.Clear();
            rangeStart = -1;
            rangeEnd = -1;
            UpdateRangeLabel();

            try
            {
                // Tub 형식에 따라 신버전 catalog 파서 또는 구버전 record JSON 파서를 선택한다.
                TubLoadResult result = await Task.Run(() =>
                    isOldRecordTub
                        ? ReadOldTubFrames(selectedTubPath, recordFiles)
                        : ReadTubFrames(selectedTubPath, catalogFiles));
                tubFrames.AddRange(result.Frames);
                ResetTubView();

                if (tubFrames.Count > 0) ShowFrame(0);
                RenderTubGraph();
                foreach (string error in result.Errors) AddLog(error);
                AddLog($"Load Tub 완료: {tubFrames.Count}개 프레임 ({(isOldRecordTub ? "구버전 record JSON" : "catalog")} 형식)");

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
            // 신버전 Donkeycar Tub의 catalog 파일을 읽어 프레임 데이터로 변환한다.
            TubLoadResult result = new TubLoadResult();
            Dictionary<string, Dictionary<string, string>> imageLookupCache = new();

            foreach (string catalogFile in catalogFiles)
            {
                // catalog가 하위 tub 폴더에 있을 수 있으므로 해당 catalog 폴더를 이미지 기준 경로로 사용한다.
                string tubBasePath = Path.GetDirectoryName(catalogFile) ?? selectedTubPath;
                string imageBasePath = GetImageBasePath(tubBasePath);
                Dictionary<string, string>? imageLookup = null;

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
                            ImagePath = FindImagePath(tubBasePath, imageBasePath, imageLookupCache, ref imageLookup, imageFileName),
                            Angle = GetDoubleValue(root, "user/angle"),
                            Throttle = GetDoubleValue(root, "user/throttle")
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

        private static TubLoadResult ReadOldTubFrames(string selectedTubPath, string[] recordFiles)
        {
            // 구버전 Donkeycar Tub은 record_0.json 같은 개별 JSON 파일에 프레임 정보를 저장한다.
            TubLoadResult result = new TubLoadResult();
            Dictionary<string, Dictionary<string, string>> imageLookupCache = new();

            foreach (string recordFile in recordFiles)
            {
                // record 파일도 하위 tub 폴더에 있을 수 있으므로 record가 있는 폴더 기준으로 이미지를 찾는다.
                string tubBasePath = Path.GetDirectoryName(recordFile) ?? selectedTubPath;
                string imageBasePath = GetImageBasePath(tubBasePath);
                Dictionary<string, string>? imageLookup = null;

                try
                {
                    using JsonDocument document = JsonDocument.Parse(File.ReadAllText(recordFile));
                    JsonElement root = document.RootElement;
                    string imageFileName = GetStringValue(root, "cam/image_array");

                    if (string.IsNullOrWhiteSpace(imageFileName))
                    {
                        result.Errors.Add($"record 이미지 정보 없음: {Path.GetFileName(recordFile)}");
                        continue;
                    }

                    int fallbackFrameNumber = GetFileOrderNumber(recordFile);
                    if (fallbackFrameNumber == int.MaxValue)
                    {
                        // 파일명에서 번호를 못 찾으면 읽은 순서를 프레임 번호로 사용한다.
                        fallbackFrameNumber = result.Frames.Count;
                    }

                    TubFrame frame = new TubFrame
                    {
                        FrameNumber = GetIntValue(root, "_index", fallbackFrameNumber),
                        ImageFileName = imageFileName,
                        ImagePath = FindImagePath(tubBasePath, imageBasePath, imageLookupCache, ref imageLookup, imageFileName),
                        SourceDataPath = recordFile,
                        Angle = GetDoubleValue(root, "user/angle"),
                        Throttle = GetDoubleValue(root, "user/throttle")
                    };

                    result.Frames.Add(frame);
                }
                catch (JsonException ex)
                {
                    result.Errors.Add($"record 파싱 오류: {Path.GetFileName(recordFile)} - {ex.Message}");
                }
                catch (IOException ex)
                {
                    result.Errors.Add($"record 읽기 오류: {Path.GetFileName(recordFile)} - {ex.Message}");
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
            if (tubFrames.Count == 0) UpdateFrameInfoLabels(null, -1);

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

            // 캐시가 소유한 이미지를 표시한다. (캐시가 Dispose를 책임지므로 여기서 Dispose하지 않는다.)
            picFrame.Image = frameImageCache.Get(frame.ImagePath);

            if (!File.Exists(frame.ImagePath)) missingImageFrames.Add(index);

            UpdateFrameInfoLabels(frame, index);
        }

        private void UpdateFrameInfoLabels(TubFrame? frame, int index)
        {
            // 디자이너가 만든 제목 라벨은 고정하고, 실제 값 라벨만 갱신한다.
            if (frame == null || index < 0 || tubFrames.Count == 0)
            {
                lblFrame2.Text = "000000";
                lblAngle2.Text = "0.00";
                lblThrottle2.Text = "0.00";
                lblCurrentFrame2.Text = "0 / 0";
                return;
            }

            lblFrame2.Text = frame.FrameNumber.ToString("D6");
            lblAngle2.Text = frame.Angle.ToString("0.00");
            lblThrottle2.Text = frame.Throttle.ToString("0.00");
            lblCurrentFrame2.Text = $"{index + 1:N0} / {tubFrames.Count:N0}";
        }

        private static string GetImageBasePath(string selectedTubPath)
        {
            string imagesPath = Path.Combine(selectedTubPath, "images");
            if (Directory.Exists(imagesPath)) return imagesPath;
            string imageArrayPath = Path.Combine(selectedTubPath, "image_array");
            if (Directory.Exists(imageArrayPath)) return imageArrayPath;
            return selectedTubPath;
        }

        private static Dictionary<string, string> GetImageLookup(string selectedTubPath, Dictionary<string, Dictionary<string, string>> imageLookupCache)
        {
            // 이미지 폴더명이 달라도 파일명으로 찾을 수 있도록 tub 폴더 아래의 이미지 파일 목록을 캐시한다.
            if (imageLookupCache.TryGetValue(selectedTubPath, out Dictionary<string, string>? cachedLookup))
            {
                return cachedLookup;
            }

            string[] imageExtensions = { ".jpg", ".jpeg", ".png" };
            Dictionary<string, string> imageLookup = Directory.GetFiles(selectedTubPath, "*.*", SearchOption.AllDirectories)
                .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                .GroupBy(file => Path.GetFileName(file), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            imageLookupCache[selectedTubPath] = imageLookup;
            return imageLookup;
        }

        private static string FindImagePath(
            string selectedTubPath,
            string imageBasePath,
            Dictionary<string, Dictionary<string, string>> imageLookupCache,
            ref Dictionary<string, string>? imageLookup,
            string imageFileName)
        {
            // catalog의 cam/image_array 파일명을 기준으로 이미지 경로를 결정한다.
            // 기본 위치에서 못 찾을 때만 tub 하위 전체 검색 캐시를 만든다.
            if (Path.IsPathRooted(imageFileName)) return imageFileName;
            string normalizedImageFileName = imageFileName.Replace('/', Path.DirectorySeparatorChar);
            string fileNameOnly = Path.GetFileName(normalizedImageFileName);

            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(normalizedImageFileName)))
            {
                string catalogRelativePath = Path.Combine(selectedTubPath, normalizedImageFileName);
                if (File.Exists(catalogRelativePath))
                {
                    return catalogRelativePath;
                }
            }

            string basePath = Path.Combine(imageBasePath, fileNameOnly);
            if (File.Exists(basePath))
            {
                return basePath;
            }

            imageLookup ??= GetImageLookup(selectedTubPath, imageLookupCache);
            if (imageLookup.TryGetValue(fileNameOnly, out string? foundImagePath))
            {
                return foundImagePath;
            }

            return basePath;
        }

        private static int GetFileOrderNumber(string filePath)
        {
            // catalog_10.catalog, record_10.json 같은 파일명을 숫자 기준으로 정렬하기 위해 번호를 추출한다.
            Match match = Regex.Match(Path.GetFileNameWithoutExtension(filePath), @"(\d+)$");
            return match.Success ? int.Parse(match.Groups[1].Value) : int.MaxValue;
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

        private void InitializeGraphControls()
        {
            tabGraph.Controls.Clear();

            TableLayoutPanel graphLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(12)
            };
            graphLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            graphLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            FlowLayoutPanel graphToolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 4, 0, 4)
            };

            btnReloadGraph.Text = "그래프 새로고침";
            btnReloadGraph.Size = new Size(150, 30);
            btnReloadGraph.Click += (_, _) => RenderTubGraph();

            chkGraphAngle.Text = "조향각";
            chkGraphAngle.Checked = true;
            chkGraphAngle.AutoSize = true;
            chkGraphAngle.Margin = new Padding(14, 7, 8, 0);
            chkGraphAngle.CheckedChanged += (_, _) => RenderTubGraph();

            chkGraphThrottle.Text = "속도";
            chkGraphThrottle.Checked = true;
            chkGraphThrottle.AutoSize = true;
            chkGraphThrottle.Margin = new Padding(8, 7, 20, 0);
            chkGraphThrottle.CheckedChanged += (_, _) => RenderTubGraph();

            lblGraphSummary.AutoSize = true;
            lblGraphSummary.Margin = new Padding(12, 8, 0, 0);
            lblGraphSummary.Text = "Tub 데이터를 불러오면 그래프가 표시됩니다.";

            graphToolbar.Controls.Add(btnReloadGraph);
            graphToolbar.Controls.Add(chkGraphAngle);
            graphToolbar.Controls.Add(chkGraphThrottle);
            graphToolbar.Controls.Add(lblGraphSummary);

            picDataGraph.Dock = DockStyle.Fill;
            picDataGraph.BackColor = Color.White;
            picDataGraph.BorderStyle = BorderStyle.FixedSingle;
            picDataGraph.Margin = new Padding(0);
            picDataGraph.SizeMode = PictureBoxSizeMode.StretchImage;
            picDataGraph.Resize += (_, _) => RedrawGraphAfterLayout();
            picDataGraph.MouseMove += picDataGraph_MouseMove;
            picDataGraph.MouseLeave += (_, _) => lblGraphHover.Visible = false;

            // 그래프 위에 마우스를 올렸을 때 현재 프레임 값을 작은 정보창으로 표시한다.
            lblGraphHover.AutoSize = true;
            lblGraphHover.BackColor = Color.FromArgb(40, 40, 40);
            lblGraphHover.ForeColor = Color.White;
            lblGraphHover.Font = new Font(SystemFonts.DefaultFont.FontFamily, 9, FontStyle.Bold);
            lblGraphHover.Padding = new Padding(8, 6, 8, 6);
            lblGraphHover.BorderStyle = BorderStyle.FixedSingle;
            lblGraphHover.Visible = false;
            picDataGraph.Controls.Add(lblGraphHover);
            lblGraphHover.BringToFront();

            graphLayout.Controls.Add(graphToolbar, 0, 0);
            graphLayout.Controls.Add(picDataGraph, 0, 1);
            tabGraph.Controls.Add(graphLayout);
            RedrawGraphAfterLayout();
        }

        private void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            RedrawGraphAfterLayout();
        }

        private void RedrawGraphAfterLayout()
        {
            if (tabMain.SelectedTab != tabGraph) return;
            BeginInvoke(new Action(RenderTubGraph));
        }

        private void RenderTubGraph()
        {
            if (picDataGraph.ClientSize.Width <= 0 || picDataGraph.ClientSize.Height <= 0) return;

            Bitmap bitmap = new Bitmap(picDataGraph.ClientSize.Width, picDataGraph.ClientSize.Height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);

            List<TubFrame> visibleFrames = tubFrames.Where(frame => !frame.Deleted).ToList();
            if (visibleFrames.Count == 0)
            {
                graphPlotBounds = Rectangle.Empty;
                graphVisibleFrames = new List<TubFrame>();
                lblGraphHover.Visible = false;
                DrawCenteredGraphMessage(graphics, bitmap.Size, "표시할 Tub 데이터가 없습니다.");
                SetGraphImage(bitmap);
                lblGraphSummary.Text = "프레임 0개";
                return;
            }

            Rectangle plot = new Rectangle(64, 24, Math.Max(10, bitmap.Width - 128), Math.Max(10, bitmap.Height - 72));
            graphPlotBounds = plot;
            graphVisibleFrames = visibleFrames;
            DrawGraphFrame(graphics, plot);

            // 프레임 수가 많아도 그래프 폭만큼만 샘플링해서 UI 렉을 줄인다.
            if (chkGraphAngle.Checked)
            {
                DrawGraphSeries(graphics, plot, visibleFrames, frame => frame.Angle, -1.0, 1.0, Color.RoyalBlue);
            }

            if (chkGraphThrottle.Checked)
            {
                DrawGraphSeries(graphics, plot, visibleFrames, frame => frame.Throttle, 0.0, 1.0, Color.SeaGreen);
            }

            DrawGraphLegend(graphics, plot);
            DrawGraphXAxis(graphics, plot, visibleFrames);
            SetGraphImage(bitmap);

            double avgAngle = visibleFrames.Average(frame => frame.Angle);
            double avgThrottle = visibleFrames.Average(frame => frame.Throttle);
            lblGraphSummary.Text = $"프레임 {visibleFrames.Count:N0}개  |  평균 조향각 {avgAngle:0.000}  |  평균 속도 {avgThrottle:0.000}";
        }

        private void SetGraphImage(Bitmap bitmap)
        {
            Image? oldImage = picDataGraph.Image;
            picDataGraph.Image = bitmap;
            oldImage?.Dispose();
        }

        private static void DrawGraphFrame(Graphics graphics, Rectangle plot)
        {
            using Pen axisPen = new Pen(Color.FromArgb(80, 80, 80));
            using Pen gridPen = new Pen(Color.FromArgb(225, 225, 225));
            using Brush textBrush = new SolidBrush(Color.FromArgb(70, 70, 70));
            using Font labelFont = new Font(SystemFonts.DefaultFont.FontFamily, 8);

            graphics.DrawRectangle(axisPen, plot);

            for (int i = 0; i <= 4; i++)
            {
                int y = plot.Top + (plot.Height * i / 4);
                graphics.DrawLine(gridPen, plot.Left, y, plot.Right, y);

                double angleValue = 1.0 - i * 0.5;
                double throttleValue = 1.0 - i * 0.25;
                graphics.DrawString(angleValue.ToString("0.00"), labelFont, textBrush, 8, y - 7);
                graphics.DrawString(throttleValue.ToString("0.00"), labelFont, textBrush, plot.Right + 10, y - 7);
            }

            graphics.DrawString("조향각", labelFont, Brushes.RoyalBlue, 12, plot.Top - 18);
            graphics.DrawString("속도", labelFont, Brushes.SeaGreen, plot.Right + 10, plot.Top - 18);
        }

        private static void DrawGraphSeries(
            Graphics graphics,
            Rectangle plot,
            List<TubFrame> frames,
            Func<TubFrame, double> valueSelector,
            double minValue,
            double maxValue,
            Color color)
        {
            if (frames.Count == 0) return;

            using Pen pen = new Pen(color, 2);
            int sampleCount = Math.Min(Math.Max(1, plot.Width), frames.Count);
            PointF[] points = new PointF[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                int frameIndex = sampleCount == 1
                    ? 0
                    : (int)Math.Round(i * (frames.Count - 1) / (double)(sampleCount - 1));

                double value = Math.Clamp(valueSelector(frames[frameIndex]), minValue, maxValue);
                float x = sampleCount == 1
                    ? plot.Left
                    : plot.Left + (float)(i * plot.Width / (double)(sampleCount - 1));
                float y = plot.Bottom - (float)((value - minValue) / (maxValue - minValue) * plot.Height);
                points[i] = new PointF(x, y);
            }

            if (points.Length == 1)
            {
                using Brush pointBrush = new SolidBrush(color);
                graphics.FillEllipse(pointBrush, points[0].X - 3, points[0].Y - 3, 6, 6);
                return;
            }

            graphics.DrawLines(pen, points);
        }

        private static void DrawGraphLegend(Graphics graphics, Rectangle plot)
        {
            using Font labelFont = new Font(SystemFonts.DefaultFont.FontFamily, 8);
            int x = plot.Left + 12;
            int y = plot.Top - 14;

            using Pen anglePen = new Pen(Color.RoyalBlue, 3);
            using Pen throttlePen = new Pen(Color.SeaGreen, 3);
            graphics.DrawLine(anglePen, x, y, x + 24, y);
            graphics.DrawString("조향각", labelFont, Brushes.Black, x + 30, y - 8);
            graphics.DrawLine(throttlePen, x + 112, y, x + 136, y);
            graphics.DrawString("속도", labelFont, Brushes.Black, x + 142, y - 8);
        }

        private static void DrawGraphXAxis(Graphics graphics, Rectangle plot, List<TubFrame> frames)
        {
            using Font labelFont = new Font(SystemFonts.DefaultFont.FontFamily, 8);
            using Brush textBrush = new SolidBrush(Color.FromArgb(70, 70, 70));

            string first = $"프레임 {frames.First().FrameNumber:D6}";
            string last = $"프레임 {frames.Last().FrameNumber:D6}";
            graphics.DrawString(first, labelFont, textBrush, plot.Left, plot.Bottom + 10);

            SizeF lastSize = graphics.MeasureString(last, labelFont);
            graphics.DrawString(last, labelFont, textBrush, plot.Right - lastSize.Width, plot.Bottom + 10);
        }

        private void picDataGraph_MouseMove(object? sender, MouseEventArgs e)
        {
            if (graphVisibleFrames.Count == 0 || graphPlotBounds == Rectangle.Empty || !graphPlotBounds.Contains(e.Location))
            {
                lblGraphHover.Visible = false;
                return;
            }

            double ratio = (e.X - graphPlotBounds.Left) / (double)Math.Max(1, graphPlotBounds.Width);
            int frameIndex = (int)Math.Round(ratio * (graphVisibleFrames.Count - 1));
            frameIndex = Math.Clamp(frameIndex, 0, graphVisibleFrames.Count - 1);

            TubFrame frame = graphVisibleFrames[frameIndex];
            lblGraphHover.Text = $"프레임 {frame.FrameNumber:D6}\n조향각 {frame.Angle:0.000}\n속도 {frame.Throttle:0.000}";

            int x = Math.Min(e.X + 14, picDataGraph.ClientSize.Width - lblGraphHover.Width - 8);
            int y = Math.Min(e.Y + 14, picDataGraph.ClientSize.Height - lblGraphHover.Height - 8);
            lblGraphHover.Location = new Point(Math.Max(8, x), Math.Max(8, y));
            lblGraphHover.Visible = true;
            lblGraphHover.BringToFront();
        }

        private static void DrawCenteredGraphMessage(Graphics graphics, Size size, string message)
        {
            using Font font = new Font(SystemFonts.DefaultFont.FontFamily, 11, FontStyle.Bold);
            SizeF textSize = graphics.MeasureString(message, font);
            float x = (size.Width - textSize.Width) / 2;
            float y = (size.Height - textSize.Height) / 2;
            graphics.DrawString(message, font, Brushes.DimGray, x, y);
        }

        private void AddLog(string message) => txtLog.AppendText(Environment.NewLine + message);
        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e) => ShowFrame(lstFrames.SelectedIndex);
        private void lvTimeline_SelectedIndexChanged(object sender, EventArgs e) { if (!isUpdatingTimelineSelection && lvTimeline.SelectedItems.Count > 0 && lvTimeline.SelectedItems[0].Tag is int index) ShowFrame(index); }
        private void trackFrame_Scroll(object sender, EventArgs e) => ShowFrame(trackFrame.Value);
        private void btnFirst_Click(object? sender, EventArgs e) { int i = FirstActiveIndex(); if (i >= 0) ShowFrame(i); }
        private void btnPrev_Click(object? sender, EventArgs e) { int i = PrevActiveIndex(trackFrame.Value); if (i >= 0) ShowFrame(i); }
        private void btnNext_Click(object? sender, EventArgs e) { int i = NextActiveIndex(trackFrame.Value); if (i >= 0) ShowFrame(i); }
        private void btnLast_Click(object? sender, EventArgs e) { int i = LastActiveIndex(); if (i >= 0) ShowFrame(i); }

        // 삭제된 프레임을 건너뛰는 탐색 도우미
        private int NextActiveIndex(int from)
        {
            for (int i = from + 1; i < tubFrames.Count; i++)
                if (!tubFrames[i].Deleted) return i;
            return -1;
        }

        private int AdvanceActiveIndex(int from, int frameStep)
        {
            // 고배속에서는 UI 타이머를 너무 촘촘하게 돌리지 않고, 한 번에 여러 유효 프레임을 진행해 렉을 줄인다.
            int current = from;
            int steps = Math.Max(1, frameStep);

            for (int i = 0; i < steps; i++)
            {
                int next = NextActiveIndex(current);
                if (next < 0) return i == 0 ? -1 : current;
                current = next;
            }

            return current;
        }

        private int PrevActiveIndex(int from)
        {
            for (int i = from - 1; i >= 0; i--)
                if (!tubFrames[i].Deleted) return i;
            return -1;
        }

        private int FirstActiveIndex()
        {
            for (int i = 0; i < tubFrames.Count; i++)
                if (!tubFrames[i].Deleted) return i;
            return -1;
        }

        private int LastActiveIndex()
        {
            for (int i = tubFrames.Count - 1; i >= 0; i--)
                if (!tubFrames[i].Deleted) return i;
            return -1;
        }

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
            RenderTubGraph();
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
                // 급변(스파이크): 직전·직후 유효 프레임 모두와 임계 이상 차이날 때만 판정(복귀 프레임 오검출 방지)
                if (!outlier && k > 0 && k < active.Count - 1)
                {
                    double prev = tubFrames[active[k - 1]].Angle;
                    double next = tubFrames[active[k + 1]].Angle;
                    if (Math.Abs(angle - prev) > jumpThreshold && Math.Abs(angle - next) > jumpThreshold) outlier = true;
                }

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
            RenderTubGraph();
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
            RenderTubGraph();
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
                "· catalog 파일은 백업 후 해당 기록을 제거합니다.\n" +
                "· 구버전 record JSON 파일은 [deleted] 폴더로 이동합니다.\n\n계속하시겠습니까?",
                "휴지통 비우기 (안전 모드)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.No) return;

            try
            {
                string deletedDir = Path.Combine(tubPath, "deleted");
                Directory.CreateDirectory(deletedDir);

                // 1) tub 폴더의 catalog 파일들을 훑어 삭제 대상 이미지 기록을 제외하고 재작성한다.
                //    (프레임의 원본 catalog를 따로 저장하지 않으므로 이미지 파일명으로 직접 찾는다. 변경된 catalog만 1회 백업)
                HashSet<string> removeImages = new HashSet<string>(deleted.Select(f => f.ImageFileName));
                foreach (string catalogFile in Directory.GetFiles(tubPath, "catalog_*.catalog", SearchOption.AllDirectories))
                {
                    bool removedAny = false;
                    List<string> keptLines = new List<string>();
                    foreach (string line in File.ReadLines(catalogFile))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string? imageName = TryGetImageName(line);
                        if (imageName != null && removeImages.Contains(imageName)) { removedAny = true; continue; }
                        keptLines.Add(line);
                    }
                    if (!removedAny) continue;

                    string backupPath = Path.Combine(deletedDir, Path.GetFileName(catalogFile) + ".backup");
                    if (!File.Exists(backupPath)) File.Copy(catalogFile, backupPath);
                    File.WriteAllLines(catalogFile, keptLines);
                }

                // 2) 구버전 Tub의 record_*.json 파일을 deleted 폴더로 이동한다.
                int movedRecords = 0;
                HashSet<string> recordFiles = deleted
                    .Select(f => f.SourceDataPath)
                    .Where(path => IsOldRecordFile(path) && File.Exists(path))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (string recordFile in recordFiles)
                {
                    string dest = GetDeletedFilePath(deletedDir, recordFile);
                    File.Move(recordFile, dest);
                    movedRecords++;
                }

                // 3) 이미지 파일을 deleted 폴더로 이동
                int movedImages = 0;
                foreach (TubFrame f in deleted)
                {
                    if (File.Exists(f.ImagePath))
                    {
                        string dest = GetDeletedFilePath(deletedDir, f.ImagePath);
                        File.Move(f.ImagePath, dest);
                        movedImages++;
                    }
                }

                // 4) 메모리 목록에서 제거 후 뷰 재구성
                tubFrames.RemoveAll(f => f.Deleted);
                missingImageFrames.Clear();
                picFrame.Image = null;
                frameImageCache.Clear();
                ResetTubView();
                lstTrash.Items.Clear();
                if (tubFrames.Count > 0) ShowFrame(0);
                RenderTubGraph();

                AddLog($"휴지통 비우기 완료: 기록 {deleted.Count}건 제거, record {movedRecords}개 이동, 이미지 {movedImages}개 이동 (백업 위치: {deletedDir}).");
                MessageBox.Show($"휴지통을 비웠습니다.\n\n제거된 기록: {deleted.Count}건\n이동된 record JSON: {movedRecords}개\n이동된 이미지: {movedImages}개\n백업 위치: {deletedDir}", "휴지통 비우기 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static bool IsOldRecordFile(string path)
        {
            // 구버전 Donkeycar Tub의 record JSON만 실제 파일 이동 대상으로 본다.
            return !string.IsNullOrWhiteSpace(path)
                && Path.GetFileName(path).StartsWith("record_", StringComparison.OrdinalIgnoreCase)
                && string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetDeletedFilePath(string deletedDir, string sourcePath)
        {
            // 복구할 수 있도록 deleted 폴더에 원본 파일명을 유지하되, 이름이 겹치면 번호를 붙인다.
            string fileName = Path.GetFileName(sourcePath);
            string dest = Path.Combine(deletedDir, fileName);
            if (!File.Exists(dest)) return dest;

            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            for (int i = 1; ; i++)
            {
                string candidate = Path.Combine(deletedDir, $"{name}_{i}{extension}");
                if (!File.Exists(candidate)) return candidate;
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
            public string SourceDataPath { get; set; } = "";
            public double Angle { get; set; }
            public double Throttle { get; set; }
            public bool Deleted { get; set; }
            public string DeleteReason { get; set; } = "";
            public override string ToString() => Deleted ? $"Frame {FrameNumber:D6} [삭제됨]" : $"Frame {FrameNumber:D6}";
        }

        private sealed class TubLoadResult
        {
            public List<TubFrame> Frames { get; } = new();
            public List<string> MissingImages { get; } = new();
            public List<string> Errors { get; } = new();
        }

        // 휴지통 목록(lstTrash)에 표시되는 삭제 프레임 항목. 복원 시 원본 프레임으로 역매핑한다.
        private sealed class TrashEntry
        {
            public TubFrame Frame { get; }
            public TrashEntry(TubFrame frame) => Frame = frame;
            public override string ToString() => $"Frame {Frame.FrameNumber:D6} · {Frame.DeleteReason}";
        }

        // 표시용 이미지 LRU 캐시. 용량을 초과하면 가장 오래 전 사용한 이미지를 Dispose하여 메모리를 제한한다.
        private sealed class FrameImageCache
        {
            private readonly int capacity;
            private readonly Func<string, Image> loader;
            private readonly Dictionary<string, Image> map = new();
            private readonly LinkedList<string> order = new(); // 앞쪽이 최근 사용

            public FrameImageCache(int capacity, Func<string, Image> loader)
            {
                this.capacity = Math.Max(1, capacity);
                this.loader = loader;
            }

            public Image Get(string key)
            {
                if (map.TryGetValue(key, out Image? cached))
                {
                    order.Remove(key);
                    order.AddFirst(key);
                    return cached;
                }

                Image image = loader(key);
                map[key] = image;
                order.AddFirst(key);

                while (order.Count > capacity)
                {
                    string oldest = order.Last!.Value;
                    order.RemoveLast();
                    if (map.Remove(oldest, out Image? evicted)) evicted.Dispose();
                }
                return image;
            }

            public void Clear()
            {
                foreach (Image image in map.Values) image.Dispose();
                map.Clear();
                order.Clear();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBoxDataLoad_Enter(object sender, EventArgs e)
        {

        }

        private void cmbPlaySpeed_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyPlaybackSpeed();
        }

        private void SetPlaybackState(bool shouldPlay)
        {
            bool isPlaying = shouldPlay && StartPlayback();
            if (!isPlaying) StopPlayback();
            UpdatePlaybackControlsVisual(isPlaying);
        }

        private bool StartPlayback()
        {
            if (tubFrames.Count == 0) return false;

            int firstActive = FirstActiveIndex();
            if (firstActive < 0) return false;

            // 마지막 프레임에서 재생 버튼을 누르면 처음 유효 프레임부터 다시 시작한다.
            if (NextActiveIndex(trackFrame.Value) < 0)
            {
                ShowFrame(firstActive);
            }

            ApplyPlaybackSpeed();
            playTimer.Start();
            return true;
        }

        private void StopPlayback()
        {
            playTimer.Stop();
        }

        private void ApplyPlaybackSpeed()
        {
            int speed = GetSelectedPlaybackSpeed();
            double targetInterval = PlaybackBaseIntervalMs / (double)speed;

            playTimer.Interval = Math.Max(PlaybackMinimumIntervalMs, (int)Math.Round(targetInterval));
            playbackFrameStep = Math.Max(1, (int)Math.Ceiling(playTimer.Interval / targetInterval));
        }

        private int GetSelectedPlaybackSpeed()
        {
            string speedText = cmbPlaySpeed.SelectedItem?.ToString() ?? "1x";
            speedText = speedText.Trim().TrimEnd('x', 'X');
            return int.TryParse(speedText, out int speed) ? Math.Clamp(speed, 1, 8) : 1;
        }

        private void UpdatePlaybackControlsVisual(bool isPlaying)
        {
            btnPlayStop.Text = isPlaying ? "정지" : "재생";
            btnPlayStop.Image = isPlaying ? Properties.Resources.icons8_stop_30 : Properties.Resources.icons8_play_30;
        }

        private void UpdateAutoPlayLoopVisual()
        {
            chkAutoPlay.BackColor = chkAutoPlay.Checked ? Color.FromArgb(59, 130, 246) : Color.White;
            chkAutoPlay.ForeColor = chkAutoPlay.Checked ? Color.White : Color.Black;
        }

        private void chkAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAutoPlayLoopVisual();
        }
    }
}
