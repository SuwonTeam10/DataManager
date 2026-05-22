using System.Text.Json;
using static DataManager.ICE;
using static DataManager.ICE.RemoteExecutor;
using System.Text.RegularExpressions;

namespace DataManager
{
    public partial class Form1 : Form
    {
        private ICE.ICommandExecutor _executor; 

        // 1. 학습 버튼 클릭 시
        private void btn_Train_Click(object sender, EventArgs e)
        {
            if (_executor == null)
            {
                MessageBox.Show("먼저 로컬/원격 연결 설정을 완료해주세요!", "설정 필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(configPath))
            {
                MessageBox.Show("Donkeycar 프로젝트 폴더(Config)를 먼저 로드해주세요!", "폴더 누락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _executor.ExecuteTrain(configPath, (log) =>
            {
                this.Invoke(new Action(() =>
                {
                    UpdateChartRealTime(log);
                }));
            });
        }

        // 2. 빠져있던 차트 업데이트 함수
        private void UpdateChartRealTime(string logText)
        {
            if (string.IsNullOrEmpty(logText)) return;

            txtLog.AppendText(logText + Environment.NewLine);

            string pattern = @"loss:\s*([0-9]*\.?[0-9]+)";
            Match match = Regex.Match(logText, pattern);

            if (match.Success)
            {
                double lossValue = Convert.ToDouble(match.Groups[1].Value);
                
                // <최인성> UI님, loss값 차트 이름 뭐에요? 못 찾았습니다,,,
                // chartLoss.Series["Loss"].Points.AddY(lossValue);
            }
        }

        // 3. 로컬 라디오 버튼을 클릭했을 때 (기본값)
        // (디자인 창에서 로컬 라디오 버튼 더블클릭 후 연결하세요)
        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoLocal.Checked)
            {
                _executor = new ICE.LocalExecutor();
            }
        }

        // 4. 원격 라디오 버튼을 클릭했을 때
        // (디자인 창에서 원격 라디오 버튼 더블클릭 후 연결하세요)
        private void rbRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // 로그인 성공 시 원격 모드 셋팅
                        _executor = new ICE.RemoteExecutor(loginForm.Host, loginForm.User, loginForm.Pass);
                    }
                    else
                    {
                        // 로그인 창 닫으면 오류 없이 자동으로 로컬 버튼으로 원복됨!
                        rdoLocal.Checked = true;
                    }
                }
            }
        }

        // 1. 로컬 라디오 버튼 상태 변경 시 (로컬 함수)
        private void rdoLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoLocal.Checked)
            {
                _executor = new ICE.LocalExecutor();
            }
        }

        // 2. 원격 라디오 버튼 상태 변경 시 (원격 함수 & 로그인 창 호출)
        // Form1.cs 내부의 원격 라디오버튼 로직 업데이트
        private void rdoRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // 연결 성공! 실행기 만들고 프로필 라벨 업데이트
                        _executor = new ICE.RemoteExecutor(loginForm.Host, loginForm.User, loginForm.Pass);

                        lblProfile.Text = $"👤 {loginForm.User} 접속중"; // 아이디 표시
                        lblProfile.Tag = loginForm.Host; // Tag에 IP를 숨겨둡니다.
                        lblProfile.ForeColor = Color.Blue; // 접속된 느낌으로 파란색!
                        lblProfile.Cursor = Cursors.Hand; // 마우스 올리면 손가락 모양
                    }
                    else
                    {
                        rdoLocal.Checked = true;
                        lblProfile.Text = "로컬 모드";
                        lblProfile.ForeColor = Color.Black;
                    }
                }
            }
        }

        // 디자인 창에서 lblProfile을 더블클릭해서 연결하세요! (로그아웃 기능)
        private void lblProfile_Click(object sender, EventArgs e)
        {
            // 원격 접속 중일 때만 반응
            if (rdoRemote.Checked)
            {
                string ip = lblProfile.Tag?.ToString(); // 숨겨둔 IP 꺼내기

                DialogResult result = MessageBox.Show(
                    $"현재 접속 정보\n- IP: {ip}\n\n로그아웃하고 로컬 모드로 전환하시겠습니까?",
                    "로그아웃",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Yes 누르면 강제로 로컬 라디오버튼 체크 -> 자동으로 연결 끊기고 복구됨!
                    rdoLocal.Checked = true;
                    lblProfile.Text = "로컬 모드";
                    lblProfile.ForeColor = Color.Black;
                    lblProfile.Cursor = Cursors.Default;
                }
            }
        }

        // catalog 한 줄에서 읽어온 Donkeycar 프레임 정보를 저장한다.
        private sealed class TubFrame
        {
            public int FrameNumber { get; set; }
            public string ImageFileName { get; set; } = "";
            public string ImagePath { get; set; } = "";
            public double Angle { get; set; }
            public double Throttle { get; set; }

            public override string ToString()
            {
                return $"Frame {FrameNumber:D6}";
            }
        }

        // 백그라운드에서 Tub 데이터를 읽은 뒤 UI 스레드로 전달할 결과를 저장한다.
        private sealed class TubLoadResult
        {
            public List<TubFrame> Frames { get; } = new();
            public List<string> Errors { get; } = new();
        }

        private string configPath = "";
        private string tubPath = "";
        private readonly List<TubFrame> tubFrames = new();
        private readonly HashSet<int> missingImageFrames = new();
        private readonly ImageList timelineImages = new();
        // 타임라인은 성능을 위해 현재 구간의 연속 20장만 썸네일로 표시한다.
        private const int TimelineVisibleCount = 20;
        private int currentTimelineStart = -1;
        private bool isUpdatingTimelineSelection;

        public Form1()
        {
            InitializeComponent();

            // <추가-최인성> 라디오버튼: 프로그램 시작 시 실행기를 '로컬'로 기본 세팅 (튕김 방지)
            _executor = new ICE.LocalExecutor();
            // 라디오 버튼 연결
            rdoLocal.CheckedChanged += rdoLocal_CheckedChanged;
            rdoRemote.CheckedChanged += rdoRemote_CheckedChanged;

            btnReloadTub.Click += btnReloadTub_Click;
            lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged;
            lvTimeline.SelectedIndexChanged += lvTimeline_SelectedIndexChanged;
            trackFrame.Scroll += trackFrame_Scroll;
            btnFirst.Click += btnFirst_Click;
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
            btnLast.Click += btnLast_Click;

            // 타임라인 썸네일 표시용 ImageList를 설정한다.
            timelineImages.ImageSize = new Size(36, 27);
            timelineImages.ColorDepth = ColorDepth.Depth32Bit;
            lvTimeline.LargeImageList = timelineImages;
            lvTimeline.View = View.LargeIcon;
            lvTimeline.HideSelection = false;
            lvTimeline.MultiSelect = false;
            lvTimeline.ShowItemToolTips = true;
        }

        private void lblConfigPath_Click(object sender, EventArgs e)
        {

        }

        private void groupTubNavigator_Enter(object sender, EventArgs e)
        {

        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            // Donkeycar 프로젝트 폴더를 선택하고 manage.py 존재 여부로 유효성을 확인한다.
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Donkeycar 프로젝트 폴더를 선택하세요.";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(Path.Combine(dlg.SelectedPath, "manage.py")))
                    {
                        MessageBox.Show("manage.py 파일이 없는 폴더입니다.", "Config Loader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    configPath = dlg.SelectedPath; // 선택한 프로젝트 폴더 경로 저장
                    lblConfigPath.Text = configPath; // 선택 경로를 화면에 표시
                }
            }
        }

        private async void btnLoadTub_Click(object? sender, EventArgs e)
        {
            // 사용자가 선택한 Tub 폴더에서 catalog와 프레임 데이터를 불러온다.
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Donkeycar tub 데이터 폴더를 선택하세요.";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    await LoadTubAsync(dlg.SelectedPath);
                }
            }
        }

        private async void btnReloadTub_Click(object? sender, EventArgs e)
        {
            // 마지막으로 불러온 Tub 폴더를 다시 읽어 화면 데이터를 갱신한다.
            if (string.IsNullOrWhiteSpace(tubPath))
            {
                MessageBox.Show("먼저 Tub 폴더를 선택하세요.", "Load Tub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await LoadTubAsync(tubPath);
        }

        private async Task LoadTubAsync(string selectedTubPath)
        {
            // catalog 파일이 많은 Tub도 UI가 멈추지 않도록 실제 읽기는 백그라운드에서 처리한다.
            string[] catalogFiles = Directory.GetFiles(selectedTubPath, "catalog_*.catalog")
                .OrderBy(file => file)
                .ToArray();

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

            try
            {
                // catalog 파싱은 시간이 걸릴 수 있으므로 UI 스레드 밖에서 실행한다.
                TubLoadResult result = await Task.Run(() => ReadTubFrames(selectedTubPath, catalogFiles));

                tubFrames.AddRange(result.Frames);

                ResetTubView();

                if (tubFrames.Count > 0)
                {
                    ShowFrame(0);
                }

                foreach (string error in result.Errors)
                {
                    AddLog(error);
                }

                AddLog($"Load Tub 완료: {tubFrames.Count}개 프레임");
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
            // catalog JSON Lines를 읽어 이미지 파일명, angle, throttle을 프레임 목록으로 변환한다.
            TubLoadResult result = new TubLoadResult();
            string imageBasePath = GetImageBasePath(selectedTubPath);

            foreach (string catalogFile in catalogFiles)
            {
                foreach (string line in File.ReadLines(catalogFile))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        using JsonDocument document = JsonDocument.Parse(line);
                        JsonElement root = document.RootElement;
                        string imageFileName = GetStringValue(root, "cam/image_array");

                        if (string.IsNullOrWhiteSpace(imageFileName))
                        {
                            continue;
                        }

                        TubFrame frame = new TubFrame
                        {
                            FrameNumber = GetIntValue(root, "_index", result.Frames.Count),
                            ImageFileName = imageFileName,
                            ImagePath = FindImagePath(selectedTubPath, imageBasePath, imageFileName),
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

        private void ResetTubView()
        {
            // Tub 로딩 후 프레임 목록과 TrackBar 범위를 초기화한다.
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

                for (int i = 0; i < tubFrames.Count; i++)
                {
                    TubFrame frame = tubFrames[i];
                    frameItems[i] = frame;
                }

                lstFrames.Items.AddRange(frameItems);
            }
            finally
            {
                lstFrames.EndUpdate();
            }
        }

        private void UpdateTimelineForFrame(int frameIndex)
        {
            // 현재 프레임이 포함된 연속 20장 구간만 썸네일 타임라인에 표시한다.
            int timelineStart = (frameIndex / TimelineVisibleCount) * TimelineVisibleCount;
            if (timelineStart == currentTimelineStart)
            {
                return;
            }

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
                    lvTimeline.Items.Add(new ListViewItem("", imageKey)
                    {
                        Tag = i,
                        ToolTipText = frame.ToString()
                    });
                }
            }
            finally
            {
                lvTimeline.EndUpdate();
            }
        }

        private void ShowFrame(int index)
        {
            // 선택한 프레임의 이미지, 조향각, 속도, 타임라인 선택 상태를 화면에 반영한다.
            if (index < 0 || index >= tubFrames.Count)
            {
                return;
            }

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

            if (!File.Exists(frame.ImagePath) && missingImageFrames.Add(index))
            {
                // 이미지가 없으면 중복 등록을 막고 휴지통 목록에 누락 프레임을 표시한다.
                lstTrash.Items.Add($"{frame}: missing image");
            }

            lblFrame.Text = $"프레임: {frame.FrameNumber:D6}";
            lblAngle.Text = $"조향각: {frame.Angle:0.00}";
            lblThrottle.Text = $"속도: {frame.Throttle:0.00}";
        }

        private static string GetImageBasePath(string selectedTubPath)
        {
            // Donkeycar Tub에서 이미지 폴더 이름이 다를 수 있어 가능한 기본 위치를 찾는다.
            string imagesPath = Path.Combine(selectedTubPath, "images");
            if (Directory.Exists(imagesPath))
            {
                return imagesPath;
            }

            string imageArrayPath = Path.Combine(selectedTubPath, "image_array");
            if (Directory.Exists(imageArrayPath))
            {
                return imageArrayPath;
            }

            return selectedTubPath;
        }

        private static string FindImagePath(string selectedTubPath, string imageBasePath, string imageFileName)
        {
            // 파일명 순서가 아니라 catalog의 cam/image_array 값을 기준으로 이미지 경로를 만든다.
            if (Path.IsPathRooted(imageFileName))
            {
                return imageFileName;
            }

            string normalizedImageFileName = imageFileName.Replace('/', Path.DirectorySeparatorChar);
            if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(normalizedImageFileName)))
            {
                return Path.Combine(selectedTubPath, normalizedImageFileName);
            }

            return Path.Combine(imageBasePath, normalizedImageFileName);
        }

        private static Image LoadImage(string imagePath)
        {
            // PictureBox에 표시할 현재 프레임 이미지를 로드한다.
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
            // 타임라인에 표시할 작은 썸네일 이미지를 생성한다.
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
            // catalog JSON에서 문자열 값을 안전하게 가져온다.
            if (!root.TryGetProperty(propertyName, out JsonElement value))
            {
                return "";
            }

            return value.ValueKind == JsonValueKind.String ? value.GetString() ?? "" : value.ToString();
        }

        private static int GetIntValue(JsonElement root, string propertyName, int defaultValue)
        {
            // catalog JSON에서 정수 값을 안전하게 가져온다.
            if (!root.TryGetProperty(propertyName, out JsonElement value))
            {
                return defaultValue;
            }

            if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out int result))
            {
                return result;
            }

            return int.TryParse(value.ToString(), out result) ? result : defaultValue;
        }

        private static double GetDoubleValue(JsonElement root, string propertyName)
        {
            // catalog JSON에서 실수 값을 안전하게 가져온다.
            if (!root.TryGetProperty(propertyName, out JsonElement value))
            {
                return 0;
            }

            if (value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out double result))
            {
                return result;
            }

            return double.TryParse(value.ToString(), out result) ? result : 0;
        }

        private void AddLog(string message)
        {
            // Train/Test 탭의 로그 창에 작업 상태를 누적 출력한다.
            txtLog.AppendText(Environment.NewLine + message);
        }

        private void lstFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // 프레임 목록에서 선택한 항목으로 현재 프레임을 이동한다.
            ShowFrame(lstFrames.SelectedIndex);
        }

        private void lvTimeline_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // 타임라인 썸네일을 클릭하면 해당 프레임으로 이동한다.
            if (isUpdatingTimelineSelection)
            {
                return;
            }

            if (lvTimeline.SelectedItems.Count == 0)
            {
                return;
            }

            if (lvTimeline.SelectedItems[0].Tag is int index)
            {
                ShowFrame(index);
            }
        }

        private void trackFrame_Scroll(object? sender, EventArgs e)
        {
            // TrackBar 위치에 맞춰 현재 프레임을 이동한다.
            ShowFrame(trackFrame.Value);
        }

        private void btnFirst_Click(object? sender, EventArgs e)
        {
            // 첫 번째 프레임으로 이동한다.
            ShowFrame(0);
        }

        private void btnPrev_Click(object? sender, EventArgs e)
        {
            // 이전 프레임으로 한 칸 이동한다.
            ShowFrame(Math.Max(0, trackFrame.Value - 1));
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            // 다음 프레임으로 한 칸 이동한다.
            ShowFrame(Math.Min(tubFrames.Count - 1, trackFrame.Value + 1));
        }

        private void btnLast_Click(object? sender, EventArgs e)
        {
            // 마지막 프레임으로 이동한다.
            ShowFrame(tubFrames.Count - 1);
        }

        private void lvTimeline_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
