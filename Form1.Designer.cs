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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblTubPath = new Label();
            btnLoadTub = new Button();
            groupDataView = new GroupBox();
            lblThrottle2 = new Label();
            lblAngle2 = new Label();
            lblFrame2 = new Label();
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBoxAngle = new PictureBox();
            pictureBoxFrame = new PictureBox();
            panel1 = new Panel();
            chkShowPredictAngle = new CheckBox();
            chkShowRealAngle = new CheckBox();
            lblThrottle = new Label();
            lblAngle = new Label();
            lblFrame = new Label();
            groupTimeline = new GroupBox();
            lvTimeline = new ListView();
            tabMain = new TabControl();
            tabCleaner = new TabPage();
            groupBoxTrash = new GroupBox();
            lblTrashPercent = new Label();
            lblTrashProgress = new Label();
            progressBarTrash = new ProgressBar();
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
            grpPredictResult = new GroupBox();
            panel4 = new Panel();
            lblTrainStatus2 = new Label();
            lblErrorValue = new Label();
            lblErrorValue2 = new Label();
            lblPredictAngle2 = new Label();
            lblRealAngle2 = new Label();
            lblTrainStatus = new Label();
            lblPredictAngle = new Label();
            lblRealAngle = new Label();
            grpTrainProgress = new GroupBox();
            lblProgressPercent = new Label();
            lblProgress = new Label();
            progressBarTrain = new ProgressBar();
            grpTrainControl = new GroupBox();
            btnTrain = new Button();
            btnStopTask = new Button();
            grpTestImage = new GroupBox();
            picTestImage = new PictureBox();
            btnSelectTestImage = new Button();
            grpTrainSetting = new GroupBox();
            btnSelectModel = new Button();
            lblLog = new Label();
            txtLog = new TextBox();
            btnModelTest = new Button();
            btnLoadConfig = new Button();
            lblConfigPath = new Label();
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
            lblCurrentFrame2 = new Label();
            lblCurrentFrame = new Label();
            chkAutoPlay = new CheckBox();
            lblSpeed = new Label();
            cmbPlaySpeed = new ComboBox();
            rdoLocal = new RadioButton();
            groupBox2 = new GroupBox();
            btnDisconnect = new Button();
            lblStatus2 = new Label();
            lblStatus = new Label();
            pictureBox1 = new PictureBox();
            picUser = new PictureBox();
            lblUser2 = new Label();
            lblUser = new Label();
            panel5 = new Panel();
            chkUseVenv = new CheckBox();
            rdoRemote = new RadioButton();
            groupBoxDataLoad = new GroupBox();
            panel2 = new Panel();
            groupDataView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAngle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFrame).BeginInit();
            groupTimeline.SuspendLayout();
            tabMain.SuspendLayout();
            tabCleaner.SuspendLayout();
            groupBoxTrash.SuspendLayout();
            tabTrainTest.SuspendLayout();
            grpPredictResult.SuspendLayout();
            grpTrainProgress.SuspendLayout();
            grpTrainControl.SuspendLayout();
            grpTestImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTestImage).BeginInit();
            grpTrainSetting.SuspendLayout();
            groupFrameList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackFrame).BeginInit();
            groupTubNavigator.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picUser).BeginInit();
            groupBoxDataLoad.SuspendLayout();
            SuspendLayout();
            // 
            // lblTubPath
            // 
            lblTubPath.AutoSize = true;
            lblTubPath.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblTubPath.Location = new Point(27, 334);
            lblTubPath.Margin = new Padding(4, 0, 4, 0);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(185, 31);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Tub 경로: 없음";
            // 
            // btnLoadTub
            // 
            btnLoadTub.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadTub.Image = Properties.Resources.icons8_database_administrator_50;
            btnLoadTub.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoadTub.Location = new Point(18, 231);
            btnLoadTub.Margin = new Padding(4);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Padding = new Padding(15, 0, 0, 0);
            btnLoadTub.Size = new Size(380, 87);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "     Tub 데이터 열기";
            btnLoadTub.UseVisualStyleBackColor = true;
            btnLoadTub.Click += btnLoadTub_Click;
            // 
            // groupDataView
            // 
            groupDataView.Controls.Add(lblThrottle2);
            groupDataView.Controls.Add(lblAngle2);
            groupDataView.Controls.Add(lblFrame2);
            groupDataView.Controls.Add(panel3);
            groupDataView.Controls.Add(pictureBox2);
            groupDataView.Controls.Add(pictureBoxAngle);
            groupDataView.Controls.Add(pictureBoxFrame);
            groupDataView.Controls.Add(panel1);
            groupDataView.Controls.Add(chkShowPredictAngle);
            groupDataView.Controls.Add(chkShowRealAngle);
            groupDataView.Controls.Add(lblThrottle);
            groupDataView.Controls.Add(lblAngle);
            groupDataView.Controls.Add(lblFrame);
            groupDataView.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupDataView.Location = new Point(41, 608);
            groupDataView.Margin = new Padding(4);
            groupDataView.Name = "groupDataView";
            groupDataView.Padding = new Padding(4);
            groupDataView.Size = new Size(429, 422);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // lblThrottle2
            // 
            lblThrottle2.AutoSize = true;
            lblThrottle2.Font = new Font("나눔고딕", 12F);
            lblThrottle2.Location = new Point(313, 202);
            lblThrottle2.Margin = new Padding(4, 0, 4, 0);
            lblThrottle2.Name = "lblThrottle2";
            lblThrottle2.Size = new Size(82, 36);
            lblThrottle2.TabIndex = 12;
            lblThrottle2.Text = "0.00";
            // 
            // lblAngle2
            // 
            lblAngle2.AutoSize = true;
            lblAngle2.Font = new Font("나눔고딕", 12F);
            lblAngle2.Location = new Point(313, 137);
            lblAngle2.Margin = new Padding(4, 0, 4, 0);
            lblAngle2.Name = "lblAngle2";
            lblAngle2.Size = new Size(82, 36);
            lblAngle2.TabIndex = 11;
            lblAngle2.Text = "0.00";
            // 
            // lblFrame2
            // 
            lblFrame2.AutoSize = true;
            lblFrame2.Font = new Font("나눔고딕", 12F);
            lblFrame2.Location = new Point(270, 70);
            lblFrame2.Margin = new Padding(4, 0, 4, 0);
            lblFrame2.Name = "lblFrame2";
            lblFrame2.Size = new Size(129, 36);
            lblFrame2.TabIndex = 10;
            lblFrame2.Text = "000000";
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel3.BackColor = Color.Silver;
            panel3.Location = new Point(4, 286);
            panel3.Name = "panel3";
            panel3.Size = new Size(419, 1);
            panel3.TabIndex = 9;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(15, 198);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(74, 52);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // pictureBoxAngle
            // 
            pictureBoxAngle.BackColor = Color.Transparent;
            pictureBoxAngle.Image = (Image)resources.GetObject("pictureBoxAngle.Image");
            pictureBoxAngle.Location = new Point(15, 128);
            pictureBoxAngle.Name = "pictureBoxAngle";
            pictureBoxAngle.Size = new Size(74, 52);
            pictureBoxAngle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAngle.TabIndex = 7;
            pictureBoxAngle.TabStop = false;
            // 
            // pictureBoxFrame
            // 
            pictureBoxFrame.BackColor = Color.Transparent;
            pictureBoxFrame.Image = Properties.Resources.icons8_frame_32;
            pictureBoxFrame.Location = new Point(15, 60);
            pictureBoxFrame.Name = "pictureBoxFrame";
            pictureBoxFrame.Size = new Size(74, 52);
            pictureBoxFrame.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFrame.TabIndex = 6;
            pictureBoxFrame.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Location = new Point(0, 300);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 5;
            panel1.Paint += panel1_Paint;
            // 
            // chkShowPredictAngle
            // 
            chkShowPredictAngle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowPredictAngle.AutoSize = true;
            chkShowPredictAngle.Font = new Font("나눔고딕", 10.125F);
            chkShowPredictAngle.Location = new Point(42, 363);
            chkShowPredictAngle.Margin = new Padding(4);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(237, 35);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Font = new Font("나눔고딕", 10.125F);
            chkShowRealAngle.Location = new Point(42, 306);
            chkShowRealAngle.Margin = new Padding(4);
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
            lblThrottle.Location = new Point(95, 202);
            lblThrottle.Margin = new Padding(4, 0, 4, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(75, 36);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Font = new Font("나눔고딕", 12F);
            lblAngle.Location = new Point(95, 137);
            lblAngle.Margin = new Padding(4, 0, 4, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(105, 36);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Font = new Font("나눔고딕", 12F);
            lblFrame.Location = new Point(96, 70);
            lblFrame.Margin = new Padding(4, 0, 4, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(105, 36);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임\r\n";
            // 
            // groupTimeline
            // 
            groupTimeline.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupTimeline.Location = new Point(41, 1055);
            groupTimeline.Margin = new Padding(4);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Padding = new Padding(4);
            groupTimeline.Size = new Size(2397, 98);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "썸네일 타임라인";
            // 
            // lvTimeline
            // 
            lvTimeline.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvTimeline.Location = new Point(0, 29);
            lvTimeline.Margin = new Padding(4);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(2397, 61);
            lvTimeline.TabIndex = 0;
            lvTimeline.UseCompatibleStateImageBehavior = false;
            lvTimeline.SelectedIndexChanged += lvTimeline_SelectedIndexChanged;
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabCleaner);
            tabMain.Controls.Add(tabGraph);
            tabMain.Controls.Add(tabTrainTest);
            tabMain.Location = new Point(45, 1161);
            tabMain.Margin = new Padding(4);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(2397, 347);
            tabMain.TabIndex = 7;
            // 
            // tabCleaner
            // 
            tabCleaner.Controls.Add(groupBoxTrash);
            tabCleaner.Controls.Add(chkAbnormalAngle);
            tabCleaner.Controls.Add(chkMissingImage);
            tabCleaner.Controls.Add(chkThrottleZero);
            tabCleaner.Controls.Add(btnFilter);
            tabCleaner.Controls.Add(lblRange);
            tabCleaner.Controls.Add(btnSetRight);
            tabCleaner.Controls.Add(btnSetLeft);
            tabCleaner.Location = new Point(8, 46);
            tabCleaner.Margin = new Padding(4);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(4);
            tabCleaner.Size = new Size(2381, 293);
            tabCleaner.TabIndex = 0;
            tabCleaner.Text = "데이터 정리";
            tabCleaner.UseVisualStyleBackColor = true;
            // 
            // groupBoxTrash
            // 
            groupBoxTrash.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxTrash.Controls.Add(lblTrashPercent);
            groupBoxTrash.Controls.Add(lblTrashProgress);
            groupBoxTrash.Controls.Add(progressBarTrash);
            groupBoxTrash.Controls.Add(lblTrastList);
            groupBoxTrash.Controls.Add(btnEmptyTrash);
            groupBoxTrash.Controls.Add(btnRestore);
            groupBoxTrash.Controls.Add(btnReloadTub);
            groupBoxTrash.Controls.Add(lstTrash);
            groupBoxTrash.Controls.Add(btnDelete);
            groupBoxTrash.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBoxTrash.Location = new Point(1433, 8);
            groupBoxTrash.Margin = new Padding(4);
            groupBoxTrash.Name = "groupBoxTrash";
            groupBoxTrash.Padding = new Padding(4);
            groupBoxTrash.Size = new Size(921, 285);
            groupBoxTrash.TabIndex = 20;
            groupBoxTrash.TabStop = false;
            groupBoxTrash.Text = "휴지통";
            // 
            // lblTrashPercent
            // 
            lblTrashPercent.AutoSize = true;
            lblTrashPercent.Location = new Point(861, 235);
            lblTrashPercent.Name = "lblTrashPercent";
            lblTrashPercent.Size = new Size(53, 28);
            lblTrashPercent.TabIndex = 22;
            lblTrashPercent.Text = "0%";
            // 
            // lblTrashProgress
            // 
            lblTrashProgress.AutoSize = true;
            lblTrashProgress.Location = new Point(447, 235);
            lblTrashProgress.Name = "lblTrashProgress";
            lblTrashProgress.Size = new Size(141, 28);
            lblTrashProgress.TabIndex = 21;
            lblTrashProgress.Text = "삭제 진행률:";
            // 
            // progressBarTrash
            // 
            progressBarTrash.Location = new Point(596, 235);
            progressBarTrash.Name = "progressBarTrash";
            progressBarTrash.Size = new Size(259, 30);
            progressBarTrash.Style = ProgressBarStyle.Continuous;
            progressBarTrash.TabIndex = 20;
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblTrastList.Location = new Point(535, 36);
            lblTrastList.Margin = new Padding(4, 0, 4, 0);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(111, 28);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "삭제 목록";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Font = new Font("나눔고딕", 10.125F);
            btnEmptyTrash.Location = new Point(41, 137);
            btnEmptyTrash.Margin = new Padding(4);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(337, 56);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "휴지통 비우기";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("나눔고딕", 10.125F);
            btnRestore.Location = new Point(235, 60);
            btnRestore.Margin = new Padding(4);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(143, 45);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "복원";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Font = new Font("나눔고딕", 10.125F);
            btnReloadTub.Location = new Point(41, 217);
            btnReloadTub.Margin = new Padding(4);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(337, 56);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Tub 다시 불러오기";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(535, 83);
            lstTrash.Margin = new Padding(4);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(346, 88);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("나눔고딕", 10.125F);
            btnDelete.Location = new Point(41, 60);
            btnDelete.Margin = new Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(151, 45);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Font = new Font("나눔고딕", 10.875F);
            chkAbnormalAngle.Location = new Point(617, 243);
            chkAbnormalAngle.Margin = new Padding(4);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(217, 38);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "비정상 조향각";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Font = new Font("나눔고딕", 10.875F);
            chkMissingImage.Location = new Point(617, 186);
            chkMissingImage.Margin = new Padding(4);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(190, 38);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "이미지 누락";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Font = new Font("나눔고딕", 10.875F);
            chkThrottleZero.Location = new Point(617, 126);
            chkThrottleZero.Margin = new Padding(4);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(189, 38);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "속도 0 제거";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFilter.Image = Properties.Resources.icons8_filter_48;
            btnFilter.ImageAlign = ContentAlignment.MiddleLeft;
            btnFilter.Location = new Point(617, 44);
            btnFilter.Margin = new Padding(4);
            btnFilter.Name = "btnFilter";
            btnFilter.Padding = new Padding(5, 0, 0, 0);
            btnFilter.Size = new Size(230, 52);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "필터 적용";
            btnFilter.TextAlign = ContentAlignment.MiddleRight;
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Font = new Font("나눔고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRange.Location = new Point(46, 157);
            lblRange.Margin = new Padding(4, 0, 4, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(171, 36);
            lblRange.TabIndex = 9;
            lblRange.Text = "범위: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Font = new Font("나눔고딕", 10.875F);
            btnSetRight.Location = new Point(257, 44);
            btnSetRight.Margin = new Padding(4);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(164, 59);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "끝 지정";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Font = new Font("나눔고딕", 10.875F);
            btnSetLeft.Location = new Point(46, 44);
            btnSetLeft.Margin = new Padding(4);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(180, 58);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "시작 지정";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // tabGraph
            // 
            tabGraph.Location = new Point(8, 46);
            tabGraph.Margin = new Padding(4);
            tabGraph.Name = "tabGraph";
            tabGraph.Padding = new Padding(4);
            tabGraph.Size = new Size(2381, 293);
            tabGraph.TabIndex = 1;
            tabGraph.Text = "그래프";
            tabGraph.UseVisualStyleBackColor = true;
            // 
            // tabTrainTest
            // 
            tabTrainTest.Controls.Add(grpPredictResult);
            tabTrainTest.Controls.Add(grpTrainProgress);
            tabTrainTest.Controls.Add(grpTrainControl);
            tabTrainTest.Controls.Add(grpTestImage);
            tabTrainTest.Controls.Add(grpTrainSetting);
            tabTrainTest.Controls.Add(lblLog);
            tabTrainTest.Controls.Add(txtLog);
            tabTrainTest.Controls.Add(btnModelTest);
            tabTrainTest.Location = new Point(8, 46);
            tabTrainTest.Margin = new Padding(4);
            tabTrainTest.Name = "tabTrainTest";
            tabTrainTest.Size = new Size(2381, 293);
            tabTrainTest.TabIndex = 2;
            tabTrainTest.Text = "학습/테스트";
            tabTrainTest.UseVisualStyleBackColor = true;
            // 
            // grpPredictResult
            // 
            grpPredictResult.Controls.Add(panel4);
            grpPredictResult.Controls.Add(lblTrainStatus2);
            grpPredictResult.Controls.Add(lblErrorValue);
            grpPredictResult.Controls.Add(lblErrorValue2);
            grpPredictResult.Controls.Add(lblPredictAngle2);
            grpPredictResult.Controls.Add(lblRealAngle2);
            grpPredictResult.Controls.Add(lblTrainStatus);
            grpPredictResult.Controls.Add(lblPredictAngle);
            grpPredictResult.Controls.Add(lblRealAngle);
            grpPredictResult.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpPredictResult.Location = new Point(1261, 13);
            grpPredictResult.Name = "grpPredictResult";
            grpPredictResult.Size = new Size(343, 277);
            grpPredictResult.TabIndex = 30;
            grpPredictResult.TabStop = false;
            grpPredictResult.Text = "예측 결과";
            // 
            // panel4
            // 
            panel4.BackColor = Color.LightGray;
            panel4.ForeColor = Color.LightGray;
            panel4.Location = new Point(0, 202);
            panel4.Name = "panel4";
            panel4.Size = new Size(343, 1);
            panel4.TabIndex = 25;
            // 
            // lblTrainStatus2
            // 
            lblTrainStatus2.AutoSize = true;
            lblTrainStatus2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTrainStatus2.ForeColor = Color.Green;
            lblTrainStatus2.Location = new Point(100, 224);
            lblTrainStatus2.Margin = new Padding(4, 0, 4, 0);
            lblTrainStatus2.Name = "lblTrainStatus2";
            lblTrainStatus2.Size = new Size(122, 31);
            lblTrainStatus2.TabIndex = 24;
            lblTrainStatus2.Text = "준비 완료";
            // 
            // lblErrorValue
            // 
            lblErrorValue.AutoSize = true;
            lblErrorValue.Font = new Font("나눔고딕", 10.125F);
            lblErrorValue.Location = new Point(24, 157);
            lblErrorValue.Margin = new Padding(4, 0, 4, 0);
            lblErrorValue.Name = "lblErrorValue";
            lblErrorValue.Size = new Size(64, 31);
            lblErrorValue.TabIndex = 23;
            lblErrorValue.Text = "오차";
            // 
            // lblErrorValue2
            // 
            lblErrorValue2.AutoSize = true;
            lblErrorValue2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblErrorValue2.ForeColor = Color.Red;
            lblErrorValue2.Location = new Point(232, 160);
            lblErrorValue2.Margin = new Padding(4, 0, 4, 0);
            lblErrorValue2.Name = "lblErrorValue2";
            lblErrorValue2.Size = new Size(70, 31);
            lblErrorValue2.TabIndex = 22;
            lblErrorValue2.Text = "0.00";
            // 
            // lblPredictAngle2
            // 
            lblPredictAngle2.AutoSize = true;
            lblPredictAngle2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblPredictAngle2.ForeColor = Color.RoyalBlue;
            lblPredictAngle2.Location = new Point(232, 106);
            lblPredictAngle2.Margin = new Padding(4, 0, 4, 0);
            lblPredictAngle2.Name = "lblPredictAngle2";
            lblPredictAngle2.Size = new Size(70, 31);
            lblPredictAngle2.TabIndex = 21;
            lblPredictAngle2.Text = "0.00";
            // 
            // lblRealAngle2
            // 
            lblRealAngle2.AutoSize = true;
            lblRealAngle2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblRealAngle2.ForeColor = Color.RoyalBlue;
            lblRealAngle2.Location = new Point(232, 57);
            lblRealAngle2.Margin = new Padding(4, 0, 4, 0);
            lblRealAngle2.Name = "lblRealAngle2";
            lblRealAngle2.Size = new Size(70, 31);
            lblRealAngle2.TabIndex = 20;
            lblRealAngle2.Text = "0.00";
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Font = new Font("나눔고딕", 10.125F);
            lblTrainStatus.Location = new Point(25, 224);
            lblTrainStatus.Margin = new Padding(4, 0, 4, 0);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(72, 31);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "상태:";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Font = new Font("나눔고딕", 10.125F);
            lblPredictAngle.Location = new Point(24, 106);
            lblPredictAngle.Margin = new Padding(4, 0, 4, 0);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(147, 31);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "예측 조향각";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Font = new Font("나눔고딕", 10.125F);
            lblRealAngle.Location = new Point(24, 57);
            lblRealAngle.Margin = new Padding(4, 0, 4, 0);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(147, 31);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "실제 조향각";
            // 
            // grpTrainProgress
            // 
            grpTrainProgress.Controls.Add(lblProgressPercent);
            grpTrainProgress.Controls.Add(lblProgress);
            grpTrainProgress.Controls.Add(progressBarTrain);
            grpTrainProgress.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainProgress.Location = new Point(726, 156);
            grpTrainProgress.Name = "grpTrainProgress";
            grpTrainProgress.Size = new Size(481, 137);
            grpTrainProgress.TabIndex = 29;
            grpTrainProgress.TabStop = false;
            grpTrainProgress.Text = "학습 진행률";
            // 
            // lblProgressPercent
            // 
            lblProgressPercent.AutoSize = true;
            lblProgressPercent.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgressPercent.Location = new Point(407, 49);
            lblProgressPercent.Margin = new Padding(4, 0, 4, 0);
            lblProgressPercent.Name = "lblProgressPercent";
            lblProgressPercent.Size = new Size(53, 28);
            lblProgressPercent.TabIndex = 25;
            lblProgressPercent.Text = "0%";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgress.Location = new Point(7, 49);
            lblProgress.Margin = new Padding(4, 0, 4, 0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(88, 28);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "진행률:";
            // 
            // progressBarTrain
            // 
            progressBarTrain.Location = new Point(95, 49);
            progressBarTrain.Margin = new Padding(6);
            progressBarTrain.Name = "progressBarTrain";
            progressBarTrain.Size = new Size(302, 29);
            progressBarTrain.TabIndex = 24;
            // 
            // grpTrainControl
            // 
            grpTrainControl.Controls.Add(btnTrain);
            grpTrainControl.Controls.Add(btnStopTask);
            grpTrainControl.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainControl.Location = new Point(726, 13);
            grpTrainControl.Name = "grpTrainControl";
            grpTrainControl.Size = new Size(481, 112);
            grpTrainControl.TabIndex = 28;
            grpTrainControl.TabStop = false;
            grpTrainControl.Text = "학습 제어";
            // 
            // btnTrain
            // 
            btnTrain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTrain.BackColor = Color.DodgerBlue;
            btnTrain.Font = new Font("나눔고딕 ExtraBold", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnTrain.ForeColor = Color.White;
            btnTrain.Image = Properties.Resources.icons8_play_30__2_;
            btnTrain.ImageAlign = ContentAlignment.MiddleLeft;
            btnTrain.Location = new Point(33, 48);
            btnTrain.Margin = new Padding(4);
            btnTrain.Name = "btnTrain";
            btnTrain.Padding = new Padding(15, 0, 0, 0);
            btnTrain.Size = new Size(191, 53);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "   학습 시작";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnStopTask
            // 
            btnStopTask.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnStopTask.BackColor = Color.White;
            btnStopTask.Font = new Font("나눔고딕", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStopTask.ForeColor = Color.Red;
            btnStopTask.Image = Properties.Resources.icons8_stop_30__1_;
            btnStopTask.ImageAlign = ContentAlignment.MiddleLeft;
            btnStopTask.Location = new Point(258, 48);
            btnStopTask.Margin = new Padding(6);
            btnStopTask.Name = "btnStopTask";
            btnStopTask.Padding = new Padding(15, 0, 0, 0);
            btnStopTask.Size = new Size(191, 53);
            btnStopTask.TabIndex = 25;
            btnStopTask.Text = "   학습 중지";
            btnStopTask.UseVisualStyleBackColor = false;
            // 
            // grpTestImage
            // 
            grpTestImage.Controls.Add(picTestImage);
            grpTestImage.Controls.Add(btnSelectTestImage);
            grpTestImage.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTestImage.Location = new Point(15, 156);
            grpTestImage.Name = "grpTestImage";
            grpTestImage.Size = new Size(670, 134);
            grpTestImage.TabIndex = 27;
            grpTestImage.TabStop = false;
            grpTestImage.Text = "테스트 이미지";
            // 
            // picTestImage
            // 
            picTestImage.BackColor = Color.Black;
            picTestImage.Location = new Point(402, 23);
            picTestImage.Name = "picTestImage";
            picTestImage.Size = new Size(229, 104);
            picTestImage.TabIndex = 15;
            picTestImage.TabStop = false;
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnSelectTestImage.Location = new Point(19, 41);
            btnSelectTestImage.Margin = new Padding(4);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(307, 60);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "테스트 이미지 선택";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            btnSelectTestImage.Click += btnSelectTestImage_Click;
            // 
            // grpTrainSetting
            // 
            grpTrainSetting.Controls.Add(btnSelectModel);
            grpTrainSetting.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainSetting.Location = new Point(17, 13);
            grpTrainSetting.Name = "grpTrainSetting";
            grpTrainSetting.Size = new Size(670, 137);
            grpTrainSetting.TabIndex = 26;
            grpTrainSetting.TabStop = false;
            grpTrainSetting.Text = "학습 설정";
            // 
            // btnSelectModel
            // 
            btnSelectModel.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnSelectModel.Location = new Point(17, 46);
            btnSelectModel.Margin = new Padding(4);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(177, 53);
            btnSelectModel.TabIndex = 13;
            btnSelectModel.Text = "모델 선택";
            btnSelectModel.UseVisualStyleBackColor = true;
            btnSelectModel.Click += btnSelectModel_Click;
            // 
            // lblLog
            // 
            lblLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblLog.AutoSize = true;
            lblLog.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblLog.Location = new Point(1650, 13);
            lblLog.Margin = new Padding(4, 0, 4, 0);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(64, 31);
            lblLog.TabIndex = 21;
            lblLog.Text = "로그";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLog.Location = new Point(1650, 59);
            txtLog.Margin = new Padding(4);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(701, 220);
            txtLog.TabIndex = 20;
            txtLog.Text = "준비 완료...\r\n";
            // 
            // btnModelTest
            // 
            btnModelTest.Font = new Font("나눔고딕", 10.875F);
            btnModelTest.Image = (Image)resources.GetObject("btnModelTest.Image");
            btnModelTest.ImageAlign = ContentAlignment.MiddleLeft;
            btnModelTest.Location = new Point(726, 299);
            btnModelTest.Margin = new Padding(4);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Padding = new Padding(100, 0, 0, 0);
            btnModelTest.Size = new Size(481, 53);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "모델 테스트 실행";
            btnModelTest.UseVisualStyleBackColor = true;
            btnModelTest.Click += btnModelTest_Click;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadConfig.Image = (Image)resources.GetObject("btnLoadConfig.Image");
            btnLoadConfig.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoadConfig.Location = new Point(23, 52);
            btnLoadConfig.Margin = new Padding(4);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Padding = new Padding(15, 0, 0, 0);
            btnLoadConfig.Size = new Size(380, 96);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "    설정 파일 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // lblConfigPath
            // 
            lblConfigPath.AutoSize = true;
            lblConfigPath.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblConfigPath.Location = new Point(25, 164);
            lblConfigPath.Margin = new Padding(4, 0, 4, 0);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(188, 31);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "설정 경로: 없음";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // lstFrames
            // 
            lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(8, 81);
            lstFrames.Margin = new Padding(4);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(353, 676);
            lstFrames.TabIndex = 0;
            // 
            // groupFrameList
            // 
            groupFrameList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupFrameList.Location = new Point(2073, 203);
            groupFrameList.Margin = new Padding(4);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Padding = new Padding(4);
            groupFrameList.Size = new Size(369, 827);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(39, 40);
            picFrame.Margin = new Padding(4);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(1479, 560);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            picFrame.Click += picFrame_Click;
            // 
            // btnFirst
            // 
            btnFirst.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFirst.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFirst.Location = new Point(339, 631);
            btnFirst.Margin = new Padding(4);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(113, 68);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPrev.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(479, 631);
            btnPrev.Margin = new Padding(4);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(113, 68);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Anchor = AnchorStyles.Bottom;
            btnPlayStop.BackColor = Color.DodgerBlue;
            btnPlayStop.Font = new Font("나눔고딕 ExtraBold", 10.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayStop.ForeColor = Color.White;
            btnPlayStop.Image = Properties.Resources.icons8_play_30;
            btnPlayStop.ImageAlign = ContentAlignment.MiddleLeft;
            btnPlayStop.Location = new Point(637, 631);
            btnPlayStop.Margin = new Padding(4);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Padding = new Padding(25, 0, 0, 0);
            btnPlayStop.Size = new Size(246, 68);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "재생/정지";
            btnPlayStop.UseVisualStyleBackColor = false;
            btnPlayStop.Click += btnPlayStop_Click;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNext.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(920, 631);
            btnNext.Margin = new Padding(4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(113, 68);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLast.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLast.Location = new Point(1058, 631);
            btnLast.Margin = new Padding(4);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(113, 68);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // trackFrame
            // 
            trackFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackFrame.Location = new Point(58, 723);
            trackFrame.Margin = new Padding(4);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(1448, 90);
            trackFrame.TabIndex = 6;
            trackFrame.TickFrequency = 10;
            // 
            // groupTubNavigator
            // 
            groupTubNavigator.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTubNavigator.Controls.Add(lblCurrentFrame2);
            groupTubNavigator.Controls.Add(lblCurrentFrame);
            groupTubNavigator.Controls.Add(chkAutoPlay);
            groupTubNavigator.Controls.Add(lblSpeed);
            groupTubNavigator.Controls.Add(cmbPlaySpeed);
            groupTubNavigator.Controls.Add(trackFrame);
            groupTubNavigator.Controls.Add(btnLast);
            groupTubNavigator.Controls.Add(btnNext);
            groupTubNavigator.Controls.Add(btnPlayStop);
            groupTubNavigator.Controls.Add(btnPrev);
            groupTubNavigator.Controls.Add(btnFirst);
            groupTubNavigator.Controls.Add(picFrame);
            groupTubNavigator.FlatStyle = FlatStyle.Flat;
            groupTubNavigator.ForeColor = Color.Black;
            groupTubNavigator.Location = new Point(503, 203);
            groupTubNavigator.Margin = new Padding(4);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Padding = new Padding(4);
            groupTubNavigator.Size = new Size(1562, 827);
            groupTubNavigator.TabIndex = 4;
            groupTubNavigator.TabStop = false;
            groupTubNavigator.Text = "주행 이미지 탐색기";
            groupTubNavigator.Enter += groupTubNavigator_Enter;
            // 
            // lblCurrentFrame2
            // 
            lblCurrentFrame2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCurrentFrame2.AutoSize = true;
            lblCurrentFrame2.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblCurrentFrame2.Location = new Point(83, 674);
            lblCurrentFrame2.Name = "lblCurrentFrame2";
            lblCurrentFrame2.Size = new Size(65, 28);
            lblCurrentFrame2.TabIndex = 12;
            lblCurrentFrame2.Text = "0 / 0";
            // 
            // lblCurrentFrame
            // 
            lblCurrentFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCurrentFrame.AutoSize = true;
            lblCurrentFrame.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblCurrentFrame.Location = new Point(70, 631);
            lblCurrentFrame.Name = "lblCurrentFrame";
            lblCurrentFrame.Size = new Size(134, 28);
            lblCurrentFrame.TabIndex = 11;
            lblCurrentFrame.Text = "현재 프레임\r\n";
            // 
            // chkAutoPlay
            // 
            chkAutoPlay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkAutoPlay.Appearance = Appearance.Button;
            chkAutoPlay.FlatStyle = FlatStyle.Flat;
            chkAutoPlay.Font = new Font("맑은 고딕", 7.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            chkAutoPlay.Location = new Point(1369, 643);
            chkAutoPlay.Name = "chkAutoPlay";
            chkAutoPlay.Size = new Size(137, 44);
            chkAutoPlay.TabIndex = 10;
            chkAutoPlay.Text = "자동 재생";
            chkAutoPlay.TextAlign = ContentAlignment.MiddleCenter;
            chkAutoPlay.UseVisualStyleBackColor = true;
            chkAutoPlay.CheckedChanged += chkAutoPlay_CheckedChanged;
            // 
            // lblSpeed
            // 
            lblSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("나눔고딕", 7.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblSpeed.Location = new Point(1219, 619);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(96, 24);
            lblSpeed.TabIndex = 8;
            lblSpeed.Text = "재생 속도";
            // 
            // cmbPlaySpeed
            // 
            cmbPlaySpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cmbPlaySpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPlaySpeed.FlatStyle = FlatStyle.Flat;
            cmbPlaySpeed.FormattingEnabled = true;
            cmbPlaySpeed.Items.AddRange(new object[] { "1x", "2x", "4x", "8x" });
            cmbPlaySpeed.Location = new Point(1218, 653);
            cmbPlaySpeed.Name = "cmbPlaySpeed";
            cmbPlaySpeed.Size = new Size(123, 40);
            cmbPlaySpeed.TabIndex = 7;
            // 
            // rdoLocal
            // 
            rdoLocal.AutoSize = true;
            rdoLocal.Checked = true;
            rdoLocal.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            rdoLocal.Location = new Point(42, 53);
            rdoLocal.Margin = new Padding(6);
            rdoLocal.Name = "rdoLocal";
            rdoLocal.Size = new Size(303, 38);
            rdoLocal.TabIndex = 21;
            rdoLocal.TabStop = true;
            rdoLocal.Text = "로컬 (리눅스/우분투)";
            rdoLocal.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(btnDisconnect);
            groupBox2.Controls.Add(lblStatus2);
            groupBox2.Controls.Add(lblStatus);
            groupBox2.Controls.Add(pictureBox1);
            groupBox2.Controls.Add(picUser);
            groupBox2.Controls.Add(lblUser2);
            groupBox2.Controls.Add(lblUser);
            groupBox2.Controls.Add(panel5);
            groupBox2.Controls.Add(chkUseVenv);
            groupBox2.Controls.Add(rdoRemote);
            groupBox2.Controls.Add(rdoLocal);
            groupBox2.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBox2.Location = new Point(45, 70);
            groupBox2.Margin = new Padding(6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(6);
            groupBox2.Size = new Size(2397, 111);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "서버 연결 설정";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDisconnect.Font = new Font("나눔고딕", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDisconnect.Image = (Image)resources.GetObject("btnDisconnect.Image");
            btnDisconnect.ImageAlign = ContentAlignment.MiddleLeft;
            btnDisconnect.Location = new Point(2184, 36);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Padding = new Padding(10, 0, 0, 0);
            btnDisconnect.Size = new Size(175, 55);
            btnDisconnect.TabIndex = 32;
            btnDisconnect.Text = "   연결 끊기";
            btnDisconnect.TextAlign = ContentAlignment.MiddleRight;
            btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // lblStatus2
            // 
            lblStatus2.AutoSize = true;
            lblStatus2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblStatus2.ForeColor = Color.Green;
            lblStatus2.Location = new Point(1659, 53);
            lblStatus2.Name = "lblStatus2";
            lblStatus2.Size = new Size(89, 31);
            lblStatus2.TabIndex = 31;
            lblStatus2.Text = "연결됨";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(1543, 53);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(89, 31);
            lblStatus.TabIndex = 30;
            lblStatus.Text = "상태：";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(1468, 44);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(62, 47);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 29;
            pictureBox1.TabStop = false;
            // 
            // picUser
            // 
            picUser.Image = Properties.Resources.icons8_user_301;
            picUser.Location = new Point(1022, 44);
            picUser.Name = "picUser";
            picUser.Size = new Size(62, 47);
            picUser.SizeMode = PictureBoxSizeMode.Zoom;
            picUser.TabIndex = 28;
            picUser.TabStop = false;
            // 
            // lblUser2
            // 
            lblUser2.AutoSize = true;
            lblUser2.Font = new Font("나눔고딕", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblUser2.ForeColor = Color.Blue;
            lblUser2.Location = new Point(1217, 53);
            lblUser2.Name = "lblUser2";
            lblUser2.Size = new Size(64, 31);
            lblUser2.TabIndex = 27;
            lblUser2.Text = "없음";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(1102, 53);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(97, 31);
            lblUser.TabIndex = 0;
            lblUser.Text = "사용자:";
            // 
            // panel5
            // 
            panel5.BackColor = Color.LightGray;
            panel5.Location = new Point(967, 24);
            panel5.Name = "panel5";
            panel5.Size = new Size(1, 75);
            panel5.TabIndex = 26;
            // 
            // chkUseVenv
            // 
            chkUseVenv.AutoSize = true;
            chkUseVenv.Checked = true;
            chkUseVenv.CheckState = CheckState.Checked;
            chkUseVenv.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkUseVenv.Location = new Point(679, 53);
            chkUseVenv.Margin = new Padding(6);
            chkUseVenv.Name = "chkUseVenv";
            chkUseVenv.Size = new Size(217, 38);
            chkUseVenv.TabIndex = 25;
            chkUseVenv.Text = "가상환경 적용";
            chkUseVenv.UseVisualStyleBackColor = true;
            // 
            // rdoRemote
            // 
            rdoRemote.AutoSize = true;
            rdoRemote.Font = new Font("나눔고딕", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoRemote.Location = new Point(404, 53);
            rdoRemote.Margin = new Padding(6);
            rdoRemote.Name = "rdoRemote";
            rdoRemote.Size = new Size(184, 38);
            rdoRemote.TabIndex = 22;
            rdoRemote.Text = "원격 (서버)";
            rdoRemote.UseVisualStyleBackColor = true;
            // 
            // groupBoxDataLoad
            // 
            groupBoxDataLoad.Controls.Add(lblTubPath);
            groupBoxDataLoad.Controls.Add(lblConfigPath);
            groupBoxDataLoad.Controls.Add(btnLoadConfig);
            groupBoxDataLoad.Controls.Add(btnLoadTub);
            groupBoxDataLoad.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBoxDataLoad.Location = new Point(41, 203);
            groupBoxDataLoad.Name = "groupBoxDataLoad";
            groupBoxDataLoad.Size = new Size(433, 386);
            groupBoxDataLoad.TabIndex = 23;
            groupBoxDataLoad.TabStop = false;
            groupBoxDataLoad.Text = "데이터로드";
            groupBoxDataLoad.Enter += groupBoxDataLoad_Enter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Silver;
            panel2.Location = new Point(41, 411);
            panel2.Name = "panel2";
            panel2.Size = new Size(436, 1);
            panel2.TabIndex = 24;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(2484, 1521);
            Controls.Add(panel2);
            Controls.Add(groupBoxDataLoad);
            Controls.Add(groupBox2);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Margin = new Padding(4);
            MinimumSize = new Size(2510, 1592);
            Name = "Form1";
            Text = "Donkeycar 데이터 관리 프로그램";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            groupDataView.ResumeLayout(false);
            groupDataView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAngle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFrame).EndInit();
            groupTimeline.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabCleaner.ResumeLayout(false);
            tabCleaner.PerformLayout();
            groupBoxTrash.ResumeLayout(false);
            groupBoxTrash.PerformLayout();
            tabTrainTest.ResumeLayout(false);
            tabTrainTest.PerformLayout();
            grpPredictResult.ResumeLayout(false);
            grpPredictResult.PerformLayout();
            grpTrainProgress.ResumeLayout(false);
            grpTrainProgress.PerformLayout();
            grpTrainControl.ResumeLayout(false);
            grpTestImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picTestImage).EndInit();
            grpTrainSetting.ResumeLayout(false);
            groupFrameList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackFrame).EndInit();
            groupTubNavigator.ResumeLayout(false);
            groupTubNavigator.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picUser).EndInit();
            groupBoxDataLoad.ResumeLayout(false);
            groupBoxDataLoad.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        // (removed stale GroupBox 'groupTubLoader' — replaced by 'groupBoxDataLoad' in this version)
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
        // (removed stale GroupBox 'groupConfigLoader' — replaced by 'groupBoxDataLoad' in this version)
        private GroupBox groupBoxTrash;
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
        private Button btnStopTask;
        private Panel panel1;
        private GroupBox groupBoxDataLoad;
        private Panel panel2;
        private PictureBox pictureBoxFrame;
        private PictureBox pictureBox2;
        private PictureBox pictureBoxAngle;
        private Panel panel3;
        private ComboBox cmbPlaySpeed;
        private CheckBox chkAutoPlay;
        private Label lblSpeed;
        private Label lblCurrentFrame2;
        private Label lblCurrentFrame;
        private Label lblTrashProgress;
        private ProgressBar progressBarTrash;
        private Label lblTrashPercent;
        private GroupBox grpTestImage;
        private PictureBox picTestImage;
        private GroupBox grpTrainSetting;
        private GroupBox grpTrainControl;
        private GroupBox grpTrainProgress;
        private Label lblProgressPercent;
        private GroupBox grpPredictResult;
        private Label lblErrorValue;
        private Label lblErrorValue2;
        private Label lblPredictAngle2;
        private Label lblRealAngle2;
        private Panel panel4;
        private Label lblTrainStatus2;
        private Panel panel5;
        private Label lblUser;
        private Label lblStatus2;
        private Label lblStatus;
        private PictureBox pictureBox1;
        private PictureBox picUser;
        private Label lblUser2;
        private Button btnDisconnect;
        private Label lblAngle2;
        private Label lblFrame2;
        private Label lblThrottle2;
    }
}
