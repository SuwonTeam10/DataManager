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
            progressBarTrain = new ProgressBar();
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
            rdoLocal = new RadioButton();
            groupBox2 = new GroupBox();
            chkUseVenv = new CheckBox();
            rdoRemote = new RadioButton();
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
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupTubLoader
            // 
            groupTubLoader.Controls.Add(lblTubPath);
            groupTubLoader.Controls.Add(btnLoadTub);
            groupTubLoader.Location = new Point(499, 3);
            groupTubLoader.Margin = new Padding(2);
            groupTubLoader.Name = "groupTubLoader";
            groupTubLoader.Padding = new Padding(2);
            groupTubLoader.Size = new Size(422, 70);
            groupTubLoader.TabIndex = 2;
            groupTubLoader.TabStop = false;
            // 
            // lblTubPath
            // 
            lblTubPath.AutoSize = true;
            lblTubPath.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblTubPath.Location = new Point(219, 32);
            lblTubPath.Margin = new Padding(2, 0, 2, 0);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(85, 15);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Tub 경로: 없음";
            // 
            // btnLoadTub
            // 
            btnLoadTub.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLoadTub.Location = new Point(5, 21);
            btnLoadTub.Margin = new Padding(2);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(190, 41);
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
            groupDataView.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupDataView.Location = new Point(30, 98);
            groupDataView.Margin = new Padding(2);
            groupDataView.Name = "groupDataView";
            groupDataView.Padding = new Padding(2);
            groupDataView.Size = new Size(222, 360);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // chkShowPredictAngle
            // 
            chkShowPredictAngle.AutoSize = true;
            chkShowPredictAngle.Location = new Point(18, 257);
            chkShowPredictAngle.Margin = new Padding(2);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(119, 21);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Location = new Point(18, 218);
            chkShowRealAngle.Margin = new Padding(2);
            chkShowRealAngle.Name = "chkShowRealAngle";
            chkShowRealAngle.Size = new Size(119, 21);
            chkShowRealAngle.TabIndex = 3;
            chkShowRealAngle.Text = "실제 조향각 표시";
            chkShowRealAngle.UseVisualStyleBackColor = true;
            // 
            // lblThrottle
            // 
            lblThrottle.AutoSize = true;
            lblThrottle.Font = new Font("Microsoft Sans Serif", 12F);
            lblThrottle.Location = new Point(18, 136);
            lblThrottle.Margin = new Padding(2, 0, 2, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(72, 20);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도: 0.00";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Font = new Font("Microsoft Sans Serif", 12F);
            lblAngle.Location = new Point(18, 90);
            lblAngle.Margin = new Padding(2, 0, 2, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(84, 20);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각: 0.00";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Font = new Font("Microsoft Sans Serif", 12F);
            lblFrame.Location = new Point(18, 47);
            lblFrame.Margin = new Padding(2, 0, 2, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(107, 20);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임: 000000";
            // 
            // groupTimeline
            // 
            groupTimeline.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupTimeline.Location = new Point(35, 379);
            groupTimeline.Margin = new Padding(2);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Padding = new Padding(2);
            groupTimeline.Size = new Size(1161, 122);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "썸네일 타임라인";
            // 
            // lvTimeline
            // 
            lvTimeline.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvTimeline.Location = new Point(7, 28);
            lvTimeline.Margin = new Padding(2);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(1130, 79);
            lvTimeline.TabIndex = 0;
            lvTimeline.UseCompatibleStateImageBehavior = false;
            lvTimeline.SelectedIndexChanged += lvTimeline_SelectedIndexChanged_1;
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabCleaner);
            tabMain.Controls.Add(tabGraph);
            tabMain.Controls.Add(tabTrainTest);
            tabMain.Location = new Point(35, 518);
            tabMain.Margin = new Padding(2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1161, 257);
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
            tabCleaner.Location = new Point(4, 24);
            tabCleaner.Margin = new Padding(2);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(2);
            tabCleaner.Size = new Size(1153, 229);
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
            groupBox1.Location = new Point(677, 10);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(457, 212);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "휴지통";
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTrastList.Location = new Point(283, 21);
            lblTrastList.Margin = new Padding(2, 0, 2, 0);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(59, 15);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "삭제 목록";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnEmptyTrash.Location = new Point(16, 79);
            btnEmptyTrash.Margin = new Padding(2);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(156, 37);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "휴지통 비우기";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnRestore.Location = new Point(128, 28);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(96, 37);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "복원";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnReloadTub.Location = new Point(16, 146);
            btnReloadTub.Margin = new Padding(2);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(193, 37);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Tub 다시 불러오기";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(283, 44);
            lstTrash.Margin = new Padding(2);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(151, 139);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnDelete.Location = new Point(16, 28);
            btnDelete.Margin = new Padding(2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(96, 37);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Font = new Font("맑은 고딕", 10.125F);
            chkAbnormalAngle.Location = new Point(387, 159);
            chkAbnormalAngle.Margin = new Padding(2);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(117, 23);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "비정상 조향각";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Font = new Font("맑은 고딕", 10.125F);
            chkMissingImage.Location = new Point(387, 124);
            chkMissingImage.Margin = new Padding(2);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(103, 23);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "이미지 누락";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Font = new Font("맑은 고딕", 10.125F);
            chkThrottleZero.Location = new Point(387, 88);
            chkThrottleZero.Margin = new Padding(2);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(102, 23);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "속도 0 제거";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Font = new Font("맑은 고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFilter.Location = new Point(387, 21);
            btnFilter.Margin = new Padding(2);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(82, 24);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "필터 적용";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRange.Location = new Point(23, 94);
            lblRange.Margin = new Padding(2, 0, 2, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(75, 18);
            lblRange.TabIndex = 9;
            lblRange.Text = "범위: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Font = new Font("맑은 고딕", 10.875F);
            btnSetRight.Location = new Point(158, 21);
            btnSetRight.Margin = new Padding(2);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(82, 24);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "끝 지정";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Font = new Font("맑은 고딕", 10.875F);
            btnSetLeft.Location = new Point(17, 21);
            btnSetLeft.Margin = new Padding(2);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(128, 39);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "시작 지정";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // tabGraph
            // 
            tabGraph.Location = new Point(4, 24);
            tabGraph.Margin = new Padding(2);
            tabGraph.Name = "tabGraph";
            tabGraph.Padding = new Padding(2);
            tabGraph.Size = new Size(1153, 229);
            tabGraph.TabIndex = 1;
            tabGraph.Text = "그래프";
            tabGraph.UseVisualStyleBackColor = true;
            // 
            // tabTrainTest
            // 
            tabTrainTest.Controls.Add(progressBarTrain);
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
            tabTrainTest.Location = new Point(4, 24);
            tabTrainTest.Margin = new Padding(2);
            tabTrainTest.Name = "tabTrainTest";
            tabTrainTest.Size = new Size(1153, 229);
            tabTrainTest.TabIndex = 2;
            tabTrainTest.Text = "학습/테스트";
            tabTrainTest.UseVisualStyleBackColor = true;
            // 
            // progressBarTrain
            // 
            progressBarTrain.Location = new Point(338, 86);
            progressBarTrain.Name = "progressBarTrain";
            progressBarTrain.Size = new Size(295, 30);
            progressBarTrain.TabIndex = 24;
            // 
            // lblLog
            // 
            lblLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLog.AutoSize = true;
            lblLog.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblLog.Location = new Point(761, 18);
            lblLog.Margin = new Padding(2, 0, 2, 0);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(34, 18);
            lblLog.TabIndex = 21;
            lblLog.Text = "로그";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtLog.Location = new Point(761, 52);
            txtLog.Margin = new Padding(2);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(359, 121);
            txtLog.TabIndex = 20;
            txtLog.Text = "준비 완료...\r\n";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Font = new Font("Microsoft Sans Serif", 10.875F);
            lblRealAngle.Location = new Point(15, 127);
            lblRealAngle.Margin = new Padding(2, 0, 2, 0);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(113, 18);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "실제 조향각: 0.00";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Font = new Font("Microsoft Sans Serif", 10.875F);
            lblPredictAngle.Location = new Point(15, 170);
            lblPredictAngle.Margin = new Padding(2, 0, 2, 0);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(113, 18);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "예측 조향각: 0.00";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgress.Location = new Point(250, 86);
            lblProgress.Margin = new Padding(2, 0, 2, 0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(51, 18);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "진행률:";
            // 
            // progressTrain
            // 
            progressTrain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressTrain.Location = new Point(338, 86);
            progressTrain.Margin = new Padding(2);
            progressTrain.Name = "progressTrain";
            progressTrain.Size = new Size(292, 30);
            progressTrain.TabIndex = 16;
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Font = new Font("Microsoft Sans Serif", 10.875F);
            lblTrainStatus.Location = new Point(15, 86);
            lblTrainStatus.Margin = new Padding(2, 0, 2, 0);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(98, 18);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "상태: 준비 완료";
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnSelectTestImage.Location = new Point(243, 12);
            btnSelectTestImage.Margin = new Padding(2);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(204, 34);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "테스트 이미지 선택";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            btnSelectTestImage.Click += btnSelectTestImage_Click;
            // 
            // btnSelectModel
            // 
            btnSelectModel.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnSelectModel.Location = new Point(107, 12);
            btnSelectModel.Margin = new Padding(2);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(117, 34);
            btnSelectModel.TabIndex = 13;
            btnSelectModel.Text = "모델 선택";
            btnSelectModel.UseVisualStyleBackColor = true;
            btnSelectModel.Click += btnSelectModel_Click;
            // 
            // btnModelTest
            // 
            btnModelTest.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnModelTest.Location = new Point(463, 12);
            btnModelTest.Margin = new Padding(2);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Size = new Size(145, 34);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "모델 테스트";
            btnModelTest.UseVisualStyleBackColor = true;
            btnModelTest.Click += btnModelTest_Click;
            // 
            // btnTrain
            // 
            btnTrain.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnTrain.Location = new Point(13, 12);
            btnTrain.Margin = new Padding(2);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(76, 34);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "학습";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLoadConfig.Location = new Point(11, 17);
            btnLoadConfig.Margin = new Padding(2);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(190, 45);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "설정 파일 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // lblConfigPath
            // 
            lblConfigPath.AutoSize = true;
            lblConfigPath.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblConfigPath.Location = new Point(214, 31);
            lblConfigPath.Margin = new Padding(2, 0, 2, 0);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(88, 15);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "설정 경로: 없음";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // groupConfigLoader
            // 
            groupConfigLoader.Controls.Add(lblConfigPath);
            groupConfigLoader.Controls.Add(btnLoadConfig);
            groupConfigLoader.Location = new Point(30, 3);
            groupConfigLoader.Margin = new Padding(2);
            groupConfigLoader.Name = "groupConfigLoader";
            groupConfigLoader.Padding = new Padding(2);
            groupConfigLoader.Size = new Size(415, 70);
            groupConfigLoader.TabIndex = 1;
            groupConfigLoader.TabStop = false;
            // 
            // lstFrames
            // 
            lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(12, 38);
            lstFrames.Margin = new Padding(2);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(213, 212);
            lstFrames.TabIndex = 0;
            // 
            // groupFrameList
            // 
            groupFrameList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupFrameList.Location = new Point(990, 20);
            groupFrameList.Margin = new Padding(2);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Padding = new Padding(2);
            groupFrameList.Size = new Size(229, 287);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(27, 28);
            picFrame.Margin = new Padding(2);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(672, 150);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // btnFirst
            // 
            btnFirst.Anchor = AnchorStyles.Bottom;
            btnFirst.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold);
            btnFirst.Location = new Point(85, 199);
            btnFirst.Margin = new Padding(2);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(66, 35);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Bottom;
            btnPrev.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold);
            btnPrev.Location = new Point(172, 199);
            btnPrev.Margin = new Padding(2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(59, 35);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Anchor = AnchorStyles.Bottom;
            btnPlayStop.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayStop.Location = new Point(292, 193);
            btnPlayStop.Margin = new Padding(2);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Size = new Size(123, 46);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "재생/정지";
            btnPlayStop.UseVisualStyleBackColor = true;
            btnPlayStop.Click += btnPlayStop_Click;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom;
            btnNext.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold);
            btnNext.Location = new Point(478, 199);
            btnNext.Margin = new Padding(2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(59, 35);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.Anchor = AnchorStyles.Bottom;
            btnLast.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold);
            btnLast.Location = new Point(557, 199);
            btnLast.Margin = new Padding(2);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(66, 35);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // trackFrame
            // 
            trackFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackFrame.Location = new Point(27, 245);
            trackFrame.Margin = new Padding(2);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(672, 45);
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
            groupTubNavigator.Location = new Point(257, 90);
            groupTubNavigator.Margin = new Padding(2);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Padding = new Padding(2);
            groupTubNavigator.Size = new Size(729, 294);
            groupTubNavigator.TabIndex = 4;
            groupTubNavigator.TabStop = false;
            groupTubNavigator.Text = "주행 이미지 탐색기";
            groupTubNavigator.Enter += groupTubNavigator_Enter;
            // 
            // rdoLocal
            // 
            rdoLocal.AutoSize = true;
            rdoLocal.Font = new Font("Microsoft Sans Serif", 12F);
            rdoLocal.Location = new Point(6, 17);
            rdoLocal.Name = "rdoLocal";
            rdoLocal.Size = new Size(141, 24);
            rdoLocal.TabIndex = 21;
            rdoLocal.TabStop = true;
            rdoLocal.Text = "로컬 (리눅스/우분투)";
            rdoLocal.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            groupBox2.Controls.Add(chkUseVenv);
            groupBox2.Controls.Add(rdoRemote);
            groupBox2.Controls.Add(rdoLocal);
            groupBox2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(1002, 283);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(213, 101);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "서버 연결 설정";
            // 
            // chkUseVenv
            // 
            chkUseVenv.AutoSize = true;
            chkUseVenv.Checked = true;
            chkUseVenv.CheckState = CheckState.Checked;
            chkUseVenv.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkUseVenv.Location = new Point(6, 77);
            chkUseVenv.Name = "chkUseVenv";
            chkUseVenv.Size = new Size(104, 24);
            chkUseVenv.TabIndex = 25;
            chkUseVenv.Text = "가상환경 적용";
            chkUseVenv.UseVisualStyleBackColor = true;
            // 
            // rdoRemote
            // 
            rdoRemote.AutoSize = true;
            rdoRemote.Font = new Font("Microsoft Sans Serif", 12F);
            rdoRemote.Location = new Point(6, 47);
            rdoRemote.Name = "rdoRemote";
            rdoRemote.Size = new Size(89, 24);
            rdoRemote.TabIndex = 22;
            rdoRemote.TabStop = true;
            rdoRemote.Text = "원격 (서버)";
            rdoRemote.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1242, 791);
            Controls.Add(groupBox2);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Controls.Add(groupTubLoader);
            Controls.Add(groupConfigLoader);
            Margin = new Padding(2);
            MinimumSize = new Size(1092, 685);
            Name = "Form1";
            Text = "Donkeycar 데이터 관리 프로그램";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
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
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
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
        private RadioButton rdoLocal;
        private GroupBox groupBox2;
        private RadioButton rdoRemote;
        private ProgressBar progressBarTrain;
        private CheckBox chkUseVenv;
    }
}
