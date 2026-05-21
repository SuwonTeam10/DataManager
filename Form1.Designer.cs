namespace DataManager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);////
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupTubLoader = new GroupBox();
            lblTubPath = new Label();
            btnLoadTub = new Button();
            groupDataView = new GroupBox();
            chkShowPredictAngle = new CheckBox();
            chkShowRealAngle = new CheckBox();
            lblThrottle = new Label();
            lblAngle = new Label();
            lblFrame = new Label();
            groupTimeline = new GroupBox();
            lvTimeline = new ListView();
            tabMain = new TabControl();
            tabCleaner = new TabPage();
            groupBox1 = new GroupBox();
            lblTrastList = new Label();
            btnEmptyTrash = new Button();
            btnRestore = new Button();
            btnReloadTub = new Button();
            lstTrash = new ListBox();
            btnDelete = new Button();
            chkAbnormalAngle = new CheckBox();
            chkMissingImage = new CheckBox();
            chkThrottleZero = new CheckBox();
            btnFilter = new Button();
            lblRange = new Label();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            tabGraph = new TabPage();
            tabTrainTest = new TabPage();
            lblLog = new Label();
            txtLog = new TextBox();
            lblRealAngle = new Label();
            lblPredictAngle = new Label();
            lblProgress = new Label();
            progressTrain = new ProgressBar();
            lblTrainStatus = new Label();
            btnSelectTestImage = new Button();
            btnSelectModel = new Button();
            btnModelTest = new Button();
            btnTrain = new Button();
            btnLoadConfig = new Button();
            lblConfigPath = new Label();
            groupConfigLoader = new GroupBox();
            lstFrames = new ListBox();
            groupFrameList = new GroupBox();
            picFrame = new PictureBox();
            btnFirst = new Button();
            btnPrev = new Button();
            btnPlayStop = new Button();
            btnNext = new Button();
            btnLast = new Button();
            trackFrame = new TrackBar();
            groupTubNavigator = new GroupBox();
            groupTubLoader.SuspendLayout();
            groupDataView.SuspendLayout();
            groupTimeline.SuspendLayout();
            tabMain.SuspendLayout();
            tabCleaner.SuspendLayout();
            groupBox1.SuspendLayout();
            tabTrainTest.SuspendLayout();
            groupConfigLoader.SuspendLayout();
            groupFrameList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackFrame).BeginInit();
            groupTubNavigator.SuspendLayout();
            SuspendLayout();
            // 
            // groupTubLoader
            // 
            groupTubLoader.Controls.Add(lblTubPath);
            groupTubLoader.Controls.Add(btnLoadTub);
            groupTubLoader.Location = new Point(642, 4);
            groupTubLoader.Name = "groupTubLoader";
            groupTubLoader.Size = new Size(543, 94);
            groupTubLoader.TabIndex = 2;
            groupTubLoader.TabStop = false;
            // 
            // lblTubPath
            // 
            lblTubPath.AutoSize = true;
            lblTubPath.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblTubPath.Location = new Point(282, 43);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(168, 28);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Tub 경로: 없음";
            lblTubPath.Click += lblTubPath_Click;
            // 
            // btnLoadTub
            // 
            btnLoadTub.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLoadTub.Location = new Point(6, 28);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(244, 55);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "Tub 데이터 열기";
            btnLoadTub.UseVisualStyleBackColor = true;
            btnLoadTub.Click += btnLoadTub_Click;
            // 
            // groupDataView
            // 
            groupDataView.Controls.Add(chkShowPredictAngle);
            groupDataView.Controls.Add(chkShowRealAngle);
            groupDataView.Controls.Add(lblThrottle);
            groupDataView.Controls.Add(lblAngle);
            groupDataView.Controls.Add(lblFrame);
            groupDataView.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupDataView.Location = new Point(39, 130);
            groupDataView.Name = "groupDataView";
            groupDataView.Size = new Size(285, 480);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // chkShowPredictAngle
            // 
            chkShowPredictAngle.AutoSize = true;
            chkShowPredictAngle.Location = new Point(23, 343);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(237, 35);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Location = new Point(23, 290);
            chkShowRealAngle.Name = "chkShowRealAngle";
            chkShowRealAngle.Size = new Size(237, 35);
            chkShowRealAngle.TabIndex = 3;
            chkShowRealAngle.Text = "실제 조향각 표시";
            chkShowRealAngle.UseVisualStyleBackColor = true;
            // 
            // lblThrottle
            // 
            lblThrottle.AutoSize = true;
            lblThrottle.Font = new Font("나눔고딕", 12F);
            lblThrottle.Location = new Point(23, 182);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(161, 36);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도: 0.00";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Font = new Font("나눔고딕", 12F);
            lblAngle.Location = new Point(23, 120);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(191, 36);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각: 0.00";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Font = new Font("나눔고딕", 12F);
            lblFrame.Location = new Point(23, 63);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(238, 36);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임: 000000";
            // 
            // groupTimeline
            // 
            groupTimeline.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupTimeline.Location = new Point(45, 705);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Size = new Size(1493, 163);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "썸네일 타임라인";
            // 
            // lvTimeline
            // 
            lvTimeline.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvTimeline.Location = new Point(9, 38);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(1452, 104);
            lvTimeline.TabIndex = 0;
            lvTimeline.UseCompatibleStateImageBehavior = false;
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabCleaner);
            tabMain.Controls.Add(tabGraph);
            tabMain.Controls.Add(tabTrainTest);
            tabMain.Location = new Point(45, 891);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1493, 343);
            tabMain.TabIndex = 7;
            // 
            // tabCleaner
            // 
            tabCleaner.Controls.Add(groupBox1);
            tabCleaner.Controls.Add(chkAbnormalAngle);
            tabCleaner.Controls.Add(chkMissingImage);
            tabCleaner.Controls.Add(chkThrottleZero);
            tabCleaner.Controls.Add(btnFilter);
            tabCleaner.Controls.Add(lblRange);
            tabCleaner.Controls.Add(btnSetRight);
            tabCleaner.Controls.Add(btnSetLeft);
            tabCleaner.Location = new Point(8, 46);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(3);
            tabCleaner.Size = new Size(1477, 289);
            tabCleaner.TabIndex = 0;
            tabCleaner.Text = "데이터 정리";
            tabCleaner.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(lblTrastList);
            groupBox1.Controls.Add(btnEmptyTrash);
            groupBox1.Controls.Add(btnRestore);
            groupBox1.Controls.Add(btnReloadTub);
            groupBox1.Controls.Add(lstTrash);
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Location = new Point(871, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(587, 282);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "휴지통";
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Font = new Font("나눔고딕", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTrastList.Location = new Point(364, 28);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(111, 28);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "삭제 목록";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Font = new Font("나눔고딕", 10.125F);
            btnEmptyTrash.Location = new Point(21, 105);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(200, 49);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "휴지통 비우기";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("나눔고딕", 10.125F);
            btnRestore.Location = new Point(164, 38);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(124, 49);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "복원";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Font = new Font("나눔고딕", 10.125F);
            btnReloadTub.Location = new Point(21, 195);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(248, 49);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Tub 다시 불러오기";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(364, 59);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(193, 196);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("나눔고딕", 10.125F);
            btnDelete.Location = new Point(21, 38);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(124, 49);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Font = new Font("맑은 고딕", 10.125F);
            chkAbnormalAngle.Location = new Point(498, 212);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(220, 41);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "비정상 조향각";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Font = new Font("맑은 고딕", 10.125F);
            chkMissingImage.Location = new Point(498, 165);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(193, 41);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "이미지 누락";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Font = new Font("맑은 고딕", 10.125F);
            chkThrottleZero.Location = new Point(498, 118);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(190, 41);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "속도 0 제거";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Font = new Font("맑은 고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFilter.Location = new Point(498, 28);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(164, 52);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "필터 적용";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRange.Location = new Point(29, 125);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(157, 34);
            lblRange.TabIndex = 9;
            lblRange.Text = "범위: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Font = new Font("맑은 고딕", 10.875F);
            btnSetRight.Location = new Point(203, 28);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(164, 52);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "끝 지정";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Font = new Font("맑은 고딕", 10.875F);
            btnSetLeft.Location = new Point(22, 28);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(164, 52);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "시작 지정";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // tabGraph
            // 
            tabGraph.Location = new Point(8, 46);
            tabGraph.Name = "tabGraph";
            tabGraph.Padding = new Padding(3);
            tabGraph.Size = new Size(1477, 289);
            tabGraph.TabIndex = 1;
            tabGraph.Text = "그래프";
            tabGraph.UseVisualStyleBackColor = true;
            // 
            // tabTrainTest
            // 
            tabTrainTest.Controls.Add(lblLog);
            tabTrainTest.Controls.Add(txtLog);
            tabTrainTest.Controls.Add(lblRealAngle);
            tabTrainTest.Controls.Add(lblPredictAngle);
            tabTrainTest.Controls.Add(lblProgress);
            tabTrainTest.Controls.Add(progressTrain);
            tabTrainTest.Controls.Add(lblTrainStatus);
            tabTrainTest.Controls.Add(btnSelectTestImage);
            tabTrainTest.Controls.Add(btnSelectModel);
            tabTrainTest.Controls.Add(btnModelTest);
            tabTrainTest.Controls.Add(btnTrain);
            tabTrainTest.Location = new Point(8, 46);
            tabTrainTest.Name = "tabTrainTest";
            tabTrainTest.Size = new Size(1477, 289);
            tabTrainTest.TabIndex = 2;
            tabTrainTest.Text = "학습/테스트";
            tabTrainTest.UseVisualStyleBackColor = true;
            // 
            // lblLog
            // 
            lblLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLog.AutoSize = true;
            lblLog.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblLog.Location = new Point(979, 24);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(69, 34);
            lblLog.TabIndex = 21;
            lblLog.Text = "로그";
            lblLog.Click += lblLog_Click;
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtLog.Location = new Point(979, 70);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(461, 160);
            txtLog.TabIndex = 20;
            txtLog.Text = "준비 완료...\r\n";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Font = new Font("나눔고딕", 10.875F);
            lblRealAngle.Location = new Point(19, 169);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(238, 34);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "실제 조향각: 0.00";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Font = new Font("나눔고딕", 10.875F);
            lblPredictAngle.Location = new Point(19, 226);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(238, 34);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "예측 조향각: 0.00";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgress.Location = new Point(321, 114);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(105, 34);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "진행률:";
            // 
            // progressTrain
            // 
            progressTrain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressTrain.Location = new Point(435, 114);
            progressTrain.Name = "progressTrain";
            progressTrain.Size = new Size(376, 40);
            progressTrain.TabIndex = 16;
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Font = new Font("나눔고딕", 10.875F);
            lblTrainStatus.Location = new Point(19, 114);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(202, 34);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "상태: 준비 완료";
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Font = new Font("나눔고딕", 10.875F);
            btnSelectTestImage.Location = new Point(312, 16);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(262, 46);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "테스트 이미지 선택";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            // 
            // btnSelectModel
            // 
            btnSelectModel.Font = new Font("나눔고딕", 10.875F);
            btnSelectModel.Location = new Point(138, 16);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(150, 46);
            btnSelectModel.TabIndex = 13;
            btnSelectModel.Text = "모델 선택";
            btnSelectModel.UseVisualStyleBackColor = true;
            // 
            // btnModelTest
            // 
            btnModelTest.Font = new Font("나눔고딕", 10.875F);
            btnModelTest.Location = new Point(595, 16);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Size = new Size(187, 46);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "모델 테스트";
            btnModelTest.UseVisualStyleBackColor = true;
            // 
            // btnTrain
            // 
            btnTrain.Font = new Font("나눔고딕", 10.875F);
            btnTrain.Location = new Point(17, 16);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(98, 46);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "학습";
            btnTrain.UseVisualStyleBackColor = true;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLoadConfig.Location = new Point(14, 23);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(244, 60);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "설정 파일 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // lblConfigPath
            // 
            lblConfigPath.AutoSize = true;
            lblConfigPath.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblConfigPath.Location = new Point(275, 41);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(171, 28);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "설정 경로: 없음";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // groupConfigLoader
            // 
            groupConfigLoader.Controls.Add(lblConfigPath);
            groupConfigLoader.Controls.Add(btnLoadConfig);
            groupConfigLoader.Location = new Point(39, 4);
            groupConfigLoader.Name = "groupConfigLoader";
            groupConfigLoader.Size = new Size(533, 94);
            groupConfigLoader.TabIndex = 1;
            groupConfigLoader.TabStop = false;
            // 
            // lstFrames
            // 
            lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(15, 51);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(273, 500);
            lstFrames.TabIndex = 0;
            // 
            // groupFrameList
            // 
            groupFrameList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupFrameList.Location = new Point(1273, 27);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Size = new Size(294, 583);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(35, 38);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(864, 400);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // btnFirst
            // 
            btnFirst.Anchor = AnchorStyles.Bottom;
            btnFirst.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold);
            btnFirst.Location = new Point(109, 465);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(85, 47);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Bottom;
            btnPrev.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold);
            btnPrev.Location = new Point(221, 465);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(76, 47);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Anchor = AnchorStyles.Bottom;
            btnPlayStop.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayStop.Location = new Point(376, 457);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Size = new Size(158, 62);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "재생/정지";
            btnPlayStop.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom;
            btnNext.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold);
            btnNext.Location = new Point(615, 465);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(76, 47);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.Anchor = AnchorStyles.Bottom;
            btnLast.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold);
            btnLast.Location = new Point(716, 465);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(85, 47);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // trackFrame
            // 
            trackFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackFrame.Location = new Point(35, 527);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(864, 90);
            trackFrame.TabIndex = 6;
            trackFrame.TickFrequency = 10;
            // 
            // groupTubNavigator
            // 
            groupTubNavigator.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTubNavigator.Controls.Add(trackFrame);
            groupTubNavigator.Controls.Add(btnLast);
            groupTubNavigator.Controls.Add(btnNext);
            groupTubNavigator.Controls.Add(btnPlayStop);
            groupTubNavigator.Controls.Add(btnPrev);
            groupTubNavigator.Controls.Add(btnFirst);
            groupTubNavigator.Controls.Add(picFrame);
            groupTubNavigator.Location = new Point(330, 120);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Size = new Size(937, 592);
            groupTubNavigator.TabIndex = 4;
            groupTubNavigator.TabStop = false;
            groupTubNavigator.Text = "주행 이미지 탐색기";
            groupTubNavigator.Enter += groupTubNavigator_Enter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1597, 1255);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Controls.Add(groupTubLoader);
            Controls.Add(groupConfigLoader);
            MinimumSize = new Size(1400, 900);
            Name = "Form1";
            Text = "Donkeycar 데이터 관리 프로그램";
            WindowState = FormWindowState.Maximized;
            groupTubLoader.ResumeLayout(false);
            groupTubLoader.PerformLayout();
            groupDataView.ResumeLayout(false);
            groupDataView.PerformLayout();
            groupTimeline.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabCleaner.ResumeLayout(false);
            tabCleaner.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabTrainTest.ResumeLayout(false);
            tabTrainTest.PerformLayout();
            groupConfigLoader.ResumeLayout(false);
            groupConfigLoader.PerformLayout();
            groupFrameList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackFrame).EndInit();
            groupTubNavigator.ResumeLayout(false);
            groupTubNavigator.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupTubLoader;
        private Label lblTubPath;
        private Button btnLoadTub;
        private GroupBox groupDataView;
        private CheckBox chkShowPredictAngle;
        private CheckBox chkShowRealAngle;
        private Label lblThrottle;
        private Label lblAngle;
        private Label lblFrame;
        private GroupBox groupTimeline;
        private ListView lvTimeline;
        private TabControl tabMain;
        private TabPage tabCleaner;
        private TabPage tabGraph;//
        private TabPage tabTrainTest;
        private Label lblProgress;
        private ProgressBar progressTrain;
        private Label lblTrainStatus;
        private Button btnSelectTestImage;
        private Button btnSelectModel;
        private Button btnModelTest;
        private Button btnTrain;
        private Label lblTrastList;
        private Button btnEmptyTrash;
        private Button btnRestore;
        private Button btnReloadTub;
        private ListBox lstTrash;
        private Button btnDelete;
        private CheckBox chkAbnormalAngle;
        private CheckBox chkMissingImage;
        private CheckBox chkThrottleZero;
        private Button btnFilter;
        private Label lblRange;
        private Button btnSetRight;
        private Button btnSetLeft;
        private Label lblLog;
        private TextBox txtLog;
        private Label lblRealAngle;
        private Label lblPredictAngle;
        private Button btnLoadConfig;
        private Label lblConfigPath;
        private GroupBox groupConfigLoader;
        private GroupBox groupBox1;
        private ListBox lstFrames;
        private GroupBox groupFrameList;
        private PictureBox picFrame;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnPlayStop;
        private Button btnNext;
        private Button btnLast;
        private TrackBar trackFrame;
        private GroupBox groupTubNavigator;
    }
}
