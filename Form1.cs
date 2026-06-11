using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DataManager.ICE;
using static DataManager.ICE.RemoteExecutor;

namespace DataManager
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys key);

        // ==========================================
        // 전역 변수 선언
        // ==========================================
        private ICE.ICommandExecutor _executor;
        private string modelPath = "";
        private string testImagePath = "";
        private string latestTestImagePath = "";
        private const int ModelTestPreviewUpdateIntervalMs = 5000;
        private long lastTestPreviewUpdateTick;
        private readonly ConcurrentQueue<string> modelTestLogQueue = new();
        private readonly System.Windows.Forms.Timer modelTestLogTimer = new();
        private const int ModelTestLogUiIntervalMs = 50;
        private const int ModelTestLogMaxPerTick = 10;
        private bool isModelTestRunning;
        private double? latestTestRealAngle;
        private double? latestTestPredictAngle;
        private readonly Dictionary<string, double> predictedAnglesByImageKey = new(StringComparer.OrdinalIgnoreCase);
        private string loggedInUser = "";
        private System.Windows.Forms.Timer playTimer;
        private bool _isUpdatingRadio = false;
        private ToolStripMenuItem menuProfile;
        private int _trainCount = 0;
        private int _testCount = 0;
        private ToolStripMenuItem infoTrain;
        private ToolStripMenuItem infoTest;
        private ToolStripMenuItem menuAttempts;
        private System.Text.StringBuilder _originalLogBuilder = new System.Text.StringBuilder();
        private System.Text.StringBuilder _summaryLogBuilder = new System.Text.StringBuilder();

        private string configPath = "";
        private string tubPath = "";
        private readonly List<TubFrame> tubFrames = new();
        private readonly HashSet<int> missingImageFrames = new();
        // 디스크 휴지통 보관 목록(deleted/trash.jsonl 미러). 삭제된 레코드는 tubFrames에서 빠지고 여기에만 존재한다.
        private readonly List<TrashEntry> trashStore = new();
        private readonly ImageList timelineImages = new();
        private readonly TimelineThumbnailCache timelineThumbnailCache;
        private const int TimelineMinimumVisibleCount = 20;
        private const int TimelineMaximumVisibleCount = 60;
        private const int TimelineThumbWidth = 140;
        private const int TimelineThumbHeight = 105;
        private const int TimelineIconGap = 0;
        private Size currentTimelineThumbSize = new(TimelineThumbWidth, TimelineThumbHeight);
        private int currentTimelineIconSpacingX = TimelineThumbWidth + TimelineIconGap;
        private int currentTimelineIconSpacingY = TimelineThumbHeight + TimelineIconGap;
        private const int PlaybackBaseIntervalMs = 100;
        private const int PlaybackMinimumIntervalMs = 50;
        private int currentTimelineStart = -1;
        private int currentTimelineVisibleCount = -1;
        private bool isUpdatingTimelineSelection;
        private bool isTimelineRangeDragging;
        private bool isLoadingTimeline;
        private bool hasTimelineRangeDragMoved;
        private int timelineDragStartIndex = -1;
        private int timelineDragCurrentIndex = -1;
        private int timelineDragEdgeDirection;
        private readonly System.Windows.Forms.Timer timelineDragTimer = new();
        private readonly System.Windows.Forms.Timer keyboardFrameMoveTimer = new();
        private readonly System.Windows.Forms.Timer deferredFrameUiTimer = new();
        private const int DeferredFrameUiUpdateIntervalMs = 30;
        private int deferredFrameIndex = -1;
        private int deferredPreviousFrameIndex = -1;
        private bool deferredSyncFrameListSelection;
        private bool deferredSyncTimelineSelection;
        private bool deferredUpdateTimelineWindow;
        private bool isFrameListMouseDragging;
        private int lastFrameListPreviewIndex = -1;
        private int playbackFrameStep = 1;
        private const int FrameImagePrefetchCount = 8;
        private const int PlaybackFrameImagePrefetchCount = 16;
        private CancellationTokenSource? frameImagePrefetchCts;
        private readonly object frameImagePrefetchLock = new();
        private bool isEnterSelectingFrames;
        private int keyboardFrameMoveDirection;
        private readonly SortedSet<int> selectedFrames = new();   // 공유 선택(절대 tubFrames 인덱스) - 모든 선택 기능의 단일 진실원
        private int pendingRangeAnchor = -1;                       // 시작 지정 앵커(-1=없음)
        private SortedSet<int>? dragBaseSelection = null;          // 드래그 시작 시 선택 스냅샷(타임라인·목록 공용)
        private SelectionSnapshot? pendingSelectionUndoSnapshot;    // 드래그/엔터 선택 묶음 시작 전 상태
        private readonly Stack<UndoAction> undoStack = new();       // Ctrl+Z 마지막 작업 취소
        private bool isApplyingUndo;                               // undo 실행 중 재기록 방지
        private bool isRefreshingSelectionVisuals = false;         // 선택 시각화 재진입 가드
        private int frameListDragStartIndex = -1;                  // 목록 드래그 앵커
        private int selectionAnchorIndex = -1;                     // Shift+클릭 범위 기준 앵커
        private Keys frameListDownModifiers = Keys.None;           // 목록 클릭 시 Shift/Ctrl 상태
        private Keys timelineDownModifiers = Keys.None;            // 타임라인 클릭 시 Shift/Ctrl 상태
        private const int DefaultNavigatorHeight = 827;     //
        private const int MinNavigatorHeight = 360;
        private const int TimelinePanelHeight = 168;    //
        private const int TimelineMinimumPanelHeight = 42;
        private const int TrainingLossMinimumPanelHeight = 130;
        private const int DefaultTabPanelHeight = 314;  //
        private const int MinimumTabVisibleHeight = 130;
        private const int LayoutGap = 8;
        private const int FrameResizeBarHeight = 0;
        private const int FrameResizeGripHeight = 12;
        private int navigatorHeight = DefaultNavigatorHeight;
        private bool isResizingNavigator;
        private bool suppressNextFrameClick;
        private int resizeStartMouseY;
        private int resizeStartNavigatorHeight;
        private Rectangle resizeStartNavigatorBounds;
        private Rectangle resizeStartTimelineBounds;
        private Rectangle resizeStartTabBounds;
        private int frameDragMaxTimelineHeight = -1;
        private int frameDragMaxTabHeight = -1;
        private const int MinimumResponsiveFormWidth = 1320;
        private const int LeftDataPanelGap = 12;
        private const int MinimumDataInfoPanelHeight = 264;

        private const int DesignBaseWidth = 2510;
        private const int DesignBaseHeight = 1592;
        private const int ResponsiveGap = 8;

        // ==========================================
        // 그룹박스 내부 컨트롤 반응형 스케일링용
        // (100%, 125%, 150%, 200% 배율에서
        // 그룹박스 크기에 맞춰 내부 컨트롤도 비율 조정)
        // ==========================================
        private readonly Dictionary<Control, Rectangle> _baseBounds = new();
        private readonly Dictionary<Control, Size> _baseClientSizes = new();
        private readonly Dictionary<Control, float> _baseFontSizes = new();
        private bool _baseLayoutCaptured = false;



        // 자동 재생 성능을 위한 표시용 이미지 LRU 캐시(메모리 상한 적용). 캐시가 Bitmap 소유권을 가진다.
        private readonly FrameImageCache frameImageCache;
        // 원본 버튼 이미지와 생성된 스케일된 이미지를 관리하여 다양한 DPI/해상도에서 아이콘 크기를 조절
        private readonly Dictionary<Button, Image> _origButtonImages = new();
        private readonly Dictionary<(Button button, string key), Image> _scaledButtonVariants = new();

        // 그래프 탭 컨트롤은 디자이너 충돌을 줄이기 위해 런타임에 생성한다.
        private readonly PictureBox picDataGraph = new();
        private readonly Button btnReloadGraph = new();
        private readonly CheckBox chkGraphAngle = new();
        private readonly CheckBox chkGraphThrottle = new();
        private readonly Label lblGraphSummary = new();
        private readonly Label lblGraphHover = new();
        private Rectangle graphPlotBounds = Rectangle.Empty;
        private List<TubFrame> graphVisibleFrames = new();
        private readonly Panel panelTrainingLoss = new();
        private readonly Label lblTrainingLossSummary = new();
        private readonly Label lblTrainingLossHover = new();
        private readonly PictureBox picTrainingLossGraph = new();
        private readonly List<TrainingLossPoint> trainingLossPoints = new();
        private Rectangle trainingLossPlotBounds = Rectangle.Empty;
        private bool isLayingOutLogPanels = false;
        // ==========================================
        // 1. 초기화 및 생성자
        // ==========================================
        public Form1()
        {
            InitializeComponent();

            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;

            // 프로그램 시작 시 기본 실행기를 로컬로 세팅
            _executor = new ICE.LocalExecutor();

            // 라디오 버튼 및 타이머 연결 (디자이너와 겹치지 않는 특수 이벤트만 유지)
            rdoLocal.CheckedChanged += rdoLocal_CheckedChanged;
            rdoRemote.CheckedChanged += rdoRemote_CheckedChanged;

            playTimer = new System.Windows.Forms.Timer();
            playTimer.Interval = 100; // 0.1초 간격
            playTimer.Tick += PlayTimer_Tick;

            keyboardFrameMoveTimer.Interval = 70;
            keyboardFrameMoveTimer.Tick += KeyboardFrameMoveTimer_Tick;

            deferredFrameUiTimer.Interval = DeferredFrameUiUpdateIntervalMs;
            deferredFrameUiTimer.Tick += DeferredFrameUiTimer_Tick;

            modelTestLogTimer.Interval = ModelTestLogUiIntervalMs;
            modelTestLogTimer.Tick += ModelTestLogTimer_Tick;

            // 표시용 이미지 캐시 초기화 (최근 64프레임 유지)
            frameImageCache = new FrameImageCache(64, LoadImage);
            timelineThumbnailCache = new TimelineThumbnailCache(240, CreateTimelineThumbnail);

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
            Resize += (_, _) =>
            {
                ApplyResponsiveMainLayout();
                RedrawGraphAfterLayout();
            };
            picFrame.Paint += picFrame_Paint;
            picFrame.MouseDown += picFrame_MouseDown;
            picFrame.MouseMove += picFrame_MouseMove;
            picFrame.MouseLeave += picFrame_MouseLeave;
            picFrame.MouseUp += picFrame_MouseUp;
            chkShowRealAngle.CheckedChanged += (_, _) => picFrame.Invalidate();
            chkShowPredictAngle.CheckedChanged += (_, _) => picFrame.Invalidate();

            // 데이터 정리(범위 지정/필터/삭제/휴지통) 이벤트 연결
            btnSetLeft.Click += btnSetLeft_Click;
            btnSetRight.Click += btnSetRight_Click;
            btnFilter.Click += btnFilter_Click;
            btnDelete.Click += btnDelete_Click;
            btnRestore.Click += btnRestore_Click;
            btnEmptyTrash.Click += btnEmptyTrash_Click;
            btnStopTask.Click += btnStopTask_Click;
            toolTip1.SetToolTip(btnDelete, "선택/범위 프레임을 휴지통으로 보냅니다. 학습에서 즉시 제외되며, [복원]으로 되돌릴 수 있습니다.");
            toolTip1.SetToolTip(btnRestore, "체크한 프레임을 휴지통에서 꺼내 다시 학습에 포함합니다.");
            toolTip1.SetToolTip(btnEmptyTrash, "휴지통의 프레임을 완전히 삭제합니다. 되돌릴 수 없습니다(원본 복사본은 deleted 폴더에 백업).");
            SetupFilterTooltips();
            lstFrames.SelectionMode = SelectionMode.MultiExtended;
            lstFrames.DrawMode = DrawMode.OwnerDrawFixed;
            lstFrames.DrawItem += lstFrames_DrawItem;
            lstFrames.MouseDown += lstFrames_MouseDown;
            lstFrames.MouseMove += lstFrames_MouseMove;
            lstFrames.MouseUp += lstFrames_MouseUp;
            lstTrash.CheckOnClick = false;
            lstTrash.MouseDown += LstTrash_MouseDown;
            lstTrash.MouseMove += LstTrash_MouseMove;
            lstTrash.MouseUp += LstTrash_MouseUp;
            lstTrash.SelectedIndexChanged += LstTrash_SelectedIndexChanged;
            tabCleaner.Resize += (_, _) => ArrangeCleanerControls();

            // 타임라인 썸네일 이미지 리스트 설정
            timelineImages.ImageSize = new Size(TimelineThumbWidth, TimelineThumbHeight);
            timelineImages.ColorDepth = ColorDepth.Depth32Bit;
            lvTimeline.LargeImageList = timelineImages;
            lvTimeline.View = View.LargeIcon;
            lvTimeline.Alignment = ListViewAlignment.Left;
            lvTimeline.AutoArrange = false;
            lvTimeline.LabelWrap = false;
            lvTimeline.Scrollable = false;
            lvTimeline.HideSelection = false;
            lvTimeline.MultiSelect = true;
            lvTimeline.ShowItemToolTips = true;
            lvTimeline.OwnerDraw = true;
            lvTimeline.DrawItem += lvTimeline_DrawItem;
            lvTimeline.HandleCreated += (_, _) => ApplyTimelineIconSpacing();
            lvTimeline.Resize += (_, _) =>
            {
                if (!isLoadingTimeline) ReloadTimelineForCurrentFrame();
            };
            lvTimeline.MouseDown += lvTimeline_MouseDown;
            lvTimeline.MouseMove += lvTimeline_MouseMove;
            lvTimeline.MouseUp += lvTimeline_MouseUp;
            lvTimeline.MouseLeave += lvTimeline_MouseLeave;
            ApplyTimelineIconSpacing();

            timelineDragTimer.Interval = 120;
            timelineDragTimer.Tick += timelineDragTimer_Tick;

            InitializeTrainingLossOverlay();
            tabAiCompile.Resize += (_, _) => LayoutImageTestTabControls();
            InitializeGraphControls();
            picTestImage.SizeMode = PictureBoxSizeMode.Zoom;
            MinimumSize = new Size(Math.Max(MinimumSize.Width, MinimumResponsiveFormWidth), MinimumSize.Height);
            groupTubNavigator.Resize += (_, _) => ArrangeNavigatorControls();
            Shown += (_, _) =>
            {
                // 실행 직후 기본 높이만 데이터 정보 판넬 아래와 맞춘다.
                navigatorHeight = Math.Max(MinNavigatorHeight, groupDataView.Bottom - groupTubNavigator.Top);

                ApplyResponsiveMainLayout();
                ArrangeCleanerControls();
                LayoutTrainingLossOverlay();
                LayoutImageTestTabControls();

                // 버튼 이미지 자동 스케일링 등록
                RegisterScaledButton(btnTrain);
                RegisterScaledButton(btnStopTask);
                RegisterScaledButton(btnDisconnect);
                RegisterScaledButton(btnPlayStop);

                // 폼 크기 변경 또는 DPI 변경 시 스케일 갱신
                this.Resize += (_, _) => UpdateAllButtonImagesScale();
                this.DpiChanged += (_, _) => UpdateAllButtonImagesScale();
            };

            CaptureBaseLayouts();

            ApplyResponsiveMainLayout();
            ArrangeCleanerControls();
            LayoutTrainingLossOverlay();
            LayoutImageTestTabControls();
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isEnterSelectingFrames = false;
                StopKeyboardFrameMoveTimer();
                CommitPendingSelectionUndo();
            }
            else if (e.KeyCode is Keys.Left or Keys.Right or Keys.A or Keys.D)
            {
                UpdateKeyboardFrameMoveTimerFromHeldKeys();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (HasFocusedTextInput(this)) return base.ProcessCmdKey(ref msg, keyData);

            Keys keyCode = keyData & Keys.KeyCode;
            Keys modifiers = keyData & Keys.Modifiers;
            if (keyCode == Keys.Z && modifiers == Keys.Control)
            {
                UndoLastAction();
                return true;
            }

            if (modifiers != Keys.None) return base.ProcessCmdKey(ref msg, keyData);

            if (keyCode is Keys.Left or Keys.A)
            {
                HandleKeyboardFrameMove(-1);
                return true;
            }

            if (keyCode is Keys.Right or Keys.D)
            {
                HandleKeyboardFrameMove(1);
                return true;
            }

            if (keyCode is Keys.Up or Keys.W)
            {
                ChangePlaybackSpeed(1);
                return true;
            }

            if (keyCode is Keys.Down or Keys.S)
            {
                ChangePlaybackSpeed(-1);
                return true;
            }

            if (keyCode == Keys.Enter)
            {
                CapturePendingSelectionUndo();
                isEnterSelectingFrames = true;
                AddCurrentFrameToSelection();
                UpdateKeyboardFrameMoveTimerFromHeldKeys();
                return true;
            }

            if (keyCode == Keys.Escape)
            {
                ClearAllFrameSelections();
                return true;
            }

            if (keyCode == Keys.Space)
            {
                btnPlayStop.PerformClick();
                return true;
            }

            if (keyCode == Keys.Delete)
            {
                btnDelete.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleKeyboardFrameMove(int direction)
        {
            if (keyboardFrameMoveTimer.Enabled && keyboardFrameMoveDirection == direction) return;

            MoveFrameByKeyboard(direction);

            if (IsEnterKeyPhysicallyDown())
            {
                keyboardFrameMoveDirection = direction;
                keyboardFrameMoveTimer.Start();
            }
        }

        private void KeyboardFrameMoveTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsEnterKeyPhysicallyDown())
            {
                StopKeyboardFrameMoveTimer();
                return;
            }

            int direction = GetHeldKeyboardFrameMoveDirection();
            if (direction == 0)
            {
                StopKeyboardFrameMoveTimer();
                return;
            }

            keyboardFrameMoveDirection = direction;
            MoveFrameByKeyboard(direction);
        }

        private void MoveFrameByKeyboard(int direction)
        {
            int index = direction < 0
                ? PrevActiveIndex(trackFrame.Value)
                : NextActiveIndex(trackFrame.Value);

            if (index < 0) return;

            ShowFrame(index);
            AddCurrentFrameToSelectionIfEnterHeld();
        }

        private void UpdateKeyboardFrameMoveTimerFromHeldKeys()
        {
            if (!IsEnterKeyPhysicallyDown())
            {
                StopKeyboardFrameMoveTimer();
                return;
            }

            int direction = GetHeldKeyboardFrameMoveDirection();
            if (direction == 0)
            {
                StopKeyboardFrameMoveTimer();
                return;
            }

            keyboardFrameMoveDirection = direction;
            keyboardFrameMoveTimer.Start();
        }

        private void StopKeyboardFrameMoveTimer()
        {
            keyboardFrameMoveTimer.Stop();
            keyboardFrameMoveDirection = 0;
        }

        private SelectionSnapshot CaptureSelectionSnapshot()
        {
            return new SelectionSnapshot(
                new SortedSet<int>(selectedFrames),
                pendingRangeAnchor,
                selectionAnchorIndex);
        }

        private static bool SameSelectionSnapshot(SelectionSnapshot a, SelectionSnapshot b)
        {
            return a.PendingRangeAnchor == b.PendingRangeAnchor
                && a.SelectionAnchorIndex == b.SelectionAnchorIndex
                && a.Frames.SetEquals(b.Frames);
        }

        private void CapturePendingSelectionUndo()
        {
            if (isApplyingUndo || pendingSelectionUndoSnapshot != null) return;
            pendingSelectionUndoSnapshot = CaptureSelectionSnapshot();
        }

        private SelectionSnapshot? BeginSelectionUndo()
        {
            if (isApplyingUndo || pendingSelectionUndoSnapshot != null) return null;
            return CaptureSelectionSnapshot();
        }

        private void CommitSelectionUndo(SelectionSnapshot? before)
        {
            if (isApplyingUndo) return;

            SelectionSnapshot? snapshot = pendingSelectionUndoSnapshot ?? before;
            pendingSelectionUndoSnapshot = null;
            if (snapshot == null) return;

            if (!SameSelectionSnapshot(snapshot, CaptureSelectionSnapshot()))
            {
                undoStack.Push(UndoAction.FromSelection(snapshot));
            }
        }

        private void CommitPendingSelectionUndo()
        {
            CommitSelectionUndo(null);
        }

        private void PushDeleteUndo(IEnumerable<TrashEntry> entries)
        {
            if (isApplyingUndo) return;
            List<TrashEntry> list = entries.ToList();
            if (list.Count > 0) undoStack.Push(UndoAction.FromDelete(list));
        }

        private void PushRestoreUndo(IEnumerable<TubFrame> frames)
        {
            if (isApplyingUndo) return;
            List<TubFrame> list = frames.ToList();
            if (list.Count > 0) undoStack.Push(UndoAction.FromRestore(list));
        }

        private void RestoreSelectionSnapshot(SelectionSnapshot snapshot)
        {
            selectedFrames.Clear();
            selectedFrames.UnionWith(snapshot.Frames.Where(i => i >= 0 && i < tubFrames.Count));
            pendingRangeAnchor = snapshot.PendingRangeAnchor;
            selectionAnchorIndex = snapshot.SelectionAnchorIndex;
            dragBaseSelection = null;
            pendingSelectionUndoSnapshot = null;
            isEnterSelectingFrames = false;
            RefreshSelectionVisuals();
        }

        private void UndoLastAction()
        {
            if (undoStack.Count == 0)
            {
                AddLog("되돌릴 작업이 없습니다.");
                return;
            }

            UndoAction action = undoStack.Pop();
            isApplyingUndo = true;
            try
            {
                switch (action.Kind)
                {
                    case UndoActionKind.Selection:
                        if (action.Selection != null) RestoreSelectionSnapshot(action.Selection);
                        AddLog("프레임 선택을 이전 상태로 되돌렸습니다.");
                        break;
                    case UndoActionKind.Delete:
                        UndoDeleteAction(action.TrashEntries);
                        break;
                    case UndoActionKind.Restore:
                        UndoRestoreAction(action.RestoredFrames);
                        break;
                }
            }
            finally
            {
                isApplyingUndo = false;
            }
        }

        private void UndoDeleteAction(List<TrashEntry> entries)
        {
            List<TrashEntry> restorable = entries.Where(entry => trashStore.Contains(entry)).ToList();
            if (restorable.Count == 0)
            {
                AddLog("삭제 취소 실패: 복원할 휴지통 항목을 찾을 수 없습니다.");
                return;
            }

            TubFrame? frameToKeep = trackFrame.Value >= 0 && trackFrame.Value < tubFrames.Count ? tubFrames[trackFrame.Value] : null;
            int fallbackIndex = trackFrame.Value;
            List<TubFrame> restoredFrames = new();

            foreach (TrashEntry entry in restorable)
            {
                RestoreEntry(entry);
                TubFrame? frame = CreateRestoredTubFrame(entry);
                if (frame != null) restoredFrames.Add(frame);
            }

            WriteTrashStore();
            SyncManifestToCatalogs();
            RefreshTubViewAfterRestore(restoredFrames, frameToKeep, fallbackIndex);
            AddLog($"삭제 작업을 취소하고 {restoredFrames.Count}개 프레임을 복원했습니다.");
        }

        private void UndoRestoreAction(List<TubFrame> frames)
        {
            List<(TubFrame frame, string reason)> targets = new();
            foreach (TubFrame frame in frames)
            {
                TubFrame? current = tubFrames.FirstOrDefault(item =>
                    item.FrameNumber == frame.FrameNumber &&
                    string.Equals(item.ImageFileName, frame.ImageFileName, StringComparison.OrdinalIgnoreCase));
                if (current != null) targets.Add((current, "복원 취소"));
            }

            if (targets.Count == 0)
            {
                AddLog("복원 취소 실패: 다시 휴지통으로 보낼 프레임을 찾을 수 없습니다.");
                return;
            }

            HashSet<TubFrame> targetFrames = targets.Select(t => t.frame).ToHashSet();
            TubFrame? frameToKeep = FindFrameToShowAfterRemoval(targetFrames, trackFrame.Value);
            int fallbackIndex = trackFrame.Value;

            int moved = DeleteFramesToTrash(targets);
            RefreshTubViewAfterDeletion(frameToKeep, fallbackIndex);
            AddLog($"복원 작업을 취소하고 {moved}개 프레임을 다시 휴지통으로 이동했습니다.");
        }

        private int GetHeldKeyboardFrameMoveDirection()
        {
            if (keyboardFrameMoveDirection < 0 && (IsKeyPhysicallyDown(Keys.Left) || IsKeyPhysicallyDown(Keys.A))) return -1;
            if (keyboardFrameMoveDirection > 0 && (IsKeyPhysicallyDown(Keys.Right) || IsKeyPhysicallyDown(Keys.D))) return 1;
            if (IsKeyPhysicallyDown(Keys.Left) || IsKeyPhysicallyDown(Keys.A)) return -1;
            if (IsKeyPhysicallyDown(Keys.Right) || IsKeyPhysicallyDown(Keys.D)) return 1;
            return 0;
        }

        private static bool HasFocusedTextInput(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Focused && (control is TextBoxBase || control is ComboBox)) return true;
                if (control.ContainsFocus && HasFocusedTextInput(control)) return true;
            }

            return false;
        }

        private void SetupFilterTooltips()
        {
            toolTip1.SetToolTip(chkThrottleZero,
                "속도(throttle)가 0인 프레임을 찾습니다.\n" +
                "정지 상태나 출발 전/후 데이터가 포함될 수 있습니다.\n" +
                "체크 후 [필터 적용]을 누르면 해당 프레임이 휴지통으로 이동합니다.");

            toolTip1.SetToolTip(chkMissingImage,
                "catalog 또는 record에는 기록이 있지만 실제 이미지 파일을 찾을 수 없는 프레임을 찾습니다.\n" +
                "이미지가 없으면 학습/테스트에서 오류가 날 수 있습니다.\n" +
                "체크 후 [필터 적용]을 누르면 해당 프레임이 휴지통으로 이동합니다.");

            toolTip1.SetToolTip(chkAbnormalAngle,
                "비정상 조향각(angle) 프레임을 찾습니다.\n" +
                "기준: -1.0~1.0 범위 밖, 평균에서 표준편차의 3배보다 멀리 벗어난 값, 또는 앞뒤 프레임과 0.8 초과로 급변한 값입니다.\n" +
                "체크 후 [필터 적용]을 누르면 해당 프레임이 휴지통으로 이동합니다.");
        }

        private void ArrangeTimelineAndTabs()
        {
            int bottomGap = 8;
            int resizeBarTopGap = 2;
            int minimumNavigatorHeight = GetMinimumNavigatorHeight();
            int appliedNavigatorHeight = Math.Max(navigatorHeight, minimumNavigatorHeight);
            int extraHeight = Math.Max(0, ClientSize.Height - 881);


            int tabHeight = DefaultTabPanelHeight + (extraHeight * 30 / 100);
            int timelineHeight = TimelinePanelHeight + (extraHeight * 15 / 100);
            tabHeight = Math.Max(MinimumTabVisibleHeight, tabHeight);
            timelineHeight = Math.Max(TimelineMinimumPanelHeight, timelineHeight);

            int availableBelowNavigator = ClientSize.Height
                - bottomGap
                - groupTubNavigator.Top
                - appliedNavigatorHeight
                - resizeBarTopGap
                - FrameResizeBarHeight
                - (LayoutGap * 2);

            int shortage = (timelineHeight + tabHeight) - availableBelowNavigator;
            if (shortage > 0)
            {
                int tabShrink = Math.Min(shortage, tabHeight - MinimumTabVisibleHeight);
                tabHeight -= tabShrink;
                shortage -= tabShrink;
            }

            if (shortage > 0)
            {
                int timelineShrink = Math.Min(shortage, timelineHeight - TimelineMinimumPanelHeight);
                timelineHeight -= timelineShrink;
                shortage -= timelineShrink;
            }

            if (shortage > 0)
            {
                appliedNavigatorHeight = Math.Max(minimumNavigatorHeight, appliedNavigatorHeight - shortage);
            }

            tabMain.Height = tabHeight;
            tabMain.Top = ClientSize.Height - tabMain.Height - bottomGap;

            groupTimeline.Height = timelineHeight;
            groupTimeline.Top = tabMain.Top - groupTimeline.Height - LayoutGap;
            groupTimeline.BringToFront();
            if (panelTrainingLoss.Visible)
            {
                LayoutTrainingLossOverlay();
                panelTrainingLoss.BringToFront();
            }

            groupTubNavigator.Height = appliedNavigatorHeight;
            groupFrameList.Height = appliedNavigatorHeight;
            ArrangeLeftDataPanels(groupTubNavigator.Bottom);

            ArrangeNavigatorControls();
        }



        private void ApplyResponsiveMainLayout()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            int gap = ResponsiveGap;

            float scaleX = ClientSize.Width / (float)DesignBaseWidth;
            float scaleY = ClientSize.Height / (float)DesignBaseHeight;

            int Sx(int value) => (int)Math.Round(value * scaleX);
            int Sy(int value) => (int)Math.Round(value * scaleY);

            // 200% 디자인 기준 위치/크기 비율 적용
            groupBox2.SetBounds(
                Sx(56), Sy(42),
                Sx(2397), Sy(111)
            );

            groupBoxDataLoad.SetBounds(
                Sx(56), Sy(165),
                Sx(433), Sy(386)
            );

            groupDataView.SetBounds(
                Sx(56), Sy(563),
                Sx(429), Sy(408)
            );

            groupTubNavigator.SetBounds(
                Sx(503), Sy(165),
                Sx(1562), Sy(827)
            );

            groupFrameList.SetBounds(
                Sx(2080), Sy(165),
                Sx(369), Sy(827)
            );

            groupTimeline.SetBounds(
                Sx(56), Sy(1005),
                 Sx(2397), Sy(168)
            );


            tabMain.SetBounds(
                Sx(56), Sy(1185),
                Sx(2397), Sy(314)
            );

            frameDragMaxTimelineHeight = groupTimeline.Height;
            frameDragMaxTabHeight = tabMain.Height;

            ArrangeNavigatorControls();
            ArrangeCleanerControls();
            ScaleProblemAreaControls();
            ApplySelectedTabLayout();
            ReloadTimelineForCurrentFrame();
        }


        private int GetMinimumNavigatorHeight()
        {
            return Math.Max(MinNavigatorHeight, groupBoxDataLoad.Height + LeftDataPanelGap + MinimumDataInfoPanelHeight);
        }

        private void ArrangeLeftDataPanels(int targetBottom)
        {
            groupBoxDataLoad.Top = groupTubNavigator.Top;
            groupDataView.Top = groupBoxDataLoad.Bottom + LeftDataPanelGap;
            groupDataView.Height = Math.Max(MinimumDataInfoPanelHeight, targetBottom - groupDataView.Top);
        }

        private void ArrangeNavigatorControls()
        {
            int width = groupTubNavigator.ClientSize.Width;
            int height = groupTubNavigator.ClientSize.Height;
            if (width <= 0 || height <= 0) return;

            float layoutScale = GetNavigatorLayoutScale(width, height);

            // 주행 이미지 탐색기 내부 글씨 크기 보정
            // 200% 디자인 기준을 유지하되, 창이 작을 때는 글씨가 겹치지 않도록 제한
            float dpiCompensation = 192f / DeviceDpi;

            // DPI 보정값이 너무 커져도 실제 버튼 배치 공간보다 글씨가 커지지 않게 제한
            float navigatorFontScale = Math.Min(dpiCompensation, layoutScale * 1.15f);
            navigatorFontScale = Math.Clamp(navigatorFontScale, 1.0f, 1.45f);

            lblCurrentFrame.Font = new Font("나눔고딕", 9f * navigatorFontScale, FontStyle.Regular);
            lblCurrentFrame2.Font = new Font("나눔고딕", 9f * navigatorFontScale, FontStyle.Regular);

            lblSpeed.Font = new Font("나눔고딕", 8f * navigatorFontScale, FontStyle.Regular);
            cmbPlaySpeed.Font = new Font("나눔고딕", 8f * navigatorFontScale, FontStyle.Regular);
            chkAutoPlay.Font = new Font("맑은 고딕", 8f * navigatorFontScale, FontStyle.Regular);

            btnFirst.Font = new Font("Microsoft Sans Serif", 11f * navigatorFontScale, FontStyle.Bold);
            btnPrev.Font = new Font("Microsoft Sans Serif", 11f * navigatorFontScale, FontStyle.Bold);
            btnNext.Font = new Font("Microsoft Sans Serif", 11f * navigatorFontScale, FontStyle.Bold);
            btnLast.Font = new Font("Microsoft Sans Serif", 11f * navigatorFontScale, FontStyle.Bold);
            btnPlayStop.Font = new Font("나눔고딕 ExtraBold", 11f * navigatorFontScale, FontStyle.Bold);

            // 글자가 잘릴 때 말줄임 처리
            btnFirst.AutoEllipsis = true;
            btnPrev.AutoEllipsis = true;
            btnNext.AutoEllipsis = true;
            btnLast.AutoEllipsis = true;
            btnPlayStop.AutoEllipsis = true;
            chkAutoPlay.AutoEllipsis = true;

            int sidePadding = ScaleLayout(24, layoutScale);
            int imageTop = ScaleLayout(25, layoutScale);
            int imageSidePadding = ScaleLayout(25, layoutScale);
            int buttonHeight = ScaleLayout(42, layoutScale);
            int comboHeight = ScaleLayout(28, layoutScale);
            int speedComboWidth = ScaleLayout(80, layoutScale);
            int autoPlayWidth = ScaleLayout(88, layoutScale);
            int speedGap = ScaleLayout(16, layoutScale);
            int speedWidth = speedComboWidth + speedGap + autoPlayWidth;
            int controlsBottomPadding = ScaleLayout(82, layoutScale);
            int controlY = Math.Max(ScaleLayout(220, layoutScale), height - controlsBottomPadding - buttonHeight);
            int labelValueGap = ScaleLayout(27, layoutScale);
            int trackY = Math.Min(height - ScaleLayout(65, layoutScale), controlY + ScaleLayout(58, layoutScale));

            int imageBottom = Math.Max(imageTop + ScaleLayout(160, layoutScale), controlY - ScaleLayout(18, layoutScale));
            picFrame.SetBounds(
                imageSidePadding,
                imageTop,
                Math.Max(160, width - (imageSidePadding * 2)),
                Math.Max(120, imageBottom - imageTop));

            lblCurrentFrame.Location = new Point(ScaleLayout(45, layoutScale), controlY);
            lblCurrentFrame2.Location = new Point(ScaleLayout(53, layoutScale), controlY + labelValueGap);

            int speedX = width - sidePadding - speedWidth;
            lblSpeed.Location = new Point(speedX, controlY - ScaleLayout(7, layoutScale));
            cmbPlaySpeed.SetBounds(speedX, controlY + ScaleLayout(21, layoutScale), speedComboWidth, comboHeight);
            chkAutoPlay.SetBounds(speedX + speedComboWidth + speedGap, controlY + ScaleLayout(18, layoutScale), autoPlayWidth, comboHeight);

            int leftReserved = ScaleLayout(150, layoutScale);
            int rightReserved = speedWidth + sidePadding;
            int areaLeft = leftReserved;
            int areaRight = Math.Max(areaLeft, width - sidePadding - rightReserved);
            int areaWidth = areaRight - areaLeft;

            int gap = Math.Clamp(areaWidth / 32, ScaleLayout(8, layoutScale), ScaleLayout(18, layoutScale));
            int smallWidth = Math.Clamp((areaWidth - ScaleLayout(122, layoutScale) - (gap * 4)) / 4, ScaleLayout(52, layoutScale), ScaleLayout(92, layoutScale));
            int playWidth = Math.Clamp(areaWidth - (smallWidth * 4) - (gap * 4), ScaleLayout(122, layoutScale), ScaleLayout(210, layoutScale));
            int totalWidth = (smallWidth * 4) + playWidth + (gap * 4);
            int x = areaLeft + Math.Max(0, (areaWidth - totalWidth) / 2);

            btnFirst.SetBounds(x, controlY, smallWidth, buttonHeight);
            x += smallWidth + gap;
            btnPrev.SetBounds(x, controlY, smallWidth, buttonHeight);
            x += smallWidth + gap;
            btnPlayStop.SetBounds(x, controlY, playWidth, buttonHeight);
            x += playWidth + gap;
            btnNext.SetBounds(x, controlY, smallWidth, buttonHeight);
            x += smallWidth + gap;
            btnLast.SetBounds(x, controlY, smallWidth, buttonHeight);

            trackFrame.SetBounds(
                ScaleLayout(37, layoutScale),
                trackY,
                Math.Max(120, width - ScaleLayout(73, layoutScale)),
                trackFrame.Height);
        }

        private static float GetNavigatorLayoutScale(int width, int height)
        {
            float widthScale = width / 900f;
            float heightScale = height / 520f;
            return Math.Clamp(Math.Min(widthScale, heightScale * 1.15f), 1f, 1.35f);
        }

        private static int ScaleLayout(int value, float scale)
        {
            return (int)Math.Round(value * scale);
        }

        private void ArrangeCleanerControls()
        {
            if (tabCleaner.ClientSize.Width <= 0 || tabCleaner.ClientSize.Height <= 0) return;

            int margin = 8;
            int trashHeight = Math.Max(178, tabCleaner.ClientSize.Height - (margin * 2));
            groupBoxTrash.Height = trashHeight;

            lstTrash.Height = Math.Max(90, trashHeight - lstTrash.Top - margin);
        }

        // ==========================================
        // 최초 실행 시 컨트롤 원본 위치/크기 저장
        // 이후 화면 크기 변경 시 이 값을 기준으로
        // 내부 컨트롤을 다시 계산한다.
        // ==========================================
        private void CaptureBaseLayouts()
        {
            if (_baseLayoutCaptured) return;

            // 상단 서버 연결 설정
            CaptureBaseLayout(groupBox2);

            // 왼쪽 데이터 로드 / 데이터 정보
            CaptureBaseLayout(groupBoxDataLoad);
            CaptureBaseLayout(groupDataView);

            // 하단 데이터 정리 탭
            CaptureBaseLayout(tabCleaner);

            // 하단 학습/테스트 탭
            CaptureBaseLayout(tabTrainTest);

            // AI 컴파일 탭
            CaptureBaseLayout(tabAiCompile);

            _baseLayoutCaptured = true;
        }


        // 부모 컨트롤 내부의 모든 자식 컨트롤 정보를 재귀적으로 저장
        private void CaptureBaseLayout(Control parent)
        {
            _baseClientSizes[parent] = parent.ClientSize;

            foreach (Control child in parent.Controls)
            {
                _baseBounds[child] = child.Bounds;
                _baseFontSizes[child] = child.Font.Size;

                if (child.HasChildren)
                {
                    CaptureBaseLayout(child);
                }
            }
        }

        //그룹박스들 이름들도 같이 키우기
        private void ScaleGroupBoxTitleFont(GroupBox groupBox)
        {
            float scaleX = ClientSize.Width / (float)DesignBaseWidth;
            float scaleY = ClientSize.Height / (float)DesignBaseHeight;

            float fontScale = Math.Min(scaleX, scaleY);

            // 125% 화면에서 너무 작아지는 것 방지
            fontScale = Math.Clamp(fontScale, 1.0f, 1.6f);

            groupBox.Font = new Font(
                groupBox.Font.FontFamily,
                9.5f * fontScale,
                groupBox.Font.Style
            );
        }

        // ==========================================
        // 부모(GroupBox / TabPage / Panel) 크기 변화 비율을 계산하여
        // 내부 버튼, 라벨, 체크박스, 텍스트박스 등의
        // 위치/크기/폰트를 함께 확대 또는 축소한다.
        // ==========================================
        private void ScaleChildrenByParent(Control parent)
        {
            if (!_baseLayoutCaptured) return;
            if (!_baseClientSizes.TryGetValue(parent, out Size baseSize)) return;
            if (baseSize.Width <= 0 || baseSize.Height <= 0) return;

            float scaleX = parent.ClientSize.Width / (float)baseSize.Width;
            float scaleY = parent.ClientSize.Height / (float)baseSize.Height;
            float fontScale = Math.Min(scaleX, scaleY);

            foreach (Control child in parent.Controls)
            {
                if (_baseBounds.TryGetValue(child, out Rectangle baseRect))
                {
                    child.SetBounds(
                        (int)Math.Round(baseRect.X * scaleX),
                        (int)Math.Round(baseRect.Y * scaleY),
                        Math.Max(1, (int)Math.Round(baseRect.Width * scaleX)),
                        Math.Max(1, (int)Math.Round(baseRect.Height * scaleY))
                    );
                }

                if (_baseFontSizes.TryGetValue(child, out float baseFontSize))
                {
                    float newFontSize = Math.Clamp(baseFontSize * fontScale, 9f, 20f);

                    child.Font = new Font(
                        child.Font.FontFamily,
                        newFontSize,
                        child.Font.Style
                    );
                }

                // GroupBox, Panel, TabPage 안의 자식 컨트롤까지 재귀적으로 스케일 적용
                if (child.HasChildren)
                {
                    ScaleChildrenByParent(child);
                }
            }
        }

        // ==========================================
        // 100%, 125%, 150%, 200% 배율 환경에서
        // 작아 보이는 주요 영역의 내부 컨트롤을
        // 선택적으로 반응형 스케일 적용
        // ==========================================
        private void ScaleProblemAreaControls()
        {
            ScaleGroupBoxTitleFont(groupBox2);
            ScaleGroupBoxTitleFont(groupBoxDataLoad);
            ScaleGroupBoxTitleFont(groupDataView);
            ScaleGroupBoxTitleFont(groupTubNavigator);
            ScaleGroupBoxTitleFont(groupFrameList);
            ScaleGroupBoxTitleFont(groupTimeline);
            ScaleGroupBoxTitleFont(groupBoxTrash);

            ScaleTabTitleFont(tabMain);

            ScaleChildrenByParent(groupBox2);
            ScaleChildrenByParent(groupBoxDataLoad);
            ScaleChildrenByParent(groupDataView);
            /*
            ScaleChildrenByParent(tabCleaner);
            ScaleChildrenByParent(tabTrainTest);
            ScaleChildrenByParent(tabAiCompile);
            */
        }


        private void ApplySelectedTabLayout()
        {
            if (!_baseLayoutCaptured) return;

            if (tabMain.SelectedTab == tabCleaner)
            {
                ScaleChildrenByParent(tabCleaner);
                ArrangeCleanerControls();
            }
            else if (tabMain.SelectedTab == tabTrainTest)
            {
                ScaleChildrenByParent(tabTrainTest);
                if (panelTrainingLoss.Visible) LayoutTrainingLossOverlay();
            }
            else if (tabMain.SelectedTab == tabAiCompile)
            {
                ScaleChildrenByParent(tabAiCompile);
                LayoutImageTestTabControls();
            }
        }


        private void ScaleTabTitleFont(TabControl tabControl)
        {
            float scaleX = ClientSize.Width / (float)DesignBaseWidth;
            float scaleY = ClientSize.Height / (float)DesignBaseHeight;

            float fontScale = Math.Min(scaleX, scaleY);
            fontScale = Math.Clamp(fontScale, 1.0f, 1.6f);

            tabControl.Font = new Font(
                tabControl.Font.FontFamily,
                9.5f * fontScale,
                tabControl.Font.Style
            );
        }

        private bool IsFrameResizeGrip(Point location)
        {
            return location.Y >= picFrame.ClientSize.Height - FrameResizeGripHeight;
        }

        private void ApplyFrameDragLayout(int desiredNavigatorHeight)
        {
            int bottomGap = 8;
            int availableBottom = ClientSize.Height - bottomGap;
            int minimumNavigatorHeight = GetMinimumNavigatorHeight();

            int navigatorTop = resizeStartNavigatorBounds.Top;
            int navigatorHeightToApply = Math.Max(minimumNavigatorHeight, desiredNavigatorHeight);
            int timelineHeight = Math.Max(resizeStartTimelineBounds.Height, frameDragMaxTimelineHeight);
            int tabHeight = Math.Max(resizeStartTabBounds.Height, frameDragMaxTabHeight);

            int timelineTop = navigatorTop + navigatorHeightToApply + LayoutGap;
            int tabTop = timelineTop + timelineHeight + LayoutGap;
            int overflow = tabTop + tabHeight - availableBottom;

            if (overflow > 0)
            {
                int tabShrink = Math.Min(overflow, tabHeight - MinimumTabVisibleHeight);
                tabHeight -= tabShrink;
                overflow -= tabShrink;
            }

            if (overflow > 0)
            {
                int timelineShrink = Math.Min(overflow, timelineHeight - TimelineMinimumPanelHeight);
                timelineHeight -= timelineShrink;
                overflow -= timelineShrink;
                tabTop = navigatorTop + navigatorHeightToApply + LayoutGap + timelineHeight + LayoutGap;
            }

            if (overflow > 0)
            {
                navigatorHeightToApply = Math.Max(minimumNavigatorHeight, navigatorHeightToApply - overflow);
                timelineTop = navigatorTop + navigatorHeightToApply + LayoutGap;
                tabTop = timelineTop + timelineHeight + LayoutGap;
            }

            navigatorHeight = navigatorHeightToApply;

            groupTubNavigator.Height = navigatorHeightToApply;
            groupFrameList.Height = navigatorHeightToApply;
            ArrangeLeftDataPanels(groupTubNavigator.Bottom);

            groupTimeline.Top = timelineTop;
            groupTimeline.Height = timelineHeight;
            tabMain.Top = tabTop;
            tabMain.Height = tabHeight;

            ArrangeNavigatorControls();
            ArrangeCleanerControls();
            if (panelTrainingLoss.Visible)
            {
                LayoutTrainingLossOverlay();
                panelTrainingLoss.BringToFront();
            }
        }

        private void picFrame_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !IsFrameResizeGrip(e.Location)) return;

            isResizingNavigator = true;
            suppressNextFrameClick = true;
            resizeStartMouseY = PointToClient(Cursor.Position).Y;
            resizeStartNavigatorHeight = groupTubNavigator.Height;
            resizeStartNavigatorBounds = groupTubNavigator.Bounds;
            resizeStartTimelineBounds = groupTimeline.Bounds;
            resizeStartTabBounds = tabMain.Bounds;
            frameDragMaxTimelineHeight = Math.Max(frameDragMaxTimelineHeight, groupTimeline.Height);
            frameDragMaxTabHeight = Math.Max(frameDragMaxTabHeight, tabMain.Height);
            picFrame.Capture = true;
            picFrame.Cursor = Cursors.HSplit;
        }

        private void picFrame_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isResizingNavigator)
            {
                picFrame.Cursor = IsFrameResizeGrip(e.Location) ? Cursors.HSplit : Cursors.Default;
                return;
            }

            int currentMouseY = PointToClient(Cursor.Position).Y;
            ApplyFrameDragLayout(resizeStartNavigatorHeight + currentMouseY - resizeStartMouseY);
        }

        private void picFrame_MouseLeave(object? sender, EventArgs e)
        {
            if (!isResizingNavigator)
            {
                picFrame.Cursor = Cursors.Default;
            }
        }

        private void picFrame_MouseUp(object? sender, MouseEventArgs e)
        {
            isResizingNavigator = false;
            picFrame.Capture = false;
            picFrame.Cursor = IsFrameResizeGrip(e.Location) ? Cursors.HSplit : Cursors.Default;
        }

        // UI 스레드 안전하게 상태 라벨을 바꿔주는 도우미
        private void UpdateStatusLabel(string text, Color color)
        {
            if (lblStatus2 != null)
            {
                lblStatus2.Invoke(new Action(() =>
                {
                    lblStatus2.Text = text;
                    lblStatus2.ForeColor = color;
                }));
            }
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

            // 메인 프레임 목록에서 Delete 키 누르면 휴지통 이동
            lstFrames.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Delete) btnDelete_Click(null, EventArgs.Empty); };

            // 기본 재생속도 1x로 설정
            if (cmbPlaySpeed.SelectedIndex < 0) cmbPlaySpeed.SelectedIndex = 0;
            ApplyPlaybackSpeed();
            UpdatePlaybackControlsVisual(false);
            UpdateAutoPlayLoopVisual();

            lblConfigPath.AutoEllipsis = true;
            lblTubPath.AutoEllipsis = true;

            // 새로 추가되는 AI 컴파일 탭 전용 설정 
            if (lstHighErrorFrames != null)
            {
                // 1. 리스트박스 다중 선택(드래그, Shift, Ctrl 클릭) 모드 켜기
                lstHighErrorFrames.SelectionMode = SelectionMode.MultiExtended;

                // 2. AI 컴파일 목록을 클릭하면 메인 화면에 사진 띄워주기 연동
                lstHighErrorFrames.SelectedIndexChanged += lstHighErrorFrames_SelectedIndexChanged;

                // 3. AI 컴파일 목록에서도 Delete 키 누르면 전용 삭제 버튼(휴지통 이동) 실행!
                lstHighErrorFrames.KeyDown += (s, ev) =>
                {
                    if (ev.KeyCode == Keys.Delete) btnDeleteHighError_Click(null, EventArgs.Empty);
                };
            }
        }

        // 상단 메뉴바 자동 생성기 (UI/UX 패치)
        private void CreateTopMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.WhiteSmoke;
            menuStrip.Padding = new Padding(5, 5, 5, 5);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
            menuStrip.BringToFront();

            // ★ 설명서를 상황별로 명확하게 분리!
            ToolStripMenuItem menuManual = new ToolStripMenuItem("📖 Donkeycar 사용 설명서");
            menuManual.DropDownItems.Add("[공통] 1. 이상한 주행 데이터 필터/삭제로 휴지통 비우기");
            menuManual.DropDownItems.Add("[공통] 2. [학습 시작] 클릭 (완료 시 models/mypilot.h5 생성)");
            menuManual.DropDownItems.Add("-");
            menuManual.DropDownItems.Add("[로컬 모드] 폴더를 고를 때 'Linux' 탭이나 \\\\wsl.localhost\\ 주소를 이용하세요.");
            menuManual.DropDownItems.Add("[원격 서버] 경로는 프로그램이 알아서 똑똑하게 자동 설정합니다. (그냥 예 누르시면 됩니다)");
            menuManual.MouseEnter += (s, e) => menuManual.ShowDropDown();

            ToolStripMenuItem menuHotkeys = new ToolStripMenuItem("⌨️ 단축키 안내");
            menuHotkeys.DropDownItems.Add("Space Bar : 자동 재생 / 정지 토글");
            menuHotkeys.DropDownItems.Add("A / D 또는 ← / → : 이전/다음 프레임 이동");
            menuHotkeys.DropDownItems.Add("W / S 또는 ↑ / ↓ : 재생 속도 조절");
            menuHotkeys.DropDownItems.Add("Enter : 현재 프레임 선택 추가");
            menuHotkeys.DropDownItems.Add("Esc : 선택한 프레임/범위 전체 취소");
            menuHotkeys.DropDownItems.Add("Delete : 선택한 프레임 또는 지정 범위를 휴지통으로 이동");
            menuHotkeys.DropDownItems.Add("Ctrl + Z : 마지막 선택/삭제/복원 작업 되돌리기");
            menuHotkeys.MouseEnter += (s, e) => menuHotkeys.ShowDropDown();

            menuAttempts = new ToolStripMenuItem("📊 오늘의 시도");
            menuAttempts.Alignment = ToolStripItemAlignment.Right;
            menuAttempts.Font = new Font("맑은 고딕", 9, FontStyle.Bold);
            menuAttempts.DropDownDirection = ToolStripDropDownDirection.BelowLeft;

            infoTrain = new ToolStripMenuItem($"학습 시도: {_trainCount}회");
            infoTrain.Enabled = false;
            infoTest = new ToolStripMenuItem($"모델 테스트 시도: {_testCount}회");
            infoTest.Enabled = false;

            menuAttempts.DropDownItems.Add(infoTrain);
            menuAttempts.DropDownItems.Add(infoTest);
            menuAttempts.MouseEnter += (s, e) => menuAttempts.ShowDropDown();

            menuStrip.Items.Add(menuManual);
            menuStrip.Items.Add(menuHotkeys);
            menuStrip.Items.Add(menuAttempts);

            if (btnDisconnect != null)
            {
                btnDisconnect.Click -= btnDisconnect_Click;
                btnDisconnect.Click += btnDisconnect_Click;
            }
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
                            // ★ 이 두 줄이 실수로 지워졌었습니다! (서버 접속 엔진 가동)
                            _executor = new ICE.RemoteExecutor(loginForm.Host, loginForm.User, loginForm.Pass);
                            loggedInUser = loginForm.User;

                            if (lblUser2 != null)
                            {
                                lblUser2.Text = loginForm.User;
                                lblUser2.ForeColor = Color.Blue;
                            }
                            UpdateStatusLabel("원격 연결됨", Color.Green);
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
                if (string.IsNullOrEmpty(loggedInUser))
                {
                    MessageBox.Show("원격 서버에 먼저 로그인(연결)해 주세요!", "로그인 필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                configPath = $"/home/{loggedInUser}/mycar";
                lblConfigPath.Text = configPath;
                MessageBox.Show($"[원격 모드]\n{loggedInUser}님의 서버 폴더({configPath})로 작업 기준점이 설정되었습니다!\n\n이제 'Tub 데이터 열기'를 진행해 주세요.", "설정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ★ 로컬 모드일 때 우분투(WSL) 폴더 찾는 방법 상세 안내!
            MessageBox.Show("[로컬(WSL) 모드 안내]\n\n윈도우 탐색기가 열리면, 좌측 폴더 목록 맨 아래의 'Linux' 아이콘을 클릭하거나 상단 주소창에 아래 경로를 직접 입력하세요.\n\n👉 \\\\wsl.localhost\\Ubuntu-22.04\\home\\(사용자명)\\mycar\n\n해당 mycar 폴더를 찾아 선택해 주시면 됩니다.", "WSL 폴더 선택 안내", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "manage.py 파일이 있는 우분투 내부의 mycar 폴더를 찾아 선택해주세요.";
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
                    isModelTestRunning = false;
                    ClearPendingModelTestLogs();

                    txtLog.AppendText(Environment.NewLine + "🛑 [알림] 사용자에 의해 작업이 강제 중지되었습니다." + Environment.NewLine);
                    _summaryLogBuilder.AppendLine("🛑 [알림] 사용자에 의해 작업이 강제 중지되었습니다.");

                    if (_originalLogBuilder.Length > 0) SaveLogsToFile();

                    // 강제 중지 후 학습/테스트 UI 상태를 대기 상태로 되돌린다.
                    if (progressBarTrain != null) progressBarTrain.Value = 0;
                    if (lblProgressPercent != null) lblProgressPercent.Text = "0%";
                    if (lblTrainStatus2 != null)
                    {
                        lblTrainStatus2.Text = "준비 완료";
                        lblTrainStatus2.ForeColor = Color.Green;
                    }
                    UpdateStatusLabel("대기 중", Color.Green);
                }
            }
        }

        private void SaveLogsToFile()
        {
            try
            {
                // 1. 바탕화면 경로 가져오기
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // 2. 바탕화면 안에 만들 '전용 폴더 경로' 지정 (이름은 원하시는 대로 변경 가능합니다)
                string logDirectory = Path.Combine(desktopPath, "Donkeycar_Logs");

                // 3. 만약 해당 폴더가 없다면 새로 만들기!
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // 4. 바탕화면이 아닌, 방금 만든 '전용 폴더(logDirectory)' 안에 파일 경로 지정
                string origPath = Path.Combine(logDirectory, $"TrainLog_Original_{timeStamp}.txt");
                string summaryPath = Path.Combine(logDirectory, $"TrainLog_Summary_{timeStamp}.txt");

                // 5. 텍스트 파일 저장
                File.WriteAllText(origPath, _originalLogBuilder.ToString());
                File.WriteAllText(summaryPath, _summaryLogBuilder.ToString());

                // 6. UI 로그 메시지도 폴더 이름을 알려주도록 친절하게 수정
                txtLog.AppendText(Environment.NewLine + $"💾 [로그 저장 완료] 바탕화면의 'Donkeycar_Logs' 폴더를 확인해 주세요!");
                txtLog.AppendText(Environment.NewLine + $"- 원본 로그: {Path.GetFileName(origPath)}");
                txtLog.AppendText(Environment.NewLine + $"- 요약 로그: {Path.GetFileName(summaryPath)}");
            }
            catch (Exception ex)
            {
                txtLog.AppendText(Environment.NewLine + $"🚨 로그 파일 저장 실패: {ex.Message}");
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

            // ★ 새로 추가된 안전장치: 사용자가 데이터를 열지 않고 학습을 누르는 것을 방지합니다!
            if (string.IsNullOrEmpty(tubPath))
            {
                MessageBox.Show("학습할 Tub 데이터를 먼저 열어주세요!", "데이터 누락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ★ 로컬/원격 환경에 맞춘 동적 알림창!
            string modeText = rdoRemote.Checked ? "원격 서버" : "로컬(WSL)";
            DialogResult res = MessageBox.Show($"{modeText}({configPath})에서 AI 모델 학습을 시작하시겠습니까?\n\n선택된 데이터: {tubPath}\n(학습에는 시간이 오래 걸릴 수 있습니다.)", "학습 시작 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No) return;

            ResetTrainingLossGraph();
            txtLog.Text = "";
            if (txtLogOriginal != null) txtLogOriginal.Text = "";

            txtLog.AppendText("[Train] AI 모델 학습 요약을 시작합니다..." + Environment.NewLine);
            if (txtLogOriginal != null) txtLogOriginal.AppendText("[Train] AI 모델 원본 로그를 기록합니다..." + Environment.NewLine);

            _originalLogBuilder.Clear();
            _summaryLogBuilder.Clear();
            _originalLogBuilder.AppendLine($"--- Donkeycar 학습 원본 로그 ({DateTime.Now}) ---");
            _summaryLogBuilder.AppendLine($"--- Donkeycar 학습 요약 로그 ({DateTime.Now}) ---");

            UpdateStatusLabel("학습 중", Color.DarkOrange);

            // ★ 카운트는 버튼 누를 때 한 번만 오르도록 여기서만 처리! (기존 코드 완벽 유지)
            _trainCount++;
            if (infoTrain != null) infoTrain.Text = $"오늘 학습 시도: {_trainCount}회";

            bool useVenv = chkUseVenv != null ? chkUseVenv.Checked : true;

            // ★ 수정된 부분: configPath와 함께 tubPath도 파이썬 스크립트로 쏴줍니다!
            _executor.ExecuteTrain(configPath, tubPath, useVenv, (log) =>
            {
                // 프리징 방지를 위해 BeginInvoke 사용 (기존 코드 완벽 유지)
                this.BeginInvoke(new Action(() => { UpdateChartRealTime(log); }));
            });
        }

        private void UpdateChartRealTime(string logText)
        {
            if (string.IsNullOrEmpty(logText)) return;

            bool isJunkLog = logText.Contains('\r') || logText.Contains('\b') || logText.Count(c => c == '=') > 10 || logText.Contains("\u001b");

            if (!isJunkLog)
            {
                if (txtLogOriginal != null)
                {
                    txtLogOriginal.AppendText(logText + Environment.NewLine);
                    txtLogOriginal.ScrollToCaret();
                }
                _originalLogBuilder.AppendLine(logText);

                bool isSummaryImportant =
                    logText.StartsWith("Epoch ") ||
                    logText.Contains("val_loss") ||
                    logText.StartsWith("Records #") ||
                    logText.StartsWith("[Train]") ||
                    logText.StartsWith("🛑") ||
                    logText.StartsWith("✅") ||
                    logText.StartsWith("📁") ||
                    logText.StartsWith("💾") ||
                    logText.Contains("Error:") ||
                    logText.Contains("---TRAINING_COMPLETE---");

                if (isSummaryImportant)
                {
                    txtLog.AppendText(logText + Environment.NewLine);
                    txtLog.ScrollToCaret();
                    _summaryLogBuilder.AppendLine(logText);
                }
            }

            bool capturedLoss = CaptureTrainingLoss(logText);
            if (capturedLoss && IsEpochLossUpdateLog(logText))
            {
                ShowTrainingLossGraph(isFinal: false);
            }

            if (logText.Contains("[Errno 2]") || logText.Contains("Error") || logText.Contains("Exception"))
            {
                MessageBox.Show($"학습 중 파이썬 오류가 발생했습니다.\n\n내용: {logText}", "학습 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusLabel("대기 중", Color.Green);
                SaveLogsToFile();
                return;
            }

            if (logText.Trim() == "---TRAINING_COMPLETE---")
            {
                if (progressBarTrain != null) progressBarTrain.Value = progressBarTrain.Maximum;
                if (lblProgressPercent != null) lblProgressPercent.Text = "100%";

                txtLog.AppendText(Environment.NewLine + "--------------------------------------------------");
                txtLog.AppendText(Environment.NewLine + "✅ [학습 완료] AI 모델 생성이 끝났습니다.");
                txtLog.AppendText(Environment.NewLine + $"📁 자동 저장 위치: {configPath}/models/mypilot.h5");
                txtLog.AppendText(Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);

                ShowTrainingLossGraph(isFinal: true);
                SaveLogsToFile();

                MessageBox.Show($"🎉 AI 모델 학습이 성공적으로 완료되었습니다!\n\n[자동 저장 위치]\n{configPath}/models/mypilot.h5\n\n[다음 단계 안내]\n1. 좌측의 [모델 선택] 버튼을 누르세요. (자동으로 세팅됩니다.)\n2. [테스트 이미지 선택]을 누르세요. (원격 서버 폴더 자동 세팅)\n3. [모델 테스트 실행] 버튼을 눌러보세요!", "학습 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatusLabel("대기 중", Color.Green);
            }
            else if (logText.Contains("Killed") || logText.Contains("Segmentation fault"))
            {
                MessageBox.Show("🚨 학습이 비정상적으로 종료되었습니다. (메모리 부족 등)\n학습 옵션을 조절하거나 서버 상태를 확인하세요.", "학습 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatusLabel("대기 중", Color.Green);
                SaveLogsToFile();
            }

            Match epochMatch = Regex.Match(logText, @"Epoch\s+(\d+)/(\d+)");
            if (epochMatch.Success && progressBarTrain != null)
            {
                int current = int.Parse(epochMatch.Groups[1].Value);
                int total = int.Parse(epochMatch.Groups[2].Value);
                progressBarTrain.Maximum = total;
                progressBarTrain.Value = current <= total ? current : total;

                if (lblProgressPercent != null)
                {
                    int percent = (int)((double)current / total * 100);
                    lblProgressPercent.Text = $"{percent}%";
                }
            }
        }

        private void InitializeTrainingLossOverlay()
        {
            panelTrainingLoss.Dock = DockStyle.None;
            panelTrainingLoss.BackColor = Color.White;
            panelTrainingLoss.Padding = new Padding(6, 4, 6, 6);
            panelTrainingLoss.Visible = true;

            lblTrainingLossSummary.Dock = DockStyle.None;
            lblTrainingLossSummary.TextAlign = ContentAlignment.MiddleLeft;
            lblTrainingLossSummary.Font = new Font("나눔고딕", 9F, FontStyle.Bold);
            lblTrainingLossSummary.Text = "학습 완료 후 loss 그래프가 여기에 표시됩니다.";

            picTrainingLossGraph.Dock = DockStyle.None;
            picTrainingLossGraph.BackColor = Color.White;
            picTrainingLossGraph.BorderStyle = BorderStyle.FixedSingle;
            picTrainingLossGraph.SizeMode = PictureBoxSizeMode.StretchImage;
            picTrainingLossGraph.Paint += picTrainingLossGraph_Paint;
            picTrainingLossGraph.MouseMove += picTrainingLossGraph_MouseMove;
            picTrainingLossGraph.MouseLeave += (_, _) =>
            {
                lblTrainingLossHover.Visible = false;
                picTrainingLossGraph.Cursor = Cursors.Default;
            };
            picTrainingLossGraph.Resize += (_, _) =>
            {
                if (panelTrainingLoss.Visible) DrawTrainingLossGraph();
            };

            lblTrainingLossHover.AutoSize = true;
            lblTrainingLossHover.BackColor = Color.FromArgb(40, 40, 40);
            lblTrainingLossHover.ForeColor = Color.White;
            lblTrainingLossHover.Font = new Font(SystemFonts.DefaultFont.FontFamily, 9, FontStyle.Bold);
            lblTrainingLossHover.Padding = new Padding(8, 5, 8, 5);
            lblTrainingLossHover.BorderStyle = BorderStyle.FixedSingle;
            lblTrainingLossHover.Visible = false;
            picTrainingLossGraph.Controls.Add(lblTrainingLossHover);

            panelTrainingLoss.Controls.Add(picTrainingLossGraph);
            panelTrainingLoss.Controls.Add(lblTrainingLossSummary);
            panelTrainingLoss.Resize += (_, _) =>
            {
                LayoutTrainingLossOverlay();
                if (panelTrainingLoss.Visible) DrawTrainingLossGraph();
            };
            tabTrainTest.Resize += (_, _) =>
            {
                if (!panelTrainingLoss.Visible) return;
                LayoutTrainingLossOverlay();
                DrawTrainingLossGraph();
            };
            tabTrainTest.Controls.Add(panelTrainingLoss);
            LayoutTrainingLossOverlay();
            DrawTrainingLossGraph();
        }

        private void LayoutTrainingLossOverlay()
        {
            if (isLayingOutLogPanels) return;
            isLayingOutLogPanels = true;

            try
            {
            int margin = 12;
            int gap = 14;
            int top = margin;
            int controlsRight = Math.Max(grpTrainControl.Right, grpTrainProgress.Right);
            int contentLeft = controlsRight + gap;
            int contentWidth = tabTrainTest.ClientSize.Width - contentLeft - margin;
            int height = tabTrainTest.ClientSize.Height - top - margin;

            int graphWidth = Math.Max(360, (int)(contentWidth * 0.46));
            int logAreaWidth = contentWidth - graphWidth - gap * 2;

            if (logAreaWidth < 260)
            {
                graphWidth = Math.Max(300, contentWidth - 260 - gap * 2);
                logAreaWidth = contentWidth - graphWidth - gap * 2;
            }

            int logWidth = Math.Max(100, logAreaWidth / 2);
            int logHeight = Math.Max(TrainingLossMinimumPanelHeight, height);

            txtLog.SetBounds(
                Math.Max(margin, contentLeft),
                top,
                logWidth,
                logHeight);

            txtLogOriginal.SetBounds(
                txtLog.Right + gap,
                top,
                logWidth,
                logHeight);

            panelTrainingLoss.SetBounds(
                txtLogOriginal.Right + gap,
                top,
                Math.Max(300, tabTrainTest.ClientSize.Width - txtLogOriginal.Right - gap - margin),
                logHeight);

            int innerLeft = panelTrainingLoss.Padding.Left;
            int innerTop = panelTrainingLoss.Padding.Top;
            int innerWidth = Math.Max(1, panelTrainingLoss.ClientSize.Width - panelTrainingLoss.Padding.Horizontal);
            int summaryWidth = innerWidth;

            lblTrainingLossSummary.SetBounds(innerLeft, innerTop, summaryWidth, 30);

            int graphTop = lblTrainingLossSummary.Bottom + 4;
            int graphHeight = Math.Max(70, panelTrainingLoss.ClientSize.Height - graphTop - panelTrainingLoss.Padding.Bottom);
            picTrainingLossGraph.SetBounds(innerLeft, graphTop, innerWidth, graphHeight);
            panelTrainingLoss.PerformLayout();
            picTrainingLossGraph.Update();
            }
            finally
            {
                isLayingOutLogPanels = false;
            }
        }

        private void LayoutImageTestTabControls()
        {
            return;
        }

        private void LayoutImageTestLeftGroups()
        {
            btnSelectModel.SetBounds(8, 18, 145, 26);
            btnSelectTestImage.SetBounds(163, 18, 165, 26);
            btnStopTest.SetBounds(328, 18, 134, 26);
            btnStopTest.TextAlign = ContentAlignment.MiddleRight;
            btnStopTest.Padding = new Padding(6, 0, 6, 0);

            btnModelTest.SetBounds(10, 20, 230, 30);
            picTestImage.SetBounds(300, 12, 114, 50);
        }

        private void LayoutPredictResultGroup()
        {
            int labelX = 12;
            int valueX = Math.Max(104, grpPredictResult.ClientSize.Width - 52);
            int row1 = 18;
            int row2 = 43;
            int row3 = 68;
            int bottomRow = Math.Max(98, grpPredictResult.ClientSize.Height - 24);

            lblRealAngle.SetBounds(labelX, row1, 84, 22);
            lblPredictAngle.SetBounds(labelX, row2, 84, 22);
            lblErrorValue.SetBounds(labelX, row3, 84, 22);
            lblRealAngle2.SetBounds(valueX, row1, 46, 22);
            lblPredictAngle2.SetBounds(valueX, row2, 46, 22);
            lblErrorValue2.SetBounds(valueX, row3, 46, 22);
            panel4.SetBounds(0, Math.Max(92, grpPredictResult.ClientSize.Height - 34), grpPredictResult.ClientSize.Width, 1);
            lblTrainStatus.SetBounds(labelX, bottomRow, 38, 20);
            lblTrainStatus2.SetBounds(labelX + 45, bottomRow, 92, 20);
        }

        private void ResetTrainingLossGraph()
        {
            trainingLossPoints.Clear();
            HideTrainingLossGraph();
        }

        private void HideTrainingLossGraph()
        {
            lblTrainingLossSummary.Text = "학습 완료 후 loss 그래프가 여기에 표시됩니다.";
            lblTrainingLossHover.Visible = false;
            trainingLossPlotBounds = Rectangle.Empty;
            panelTrainingLoss.Visible = true;
            LayoutTrainingLossOverlay();
            DrawTrainingLossGraph();
        }

        private bool CaptureTrainingLoss(string logText)
        {
            Match lossMatch = Regex.Match(logText, @"\bloss\s*[:=]\s*([-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)");
            if (!lossMatch.Success) return false;

            if (!double.TryParse(lossMatch.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double loss))
            {
                return false;
            }

            double? valLoss = null;
            Match valLossMatch = Regex.Match(logText, @"\bval_loss\s*[:=]\s*([-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)");
            if (valLossMatch.Success
                && double.TryParse(valLossMatch.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValLoss))
            {
                valLoss = parsedValLoss;
            }

            int frameIndex = MapLossOrderToFrameIndex(trainingLossPoints.Count, trainingLossPoints.Count + 1);
            trainingLossPoints.Add(new TrainingLossPoint(trainingLossPoints.Count + 1, frameIndex, loss, valLoss));
            return true;
        }

        private static bool IsEpochLossUpdateLog(string logText)
        {
            if (logText.Contains("val_loss")) return true;
            if (Regex.IsMatch(logText, @"^\s*Epoch\s+\d+/\d+")) return true;

            Match stepMatch = Regex.Match(logText, @"^\s*(\d+)/(\d+).*?\bloss\s*[:=]");
            return stepMatch.Success
                && int.TryParse(stepMatch.Groups[1].Value, out int currentStep)
                && int.TryParse(stepMatch.Groups[2].Value, out int totalStep)
                && currentStep >= totalStep;
        }

        private void ShowTrainingLossGraph(bool isFinal)
        {
            if (trainingLossPoints.Count == 0)
            {
                lblTrainingLossSummary.Text = $"학습 loss 데이터 없음  |  프레임 {tubFrames.Count:N0}개  |  loss가 낮아질수록 예측 오차가 줄어듭니다.";
            }
            else
            {
                TrainingLossPoint latest = trainingLossPoints[^1];
                int score = CalculateTrainingScore(latest.Loss);
                string title = isFinal ? "전체 학습 점수" : "학습 중 점수";
                lblTrainingLossSummary.Text =
                    $"{title} {score}점 ({GetTrainingScoreGrade(score)})  |  프레임 {tubFrames.Count:N0}개  |  현재 loss {latest.Loss:0.#####}  |  loss가 낮아질수록 예측 오차가 줄어듭니다.";
            }

            if (isFinal)
            {
                tabMain.SelectedTab = tabTrainTest;
            }

            panelTrainingLoss.Visible = true;
            panelTrainingLoss.BringToFront();
            ApplySelectedTabLayout();
            RedrawTrainingLossGraphAfterLayout();
        }

        private void RedrawTrainingLossGraphAfterLayout()
        {
            LayoutTrainingLossOverlay();
            DrawTrainingLossGraph();
            BeginInvoke((Action)(() =>
            {
                LayoutTrainingLossOverlay();
                DrawTrainingLossGraph();
            }));
        }

        private void DrawTrainingLossGraph()
        {
            picTrainingLossGraph.Invalidate();
        }

        private void picTrainingLossGraph_Paint(object? sender, PaintEventArgs e)
        {
            if (picTrainingLossGraph.ClientSize.Width <= 0 || picTrainingLossGraph.ClientSize.Height <= 0) return;

            Graphics graphics = e.Graphics;
            Size graphSize = picTrainingLossGraph.ClientSize;
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (trainingLossPoints.Count == 0)
            {
                trainingLossPlotBounds = Rectangle.Empty;
                DrawCenteredGraphMessage(graphics, graphSize, "학습 로그에서 loss 값을 찾지 못했습니다.");
                return;
            }

            Rectangle plot = new Rectangle(58, 8, Math.Max(80, graphSize.Width - 70), Math.Max(24, graphSize.Height - 18));
            trainingLossPlotBounds = plot;

            double minLoss = trainingLossPoints.Min(point => point.Loss);
            double maxLoss = trainingLossPoints.Max(point => point.Loss);
            if (Math.Abs(maxLoss - minLoss) < 1e-12)
            {
                double pad = Math.Max(0.001, Math.Abs(maxLoss) * 0.1);
                minLoss = Math.Max(0, minLoss - pad);
                maxLoss += pad;
            }

            using Pen framePen = new Pen(Color.FromArgb(120, 120, 120));
            using Pen gridPen = new Pen(Color.FromArgb(225, 225, 225));
            using Pen lossPen = new Pen(Color.RoyalBlue, 2f);
            using Brush textBrush = new SolidBrush(Color.FromArgb(55, 55, 55));
            using Font smallFont = new Font("나눔고딕", 8F);

            for (int i = 0; i <= 4; i++)
            {
                int y = plot.Top + (plot.Height * i / 4);
                double value = maxLoss - ((maxLoss - minLoss) * i / 4);
                graphics.DrawLine(gridPen, plot.Left, y, plot.Right, y);
                graphics.DrawString(value.ToString("0.####", CultureInfo.InvariantCulture), smallFont, textBrush, 8, y - 7);
            }

            graphics.DrawRectangle(framePen, plot);
            PointF[] points = trainingLossPoints
                .Select((_, index) => GetTrainingLossPointLocation(index, minLoss, maxLoss, plot))
                .ToArray();

            if (points.Length == 1)
            {
                graphics.FillEllipse(Brushes.RoyalBlue, points[0].X - 3, points[0].Y - 3, 6, 6);
            }
            else
            {
                graphics.DrawLines(lossPen, points);
            }

            graphics.DrawString("loss", smallFont, Brushes.RoyalBlue, plot.Left + 6, plot.Top + 4);
            graphics.DrawString("학습 진행", smallFont, textBrush, plot.Right - 56, plot.Bottom - 16);
        }

        private PointF GetTrainingLossPointLocation(int lossIndex, double minLoss, double maxLoss, Rectangle plot)
        {
            double xRatio = trainingLossPoints.Count <= 1 ? 0 : (double)lossIndex / (trainingLossPoints.Count - 1);
            double yRatio = (trainingLossPoints[lossIndex].Loss - minLoss) / (maxLoss - minLoss);
            float x = plot.Left + (float)(plot.Width * xRatio);
            float y = plot.Bottom - (float)(plot.Height * yRatio);
            return new PointF(x, y);
        }

        private void picTrainingLossGraph_MouseMove(object? sender, MouseEventArgs e)
        {
            int lossIndex = GetTrainingLossIndexAt(e.Location);
            if (lossIndex < 0)
            {
                lblTrainingLossHover.Visible = false;
                picTrainingLossGraph.Cursor = Cursors.Default;
                return;
            }

            TrainingLossPoint point = trainingLossPoints[lossIndex];
            int score = CalculateTrainingScore(point.Loss);
            lblTrainingLossHover.Text = $"loss {point.Loss:0.#####}\n점수 {score}점 ({GetTrainingScoreGrade(score)})";
            if (point.ValLoss.HasValue) lblTrainingLossHover.Text += $"\nval_loss {point.ValLoss.Value:0.#####}";

            int x = Math.Min(e.X + 14, picTrainingLossGraph.ClientSize.Width - lblTrainingLossHover.Width - 8);
            int y = Math.Min(e.Y + 14, picTrainingLossGraph.ClientSize.Height - lblTrainingLossHover.Height - 8);
            lblTrainingLossHover.Location = new Point(Math.Max(8, x), Math.Max(8, y));
            lblTrainingLossHover.Visible = true;
            lblTrainingLossHover.BringToFront();
            picTrainingLossGraph.Cursor = Cursors.Default;
        }

        private int GetTrainingLossIndexAt(Point location)
        {
            if (trainingLossPoints.Count == 0 || trainingLossPlotBounds == Rectangle.Empty) return -1;
            if (!trainingLossPlotBounds.Contains(location)) return -1;

            double ratio = trainingLossPlotBounds.Width <= 0
                ? 0
                : (double)(location.X - trainingLossPlotBounds.Left) / trainingLossPlotBounds.Width;
            int index = (int)Math.Round(ratio * (trainingLossPoints.Count - 1));
            return Math.Clamp(index, 0, trainingLossPoints.Count - 1);
        }

        private int MapLossOrderToFrameIndex(int lossOrder, int totalLossCount)
        {
            if (tubFrames.Count == 0) return -1;
            int estimatedTotal = Math.Max(1, totalLossCount);
            double ratio = estimatedTotal <= 1 ? 0 : (double)lossOrder / (estimatedTotal - 1);
            return Math.Clamp((int)Math.Round(ratio * (tubFrames.Count - 1)), 0, tubFrames.Count - 1);
        }

        private static int CalculateTrainingScore(double loss)
        {
            if (double.IsNaN(loss) || double.IsInfinity(loss) || loss < 0) return 0;
            double score = 100.0 / (1.0 + (loss * 100.0));
            return Math.Clamp((int)Math.Round(score), 0, 100);
        }

        private static string GetTrainingScoreGrade(int score)
        {
            if (score >= 90) return "매우 좋음";
            if (score >= 75) return "좋음";
            if (score >= 60) return "보통";
            if (score >= 40) return "낮음";
            return "나쁨";
        }

        private void btnSelectModel_Click(object sender, EventArgs e)
        {
            // ★ 학습하지 않고 모델을 선택할 경우를 대비한 강력한 경고 문구 추가!
            DialogResult res = MessageBox.Show("기본 모델 경로(models/mypilot.h5)를 자동으로 지정하시겠습니까?\n\n💡 주의: 아직 AI 학습을 한 번도 완료하지 않아 파일이 없다면, 테스트 실행 시 파이썬 오류가 발생합니다!\n\n(아니요를 누르면 윈도우에서 직접 선택합니다.)", "모델 설정", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Cancel) return;

            if (res == DialogResult.Yes)
            {
                modelPath = "models/mypilot.h5";
                txtLog.AppendText(Environment.NewLine + $"[Info] 기본 모델 자동 설정: {modelPath}");
                MessageBox.Show($"모델이 설정되었습니다:\n{modelPath}");
            }
            else if (res == DialogResult.No)
            {
                if (rdoRemote.Checked)
                {
                    MessageBox.Show("원격(서버) 모드에서는 윈도우의 파일을 지정할 수 없습니다.\n자동으로 기본 모델을 사용합니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    modelPath = "models/mypilot.h5";
                }
                else
                {
                    using (OpenFileDialog dlg = new OpenFileDialog() { Filter = "Keras Models (*.h5)|*.h5|All files (*.*)|*.*" })
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            modelPath = dlg.FileName;
                            MessageBox.Show($"모델이 선택되었습니다:\n{modelPath}");
                        }
                    }
                }
            }
        }

        private void btnSelectTestImage_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("현재 화면에 열려있는 'Tub 데이터'를 그대로 테스트 이미지로 사용하시겠습니까?\n\n(아니요를 누르면 다른 폴더를 직접 선택합니다.)", "테스트 이미지 설정", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Cancel) return;

            if (res == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(tubPath)) { MessageBox.Show("열려있는 Tub 데이터가 없습니다."); return; }
                testImagePath = rdoRemote.Checked ? "data/" : tubPath;
                ShowTestImagePreview(FindFirstTestImagePath());
                txtLog.AppendText(Environment.NewLine + $"[Info] 현재 데이터로 테스트 설정: {testImagePath}");
            }
            else if (res == DialogResult.No)
            {
                if (rdoRemote.Checked)
                {
                    MessageBox.Show("원격(서버) 모드에서는 윈도우 탐색기를 띄울 수 없습니다.\n자동으로 서버의 기본 data/ 폴더를 사용합니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    testImagePath = "data/";
                }
                else
                {
                    using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            testImagePath = dlg.SelectedPath;
                            ShowTestImagePreview(FindFirstTestImagePath());
                        }
                    }
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

            // ★ 수정됨: 요약 텍스트박스(txtTestLog) 지우고 오리지널 텍스트박스만 사용!
            if (txtTestLogOriginal != null) txtTestLogOriginal.Text = "";

            if (txtTestLogOriginal != null) txtTestLogOriginal.AppendText($"[Test] {Path.GetFileName(modelPath)} 예측 시작 (완료 시 자동 오차 추출)..." + Environment.NewLine);

            // 파일 저장용 보따리 초기화
            _originalLogBuilder.Clear();
            _summaryLogBuilder.Clear();
            _originalLogBuilder.AppendLine($"--- Donkeycar 테스트 원본 로그 ({DateTime.Now}) ---");
            _summaryLogBuilder.AppendLine($"--- Donkeycar 테스트 요약 로그 ({DateTime.Now}) ---");

            ResetModelTestResult();
            ShowTestImagePreview(FindFirstTestImagePath());

            ClearPendingModelTestLogs();
            isModelTestRunning = true;
            modelTestLogTimer.Start();

            UpdateStatusLabel("모델 테스트 중", Color.DarkOrange);
            _testCount++;
            if (infoTest != null) infoTest.Text = $"오늘 테스트 시도: {_testCount}회";

            bool useVenv = chkUseVenv != null ? chkUseVenv.Checked : true;
            _executor.ExecuteTest(configPath, modelPath, testImagePath, useVenv, (log) =>
            {
                modelTestLogQueue.Enqueue(log);
            });
        }

        private void ResetModelTestResult()
        {
            // 모델 테스트 시작 전 이전 결과를 지워서 새 로그 값만 보이게 한다.
            latestTestImagePath = "";
            latestTestRealAngle = null;
            latestTestPredictAngle = null;
            predictedAnglesByImageKey.Clear();
            lblRealAngle2.Text = "-";
            lblPredictAngle2.Text = "-";
            lblErrorValue2.Text = "-";
            lblTrainStatus2.Text = "테스트 중";
            lblTrainStatus2.ForeColor = Color.DarkOrange;
        }

        private void ModelTestLogTimer_Tick(object? sender, EventArgs e)
        {
            int processed = 0;
            while (processed < ModelTestLogMaxPerTick && modelTestLogQueue.TryDequeue(out string? logText))
            {
                HandleModelTestLog(logText);
                processed++;
            }

            if (!isModelTestRunning && modelTestLogQueue.IsEmpty)
            {
                modelTestLogTimer.Stop();
            }
        }

        private void ClearPendingModelTestLogs()
        {
            while (modelTestLogQueue.TryDequeue(out _))
            {
            }
        }

        private void HandleModelTestLog(string logText)
        {
            if (string.IsNullOrWhiteSpace(logText)) return;

            if (logText.StartsWith("[NO_MODEL]"))
            {
                isModelTestRunning = false;
                string missingPath = logText.Substring(10).Trim();
                MessageBox.Show($"지정된 경로에 AI 모델(.h5) 파일이 존재하지 않습니다.\n\n경로: {missingPath}\n\n아직 학습이 완료되지 않았거나 파일이 지워졌습니다. 먼저 [학습 시작]을 눌러 모델을 생성해 주세요.", "모델 파일 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateStatusLabel("대기 중", Color.Green);
                return;
            }

            bool isJunkLog = logText.Contains('\r') || logText.Contains('\b') || logText.Count(c => c == '=') > 10 || logText.Contains("\u001b");
            if (isJunkLog) return;

            // 1. 테스트 원본 로그만 출력 (요약 로그 출력 로직 완전 삭제)
            if (txtTestLogOriginal != null)
            {
                txtTestLogOriginal.AppendText(Environment.NewLine + logText);
                txtTestLogOriginal.ScrollToCaret();
            }
            _originalLogBuilder.AppendLine(logText);

            // 파일 저장을 위해 요약 빌더에도 같이 넣어둠
            _summaryLogBuilder.AppendLine(logText);

            string? imageReference = TryFindImageReferenceFromLog(logText);
            string? imagePath = TryFindImagePathFromLog(logText);

            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:real|actual|label|target|user/angle|실제\s*조향각|실제)\s*[:=]\s*(-?\d+(?:\.\d+)?)",
                    @"^\s*angle\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double realAngle))
            {
                latestTestRealAngle = realAngle;
                if (lblRealAngle2 != null) lblRealAngle2.Text = realAngle.ToString("0.000");
            }

            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:predict(?:ed)?|prediction|pred|pilot/angle|예측\s*조향각|예측)\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double predictAngle))
            {
                latestTestPredictAngle = predictAngle;
                string? predictionImageReference = imagePath ?? imageReference ?? latestTestImagePath;
                StorePredictedAngleForImage(predictionImageReference, predictAngle);

                if (lblPredictAngle2 != null) lblPredictAngle2.Text = predictAngle.ToString("0.000");

                picFrame.Invalidate();
            }

            double? parsedErrorValue = null;
            if (TryExtractLogValue(logText, new[]
                {
                    @"(?:error|loss|diff|오차)\s*[:=]\s*(-?\d+(?:\.\d+)?)"
                }, out double errorValue))
            {
                parsedErrorValue = Math.Abs(errorValue);
            }
            else if (latestTestRealAngle.HasValue && latestTestPredictAngle.HasValue)
            {
                parsedErrorValue = Math.Abs(latestTestRealAngle.Value - latestTestPredictAngle.Value);
            }

            if (parsedErrorValue.HasValue && lblErrorValue2 != null)
            {
                lblErrorValue2.Text = parsedErrorValue.Value.ToString("0.000");
            }

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                ShowTestImagePreview(imagePath, throttle: true);
            }
            UpdateModelTestResultForPreview(imagePath ?? "", imageReference, parsedErrorValue);

            if (logText.Contains("Finished", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("complete", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("완료", StringComparison.OrdinalIgnoreCase))
            {
                isModelTestRunning = false;
                lblTrainStatus2.Text = "테스트 완료";
                lblTrainStatus2.ForeColor = Color.Green;
                UpdateStatusLabel("대기 중", Color.Green);

                // ★ 추가됨: 테스트가 끝나면 AI 컴파일(오차 추출) 버튼을 코드가 알아서 눌러줌!
                if (txtTestLogOriginal != null)
                {
                    txtTestLogOriginal.AppendText(Environment.NewLine + "✅ 테스트 완료! 자동으로 오차 데이터를 추출합니다..." + Environment.NewLine);
                    txtTestLogOriginal.ScrollToCaret();
                }

                // 오차 추출 버튼 강제 클릭 이벤트 발생
                btnRunAICompile_Click(null, EventArgs.Empty);
            }
            else if (logText.Contains("Error", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("Exception", StringComparison.OrdinalIgnoreCase)
                || logText.Contains("Traceback", StringComparison.OrdinalIgnoreCase))
            {
                isModelTestRunning = false;
                lblTrainStatus2.Text = "오류";
                lblTrainStatus2.ForeColor = Color.Red;
                UpdateStatusLabel("대기 중", Color.Green);
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
            string? rawPath = TryFindImageReferenceFromLog(logText);
            if (string.IsNullOrWhiteSpace(rawPath)) return null;
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

        private static string? TryFindImageReferenceFromLog(string logText)
        {
            Match match = Regex.Match(logText, @"(?<path>[^\s""']+\.(?:jpg|jpeg|png|bmp))", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups["path"].Value.Trim() : null;
        }

        private void StorePredictedAngleForImage(string? imagePath, double predictAngle)
        {
            foreach (string key in GetImagePredictionKeys(imagePath))
            {
                predictedAnglesByImageKey[key] = predictAngle;
            }
        }

        private bool TryGetPredictedAngleForFrame(TubFrame frame, out double predictAngle)
        {
            foreach (string key in GetImagePredictionKeys(frame.ImagePath).Concat(GetImagePredictionKeys(frame.ImageFileName)))
            {
                if (predictedAnglesByImageKey.TryGetValue(key, out predictAngle))
                {
                    return true;
                }
            }

            predictAngle = 0;
            return false;
        }

        private bool TryFindTubFrameByImageReference(string? imagePath, out TubFrame? frame)
        {
            HashSet<string> keys = GetImagePredictionKeys(imagePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (TubFrame candidate in tubFrames)
            {
                foreach (string frameKey in GetImagePredictionKeys(candidate.ImagePath).Concat(GetImagePredictionKeys(candidate.ImageFileName)))
                {
                    if (keys.Contains(frameKey))
                    {
                        frame = candidate;
                        return true;
                    }
                }
            }

            frame = null;
            return false;
        }

        private void UpdatePredictionResultLabels(TubFrame frame)
        {
            if (TryGetPredictedAngleForFrame(frame, out double predictAngle))
            {
                UpdatePredictionResultLabels(frame, predictAngle);
                return;
            }

            lblRealAngle2.Text = frame.Angle.ToString("0.000");
            lblPredictAngle2.Text = "-";
            lblErrorValue2.Text = "-";
        }

        private void UpdatePredictionResultLabels(TubFrame frame, double predictAngle)
        {
            lblRealAngle2.Text = frame.Angle.ToString("0.000");
            lblPredictAngle2.Text = predictAngle.ToString("0.000");
            lblErrorValue2.Text = Math.Abs(frame.Angle - predictAngle).ToString("0.000");
        }

        private void UpdateModelTestResultForPreview(string imagePath, string? imageReference, double? parsedErrorValue)
        {
            string? predictionImageReference = imagePath ?? imageReference;
            if (TryFindTubFrameByImageReference(predictionImageReference, out TubFrame? previewFrame) && previewFrame is not null)
            {
                UpdatePredictionResultLabels(previewFrame);
                return;
            }

            lblRealAngle2.Text = latestTestRealAngle.HasValue
                ? latestTestRealAngle.Value.ToString("0.000")
                : "-";
            lblPredictAngle2.Text = latestTestPredictAngle.HasValue
                ? latestTestPredictAngle.Value.ToString("0.000")
                : "-";
            lblErrorValue2.Text = parsedErrorValue.HasValue
                ? parsedErrorValue.Value.ToString("0.000")
                : "-";
        }

        private static IEnumerable<string> GetImagePredictionKeys(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) yield break;

            string normalized = imagePath.Trim().Replace('/', Path.DirectorySeparatorChar);
            yield return normalized;

            string fileName = Path.GetFileName(normalized);
            if (!string.IsNullOrWhiteSpace(fileName)) yield return fileName;
        }

        private bool ShowTestImagePreview(string? imagePath, bool throttle = false)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath)) return false;
            if (string.Equals(latestTestImagePath, imagePath, StringComparison.OrdinalIgnoreCase)) return false;

            if (throttle)
            {
                long now = Environment.TickCount64;
                if (now - lastTestPreviewUpdateTick < ModelTestPreviewUpdateIntervalMs) return false;
                lastTestPreviewUpdateTick = now;
            }

            Image? oldImage = picTestImage.Image;
            picTestImage.Image = LoadImage(imagePath);
            oldImage?.Dispose();
            latestTestImagePath = imagePath;
            return true;
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
            int current = trackFrame.Value;
            bool selectingWhilePlaying = IsEnterSelectionActive();
            int next = selectingWhilePlaying
                ? AdvanceActiveIndex(current, playbackFrameStep)
                : AdvancePlayableIndex(current, playbackFrameStep);
            if (next < 0)
            {
                if (chkAutoPlay.Checked)
                {
                    int firstPlayable = selectingWhilePlaying ? FirstActiveIndex() : FirstPlayableIndex();
                    if (firstPlayable >= 0)
                    {
                        AddActiveFrameRangeToSelectionIfEnterHeld(current, selectingWhilePlaying ? LastActiveIndex() : LastPlayableIndex());
                        ShowFrame(firstPlayable, syncFrameListSelection: !HasSelection(), syncTimelineSelection: !HasSelection());
                        AddCurrentFrameToSelectionIfEnterHeld();
                        return;
                    }
                }

                SetPlaybackState(false);
                return;
            }

            AddActiveFrameRangeToSelectionIfEnterHeld(current, next);
            ShowFrame(next, syncFrameListSelection: !HasSelection(), syncTimelineSelection: !HasSelection());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_executor != null) _executor.Stop();
            CancelFrameImagePrefetch();
            frameImageCache.Clear();
            timelineThumbnailCache.Clear();
            ClearScaledButtonImages();
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

        private async Task LoadTubAsync(string selectedTubPath, bool quiet = false)
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

            if (catalogFiles.Length == 0 && recordFiles.Length == 0 && !quiet)
            {
                MessageBox.Show("catalog_*.catalog 또는 record_*.json 파일을 찾을 수 없습니다.", "Load Tub", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string actualTubPath = ResolveActualTubPath(selectedTubPath, catalogFiles, recordFiles, isOldRecordTub);
            tubPath = actualTubPath;
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
            CancelFrameImagePrefetch();
            frameImageCache.Clear();
            timelineThumbnailCache.Clear();
            selectedFrames.Clear();
            pendingRangeAnchor = -1;
            dragBaseSelection = null;
            UpdateSelectionLabel();

            try
            {
                // Tub 형식에 따라 신버전 catalog 파서 또는 구버전 record JSON 파서를 선택한다.
                TubLoadResult result = await Task.Run(() =>
                    isOldRecordTub
                        ? ReadOldTubFrames(actualTubPath, recordFiles)
                        : ReadTubFrames(actualTubPath, catalogFiles));
                tubFrames.AddRange(result.Frames);
                ResetTubView();

                if (tubFrames.Count > 0) ShowFrame(0);
                LoadTrashStore();
                RebuildTrashList();
                RenderTubGraph();
                foreach (string error in result.Errors) AddLog(error);
                AddLog($"Load Tub 완료: {tubFrames.Count}개 프레임 ({(isOldRecordTub ? "구버전 record JSON" : "catalog")} 형식)");

                // 순차적 안내 팝업창
                if (!quiet) MessageBox.Show($"주행 데이터 {tubFrames.Count}장을 성공적으로 불러왔습니다!\n\n[다음 단계 안내]\n1. 화면 하단의 슬라이더를 움직여 비정상적인 주행 사진이 있는지 확인하세요.\n2. 필요하다면 데이터 필터링/삭제 기능을 이용해 정리하세요.\n3. 정리가 완료되었다면 좌측 하단의 [학습] 버튼을 눌러 AI 훈련을 시작하세요.", "데이터 로드 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static string ResolveActualTubPath(string selectedTubPath, string[] catalogFiles, string[] recordFiles, bool isOldRecordTub)
        {
            // 사용자가 선택한 폴더 또는 catalog/record가 실제로 있는 폴더를 학습용 tub 경로로 사용한다.
            if (File.Exists(Path.Combine(selectedTubPath, "manifest.json")))
            {
                return selectedTubPath;
            }

            string[] tubFiles = isOldRecordTub ? recordFiles : catalogFiles;
            if (tubFiles.Length > 0)
            {
                return Path.GetDirectoryName(tubFiles[0]) ?? selectedTubPath;
            }

            return selectedTubPath;
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
            isEnterSelectingFrames = false;
            selectedFrames.Clear();
            pendingRangeAnchor = -1;
            selectionAnchorIndex = -1;
            dragBaseSelection = null;

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

        private TubFrame? FindFrameToShowAfterRemoval(HashSet<TubFrame> removeSet, int currentIndex)
        {
            if (tubFrames.Count == 0) return null;

            if (currentIndex >= 0 && currentIndex < tubFrames.Count && !removeSet.Contains(tubFrames[currentIndex]))
                return tubFrames[currentIndex];

            for (int i = Math.Max(0, currentIndex + 1); i < tubFrames.Count; i++)
                if (!removeSet.Contains(tubFrames[i])) return tubFrames[i];

            for (int i = Math.Min(currentIndex - 1, tubFrames.Count - 1); i >= 0; i--)
                if (!removeSet.Contains(tubFrames[i])) return tubFrames[i];

            return null;
        }

        private void RefreshTubViewAfterDeletion(TubFrame? frameToKeep, int fallbackIndex)
        {
            ResetTubView();
            RebuildTrashList();
            RenderTubGraph();

            if (tubFrames.Count == 0)
            {
                picFrame.Image = null;
                UpdateFrameInfoLabels(null, -1);
                SetPlaybackState(false);
                return;
            }

            int index = frameToKeep != null ? tubFrames.IndexOf(frameToKeep) : -1;
            if (index < 0) index = Math.Clamp(fallbackIndex, 0, tubFrames.Count - 1);
            ShowFrame(index);
        }

        private void RefreshTubViewAfterRestore(IEnumerable<TubFrame> restoredFrames, TubFrame? frameToKeep, int fallbackIndex)
        {
            foreach (TubFrame frame in restoredFrames)
            {
                if (!tubFrames.Any(existing =>
                    existing.FrameNumber == frame.FrameNumber &&
                    string.Equals(existing.ImageFileName, frame.ImageFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    tubFrames.Add(frame);
                }
            }

            tubFrames.Sort((a, b) =>
            {
                int frameCompare = a.FrameNumber.CompareTo(b.FrameNumber);
                if (frameCompare != 0) return frameCompare;
                return string.Compare(a.ImageFileName, b.ImageFileName, StringComparison.OrdinalIgnoreCase);
            });

            ResetTubView();
            RebuildTrashList();
            RenderTubGraph();

            if (tubFrames.Count == 0)
            {
                picFrame.Image = null;
                UpdateFrameInfoLabels(null, -1);
                SetPlaybackState(false);
                return;
            }

            int index = frameToKeep != null ? tubFrames.IndexOf(frameToKeep) : -1;
            if (index < 0) index = Math.Clamp(fallbackIndex, 0, tubFrames.Count - 1);
            ShowFrame(index);
        }

        private void UpdateTimelineForFrame(int frameIndex)
        {
            bool wasLoadingTimeline = isLoadingTimeline;
            isLoadingTimeline = true;
            try
            {
                PrepareTimelineBeforeImageLoad();

                int visibleCount = GetTimelineVisibleCount();
                int timelineStart = GetCenteredTimelineStart(frameIndex, visibleCount);
                if (timelineStart == currentTimelineStart && visibleCount == currentTimelineVisibleCount) return;

                currentTimelineStart = timelineStart;
                currentTimelineVisibleCount = visibleCount;
                ApplyTimelineIconSpacing();
                lvTimeline.BeginUpdate();
                lvTimeline.Items.Clear();
                timelineImages.Images.Clear();

                try
                {
                    int timelineEnd = Math.Min(tubFrames.Count, timelineStart + visibleCount);
                    for (int i = timelineStart; i < timelineEnd; i++)
                    {
                        TubFrame frame = tubFrames[i];
                        string imageKey = i.ToString();
                        timelineImages.Images.Add(imageKey, timelineThumbnailCache.GetClone(frame.ImagePath, currentTimelineThumbSize));
                        lvTimeline.Items.Add(new ListViewItem("", imageKey) { Tag = i, ToolTipText = frame.ToString() });
                    }
                }
                finally { lvTimeline.EndUpdate(); }

                RefreshTimelineNow();

                SyncTimelineSelectionFromModel();
            }
            finally
            {
                isLoadingTimeline = wasLoadingTimeline;
            }
        }

        private void RefreshTimelineNow()
        {
            if (!lvTimeline.IsHandleCreated) return;

            lvTimeline.Invalidate();
            lvTimeline.Update();
        }

        private void InvalidateTimelineFrame(int frameIndex)
        {
            if (!lvTimeline.IsHandleCreated || currentTimelineStart < 0) return;

            int visibleIndex = frameIndex - currentTimelineStart;
            if (visibleIndex < 0 || visibleIndex >= lvTimeline.Items.Count) return;

            lvTimeline.Invalidate(lvTimeline.Items[visibleIndex].Bounds);
        }

        private void InvalidateTimelineFrames(params int[] frameIndices)
        {
            foreach (int frameIndex in frameIndices)
            {
                InvalidateTimelineFrame(frameIndex);
            }
        }

        private int GetTimelineVisibleCount()
        {
            int availableWidth = Math.Max(0, lvTimeline.ClientSize.Width);
            int countByWidth = availableWidth / Math.Max(1, currentTimelineIconSpacingX);
            return Math.Clamp(countByWidth, 1, Math.Max(TimelineMaximumVisibleCount, countByWidth));
        }

        private int GetCenteredTimelineStart(int frameIndex, int visibleCount)
        {
            if (tubFrames.Count == 0 || visibleCount <= 0) return 0;

            int centerOffset = Math.Max(0, (visibleCount - 1) / 2);
            int maxStart = Math.Max(0, tubFrames.Count - visibleCount);
            return Math.Clamp(frameIndex - centerOffset, 0, maxStart);
        }

        private void lvTimeline_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            if (e.Item.ImageKey is not string imageKey || !timelineImages.Images.ContainsKey(imageKey)) return;

            Rectangle imageBounds = new Rectangle(e.Bounds.Location, currentTimelineThumbSize);
            imageBounds.Intersect(lvTimeline.ClientRectangle);
            if (imageBounds.Width <= 0 || imageBounds.Height <= 0) return;

            Image? timelineImage = timelineImages.Images[imageKey];
            if (timelineImage == null) return;
            e.Graphics.DrawImage(timelineImage, imageBounds);

            bool isExcludedFrame = e.Item.Tag is int selectedFrameIndex
                && selectedFrames.Contains(selectedFrameIndex);
            if (isExcludedFrame)
            {
                using SolidBrush excludedBrush = new SolidBrush(Color.FromArgb(120, Color.Gray));
                e.Graphics.FillRectangle(excludedBrush, imageBounds);

                int iconSize = Math.Clamp(Math.Min(imageBounds.Width, imageBounds.Height) / 3, 14, 28);
                Rectangle iconBounds = new Rectangle(
                    imageBounds.Left + (imageBounds.Width - iconSize) / 2,
                    imageBounds.Top + (imageBounds.Height - iconSize) / 2,
                    iconSize,
                    iconSize);
                DrawClosedEyeIcon(e.Graphics, iconBounds, Color.White);
            }
            else if (e.Item.Selected)
            {
                using SolidBrush selectedBrush = new SolidBrush(Color.FromArgb(80, Color.DodgerBlue));
                e.Graphics.FillRectangle(selectedBrush, imageBounds);
            }

            if (e.Item.Tag is int frameIndex && frameIndex == trackFrame.Value)
            {
                Rectangle borderBounds = imageBounds;
                borderBounds.Width -= 1;
                borderBounds.Height -= 1;

                using Pen currentFramePen = new Pen(Color.Orange, 3);
                e.Graphics.DrawRectangle(currentFramePen, borderBounds);
            }
        }

        private void ReloadTimelineForCurrentFrame()
        {
            ApplyTimelineIconSpacing();
            if (tubFrames.Count == 0) return;

            currentTimelineStart = -1;
            currentTimelineVisibleCount = -1;
            int frameIndex = Math.Min(trackFrame.Value, tubFrames.Count - 1);
            UpdateTimelineForFrame(frameIndex);
        }

        // 썸네일을 넣기 전에 아이콘 크기와 간격만 확정해서 로딩 중 레이아웃이 움직이지 않게 한다.
        private void PrepareTimelineBeforeImageLoad()
        {
            UpdateTimelineImageMetrics();
            if (panelTrainingLoss.Visible)
            {
                LayoutTrainingLossOverlay();
                panelTrainingLoss.BringToFront();
            }
        }

        private void lvTimeline_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || tubFrames.Count == 0) return;

            ListViewItem? item = lvTimeline.GetItemAt(e.X, e.Y);
            if (item?.Tag is not int frameIndex) return;

            CapturePendingSelectionUndo();
            isTimelineRangeDragging = true;
            hasTimelineRangeDragMoved = false;
            timelineDragStartIndex = frameIndex;
            timelineDragCurrentIndex = frameIndex;
            timelineDownModifiers = Control.ModifierKeys & (Keys.Shift | Keys.Control);
            dragBaseSelection = new SortedSet<int>(selectedFrames);
            lvTimeline.Capture = true;
        }

        private void lvTimeline_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isTimelineRangeDragging) return;

            int edgeMargin = 18;
            timelineDragEdgeDirection = e.X >= lvTimeline.ClientSize.Width - edgeMargin
                ? 1
                : e.X <= edgeMargin
                    ? -1
                    : 0;

            if (timelineDragEdgeDirection == 0)
            {
                timelineDragTimer.Stop();
            }
            else if (!timelineDragTimer.Enabled)
            {
                timelineDragTimer.Start();
            }

            int x = Math.Clamp(e.X, 0, Math.Max(0, lvTimeline.ClientSize.Width - 1));
            int y = Math.Clamp(e.Y, 0, Math.Max(0, lvTimeline.ClientSize.Height - 1));
            ListViewItem? item = lvTimeline.GetItemAt(x, y);
            if (item?.Tag is int frameIndex)
            {
                if (!hasTimelineRangeDragMoved && frameIndex == timelineDragStartIndex) return;

                hasTimelineRangeDragMoved = true;
                UpdateTimelineDragRange(frameIndex);
            }
        }

        private void lvTimeline_MouseUp(object? sender, MouseEventArgs e)
        {
            int clickedIndex = timelineDragStartIndex;
            bool shouldShowFrame = isTimelineRangeDragging && !hasTimelineRangeDragMoved;
            Keys mods = timelineDownModifiers;
            StopTimelineRangeDrag();

            if (shouldShowFrame && clickedIndex >= 0)
            {
                if ((mods & Keys.Control) != 0)
                {
                    // Ctrl+클릭: 프레임 목록과 같이 해당 프레임을 공유 선택에서 토글한다.
                    if (!selectedFrames.Remove(clickedIndex)) selectedFrames.Add(clickedIndex);
                    selectionAnchorIndex = clickedIndex;
                    RefreshSelectionVisuals();
                    ShowFrame(clickedIndex);
                }
                else if ((mods & Keys.Shift) != 0)
                {
                    // Shift+클릭: 프레임 목록과 같이 앵커~클릭 범위를 공유 선택에 누적한다.
                    int anchor = selectionAnchorIndex >= 0 ? selectionAnchorIndex : clickedIndex;
                    CommitRange(anchor, clickedIndex);
                    RefreshSelectionVisuals();
                    ShowFrame(clickedIndex);
                }
                else
                {
                    // 일반 클릭: 선택 해제 + 이동
                    ClearSelection();
                    ShowFrame(clickedIndex);
                    selectionAnchorIndex = clickedIndex;
                }
                CommitPendingSelectionUndo();
            }
            else
            {
                CommitPendingSelectionUndo();
            }
        }

        private void lvTimeline_MouseLeave(object? sender, EventArgs e)
        {
            if (!isTimelineRangeDragging)
            {
                timelineDragEdgeDirection = 0;
                timelineDragTimer.Stop();
            }
        }

        private void timelineDragTimer_Tick(object? sender, EventArgs e)
        {
            if (!isTimelineRangeDragging || !hasTimelineRangeDragMoved || timelineDragEdgeDirection == 0) return;

            int nextIndex = Math.Clamp(timelineDragCurrentIndex + timelineDragEdgeDirection, 0, tubFrames.Count - 1);
            if (nextIndex == timelineDragCurrentIndex) return;

            UpdateTimelineDragRange(nextIndex);
            UpdateTimelineForFrame(nextIndex);
        }

        private void StopTimelineRangeDrag()
        {
            if (!isTimelineRangeDragging) return;

            isTimelineRangeDragging = false;
            timelineDragEdgeDirection = 0;
            timelineDragTimer.Stop();
            lvTimeline.Capture = false;
            dragBaseSelection = null;
            timelineDownModifiers = Keys.None;
        }

        private void UpdateTimelineDragRange(int currentIndex)
        {
            timelineDragCurrentIndex = Math.Clamp(currentIndex, 0, tubFrames.Count - 1);
            int lo = Math.Min(timelineDragStartIndex, timelineDragCurrentIndex);
            int hi = Math.Max(timelineDragStartIndex, timelineDragCurrentIndex);
            selectedFrames.Clear();
            if (dragBaseSelection != null) selectedFrames.UnionWith(dragBaseSelection);
            for (int i = lo; i <= hi; i++) if (i >= 0 && i < tubFrames.Count) selectedFrames.Add(i);
            RefreshSelectionVisuals();
            ShowDragPreviewFrame(timelineDragCurrentIndex);
        }

        // 공유 선택을 모두 해제하고 시각화를 갱신한다.
        private void ClearSelection()
        {
            SelectionSnapshot? before = BeginSelectionUndo();
            selectedFrames.Clear();
            pendingRangeAnchor = -1;
            selectionAnchorIndex = -1;
            dragBaseSelection = null;
            isEnterSelectingFrames = false;
            timelineDragStartIndex = -1;
            timelineDragCurrentIndex = -1;
            hasTimelineRangeDragMoved = false;
            RefreshSelectionVisuals();
            CommitSelectionUndo(before);
        }

        // [a..b] 구간을 공유 선택에 누적한다.
        private void CommitRange(int a, int b)
        {
            if (tubFrames.Count == 0) return;
            int lo = Math.Clamp(Math.Min(a, b), 0, tubFrames.Count - 1);
            int hi = Math.Clamp(Math.Max(a, b), 0, tubFrames.Count - 1);
            for (int i = lo; i <= hi; i++) selectedFrames.Add(i);
        }

        private void AddCurrentFrameToSelection()
        {
            int index = trackFrame.Value;
            if (index < 0 || index >= tubFrames.Count) return;
            if (!selectedFrames.Add(index)) return;

            if (isEnterSelectingFrames && playTimer.Enabled)
                RefreshSelectionVisualsLight(new[] { index });
            else
                RefreshSelectionVisuals();
        }

        private void ClearAllFrameSelections()
        {
            ClearSelection();
            RefreshTimelineNow();
        }

        private bool HasSelection()
        {
            return selectedFrames.Count > 0;
        }

        private bool IsEnterKeyPhysicallyDown()
        {
            return IsKeyPhysicallyDown(Keys.Enter);
        }

        private bool IsEnterSelectionActive()
        {
            return isEnterSelectingFrames || IsEnterKeyPhysicallyDown();
        }

        private static bool IsKeyPhysicallyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & unchecked((short)0x8000)) != 0;
        }

        private void AddCurrentFrameToSelectionIfEnterHeld()
        {
            isEnterSelectingFrames = IsEnterSelectionActive();
            if (!isEnterSelectingFrames) return;

            AddCurrentFrameToSelection();
        }

        private void AddActiveFrameRangeToSelectionIfEnterHeld(int from, int to)
        {
            isEnterSelectingFrames = IsEnterSelectionActive();
            if (!isEnterSelectingFrames) return;
            if (from < 0 || to < 0 || from >= tubFrames.Count || to >= tubFrames.Count) return;

            CapturePendingSelectionUndo();
            int index = NextActiveIndex(from);
            if (index < 0) return;
            List<int> addedFrames = new();
            while (index >= 0 && index <= to)
            {
                if (selectedFrames.Add(index)) addedFrames.Add(index);
                index = NextActiveIndex(index);
            }

            if (addedFrames.Count == 0) return;
            if (isEnterSelectingFrames && playTimer.Enabled)
                RefreshSelectionVisualsLight(addedFrames);
            else
                RefreshSelectionVisuals();
        }

        // 선택 요약 라벨 텍스트.
        private string DescribeSelection()
        {
            string anchorSuffix = pendingRangeAnchor >= 0 ? $" · 지정 시작 @{pendingRangeAnchor}" : "";
            if (selectedFrames.Count == 0) return "선택: 없음" + anchorSuffix;

            int runs = 0, prev = int.MinValue;
            foreach (int i in selectedFrames)
            {
                if (i != prev + 1) runs++;
                prev = i;
            }
            return $"선택: {selectedFrames.Count}개 / {runs}범위" + anchorSuffix;
        }

        private void UpdateSelectionLabel()
        {
            lblRange.Text = DescribeSelection();
        }

        // 삭제/필터 대상 인덱스(선택이 있으면 그 집합, 없으면 전체).
        private List<int> GetTargetIndices()
        {
            if (selectedFrames.Count > 0) return selectedFrames.Where(i => i >= 0 && i < tubFrames.Count).ToList();
            return Enumerable.Range(0, tubFrames.Count).ToList();
        }

        // 공유 선택을 목록·타임라인·라벨에 반영하는 단일 동기화점(그래프 음영은 커밋에서 추가).
        private void RefreshSelectionVisuals()
        {
            if (isRefreshingSelectionVisuals) return;
            isRefreshingSelectionVisuals = true;
            try
            {
                lstFrames.SelectedIndexChanged -= lstFrames_SelectedIndexChanged;
                try
                {
                    lstFrames.BeginUpdate();
                    lstFrames.ClearSelected();
                    if (selectedFrames.Count > 0)
                    {
                        foreach (int i in selectedFrames)
                            if (i >= 0 && i < lstFrames.Items.Count) lstFrames.SetSelected(i, true);
                    }
                    else if (trackFrame.Value >= 0 && trackFrame.Value < lstFrames.Items.Count)
                    {
                        lstFrames.SetSelected(trackFrame.Value, true);
                    }
                    lstFrames.EndUpdate();
                }
                finally { lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged; }

                SyncTimelineSelectionFromModel();
                UpdateSelectionLabel();
                lstFrames.Invalidate();
            }
            finally { isRefreshingSelectionVisuals = false; }
            RenderTubGraph();
        }

        // 재생 중 Enter 연속 선택은 전체 그래프/전체 목록을 매 프레임 다시 만들지 않고,
        // 방금 선택된 프레임만 갱신해서 재생이 멈춘 것처럼 느려지는 상황을 줄인다.
        private void RefreshSelectionVisualsLight(IEnumerable<int> changedFrames)
        {
            if (isRefreshingSelectionVisuals) return;
            isRefreshingSelectionVisuals = true;
            try
            {
                List<int> changed = changedFrames
                    .Where(i => i >= 0 && i < tubFrames.Count)
                    .Distinct()
                    .ToList();

                lstFrames.SelectedIndexChanged -= lstFrames_SelectedIndexChanged;
                try
                {
                    lstFrames.BeginUpdate();
                    foreach (int i in changed)
                    {
                        if (i >= 0 && i < lstFrames.Items.Count) lstFrames.SetSelected(i, true);
                    }
                    lstFrames.EndUpdate();
                }
                finally { lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged; }

                foreach (int i in changed) InvalidateTimelineFrame(i);
                UpdateSelectionLabel();
                lstFrames.Invalidate();
            }
            finally { isRefreshingSelectionVisuals = false; }
        }

        // 타임라인 항목 선택/포커스를 공유 선택 기준으로 맞춘다(목록 갱신 없이 타임라인만).
        private void SyncTimelineSelectionFromModel()
        {
            if (lvTimeline.Items.Count == 0) return;

            isUpdatingTimelineSelection = true;
            try
            {
                foreach (ListViewItem item in lvTimeline.Items)
                {
                    if (item.Tag is not int frameIndex) continue;

                    bool shouldBeSelected = selectedFrames.Contains(frameIndex);
                    bool shouldBeFocused = frameIndex == trackFrame.Value;
                    if (item.Selected == shouldBeSelected && item.Focused == shouldBeFocused) continue;

                    item.Selected = shouldBeSelected;
                    item.Focused = shouldBeFocused;
                    lvTimeline.Invalidate(item.Bounds);
                }

                int visibleIndex = trackFrame.Value - currentTimelineStart;
                if (visibleIndex >= 0 && visibleIndex < lvTimeline.Items.Count)
                {
                    lvTimeline.Items[visibleIndex].EnsureVisible();
                }
            }
            finally
            {
                isUpdatingTimelineSelection = false;
            }
        }

        private void ShowFrame(int index, bool syncFrameListSelection = true, bool syncTimelineSelection = true, bool updateTimelineWindow = true)
        {
            if (index < 0 || index >= tubFrames.Count) return;

            TubFrame frame = tubFrames[index];
            int previousFrameIndex = trackFrame.Value;

            trackFrame.Value = index;

            // 프레임 이미지는 썸네일 타임라인 재로딩보다 먼저 갱신한다.
            // 5프레임 이후에는 타임라인이 현재 프레임을 가운데로 맞추며 자주 다시 로딩되므로,
            // 이미지 갱신이 뒤로 밀리면 방향키를 길게 눌렀을 때 화면 표시가 늦어질 수 있다.
            picFrame.Image = frameImageCache.Get(frame.ImagePath);

            if (!File.Exists(frame.ImagePath)) missingImageFrames.Add(index);

            UpdateFrameInfoLabels(frame, index);
            UpdatePredictionResultLabels(frame);
            picFrame.Invalidate();
            picFrame.Update();

            if (updateTimelineWindow)
            {
                UpdateTimelineForFrame(index);
            }

            ScheduleDeferredFrameUiUpdate(index, previousFrameIndex, syncFrameListSelection, syncTimelineSelection, updateTimelineWindow: false);
        }

        private void ScheduleDeferredFrameUiUpdate(int index, int previousFrameIndex, bool syncFrameListSelection, bool syncTimelineSelection, bool updateTimelineWindow)
        {
            deferredFrameIndex = index;
            deferredPreviousFrameIndex = previousFrameIndex;
            deferredSyncFrameListSelection = syncFrameListSelection;
            deferredSyncTimelineSelection = syncTimelineSelection;
            deferredUpdateTimelineWindow = updateTimelineWindow;

            deferredFrameUiTimer.Stop();
            deferredFrameUiTimer.Start();
        }

        private void DeferredFrameUiTimer_Tick(object? sender, EventArgs e)
        {
            deferredFrameUiTimer.Stop();
            ApplyDeferredFrameUiUpdate();
        }

        private void ApplyDeferredFrameUiUpdate()
        {
            int index = deferredFrameIndex;
            if (index < 0 || index >= tubFrames.Count) return;

            int previousFrameIndex = deferredPreviousFrameIndex;
            bool preserveAccumulatedSelection = HasSelection();
            bool syncFrameListSelection = deferredSyncFrameListSelection;
            bool syncTimelineSelection = deferredSyncTimelineSelection;
            bool updateTimelineWindow = deferredUpdateTimelineWindow;

            if (syncFrameListSelection && !preserveAccumulatedSelection)
            {
                lstFrames.SelectedIndexChanged -= lstFrames_SelectedIndexChanged;
                try
                {
                    if (index < lstFrames.Items.Count)
                    {
                        lstFrames.ClearSelected();
                        lstFrames.SetSelected(index, true);
                    }
                }
                finally
                {
                    lstFrames.SelectedIndexChanged += lstFrames_SelectedIndexChanged;
                }
            }

            if (updateTimelineWindow)
            {
                UpdateTimelineForFrame(index);
            }

            int timelineItemIndex = index - currentTimelineStart;
            if (preserveAccumulatedSelection)
            {
                SyncTimelineSelectionFromModel();
                InvalidateTimelineFrames(previousFrameIndex, index);
                ScheduleFrameImagePrefetch(index);
                return;
            }

            if (syncTimelineSelection && timelineItemIndex >= 0 && timelineItemIndex < lvTimeline.Items.Count)
            {
                isUpdatingTimelineSelection = true;
                try
                {
                    ListViewItem selectedItem = lvTimeline.Items[timelineItemIndex];
                    foreach (ListViewItem item in lvTimeline.Items)
                    {
                        bool shouldBeSelected = item == selectedItem;
                        bool shouldBeFocused = item == selectedItem;
                        if (item.Selected == shouldBeSelected && item.Focused == shouldBeFocused) continue;

                        item.Selected = shouldBeSelected;
                        item.Focused = shouldBeFocused;
                        lvTimeline.Invalidate(item.Bounds);
                    }
                    selectedItem.EnsureVisible();
                }
                finally
                {
                    isUpdatingTimelineSelection = false;
                }
            }

            InvalidateTimelineFrames(previousFrameIndex, index);
            ScheduleFrameImagePrefetch(index);
        }

        private void ScheduleFrameImagePrefetch(int frameIndex)
        {
            if (tubFrames.Count == 0) return;

            int imagePrefetchCount = playTimer.Enabled ? PlaybackFrameImagePrefetchCount : FrameImagePrefetchCount;
            int thumbnailPrefetchCount = Math.Max(currentTimelineVisibleCount, TimelineMinimumVisibleCount)
                + (playTimer.Enabled ? TimelineMaximumVisibleCount : TimelineMinimumVisibleCount);
            List<string> imagePaths = GetNextFrameImagePaths(frameIndex, imagePrefetchCount);
            List<string> thumbnailPaths = GetNextFrameImagePaths(frameIndex, thumbnailPrefetchCount);
            if (imagePaths.Count == 0 && thumbnailPaths.Count == 0) return;

            CancellationTokenSource cts = new();
            Size thumbnailSize = currentTimelineThumbSize;
            lock (frameImagePrefetchLock)
            {
                frameImagePrefetchCts?.Cancel();
                frameImagePrefetchCts = cts;
            }

            _ = Task.Run(() =>
            {
                CancellationToken token = cts.Token;
                try
                {
                    PrefetchFrameImages(imagePaths, token);
                    PrefetchTimelineThumbnails(thumbnailPaths, thumbnailSize, token);
                }
                catch (OperationCanceledException)
                {
                }
                catch (ObjectDisposedException)
                {
                }
                catch
                {
                    // 미리 읽기 실패는 실제 프레임 표시 흐름을 막지 않도록 백그라운드에서만 정리한다.
                }
                finally
                {
                    lock (frameImagePrefetchLock)
                    {
                        if (ReferenceEquals(frameImagePrefetchCts, cts))
                        {
                            frameImagePrefetchCts = null;
                        }
                    }

                    cts.Dispose();
                }
            });
        }

        private List<string> GetNextFrameImagePaths(int frameIndex, int count)
        {
            List<string> imagePaths = new();
            int current = frameIndex;

            for (int i = 0; i < count; i++)
            {
                int next = NextActiveIndex(current);
                if (next < 0) break;

                string imagePath = tubFrames[next].ImagePath;
                if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
                {
                    imagePaths.Add(imagePath);
                }

                current = next;
            }

            return imagePaths;
        }

        private void PrefetchFrameImages(IEnumerable<string> imagePaths, CancellationToken cancellationToken)
        {
            foreach (string imagePath in imagePaths)
            {
                if (cancellationToken.IsCancellationRequested) return;
                if (frameImageCache.Contains(imagePath)) continue;

                try
                {
                    frameImageCache.Preload(imagePath, cancellationToken);
                }
                catch
                {
                    // 실제 표시 시점의 LoadImage가 누락 이미지를 처리하므로, 미리 읽기 실패는 조용히 건너뛴다.
                }
            }
        }

        private void PrefetchTimelineThumbnails(IEnumerable<string> imagePaths, Size thumbnailSize, CancellationToken cancellationToken)
        {
            foreach (string imagePath in imagePaths)
            {
                if (cancellationToken.IsCancellationRequested) return;
                if (timelineThumbnailCache.Contains(imagePath, thumbnailSize)) continue;

                try
                {
                    timelineThumbnailCache.Preload(imagePath, thumbnailSize, cancellationToken);
                }
                catch
                {
                    // 타임라인 표시 시점에 누락 썸네일을 다시 처리하므로, 미리 읽기 실패는 조용히 건너뛴다.
                }
            }
        }

        private void CancelFrameImagePrefetch()
        {
            CancellationTokenSource? cts;
            lock (frameImagePrefetchLock)
            {
                cts = frameImagePrefetchCts;
                frameImagePrefetchCts = null;
            }

            cts?.Cancel();
        }


        private void btnDisconnect_Click(object? sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                DialogResult result = MessageBox.Show($"현재 접속 중인 원격 서버의 연결을 끊고 로컬 모드로 전환하시겠습니까?", "로그아웃", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _isUpdatingRadio = true;
                    rdoLocal.Checked = true;
                    _executor.Stop();
                    _executor = new ICE.LocalExecutor();
                    _isUpdatingRadio = false;

                    loggedInUser = "";
                    if (lblUser2 != null) { lblUser2.Text = "없음"; lblUser2.ForeColor = Color.Black; }
                    UpdateStatusLabel("로컬 대기중", Color.Black);
                }
            }
        }

        private void picFrame_Paint(object? sender, PaintEventArgs e)
        {
            if (picFrame.Image == null || trackFrame.Value < 0 || trackFrame.Value >= tubFrames.Count) return;

            Rectangle imageRect = GetZoomedImageRectangle(picFrame);
            if (imageRect.Width <= 0 || imageRect.Height <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            TubFrame frame = tubFrames[trackFrame.Value];
            if (chkShowRealAngle.Checked)
            {
                DrawSteeringAngleLine(e.Graphics, imageRect, frame.Angle, Color.LimeGreen, "실제", 1.0f, -1);
            }

            if (chkShowPredictAngle.Checked && TryGetPredictedAngleForFrame(frame, out double framePredictAngle))
            {
                DrawSteeringAngleLine(e.Graphics, imageRect, framePredictAngle, Color.DeepSkyBlue, "예측", 0.72f, 1);
            }
        }

        private static Rectangle GetZoomedImageRectangle(PictureBox pictureBox)
        {
            if (pictureBox.Image == null) return Rectangle.Empty;

            float imageRatio = (float)pictureBox.Image.Width / pictureBox.Image.Height;
            float boxRatio = (float)pictureBox.ClientSize.Width / pictureBox.ClientSize.Height;

            if (imageRatio > boxRatio)
            {
                int width = pictureBox.ClientSize.Width;
                int height = (int)(width / imageRatio);
                int top = (pictureBox.ClientSize.Height - height) / 2;
                return new Rectangle(0, top, width, height);
            }
            else
            {
                int height = pictureBox.ClientSize.Height;
                int width = (int)(height * imageRatio);
                int left = (pictureBox.ClientSize.Width - width) / 2;
                return new Rectangle(left, 0, width, height);
            }
        }

        private static void DrawSteeringAngleLine(Graphics graphics, Rectangle imageRect, double angle, Color color, string label, float lengthScale, int labelSide)
        {
            double clampedAngle = Math.Clamp(angle, -1.0, 1.0);
            PointF start = new PointF(imageRect.Left + imageRect.Width / 2f, imageRect.Bottom - imageRect.Height * 0.12f);
            float lineLength = imageRect.Height * 0.45f * lengthScale;
            float endX = start.X + (float)(clampedAngle * imageRect.Width * 0.35 * lengthScale);
            float endY = start.Y - lineLength;
            PointF end = new PointF(endX, endY);

            using Pen shadowPen = new Pen(Color.FromArgb(150, Color.Black), 7)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            using Pen linePen = new Pen(color, 4)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            graphics.DrawLine(shadowPen, start, end);
            graphics.DrawLine(linePen, start, end);

            using Brush textBack = new SolidBrush(Color.FromArgb(170, Color.Black));
            using Brush textBrush = new SolidBrush(Color.White);
            using Font font = new Font("나눔고딕", 9F, FontStyle.Bold);
            string text = $"{label} {angle:0.000}";
            SizeF textSize = graphics.MeasureString(text, font);
            float labelX = labelSide < 0 ? end.X - textSize.Width - 16 : end.X + 8;
            float labelY = end.Y - textSize.Height / 2 + (labelSide > 0 ? textSize.Height + 6 : -textSize.Height - 6);
            labelX = Math.Clamp(labelX, imageRect.Left + 4, imageRect.Right - textSize.Width - 12);
            labelY = Math.Clamp(labelY, imageRect.Top + 4, imageRect.Bottom - textSize.Height - 8);
            RectangleF labelRect = new RectangleF(labelX, labelY, textSize.Width + 8, textSize.Height + 4);
            graphics.FillRectangle(textBack, labelRect);
            graphics.DrawString(text, font, textBrush, labelRect.Left + 4, labelRect.Top + 2);
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

        private static Image CreateTimelineThumbnail(string imagePath, Size thumbnailSize)
        {
            if (!File.Exists(imagePath))
            {
                Bitmap missing = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);
                using Graphics graphics = Graphics.FromImage(missing);
                graphics.Clear(Color.Black);
                using Pen pen = new Pen(Color.DarkGray);
                graphics.DrawRectangle(pen, 0, 0, thumbnailSize.Width - 1, thumbnailSize.Height - 1);
                return missing;
            }
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using Image source = Image.FromStream(stream);
            return new Bitmap(source, thumbnailSize);
        }

        // ListView 기본 큰 아이콘 간격이 넓어서 썸네일이 한 줄에 촘촘히 보이도록 직접 조정한다.
        private void ApplyTimelineIconSpacing()
        {
            if (!lvTimeline.IsHandleCreated) return;
            SendMessage(lvTimeline.Handle, LvmSetIconSpacing, IntPtr.Zero, MakeLParam(currentTimelineIconSpacingX, currentTimelineIconSpacingY));
        }

        // 썸네일이 서로 겹치지 않도록 현재 타임라인 폭에 맞춰 이미지 크기와 아이콘 간격을 함께 계산한다.
        private void UpdateTimelineImageMetrics()
        {
            int availableWidth = Math.Max(1, lvTimeline.ClientSize.Width);
            int availableHeight = Math.Max(1, lvTimeline.ClientSize.Height);
            int thumbHeight = Math.Clamp(
                availableHeight,
                36,
                TimelineThumbHeight);
            int heightBasedWidth = thumbHeight * TimelineThumbWidth / TimelineThumbHeight;
            int thumbWidth = Math.Clamp(
                heightBasedWidth,
                56,
                TimelineThumbWidth);

            Size newSize = new(thumbWidth, thumbHeight);
            if (newSize != currentTimelineThumbSize)
            {
                currentTimelineThumbSize = newSize;
                timelineImages.Images.Clear();
                timelineThumbnailCache.Clear();
                timelineImages.ImageSize = newSize;
                currentTimelineStart = -1;
                currentTimelineVisibleCount = -1;
            }

            currentTimelineIconSpacingX = currentTimelineThumbSize.Width + TimelineIconGap;
            currentTimelineIconSpacingY = currentTimelineThumbSize.Height + TimelineIconGap;
            ApplyTimelineIconSpacing();
        }


        private static IntPtr MakeLParam(int lowWord, int highWord)
        {
            return (IntPtr)((highWord << 16) | (lowWord & 0xffff));
        }

        private const int LvmFirst = 0x1000;
        private const int LvmSetIconSpacing = LvmFirst + 53;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

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
            picDataGraph.MouseClick += picDataGraph_MouseClick;
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

            BeginInvoke(new Action(() =>
            {
                ApplySelectedTabLayout();
            }));
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

            Rectangle plot = new Rectangle(54, 18, Math.Max(10, bitmap.Width - 108), Math.Max(10, bitmap.Height - 56));
            graphPlotBounds = plot;
            graphVisibleFrames = visibleFrames;
            DrawGraphFrame(graphics, plot);
            DrawSelectionShading(graphics, plot, visibleFrames.Count);

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

        // 공유 선택 구간을 그래프에 반투명 음영으로 표시한다(연속 구간을 하나의 띠로 합침).
        private void DrawSelectionShading(Graphics graphics, Rectangle plot, int count)
        {
            if (selectedFrames.Count == 0 || count <= 0) return;
            using SolidBrush brush = new SolidBrush(Color.FromArgb(48, Color.DodgerBlue));

            int runStart = int.MinValue, prev = int.MinValue;
            foreach (int idx in selectedFrames)
            {
                if (idx < 0 || idx >= count) continue;
                if (idx != prev + 1)
                {
                    if (runStart >= 0) FillSelectionBand(graphics, brush, plot, count, runStart, prev);
                    runStart = idx;
                }
                prev = idx;
            }
            if (runStart >= 0) FillSelectionBand(graphics, brush, plot, count, runStart, prev);
        }

        private static void FillSelectionBand(Graphics graphics, Brush brush, Rectangle plot, int count, int a, int b)
        {
            float xa = count == 1 ? plot.Left : plot.Left + (float)(a * plot.Width / (double)(count - 1));
            float xb = count == 1 ? plot.Left : plot.Left + (float)(b * plot.Width / (double)(count - 1));
            float left = Math.Min(xa, xb);
            float width = Math.Max(2f, Math.Abs(xb - xa));
            graphics.FillRectangle(brush, left, plot.Top, width, plot.Height);
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
            TubFrame? frame = GetGraphFrameAt(e.Location);
            if (frame == null)
            {
                lblGraphHover.Visible = false;
                picDataGraph.Cursor = Cursors.Default;
                return;
            }

            lblGraphHover.Text = $"프레임 {frame.FrameNumber:D6}\n조향각 {frame.Angle:0.000}\n속도 {frame.Throttle:0.000}";

            int x = Math.Min(e.X + 14, picDataGraph.ClientSize.Width - lblGraphHover.Width - 8);
            int y = Math.Min(e.Y + 14, picDataGraph.ClientSize.Height - lblGraphHover.Height - 8);
            lblGraphHover.Location = new Point(Math.Max(8, x), Math.Max(8, y));
            lblGraphHover.Visible = true;
            lblGraphHover.BringToFront();
            picDataGraph.Cursor = Cursors.Hand;
        }

        private void picDataGraph_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            TubFrame? frame = GetGraphFrameAt(e.Location);
            if (frame == null) return;

            int tubIndex = tubFrames.IndexOf(frame);
            if (tubIndex < 0)
            {
                tubIndex = tubFrames.FindIndex(item => item.FrameNumber == frame.FrameNumber);
            }

            if (tubIndex >= 0)
            {
                ShowFrame(tubIndex);
            }
        }

        private TubFrame? GetGraphFrameAt(Point location)
        {
            if (graphVisibleFrames.Count == 0 || graphPlotBounds == Rectangle.Empty || !graphPlotBounds.Contains(location))
            {
                return null;
            }

            double ratio = (location.X - graphPlotBounds.Left) / (double)Math.Max(1, graphPlotBounds.Width);
            int frameIndex = (int)Math.Round(ratio * (graphVisibleFrames.Count - 1));
            frameIndex = Math.Clamp(frameIndex, 0, graphVisibleFrames.Count - 1);
            return graphVisibleFrames[frameIndex];
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
        private void lstFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isRefreshingSelectionVisuals || isFrameListMouseDragging) return;
            if (lstFrames.SelectedIndex >= 0)
            {
                ShowFrame(lstFrames.SelectedIndex, syncFrameListSelection: false);
            }
        }

        private void lvTimeline_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingTimelineSelection || isTimelineRangeDragging || isRefreshingSelectionVisuals) return;
            if (lvTimeline.SelectedItems.Count > 0 && lvTimeline.SelectedItems[0].Tag is int index)
            {
                ShowFrame(index);
            }
        }

        private void lstFrames_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstFrames.Items.Count) return;

            e.DrawBackground();
            bool isExcludedFrame = selectedFrames.Contains(e.Index);
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color textColor = isSelected ? SystemColors.HighlightText : lstFrames.ForeColor;
            using Brush textBrush = new SolidBrush(textColor);

            Rectangle textBounds = e.Bounds;
            int iconSize = Math.Max(12, Math.Min(16, e.Bounds.Height - 4));
            if (isExcludedFrame)
            {
                textBounds.Width = Math.Max(0, textBounds.Width - iconSize - 8);
            }

            TextRenderer.DrawText(
                e.Graphics,
                lstFrames.Items[e.Index]?.ToString() ?? "",
                e.Font,
                textBounds,
                textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

            if (isExcludedFrame)
            {
                Rectangle iconBounds = new Rectangle(
                    e.Bounds.Right - iconSize - 4,
                    e.Bounds.Top + (e.Bounds.Height - iconSize) / 2,
                    iconSize,
                    iconSize);
                DrawClosedEyeIcon(e.Graphics, iconBounds, isSelected ? SystemColors.HighlightText : Color.DimGray);
            }

            e.DrawFocusRectangle();
        }

        private static void DrawClosedEyeIcon(Graphics graphics, Rectangle bounds, Color color)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float scaleX = bounds.Width / 64f;
            float scaleY = bounds.Height / 64f;

            PointF P(float x, float y) => new(bounds.Left + x * scaleX, bounds.Top + y * scaleY);
            using Pen pen = new Pen(color, Math.Max(1.6f, bounds.Width / 14f))
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
                LineJoin = LineJoin.Round
            };

            using GraphicsPath eyePath = new GraphicsPath();
            eyePath.AddBezier(P(10, 32), P(16, 23), P(24, 19.7f), P(32, 19.7f));
            eyePath.AddBezier(P(32, 19.7f), P(40, 19.7f), P(48, 23), P(54, 32));
            graphics.DrawPath(pen, eyePath);

            graphics.DrawLine(pen, P(17.5f, 40.5f), P(22.2f, 35.8f));
            graphics.DrawLine(pen, P(28.8f, 44.2f), P(30.1f, 37.7f));
            graphics.DrawLine(pen, P(46.5f, 40.5f), P(41.8f, 35.8f));
            graphics.DrawLine(pen, P(35.2f, 44.2f), P(33.9f, 37.7f));
        }

        // 목록 드래그 = 공유 선택에 범위 누적, 단순 클릭 = 선택 해제 + 이동(트래시 목록과 동일한 클릭-vs-드래그).
        private void lstFrames_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || tubFrames.Count == 0) return;

            isFrameListMouseDragging = true;
            frameListDragStartIndex = lstFrames.IndexFromPoint(e.Location);
            lastFrameListPreviewIndex = frameListDragStartIndex;
            frameListDownModifiers = Control.ModifierKeys & (Keys.Shift | Keys.Control);
            if (frameListDragStartIndex >= 0) CapturePendingSelectionUndo();
            dragBaseSelection = new SortedSet<int>(selectedFrames);
            if (frameListDragStartIndex >= 0) ShowDragPreviewFrame(frameListDragStartIndex);
        }

        private void lstFrames_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!isFrameListMouseDragging || e.Button != MouseButtons.Left) return;

            int index = lstFrames.IndexFromPoint(e.Location);
            if (index < 0 || index >= tubFrames.Count || index == lastFrameListPreviewIndex) return;
            lastFrameListPreviewIndex = index;

            if (frameListDragStartIndex >= 0 && index != frameListDragStartIndex)
            {
                selectedFrames.Clear();
                if (dragBaseSelection != null) selectedFrames.UnionWith(dragBaseSelection);
                CommitRange(frameListDragStartIndex, index);
                RefreshSelectionVisuals();
            }
            ShowDragPreviewFrame(index);
        }

        private void lstFrames_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isFrameListMouseDragging) return;

            bool moved = lastFrameListPreviewIndex >= 0 && lastFrameListPreviewIndex != frameListDragStartIndex;
            int clicked = frameListDragStartIndex;
            Keys mods = frameListDownModifiers;
            isFrameListMouseDragging = false;
            dragBaseSelection = null;
            frameListDragStartIndex = -1;
            lastFrameListPreviewIndex = -1;
            frameListDownModifiers = Keys.None;

            if (moved)
            {
                selectionAnchorIndex = clicked;   // 드래그 후 앵커는 시작점
                CommitPendingSelectionUndo();
                return;
            }
            if (clicked < 0 || clicked >= tubFrames.Count)
            {
                CommitPendingSelectionUndo();
                return;
            }

            if ((mods & Keys.Control) != 0)
            {
                // Ctrl+클릭: 해당 프레임을 공유 선택에서 토글
                if (!selectedFrames.Remove(clicked)) selectedFrames.Add(clicked);
                selectionAnchorIndex = clicked;
                RefreshSelectionVisuals();
                ShowFrame(clicked);
            }
            else if ((mods & Keys.Shift) != 0)
            {
                // Shift+클릭: 앵커~클릭 범위를 공유 선택에 누적
                int anchor = selectionAnchorIndex >= 0 ? selectionAnchorIndex : clicked;
                CommitRange(anchor, clicked);
                RefreshSelectionVisuals();
                ShowFrame(clicked);
            }
            else
            {
                // 일반 클릭: 선택 해제 + 이동
                ClearSelection();
                ShowFrame(clicked);
                selectionAnchorIndex = clicked;
            }

            CommitPendingSelectionUndo();
        }

        private void ShowDragPreviewFrame(int index)
        {
            if (index < 0 || index >= tubFrames.Count || trackFrame.Value == index) return;

            ShowFrame(index, syncFrameListSelection: false, syncTimelineSelection: false, updateTimelineWindow: false);
        }

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

        private bool IsPlayableFrame(int index)
        {
            return index >= 0
                && index < tubFrames.Count
                && !tubFrames[index].Deleted
                && !selectedFrames.Contains(index);
        }

        private int NextPlayableIndex(int from)
        {
            for (int i = from + 1; i < tubFrames.Count; i++)
                if (IsPlayableFrame(i)) return i;
            return -1;
        }

        private int AdvancePlayableIndex(int from, int frameStep)
        {
            int current = from;
            int steps = Math.Max(1, frameStep);

            for (int i = 0; i < steps; i++)
            {
                int next = NextPlayableIndex(current);
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

        private int FirstPlayableIndex()
        {
            for (int i = 0; i < tubFrames.Count; i++)
                if (IsPlayableFrame(i)) return i;
            return -1;
        }

        private int LastActiveIndex()
        {
            for (int i = tubFrames.Count - 1; i >= 0; i--)
                if (!tubFrames[i].Deleted) return i;
            return -1;
        }

        private int LastPlayableIndex()
        {
            for (int i = tubFrames.Count - 1; i >= 0; i--)
                if (IsPlayableFrame(i)) return i;
            return -1;
        }

        // 안 쓰는 빈 이벤트 모음 (에러 방지용)
        private void lblConfigPath_Click(object sender, EventArgs e) { }
        private void groupTubNavigator_Enter(object sender, EventArgs e) { }
        private void lvTimeline_SelectedIndexChanged_1(object sender, EventArgs e) { }

        // ==========================================
        // 5. 데이터 정리: 휴지통(삭제 상태) 관리
        // ==========================================
        // 선택된 프레임들을 catalog에서 즉시 제거하고 디스크 휴지통(deleted/trash.jsonl + images)으로 옮긴다.
        // 학습·테스트가 catalog를 직접 읽으므로 양쪽 모두에서 즉시 제외된다. 반환: 옮긴 개수.
        private int DeleteFramesToTrash(List<(TubFrame frame, string reason)> targets)
        {
            if (targets.Count == 0 || string.IsNullOrWhiteSpace(tubPath)) return 0;
            Directory.CreateDirectory(DeletedImagesDir);

            // 이미지명 → 새 휴지통 항목(중복 이미지명은 한 번만)
            List<(TrashEntry entry, string srcImagePath)> pending = new();
            Dictionary<string, TrashEntry> byImage = new(StringComparer.OrdinalIgnoreCase);
            foreach ((TubFrame frame, string reason) in targets)
            {
                if (string.IsNullOrEmpty(frame.ImageFileName) || byImage.ContainsKey(frame.ImageFileName)) continue;
                TrashEntry entry = new TrashEntry
                {
                    Frame = frame.FrameNumber,
                    Reason = reason,
                    Image = frame.ImageFileName,
                    Angle = frame.Angle,
                    Throttle = frame.Throttle,
                    Index = tubFrames.IndexOf(frame),
                    SourceDataPath = frame.SourceDataPath,
                };
                byImage[frame.ImageFileName] = entry;
                pending.Add((entry, frame.ImagePath));
            }
            if (pending.Count == 0) return 0;

            // 1) catalog를 재작성하며 삭제 대상 줄(원본)을 캡처한다. 남은 줄이 0이면 catalog 파일째 삭제.
            foreach (string catalogFile in Directory.GetFiles(tubPath, "catalog_*.catalog", SearchOption.AllDirectories))
            {
                bool changed = false;
                List<string> kept = new List<string>();
                foreach (string line in File.ReadLines(catalogFile))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string? img = TryGetImageName(line);
                    if (img != null && byImage.TryGetValue(img, out TrashEntry? e))
                    {
                        e.Line = line;
                        e.Catalog = Path.GetRelativePath(tubPath, catalogFile).Replace('\\', '/');
                        changed = true;
                        continue;
                    }
                    kept.Add(line);
                }
                if (!changed) continue;
                DeleteCatalogManifestFor(catalogFile);
                if (kept.Count == 0) File.Delete(catalogFile);
                else File.WriteAllText(catalogFile, string.Join("\n", kept) + "\n");
            }

            // 2) 이미지 이동 + (구버전이면 record json 이동) + 휴지통 등록
            foreach ((TrashEntry entry, string srcImagePath) in pending)
            {
                string dest = GetDeletedFilePath(DeletedImagesDir, string.IsNullOrEmpty(srcImagePath) ? entry.Image : srcImagePath);
                if (!string.IsNullOrEmpty(srcImagePath) && File.Exists(srcImagePath)) File.Move(srcImagePath, dest);
                entry.DeletedImageName = Path.GetFileName(dest);
                entry.DeletedImagePath = dest;

                if (IsOldRecordFile(entry.SourceDataPath) && File.Exists(entry.SourceDataPath))
                {
                    string recDest = GetDeletedFilePath(DeletedDir, entry.SourceDataPath);
                    File.Move(entry.SourceDataPath, recDest);
                    entry.RecordBackup = recDest;
                }
                trashStore.Add(entry);
            }

            WriteTrashStore();
            SyncManifestToCatalogs();

            // 3) tubFrames에서 제거(재로딩 전 즉시 정합)
            HashSet<TubFrame> removeSet = new HashSet<TubFrame>(targets.Select(t => t.frame));
            tubFrames.RemoveAll(f => removeSet.Contains(f));
            return pending.Count;
        }

        // 휴지통 항목을 catalog로 되돌린다(줄 재삽입 + 이미지 복귀). trashStore에서 제거.
        private void RestoreEntry(TrashEntry entry)
        {
            // 1) 이미지 원위치로
            if (!string.IsNullOrEmpty(entry.DeletedImagePath) && File.Exists(entry.DeletedImagePath))
            {
                string imagesBase = GetImageBasePath(tubPath);
                Directory.CreateDirectory(imagesBase);
                string destImg = Path.Combine(imagesBase, entry.Image);
                if (!File.Exists(destImg)) File.Move(entry.DeletedImagePath, destImg);
            }
            // 2) catalog 줄 재삽입(신버전)
            if (!string.IsNullOrEmpty(entry.Line))
            {
                string catalogName = string.IsNullOrEmpty(entry.Catalog) ? "catalog_0.catalog" : entry.Catalog;
                string catalogPath = Path.Combine(tubPath, catalogName.Replace('/', Path.DirectorySeparatorChar));
                string? dir = Path.GetDirectoryName(catalogPath);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                InsertLineIntoCatalog(catalogPath, entry.Line);
                DeleteCatalogManifestFor(catalogPath);
            }
            // 3) 구버전 record json 되돌리기
            if (IsOldRecordFile(entry.SourceDataPath) && !string.IsNullOrEmpty(entry.RecordBackup) && File.Exists(entry.RecordBackup))
            {
                File.Move(entry.RecordBackup, entry.SourceDataPath);
            }
            trashStore.Remove(entry);
        }

        private TubFrame? CreateRestoredTubFrame(TrashEntry entry)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(entry.Line))
                {
                    string catalogName = string.IsNullOrEmpty(entry.Catalog) ? "catalog_0.catalog" : entry.Catalog;
                    string catalogPath = Path.Combine(tubPath, catalogName.Replace('/', Path.DirectorySeparatorChar));
                    string tubBasePath = Path.GetDirectoryName(catalogPath) ?? tubPath;
                    string imageBasePath = GetImageBasePath(tubBasePath);
                    Dictionary<string, Dictionary<string, string>> imageLookupCache = new();
                    Dictionary<string, string>? imageLookup = null;

                    using JsonDocument document = JsonDocument.Parse(entry.Line);
                    JsonElement root = document.RootElement;
                    string imageFileName = GetStringValue(root, "cam/image_array");
                    if (string.IsNullOrWhiteSpace(imageFileName)) imageFileName = entry.Image;
                    string imagePath = FindImagePath(tubBasePath, imageBasePath, imageLookupCache, ref imageLookup, imageFileName);
                    if (!File.Exists(imagePath))
                    {
                        string restoredPath = Path.Combine(GetImageBasePath(tubPath), Path.GetFileName(imageFileName));
                        if (File.Exists(restoredPath)) imagePath = restoredPath;
                    }

                    return new TubFrame
                    {
                        FrameNumber = GetIntValue(root, "_index", entry.Frame),
                        ImageFileName = imageFileName,
                        ImagePath = imagePath,
                        SourceDataPath = entry.SourceDataPath,
                        Angle = GetDoubleValue(root, "user/angle"),
                        Throttle = GetDoubleValue(root, "user/throttle")
                    };
                }

                if (IsOldRecordFile(entry.SourceDataPath) && File.Exists(entry.SourceDataPath))
                {
                    string tubBasePath = Path.GetDirectoryName(entry.SourceDataPath) ?? tubPath;
                    string imageBasePath = GetImageBasePath(tubBasePath);
                    Dictionary<string, Dictionary<string, string>> imageLookupCache = new();
                    Dictionary<string, string>? imageLookup = null;

                    using JsonDocument document = JsonDocument.Parse(File.ReadAllText(entry.SourceDataPath));
                    JsonElement root = document.RootElement;
                    string imageFileName = GetStringValue(root, "cam/image_array");
                    if (string.IsNullOrWhiteSpace(imageFileName)) imageFileName = entry.Image;
                    string imagePath = FindImagePath(tubBasePath, imageBasePath, imageLookupCache, ref imageLookup, imageFileName);
                    if (!File.Exists(imagePath))
                    {
                        string restoredPath = Path.Combine(GetImageBasePath(tubPath), Path.GetFileName(imageFileName));
                        if (File.Exists(restoredPath)) imagePath = restoredPath;
                    }

                    return new TubFrame
                    {
                        FrameNumber = GetIntValue(root, "_index", entry.Frame),
                        ImageFileName = imageFileName,
                        ImagePath = imagePath,
                        SourceDataPath = entry.SourceDataPath,
                        Angle = GetDoubleValue(root, "user/angle"),
                        Throttle = GetDoubleValue(root, "user/throttle")
                    };
                }
            }
            catch (Exception ex)
            {
                AddLog($"복원 프레임 구성 오류: Frame {entry.Frame:D6} - {ex.Message}");
            }

            string fallbackImagePath = File.Exists(Path.Combine(GetImageBasePath(tubPath), entry.Image))
                ? Path.Combine(GetImageBasePath(tubPath), entry.Image)
                : entry.DeletedImagePath;

            return new TubFrame
            {
                FrameNumber = entry.Frame,
                ImageFileName = entry.Image,
                ImagePath = fallbackImagePath,
                SourceDataPath = entry.SourceDataPath,
                Angle = entry.Angle,
                Throttle = entry.Throttle
            };
        }

        // 휴지통 보관 목록(trashStore)으로 lstTrash를 다시 구성한다.
        private void RebuildTrashList()
        {
            lstTrash.BeginUpdate();
            lstTrash.Items.Clear();
            foreach (TrashEntry entry in trashStore) lstTrash.Items.Add(entry);
            lstTrash.EndUpdate();
        }

        // ListBox 항목의 표시 문자열(삭제 표시)을 갱신하기 위해 동일 항목을 재대입한다.
        // 항목 값만 교체하므로 선택 인덱스는 바뀌지 않아 SelectedIndexChanged가 발생하지 않는다.
        private void RefreshFrameListItem(int index)
        {
            if (index < 0 || index >= lstFrames.Items.Count) return;
            lstFrames.Items[index] = tubFrames[index];
        }

        // 휴지통(CheckedListBox) 클릭/드래그 상태
        // - 순수 클릭(이동 없음): 텍스트=미리보기만, 체크박스 글리프=단일 토글
        // - 드래그(다른 항목으로 이동): 시작 지점과 무관하게 다중 토글
        private bool trashDragArmed;
        private bool trashDragging;
        private bool trashDownOnGlyph;
        private bool trashDragTargetState;
        private int trashDragStartIndex = -1;
        private int trashDragLastIndex = -1;

        private void LstTrash_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) { trashDragArmed = false; trashDragging = false; return; }
            int index = lstTrash.IndexFromPoint(e.Location);
            if (index < 0 || index >= lstTrash.Items.Count) { trashDragArmed = false; trashDragging = false; return; }

            // 미리보기: 클릭한 항목의 프레임 이미지를 메인 화면에 표시 (체크 상태 불변)
            ShowTrashFramePreview(index);

            // 토글은 아직 하지 않고 드래그 가능성만 무장한다. 실제 토글 시점은:
            //  · 드래그(다른 항목으로 이동)로 확정되면 → 그 범위를 다중 토글 (TrashDragConsider)
            //  · 순수 클릭이고 체크박스 글리프였으면 → MouseUp에서 단일 토글
            //  · 순수 클릭이고 텍스트였으면 → 토글 없음(미리보기만)
            trashDragArmed = true;
            trashDragging = false;
            trashDragStartIndex = index;
            trashDragLastIndex = index;
            trashDownOnGlyph = IsOnTrashCheckbox(index, e.Location);
            trashDragTargetState = !lstTrash.GetItemChecked(index);
        }

        // 휴지통 항목(TrashEntry) → tubFrames 인덱스로 매핑해 메인 화면에 미리보기 표시.
        private void ShowTrashFramePreview(int trashIndex)
        {
            if (trashIndex < 0 || trashIndex >= lstTrash.Items.Count) return;
            if (lstTrash.Items[trashIndex] is not TrashEntry entry) return;
            // 삭제된 프레임은 tubFrames에 없으므로, deleted 폴더에 보관된 이미지를 직접 표시한다.
            picFrame.Image = File.Exists(entry.DeletedImagePath) ? frameImageCache.Get(entry.DeletedImagePath) : null;
            lblFrame2.Text = entry.Frame.ToString("D6");
            lblAngle2.Text = entry.Angle.ToString("0.00");
            lblThrottle2.Text = entry.Throttle.ToString("0.00");
            picFrame.Invalidate();
        }

        // 클릭 위치가 좌측 체크박스 글리프 영역인지 근사 판정(글리프 폭을 ItemHeight로 근사, DPI/폰트 비례).
        private bool IsOnTrashCheckbox(int index, Point location)
        {
            Rectangle item = lstTrash.GetItemRectangle(index);
            return location.X <= item.Left + lstTrash.ItemHeight;
        }

        private void LstTrash_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!trashDragArmed || (e.Button & MouseButtons.Left) == 0) return;
            int index = lstTrash.IndexFromPoint(e.Location);
            if (index < 0 || index >= lstTrash.Items.Count) return;
            TrashDragConsider(index);
        }

        // 오토스크롤(커서를 가장자리에 두면 리스트가 자동으로 스크롤되는 동작) 시에는
        // 실제 마우스 이동이 없어서 MouseMove가 안 오고, native SelectionMode.One 로직이
        // SelectedIndex만 갱신한다. 따라서 SelectedIndexChanged도 같이 후킹해 드래그를 따라가게 한다.
        private void LstTrash_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!trashDragArmed) return;
            // 좌클릭이 눌린 상태(드래그/오토스크롤)에서만 드래그로 간주. 키보드 탐색 등은 무장 해제.
            if ((MouseButtons & MouseButtons.Left) == 0) { trashDragArmed = false; return; }
            int index = lstTrash.SelectedIndex;
            if (index < 0) return;
            TrashDragConsider(index);
        }

        // 무장된 상태에서 인덱스가 시작 지점과 달라지면 '드래그'로 확정하고, 시작~현재 범위를 일괄 토글한다.
        // (같은 항목에 머무르면 아직 클릭으로 간주 — 토글하지 않음)
        private void TrashDragConsider(int index)
        {
            if (!trashDragging)
            {
                if (index == trashDragStartIndex) return;   // 이동 없음 → 클릭
                trashDragging = true;                        // 다른 항목으로 이동 → 드래그 확정
                trashDragLastIndex = trashDragStartIndex;     // 시작 항목도 포함해서 칠한다
            }
            ApplyTrashDragTo(index);
        }

        // 드래그 중 인덱스가 점프(빠른 드래그/오토스크롤)할 때 마지막 위치~현재 위치 사이를 일괄 적용.
        private void ApplyTrashDragTo(int index)
        {
            if (index == trashDragLastIndex) return;
            int start = Math.Min(trashDragLastIndex, index);
            int end = Math.Max(trashDragLastIndex, index);
            for (int i = start; i <= end; i++)
            {
                lstTrash.SetItemChecked(i, trashDragTargetState);
            }
            trashDragLastIndex = index;
        }

        private void LstTrash_MouseUp(object? sender, MouseEventArgs e)
        {
            // 순수 클릭(드래그 아님)에서 체크박스 글리프를 눌렀다면 단일 토글한다.
            if (trashDragArmed && !trashDragging && trashDownOnGlyph
                && trashDragStartIndex >= 0 && trashDragStartIndex < lstTrash.Items.Count)
            {
                lstTrash.SetItemChecked(trashDragStartIndex, trashDragTargetState);
            }
            trashDragArmed = false;
            trashDragging = false;
            trashDragLastIndex = -1;
        }

        // ==========================================
        // 6. 데이터 필터링 (범위 지정 + 조건 필터)
        // ==========================================
        // 시작 지정: 앵커만 잡아둔다(끝 지정 시 한 범위로 누적).
        private void btnSetLeft_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0) return;
            SelectionSnapshot? before = BeginSelectionUndo();
            pendingRangeAnchor = trackFrame.Value;
            UpdateSelectionLabel();
            CommitSelectionUndo(before);
        }

        // 끝 지정: 앵커~현재를 한 범위로 공유 선택에 누적(여러 번 하면 여러 범위).
        private void btnSetRight_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0) return;
            SelectionSnapshot? before = BeginSelectionUndo();
            if (pendingRangeAnchor >= 0)
            {
                CommitRange(pendingRangeAnchor, trackFrame.Value);
                pendingRangeAnchor = -1;
            }
            else
            {
                selectedFrames.Add(trackFrame.Value);
            }
            RefreshSelectionVisuals();
            CommitSelectionUndo(before);
        }

        // 필터 통계용 구간. 선택이 없으면 전체, 있으면 선택의 [최소..최대].
        private (int lo, int hi) GetEffectiveRange()
        {
            if (selectedFrames.Count == 0) return (0, tubFrames.Count - 1);
            return (selectedFrames.Min, selectedFrames.Max);
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
            HashSet<int> abnormal = chkAbnormalAngle.Checked ? DetectAbnormalAngleFrames(lo, hi) : new HashSet<int>();

            List<(TubFrame, string)> targets = new List<(TubFrame, string)>();
            foreach (int i in GetTargetIndices())
            {
                TubFrame frame = tubFrames[i];
                string? reason = null;
                if (chkThrottleZero.Checked && Math.Abs(frame.Throttle) < 1e-6) reason = "속도 0";
                else if (chkMissingImage.Checked && !File.Exists(frame.ImagePath)) reason = "이미지 누락";
                else if (chkAbnormalAngle.Checked && abnormal.Contains(i)) reason = "비정상 조향각";
                if (reason != null) targets.Add((frame, reason));
            }

            HashSet<TubFrame> targetFrames = targets.Select(t => t.Item1).ToHashSet();
            TubFrame? frameToKeep = FindFrameToShowAfterRemoval(targetFrames, trackFrame.Value);
            int fallbackIndex = trackFrame.Value;

            int trashStart = trashStore.Count;
            int moved;
            try { moved = DeleteFramesToTrash(targets); }
            catch (Exception ex) { MessageBox.Show($"필터 적용 중 오류: {ex.Message}", "필터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error); AddLog($"필터 오류: {ex.Message}"); return; }

            if (moved == 0)
            {
                MessageBox.Show("조건에 해당하는 프레임이 없습니다.", "필터 적용", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            PushDeleteUndo(trashStore.Skip(trashStart));
            AddLog($"필터로 {moved}개 프레임을 catalog에서 제거하고 휴지통으로 이동했습니다.");
            RefreshTubViewAfterDeletion(frameToKeep, fallbackIndex);
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
            if (string.IsNullOrWhiteSpace(tubPath))
            {
                MessageBox.Show("Tub 폴더 정보가 없습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedFrames.Count == 0)
            {
                MessageBox.Show("삭제할 프레임을 선택하세요.\n([시작 지정]/[끝 지정] 또는 타임라인·목록 드래그로 선택)", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<(TubFrame, string)> targets = new List<(TubFrame, string)>();
            foreach (int i in selectedFrames)
                if (i >= 0 && i < tubFrames.Count) targets.Add((tubFrames[i], "수동 삭제"));

            HashSet<TubFrame> targetFrames = targets.Select(t => t.Item1).ToHashSet();
            TubFrame? frameToKeep = FindFrameToShowAfterRemoval(targetFrames, trackFrame.Value);
            int fallbackIndex = trackFrame.Value;

            int trashStart = trashStore.Count;
            int moved;
            try { moved = DeleteFramesToTrash(targets); }
            catch (Exception ex) { MessageBox.Show($"삭제 중 오류: {ex.Message}", "삭제 오류", MessageBoxButtons.OK, MessageBoxIcon.Error); AddLog($"삭제 오류: {ex.Message}"); return; }

            PushDeleteUndo(trashStore.Skip(trashStart));
            AddLog($"{moved}개 프레임을 catalog에서 제거하고 휴지통으로 이동했습니다(복원 가능).");
            RefreshTubViewAfterDeletion(frameToKeep, fallbackIndex);
        }

        private void btnRestore_Click(object? sender, EventArgs e)
        {
            if (lstTrash.CheckedItems.Count == 0)
            {
                MessageBox.Show("복원할 항목을 휴지통 목록에서 체크하세요.", "복원", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<TrashEntry> selected = lstTrash.CheckedItems.Cast<TrashEntry>().ToList();
            TubFrame? frameToKeep = trackFrame.Value >= 0 && trackFrame.Value < tubFrames.Count ? tubFrames[trackFrame.Value] : null;
            int fallbackIndex = trackFrame.Value;
            List<TubFrame> restoredFrames = new();
            int restored = 0;
            try
            {
                foreach (TrashEntry entry in selected)
                {
                    RestoreEntry(entry);
                    TubFrame? restoredFrame = CreateRestoredTubFrame(entry);
                    if (restoredFrame != null) restoredFrames.Add(restoredFrame);
                    restored++;
                }
                WriteTrashStore();
                SyncManifestToCatalogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"복원 중 오류: {ex.Message}", "복원 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog($"복원 오류: {ex.Message}");
            }
            AddLog($"{restored}개 프레임을 복원(catalog에 재삽입)했습니다.");
            RefreshTubViewAfterRestore(restoredFrames, frameToKeep, fallbackIndex);
            PushRestoreUndo(restoredFrames);
        }

        // 완전 삭제: 디스크 휴지통(deleted/trash.jsonl + images)을 영구 삭제한다.
        // catalog는 삭제 시점에 이미 정리됐으므로 여기서는 보관분만 제거하고 목록을 비운다.
        private void btnEmptyTrash_Click(object? sender, EventArgs e)
        {
            if (trashStore.Count == 0)
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
                $"휴지통의 {trashStore.Count}개 항목을 완전히 삭제합니다.\n\n" +
                "· 되돌릴 수 없습니다(복원 불가).\n" +
                "· deleted 폴더에 보관된 이미지/기록이 영구 삭제됩니다.\n\n계속하시겠습니까?",
                "휴지통 비우기 (완전 삭제)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.No) return;

            try
            {
                int n = trashStore.Count;
                foreach (TrashEntry entry in trashStore)
                    if (!string.IsNullOrEmpty(entry.RecordBackup) && File.Exists(entry.RecordBackup)) File.Delete(entry.RecordBackup);
                if (Directory.Exists(DeletedImagesDir)) Directory.Delete(DeletedImagesDir, true);
                if (File.Exists(TrashJsonlPath)) File.Delete(TrashJsonlPath);
                trashStore.Clear();
                undoStack.Clear();
                pendingSelectionUndoSnapshot = null;
                RebuildTrashList();

                AddLog($"휴지통 비우기(완전 삭제) 완료: {n}건 영구 삭제.");
                MessageBox.Show($"휴지통을 비웠습니다(완전 삭제).\n\n삭제된 항목: {n}건", "휴지통 비우기 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        // manifest.json deleted_indexes 연동 (삭제=학습 제외, 복원 가능)
        // ==========================================

        // manifest.json을 비어있지 않은 줄로 읽어 5번째 줄(catalog 메타)을 JsonObject로 파싱한다.
        // Donkeycar Tub v2(5줄) 형식이 아니면 false.
        private bool TryReadManifest(out string[] lines, out JsonObject meta)
        {
            lines = Array.Empty<string>();
            meta = new JsonObject();
            if (string.IsNullOrWhiteSpace(tubPath)) return false;
            string manifestPath = Path.Combine(tubPath, "manifest.json");
            if (!File.Exists(manifestPath)) return false;
            try
            {
                List<string> nonEmpty = new List<string>();
                foreach (string l in File.ReadAllLines(manifestPath))
                {
                    if (!string.IsNullOrWhiteSpace(l)) nonEmpty.Add(l);
                }
                if (nonEmpty.Count < 5) return false;
                if (JsonNode.Parse(nonEmpty[4]) is not JsonObject obj) return false;
                lines = nonEmpty.ToArray();
                meta = obj;
                return true;
            }
            catch
            {
                return false;
            }
        }

        // manifest.json의 5번째 줄만 새 내용으로 바꿔 5줄을 다시 쓴다(1~4줄 원문 유지, \n 줄바꿈, BOM 없음).
        private void WriteManifestMeta(string[] lines, JsonObject meta)
        {
            string manifestPath = Path.Combine(tubPath, "manifest.json");
            string[] outLines = (string[])lines.Clone();
            outLines[4] = meta.ToJsonString();
            File.WriteAllText(manifestPath, string.Join("\n", outLines.Take(5)) + "\n");
        }

        // 현재 휴지통(Deleted) 상태를 manifest.json의 deleted_indexes에 반영한다.
        // 글로벌 인덱스 = tubFrames 내 위치(정렬된 catalog의 비어있지 않은 줄 순번).
        // 디스크 휴지통 경로
        private string DeletedDir => Path.Combine(tubPath, "deleted");
        private string DeletedImagesDir => Path.Combine(DeletedDir, "images");
        private string TrashJsonlPath => Path.Combine(DeletedDir, "trash.jsonl");

        // deleted/trash.jsonl을 읽어 휴지통 보관 목록(trashStore)을 구성한다(재시작 후 복원용).
        private void LoadTrashStore()
        {
            trashStore.Clear();
            if (string.IsNullOrWhiteSpace(tubPath) || !File.Exists(TrashJsonlPath)) return;
            foreach (string line in File.ReadAllLines(TrashJsonlPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                try
                {
                    if (JsonNode.Parse(line) is not JsonObject o) continue;
                    TrashEntry entry = new TrashEntry
                    {
                        Catalog = (string?)o["catalog"] ?? "",
                        Index = (int?)o["index"] ?? 0,
                        Frame = (int?)o["frame"] ?? 0,
                        Reason = (string?)o["reason"] ?? "",
                        Image = (string?)o["image"] ?? "",
                        DeletedImageName = (string?)o["deletedImage"] ?? "",
                        Angle = (double?)o["angle"] ?? 0,
                        Throttle = (double?)o["throttle"] ?? 0,
                        SourceDataPath = (string?)o["source"] ?? "",
                        RecordBackup = (string?)o["recordBackup"] ?? "",
                        Line = (string?)o["line"] ?? "",
                    };
                    if (string.IsNullOrEmpty(entry.DeletedImageName)) entry.DeletedImageName = entry.Image;
                    entry.DeletedImagePath = Path.Combine(DeletedImagesDir, entry.DeletedImageName);
                    trashStore.Add(entry);
                }
                catch { }
            }
        }

        // 휴지통 보관 목록을 deleted/trash.jsonl로 기록한다(비어 있으면 파일 삭제).
        private void WriteTrashStore()
        {
            if (string.IsNullOrWhiteSpace(tubPath)) return;
            Directory.CreateDirectory(DeletedDir);
            if (trashStore.Count == 0)
            {
                if (File.Exists(TrashJsonlPath)) File.Delete(TrashJsonlPath);
                return;
            }
            List<string> lines = new List<string>();
            foreach (TrashEntry e in trashStore)
            {
                JsonObject o = new JsonObject
                {
                    ["catalog"] = e.Catalog,
                    ["index"] = e.Index,
                    ["frame"] = e.Frame,
                    ["reason"] = e.Reason,
                    ["image"] = e.Image,
                    ["deletedImage"] = e.DeletedImageName,
                    ["angle"] = e.Angle,
                    ["throttle"] = e.Throttle,
                    ["source"] = e.SourceDataPath,
                    ["recordBackup"] = e.RecordBackup,
                    ["line"] = e.Line,
                };
                lines.Add(o.ToJsonString());
            }
            File.WriteAllText(TrashJsonlPath, string.Join("\n", lines) + "\n");
        }

        // catalog 변경 후 manifest.json을 실제 상태에 맞춘다: paths/current_index 재계산, deleted_indexes 초기화.
        private void SyncManifestToCatalogs()
        {
            if (!TryReadManifest(out string[] lines, out JsonObject meta))
            {
                AddLog("[manifest 안내] manifest.json이 없거나 형식이 달라 경로/인덱스 갱신을 건너뜁니다.");
                return;
            }
            string[] catalogs = Directory.GetFiles(tubPath, "catalog_*.catalog", SearchOption.AllDirectories)
                .OrderBy(GetFileOrderNumber).ThenBy(f => f).ToArray();
            JsonArray paths = new JsonArray();
            int total = 0;
            foreach (string c in catalogs)
            {
                paths.Add(JsonValue.Create(Path.GetRelativePath(tubPath, c).Replace('\\', '/')));
                foreach (string line in File.ReadLines(c)) if (!string.IsNullOrWhiteSpace(line)) total++;
            }
            meta["paths"] = paths;
            meta["current_index"] = JsonValue.Create(total);
            meta["deleted_indexes"] = new JsonArray();
            try
            {
                WriteManifestMeta(lines, meta);
            }
            catch (Exception ex)
            {
                AddLog($"[manifest 오류] manifest.json 갱신 실패: {ex.Message}");
            }
        }

        // 복원 시 줄을 catalog에 재삽입한다. 모든 줄에 _index가 있으면 그 순서로 정렬(원위치 보존), 없으면 말미에 추가.
        private static void InsertLineIntoCatalog(string catalogPath, string line)
        {
            List<string> lines = new List<string>();
            if (File.Exists(catalogPath))
                foreach (string l in File.ReadLines(catalogPath))
                    if (!string.IsNullOrWhiteSpace(l)) lines.Add(l);
            lines.Add(line);

            List<int?> idxs = lines.Select(TryGetCatalogIndex).ToList();
            if (idxs.All(v => v.HasValue))
            {
                lines = lines.Select((l, i) => (l, idx: idxs[i]!.Value))
                             .OrderBy(t => t.idx)
                             .Select(t => t.l)
                             .ToList();
            }
            File.WriteAllText(catalogPath, string.Join("\n", lines) + "\n");
        }

        // catalog 줄에서 _index(정수)를 읽는다. 없으면 null.
        private static int? TryGetCatalogIndex(string line)
        {
            try
            {
                using JsonDocument d = JsonDocument.Parse(line);
                if (d.RootElement.TryGetProperty("_index", out JsonElement v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out int i))
                    return i;
            }
            catch { }
            return null;
        }

        // catalog를 고치면 byte-offset용 .catalog_manifest가 어긋나므로 삭제한다(학습 전처리가 새로 생성).
        private static void DeleteCatalogManifestFor(string catalogFile)
        {
            string m = catalogFile + "_manifest"; // catalog_0.catalog → catalog_0.catalog_manifest
            if (File.Exists(m)) File.Delete(m);
        }


        // ==========================================
        // 내부 클래스
        // ==========================================
        private enum UndoActionKind
        {
            Selection,
            Delete,
            Restore
        }

        private sealed class SelectionSnapshot
        {
            public SelectionSnapshot(SortedSet<int> frames, int pendingRangeAnchor, int selectionAnchorIndex)
            {
                Frames = frames;
                PendingRangeAnchor = pendingRangeAnchor;
                SelectionAnchorIndex = selectionAnchorIndex;
            }

            public SortedSet<int> Frames { get; }
            public int PendingRangeAnchor { get; }
            public int SelectionAnchorIndex { get; }
        }

        private sealed class UndoAction
        {
            private UndoAction(UndoActionKind kind)
            {
                Kind = kind;
            }

            public UndoActionKind Kind { get; }
            public SelectionSnapshot? Selection { get; private init; }
            public List<TrashEntry> TrashEntries { get; private init; } = new();
            public List<TubFrame> RestoredFrames { get; private init; } = new();

            public static UndoAction FromSelection(SelectionSnapshot snapshot)
            {
                return new UndoAction(UndoActionKind.Selection) { Selection = snapshot };
            }

            public static UndoAction FromDelete(List<TrashEntry> entries)
            {
                return new UndoAction(UndoActionKind.Delete) { TrashEntries = entries };
            }

            public static UndoAction FromRestore(List<TubFrame> frames)
            {
                return new UndoAction(UndoActionKind.Restore) { RestoredFrames = frames };
            }
        }

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

        // 휴지통(디스크 보관) 항목. deleted/trash.jsonl에 직렬화되어 앱 재시작 후에도 유지된다.
        private sealed class TrashEntry
        {
            public string Catalog { get; set; } = "";        // tubPath 기준 상대 catalog 파일명(구버전 record tub은 "")
            public int Index { get; set; }                   // 삭제 시점의 글로벌 위치(참고용)
            public int Frame { get; set; }
            public string Reason { get; set; } = "";
            public string Image { get; set; } = "";          // 원본 이미지 파일명
            public string DeletedImageName { get; set; } = "";// deleted/images 안의 실제 파일명(충돌 시 _N)
            public double Angle { get; set; }
            public double Throttle { get; set; }
            public string SourceDataPath { get; set; } = ""; // 구버전 record_*.json 원본 경로
            public string RecordBackup { get; set; } = "";   // 구버전 record_*.json 이동 보관 경로
            public string Line { get; set; } = "";           // 원본 catalog JSON 줄(구버전 record tub은 "")
            public string DeletedImagePath { get; set; } = "";// deleted/images 하위 실제 경로(런타임 계산, 미직렬화)
            public override string ToString() => $"Frame {Frame:D6} · {Reason}";
        }

        private sealed class TrainingLossPoint
        {
            public int Order { get; }
            public int FrameIndex { get; }
            public double Loss { get; }
            public double? ValLoss { get; }

            public TrainingLossPoint(int order, int frameIndex, double loss, double? valLoss)
            {
                Order = order;
                FrameIndex = frameIndex;
                Loss = loss;
                ValLoss = valLoss;
            }
        }

        // 타임라인 썸네일 전용 LRU 캐시. ImageList에는 복사본을 넘겨 표시 중 이미지와 캐시 수명을 분리한다.
        private sealed class TimelineThumbnailCache
        {
            private readonly int capacity;
            private readonly Func<string, Size, Image> loader;
            private readonly object sync = new();
            private readonly Dictionary<string, Image> map = new(StringComparer.OrdinalIgnoreCase);
            private readonly LinkedList<string> order = new(); // 앞쪽이 최근 사용

            public TimelineThumbnailCache(int capacity, Func<string, Size, Image> loader)
            {
                this.capacity = Math.Max(1, capacity);
                this.loader = loader;
            }

            public Image GetClone(string imagePath, Size thumbnailSize)
            {
                string key = MakeKey(imagePath, thumbnailSize);
                lock (sync)
                {
                    if (map.TryGetValue(key, out Image? cached))
                    {
                        order.Remove(key);
                        order.AddFirst(key);
                        return (Image)cached.Clone();
                    }
                }

                Image image = loader(imagePath, thumbnailSize);
                List<Image> evictedImages = new();

                lock (sync)
                {
                    if (map.TryGetValue(key, out Image? cached))
                    {
                        image.Dispose();
                        order.Remove(key);
                        order.AddFirst(key);
                        return (Image)cached.Clone();
                    }

                    map[key] = image;
                    order.AddFirst(key);

                    while (order.Count > capacity)
                    {
                        string oldest = order.Last!.Value;
                        order.RemoveLast();
                        if (map.Remove(oldest, out Image? evicted)) evictedImages.Add(evicted);
                    }

                    image = (Image)image.Clone();
                }

                foreach (Image evictedImage in evictedImages)
                {
                    evictedImage.Dispose();
                }

                return image;
            }

            public bool Contains(string imagePath, Size thumbnailSize)
            {
                string key = MakeKey(imagePath, thumbnailSize);
                lock (sync)
                {
                    return map.ContainsKey(key);
                }
            }

            public void Preload(string imagePath, Size thumbnailSize, CancellationToken cancellationToken)
            {
                string key = MakeKey(imagePath, thumbnailSize);
                lock (sync)
                {
                    if (map.ContainsKey(key)) return;
                }

                Image image = loader(imagePath, thumbnailSize);
                if (cancellationToken.IsCancellationRequested)
                {
                    image.Dispose();
                    return;
                }

                List<Image> evictedImages = new();
                lock (sync)
                {
                    if (map.ContainsKey(key))
                    {
                        image.Dispose();
                        return;
                    }

                    map[key] = image;
                    order.AddFirst(key);

                    while (order.Count > capacity)
                    {
                        string oldest = order.Last!.Value;
                        order.RemoveLast();
                        if (map.Remove(oldest, out Image? evicted)) evictedImages.Add(evicted);
                    }
                }

                foreach (Image evictedImage in evictedImages)
                {
                    evictedImage.Dispose();
                }
            }

            public void Clear()
            {
                List<Image> images;
                lock (sync)
                {
                    images = map.Values.ToList();
                    map.Clear();
                    order.Clear();
                }

                foreach (Image image in images)
                {
                    image.Dispose();
                }
            }

            private static string MakeKey(string imagePath, Size thumbnailSize)
            {
                return $"{thumbnailSize.Width}x{thumbnailSize.Height}|{imagePath}";
            }
        }

        // 표시용 이미지 LRU 캐시. 용량을 초과하면 가장 오래 전 사용한 이미지를 Dispose하여 메모리를 제한한다.
        private sealed class FrameImageCache
        {
            private readonly int capacity;
            private readonly Func<string, Image> loader;
            private readonly object sync = new();
            private readonly Dictionary<string, Image> map = new();
            private readonly LinkedList<string> order = new(); // 앞쪽이 최근 사용

            public FrameImageCache(int capacity, Func<string, Image> loader)
            {
                this.capacity = Math.Max(1, capacity);
                this.loader = loader;
            }

            public Image Get(string key)
            {
                lock (sync)
                {
                    if (map.TryGetValue(key, out Image? cached))
                    {
                        order.Remove(key);
                        order.AddFirst(key);
                        return cached;
                    }
                }

                Image image = loader(key);
                List<Image> evictedImages = new();

                lock (sync)
                {
                    if (map.TryGetValue(key, out Image? cached))
                    {
                        image.Dispose();
                        order.Remove(key);
                        order.AddFirst(key);
                        return cached;
                    }

                    map[key] = image;
                    order.AddFirst(key);

                    while (order.Count > capacity)
                    {
                        string oldest = order.Last!.Value;
                        order.RemoveLast();
                        if (map.Remove(oldest, out Image? evicted)) evictedImages.Add(evicted);
                    }
                }

                foreach (Image evictedImage in evictedImages)
                {
                    evictedImage.Dispose();
                }

                return image;
            }

            public bool Contains(string key)
            {
                lock (sync)
                {
                    return map.ContainsKey(key);
                }
            }

            public void Preload(string key, CancellationToken cancellationToken)
            {
                lock (sync)
                {
                    if (map.ContainsKey(key)) return;
                }

                Image image = loader(key);
                if (cancellationToken.IsCancellationRequested)
                {
                    image.Dispose();
                    return;
                }

                List<Image> evictedImages = new();
                lock (sync)
                {
                    if (map.ContainsKey(key))
                    {
                        image.Dispose();
                        return;
                    }

                    map[key] = image;
                    order.AddFirst(key);

                    while (order.Count > capacity)
                    {
                        string oldest = order.Last!.Value;
                        order.RemoveLast();
                        if (map.Remove(oldest, out Image? evicted)) evictedImages.Add(evicted);
                    }
                }

                foreach (Image evictedImage in evictedImages)
                {
                    evictedImage.Dispose();
                }
            }

            public void Clear()
            {
                List<Image> images;
                lock (sync)
                {
                    images = map.Values.ToList();
                    map.Clear();
                    order.Clear();
                }

                foreach (Image image in images)
                {
                    image.Dispose();
                }
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

            int firstPlayable = FirstPlayableIndex();
            if (firstPlayable < 0) return false;

            // 마지막 프레임에서 재생 버튼을 누르면 처음 유효 프레임부터 다시 시작한다.
            if (NextPlayableIndex(trackFrame.Value) < 0)
            {
                ShowFrame(firstPlayable);
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

        private void ChangePlaybackSpeed(int direction)
        {
            if (cmbPlaySpeed.Items.Count == 0) return;

            int currentIndex = cmbPlaySpeed.SelectedIndex >= 0 ? cmbPlaySpeed.SelectedIndex : 0;
            int nextIndex = Math.Clamp(currentIndex + direction, 0, cmbPlaySpeed.Items.Count - 1);
            if (nextIndex == cmbPlaySpeed.SelectedIndex) return;

            cmbPlaySpeed.SelectedIndex = nextIndex;
        }

        private void UpdatePlaybackControlsVisual(bool isPlaying)
        {
            btnPlayStop.Text = isPlaying ? "정지" : "재생";
            // 재생/정지 아이콘은 variant별 스케일된 이미지를 사용하도록 한다.
            if (btnPlayStop != null)
            {
                string variant = isPlaying ? "stop" : "play";
                if (!_scaledButtonVariants.TryGetValue((btnPlayStop, variant), out Image? img) || img == null)
                {
                    Image src = isPlaying ? Properties.Resources.icons8_stop_30 : Properties.Resources.icons8_play_30;
                    img = CreateScaledButtonImage(src, btnPlayStop.ClientSize);
                    _scaledButtonVariants[(btnPlayStop, variant)] = img;
                }
                btnPlayStop.Image = img;
            }
        }

        private void RegisterScaledButton(Button? btn)
        {
            if (btn == null) return;

            // 이미 등록된 버튼은 무시
            if (_origButtonImages.ContainsKey(btn)) return;

            Image? orig = btn.Image;
            // 디자이너에서 이미지가 없으면 기본 플레이 아이콘으로 대체(플레이 버튼용)
            if (orig == null && btn == btnPlayStop)
            {
                orig = Properties.Resources.icons8_play_30;
            }

            if (orig == null) return;

            _origButtonImages[btn] = orig;
            // 초기 스케일 적용
            Image scaled = CreateScaledButtonImage(orig, btn.ClientSize);
            _scaledButtonVariants[(btn, "default")] = scaled;
            btn.Image = scaled;

            // btnPlayStop은 play/stop variant를 미리 생성해서 빠르게 전환할 수 있게 함
            if (btn == btnPlayStop)
            {
                Image playSrc = Properties.Resources.icons8_play_30;
                Image stopSrc = Properties.Resources.icons8_stop_30;
                _scaledButtonVariants[(btn, "play")] = CreateScaledButtonImage(playSrc, btn.ClientSize);
                _scaledButtonVariants[(btn, "stop")] = CreateScaledButtonImage(stopSrc, btn.ClientSize);
            }
        }

        private void UpdateAllButtonImagesScale()
        {
            foreach (var kv in _origButtonImages.ToList())
            {
                Button btn = kv.Key;
                Image orig = kv.Value;

                Size iconSize = GetButtonIconSize(btn);

                if (_scaledButtonVariants.TryGetValue((btn, "default"), out Image? oldDefault) && oldDefault != null)
                {
                    oldDefault.Dispose();
                }

                Image newScaled = CreateScaledButtonImage(orig, iconSize);
                _scaledButtonVariants[(btn, "default")] = newScaled;
                btn.Image = newScaled;

                if (btn == btnPlayStop)
                {
                    if (_scaledButtonVariants.TryGetValue((btn, "play"), out Image? oldPlay) && oldPlay != null)
                        oldPlay.Dispose();

                    if (_scaledButtonVariants.TryGetValue((btn, "stop"), out Image? oldStop) && oldStop != null)
                        oldStop.Dispose();

                    _scaledButtonVariants[(btn, "play")] =
                        CreateScaledButtonImage(Properties.Resources.icons8_play_30, iconSize);

                    _scaledButtonVariants[(btn, "stop")] =
                        CreateScaledButtonImage(Properties.Resources.icons8_stop_30, iconSize);
                }
            }
        }

        private Size GetButtonIconSize(Button btn)
        {
            float iconRatio = 0.68f;
            int size = (int)Math.Round(btn.ClientSize.Height * iconRatio);

            // 너무 작거나 너무 커지는 것 방지
            size = Math.Clamp(size, 18, 48);

            return new Size(size, size);
        }

        private void ClearScaledButtonImages()
        {
            foreach (var img in _scaledButtonVariants.Values) img.Dispose();
            _scaledButtonVariants.Clear();
            _origButtonImages.Clear();
        }

        private static Image CreateScaledButtonImage(Image orig, Size clientSize)
        {
            int maxH = Math.Max(16, (int)Math.Round(clientSize.Height * 0.6));
            int maxW = Math.Max(16, (int)Math.Round(clientSize.Width * 0.6));

            // 유지할 비율 계산
            double ratio = Math.Min(maxW / (double)orig.Width, maxH / (double)orig.Height);
            int targetW = Math.Max(16, (int)Math.Round(orig.Width * ratio));
            int targetH = Math.Max(16, (int)Math.Round(orig.Height * ratio));

            Bitmap bmp = new Bitmap(targetW, targetH);
            using Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawImage(orig, 0, 0, targetW, targetH);
            return bmp;
        }

        private void UpdateAutoPlayLoopVisual()
        {
            chkAutoPlay.BackColor = chkAutoPlay.Checked ? Color.FromArgb(59, 130, 246) : Color.LightSkyBlue;
            chkAutoPlay.ForeColor = chkAutoPlay.Checked ? Color.White : Color.Black;
        }

        private void chkAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAutoPlayLoopVisual();
        }

        private void picFrame_Click(object sender, EventArgs e)
        {
            if (!suppressNextFrameClick) return;

            suppressNextFrameClick = false;
        }

        // AI 컴파일 (오차 데이터)
        private sealed class HighErrorEntry
        {
            public TubFrame Frame { get; }
            public double PredictAngle { get; }
            public double Error { get; }

            public HighErrorEntry(TubFrame frame, double predictAngle, double error)
            {
                Frame = frame;
                PredictAngle = predictAngle;
                Error = error;
            }

            public override string ToString() =>
                $"Frame {Frame.FrameNumber:D6}  |  오차: {Error:0.000} (실제:{Frame.Angle:0.00} 예측:{PredictAngle:0.00})";
        }

        private void btnRunAICompile_Click(object? sender, EventArgs e)
        {
            if (tubFrames.Count == 0)
            {
                MessageBox.Show("Data 폴더 열기 경로가 없습니다. 먼저 데이터를 열어주세요.", "데이터 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (predictedAnglesByImageKey.Count == 0)
            {
                MessageBox.Show("먼저 [학습/테스트] 탭에서 '모델 테스트 실행'을 한 번 끝까지 완료해야 합니다!\n(AI가 전체 데이터를 채점한 결과를 바탕으로 오차를 추출합니다.)", "테스트 선행 필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstHighErrorFrames.BeginUpdate();
            lstHighErrorFrames.Items.Clear();

            List<HighErrorEntry> errorList = new List<HighErrorEntry>();

            foreach (var frame in tubFrames)
            {
                if (frame.Deleted) continue; // 이미 지운 건 패스

                if (TryGetPredictedAngleForFrame(frame, out double pred))
                {
                    double err = Math.Abs(frame.Angle - pred);

                    // 오차가 0.1 이상인 의미 있는(?) 쓰레기 데이터만 추출합니다. (원하시면 수치 조절 가능)
                    if (err >= 0.1)
                    {
                        errorList.Add(new HighErrorEntry(frame, pred, err));
                    }
                }
            }

            // 오차가 큰 순서대로(내림차순) 정렬
            errorList.Sort((a, b) => b.Error.CompareTo(a.Error));

            foreach (var entry in errorList)
            {
                lstHighErrorFrames.Items.Add(entry);
            }

            lstHighErrorFrames.EndUpdate();

            MessageBox.Show($"총 {errorList.Count}개의 오차 데이터를 추출했습니다!\n목록을 클릭하면 해당 프레임으로 바로 이동합니다.", "AI 컴파일 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lstHighErrorFrames_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstHighErrorFrames.SelectedItem is HighErrorEntry entry)
            {
                int index = tubFrames.IndexOf(entry.Frame);
                if (index >= 0)
                {
                    ShowFrame(index); // 클릭하면 메인 화면 사진이 해당 프레임으로 휙 바뀜!
                }
            }
        }

        private void btnDeleteHighError_Click(object? sender, EventArgs e)
        {
            if (lstHighErrorFrames.SelectedItems.Count == 0)
            {
                MessageBox.Show("휴지통으로 보낼 프레임을 목록에서 선택하세요.\n(Shift나 Ctrl을 누르고 클릭하면 여러 개 동시 선택 가능)", "선택 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(tubPath))
            {
                MessageBox.Show("Tub 폴더 정보가 없습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedEntries = lstHighErrorFrames.SelectedItems.Cast<HighErrorEntry>().ToList();
            List<(TubFrame, string)> targets = new List<(TubFrame, string)>();
            foreach (var entry in selectedEntries)
            {
                if (tubFrames.Contains(entry.Frame)) targets.Add((entry.Frame, "AI 컴파일 오차 데이터"));
            }

            HashSet<TubFrame> targetFrames = targets.Select(t => t.Item1).ToHashSet();
            TubFrame? frameToKeep = FindFrameToShowAfterRemoval(targetFrames, trackFrame.Value);
            int fallbackIndex = trackFrame.Value;

            int trashStart = trashStore.Count;
            int moved;
            try { moved = DeleteFramesToTrash(targets); }
            catch (Exception ex) { MessageBox.Show($"삭제 중 오류: {ex.Message}", "삭제 오류", MessageBoxButtons.OK, MessageBoxIcon.Error); AddLog($"삭제 오류: {ex.Message}"); return; }

            PushDeleteUndo(trashStore.Skip(trashStart));

            // 처리 완료된 항목은 AI 컴파일 목록에서 빼준다.
            foreach (var entry in selectedEntries)
            {
                lstHighErrorFrames.Items.Remove(entry);
            }

            // 삭제된 프레임은 tubFrames에서 빠졌으므로 재로딩하지 않고 현재 위치 기준으로 뷰만 재구성한다.
            RefreshTubViewAfterDeletion(frameToKeep, fallbackIndex);

            AddLog($"AI 오차 프레임 {moved}개를 catalog에서 제거하고 휴지통으로 이동했습니다.");
            MessageBox.Show($"선택한 {moved}개의 오차 프레임을 휴지통으로 이동했습니다!\n(완전 삭제는 [데이터 정리] 탭의 휴지통 비우기를 이용하세요.)", "이동 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void picFrame_Click_1(object sender, EventArgs e)
        {

        }

        private void lblTubPath_Click(object sender, EventArgs e)
        {

        }

        private void groupDataView_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkUseVenv_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }
}
