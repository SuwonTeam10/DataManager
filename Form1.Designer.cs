namespace DataManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            lstTrash = new CheckedListBox();
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
            btnModelTest = new Button();
            btnSelectModel = new Button();
            lblLog = new Label();
            txtLog = new TextBox();
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
            lblTubPath.Location = new Point(17, 209);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(93, 17);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Tub 경로: 없음";
            // 
            // btnLoadTub
            // 
            btnLoadTub.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadTub.Image = Properties.Resources.icons8_database_administrator_50;
            btnLoadTub.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoadTub.Location = new Point(12, 144);
            btnLoadTub.Margin = new Padding(3, 2, 3, 2);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Padding = new Padding(8, 0, 0, 0);
            btnLoadTub.Size = new Size(190, 40);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "   Tub 데이터 열기";
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
            groupDataView.Location = new Point(26, 380);
            groupDataView.Margin = new Padding(3, 2, 3, 2);
            groupDataView.Name = "groupDataView";
            groupDataView.Padding = new Padding(2);
            groupDataView.Size = new Size(215, 198);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // lblThrottle2
            // 
            lblThrottle2.AutoSize = true;
            lblThrottle2.Font = new Font("Microsoft Sans Serif", 12F);
            lblThrottle2.Location = new Point(156, 94);
            lblThrottle2.Margin = new Padding(2, 0, 2, 0);
            lblThrottle2.Name = "lblThrottle2";
            lblThrottle2.Size = new Size(40, 20);
            lblThrottle2.TabIndex = 12;
            lblThrottle2.Text = "0.00";
            // 
            // lblAngle2
            // 
            lblAngle2.AutoSize = true;
            lblAngle2.Font = new Font("Microsoft Sans Serif", 12F);
            lblAngle2.Location = new Point(156, 64);
            lblAngle2.Margin = new Padding(2, 0, 2, 0);
            lblAngle2.Name = "lblAngle2";
            lblAngle2.Size = new Size(40, 20);
            lblAngle2.TabIndex = 11;
            lblAngle2.Text = "0.00";
            // 
            // lblFrame2
            // 
            lblFrame2.AutoSize = true;
            lblFrame2.Font = new Font("Microsoft Sans Serif", 12F);
            lblFrame2.Location = new Point(135, 33);
            lblFrame2.Margin = new Padding(2, 0, 2, 0);
            lblFrame2.Name = "lblFrame2";
            lblFrame2.Size = new Size(63, 20);
            lblFrame2.TabIndex = 10;
            lblFrame2.Text = "000000";
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel3.BackColor = Color.Silver;
            panel3.Location = new Point(2, 134);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(269, 1);
            panel3.TabIndex = 9;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(8, 93);
            pictureBox2.Margin = new Padding(2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(37, 24);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // pictureBoxAngle
            // 
            pictureBoxAngle.BackColor = Color.Transparent;
            pictureBoxAngle.Image = (Image)resources.GetObject("pictureBoxAngle.Image");
            pictureBoxAngle.Location = new Point(8, 60);
            pictureBoxAngle.Margin = new Padding(2);
            pictureBoxAngle.Name = "pictureBoxAngle";
            pictureBoxAngle.Size = new Size(37, 24);
            pictureBoxAngle.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAngle.TabIndex = 7;
            pictureBoxAngle.TabStop = false;
            // 
            // pictureBoxFrame
            // 
            pictureBoxFrame.BackColor = Color.Transparent;
            pictureBoxFrame.Image = Properties.Resources.icons8_frame_32;
            pictureBoxFrame.Location = new Point(8, 28);
            pictureBoxFrame.Margin = new Padding(2);
            pictureBoxFrame.Name = "pictureBoxFrame";
            pictureBoxFrame.Size = new Size(37, 24);
            pictureBoxFrame.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxFrame.TabIndex = 6;
            pictureBoxFrame.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Location = new Point(0, 141);
            panel1.Margin = new Padding(2);
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
            chkShowPredictAngle.Location = new Point(27, 225);
            chkShowPredictAngle.Margin = new Padding(3, 2, 3, 2);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(119, 21);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Font = new Font("나눔고딕", 10.125F);
            chkShowRealAngle.Location = new Point(27, 189);
            chkShowRealAngle.Margin = new Padding(3, 2, 3, 2);
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
            lblThrottle.Location = new Point(47, 94);
            lblThrottle.Margin = new Padding(2, 0, 2, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(33, 20);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Font = new Font("Microsoft Sans Serif", 12F);
            lblAngle.Location = new Point(47, 64);
            lblAngle.Margin = new Padding(2, 0, 2, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(45, 20);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Font = new Font("Microsoft Sans Serif", 12F);
            lblFrame.Location = new Point(48, 33);
            lblFrame.Margin = new Padding(2, 0, 2, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(45, 20);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임\r\n";
            // 
            // groupTimeline
            // 
            groupTimeline.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupTimeline.Location = new Point(26, 368);
            groupTimeline.Margin = new Padding(3, 2, 3, 2);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Padding = new Padding(3, 2, 3, 2);
            groupTimeline.Size = new Size(1181, 61);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "썸네일 타임라인";
            // 
            // lvTimeline
            // 
            lvTimeline.Dock = DockStyle.Fill;
            lvTimeline.Location = new Point(3, 22);
            lvTimeline.Margin = new Padding(3, 2, 3, 2);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(1175, 37);
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
            tabMain.Location = new Point(29, 434);
            tabMain.Margin = new Padding(3, 2, 3, 2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1181, 217);
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
            tabCleaner.Location = new Point(4, 24);
            tabCleaner.Margin = new Padding(2);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(3, 2, 3, 2);
            tabCleaner.Size = new Size(1173, 184);
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
            groupBoxTrash.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBoxTrash.Location = new Point(436, 4);
            groupBoxTrash.Margin = new Padding(2);
            groupBoxTrash.Name = "groupBoxTrash";
            groupBoxTrash.Padding = new Padding(3, 2, 3, 2);
            groupBoxTrash.Size = new Size(592, 178);
            groupBoxTrash.TabIndex = 20;
            groupBoxTrash.TabStop = false;
            groupBoxTrash.Text = "휴지통";
            // 
            // lblTrashPercent
            // 
            lblTrashPercent.AutoSize = true;
            lblTrashPercent.Location = new Point(554, 147);
            lblTrashPercent.Margin = new Padding(2, 0, 2, 0);
            lblTrashPercent.Name = "lblTrashPercent";
            lblTrashPercent.Size = new Size(25, 15);
            lblTrashPercent.TabIndex = 22;
            lblTrashPercent.Text = "0%";
            // 
            // lblTrashProgress
            // 
            lblTrashProgress.AutoSize = true;
            lblTrashProgress.Location = new Point(287, 147);
            lblTrashProgress.Margin = new Padding(2, 0, 2, 0);
            lblTrashProgress.Name = "lblTrashProgress";
            lblTrashProgress.Size = new Size(73, 15);
            lblTrashProgress.TabIndex = 21;
            lblTrashProgress.Text = "삭제 진행률:";
            // 
            // progressBarTrash
            // 
            progressBarTrash.Location = new Point(383, 147);
            progressBarTrash.Margin = new Padding(2);
            progressBarTrash.Name = "progressBarTrash";
            progressBarTrash.Size = new Size(129, 14);
            progressBarTrash.Style = ProgressBarStyle.Continuous;
            progressBarTrash.TabIndex = 20;
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblTrastList.Location = new Point(268, 16);
            lblTrastList.Margin = new Padding(2, 0, 2, 0);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(58, 15);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "삭제 목록";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnEmptyTrash.Location = new Point(20, 64);
            btnEmptyTrash.Margin = new Padding(2);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(169, 26);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "휴지통 비우기";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnRestore.Location = new Point(117, 28);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(72, 21);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "복원";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Font = new Font("나눔고딕", 10.125F);
            btnReloadTub.Location = new Point(26, 136);
            btnReloadTub.Margin = new Padding(3, 2, 3, 2);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(169, 26);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Tub 다시 불러오기";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(268, 39);
            lstTrash.Margin = new Padding(2);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(224, 90);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Microsoft Sans Serif", 10.125F);
            btnDelete.Location = new Point(20, 28);
            btnDelete.Margin = new Padding(2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 21);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Font = new Font("나눔고딕", 10.875F);
            chkAbnormalAngle.Location = new Point(397, 152);
            chkAbnormalAngle.Margin = new Padding(3, 2, 3, 2);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(109, 22);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "비정상 조향각";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Font = new Font("나눔고딕", 10.875F);
            chkMissingImage.Location = new Point(397, 116);
            chkMissingImage.Margin = new Padding(3, 2, 3, 2);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(96, 22);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "이미지 누락";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Font = new Font("나눔고딕", 10.875F);
            chkThrottleZero.Location = new Point(397, 79);
            chkThrottleZero.Margin = new Padding(3, 2, 3, 2);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(95, 22);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "속도 0 제거";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFilter.Image = Properties.Resources.icons8_filter_48;
            btnFilter.ImageAlign = ContentAlignment.MiddleLeft;
            btnFilter.Location = new Point(309, 21);
            btnFilter.Margin = new Padding(2);
            btnFilter.Name = "btnFilter";
            btnFilter.Padding = new Padding(2, 0, 0, 0);
            btnFilter.Size = new Size(115, 24);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "필터 적용";
            btnFilter.TextAlign = ContentAlignment.MiddleRight;
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRange.Location = new Point(23, 74);
            lblRange.Margin = new Padding(2, 0, 2, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(76, 20);
            lblRange.TabIndex = 9;
            lblRange.Text = "범위: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnSetRight.Location = new Point(128, 21);
            btnSetRight.Margin = new Padding(2);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(82, 28);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "끝 지정";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Font = new Font("Microsoft Sans Serif", 10.875F);
            btnSetLeft.Location = new Point(23, 21);
            btnSetLeft.Margin = new Padding(2);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(90, 27);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "시작 지정";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // tabGraph
            // 
            tabGraph.Location = new Point(4, 24);
            tabGraph.Margin = new Padding(2);
            tabGraph.Name = "tabGraph";
            tabGraph.Padding = new Padding(3, 2, 3, 2);
            tabGraph.Size = new Size(1173, 184);
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
            tabTrainTest.Location = new Point(4, 29);
            tabTrainTest.Margin = new Padding(3, 2, 3, 2);
            tabTrainTest.Name = "tabTrainTest";
            tabTrainTest.Size = new Size(1173, 184);
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
            grpPredictResult.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpPredictResult.Location = new Point(631, 6);
            grpPredictResult.Margin = new Padding(2);
            grpPredictResult.Name = "grpPredictResult";
            grpPredictResult.Padding = new Padding(2);
            grpPredictResult.Size = new Size(220, 173);
            grpPredictResult.TabIndex = 30;
            grpPredictResult.TabStop = false;
            grpPredictResult.Text = "예측 결과";
            // 
            // panel4
            // 
            panel4.BackColor = Color.LightGray;
            panel4.ForeColor = Color.LightGray;
            panel4.Location = new Point(0, 126);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(171, 1);
            panel4.TabIndex = 25;
            // 
            // lblTrainStatus2
            // 
            lblTrainStatus2.AutoSize = true;
            lblTrainStatus2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTrainStatus2.ForeColor = Color.Green;
            lblTrainStatus2.Location = new Point(64, 140);
            lblTrainStatus2.Name = "lblTrainStatus2";
            lblTrainStatus2.Size = new Size(61, 17);
            lblTrainStatus2.TabIndex = 24;
            lblTrainStatus2.Text = "준비 완료";
            // 
            // lblErrorValue
            // 
            lblErrorValue.AutoSize = true;
            lblErrorValue.Font = new Font("나눔고딕", 10.125F);
            lblErrorValue.Location = new Point(15, 98);
            lblErrorValue.Name = "lblErrorValue";
            lblErrorValue.Size = new Size(32, 17);
            lblErrorValue.TabIndex = 23;
            lblErrorValue.Text = "오차";
            // 
            // lblErrorValue2
            // 
            lblErrorValue2.AutoSize = true;
            lblErrorValue2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblErrorValue2.ForeColor = Color.Red;
            lblErrorValue2.Location = new Point(149, 100);
            lblErrorValue2.Name = "lblErrorValue2";
            lblErrorValue2.Size = new Size(40, 17);
            lblErrorValue2.TabIndex = 22;
            lblErrorValue2.Text = "0.00";
            // 
            // lblPredictAngle2
            // 
            lblPredictAngle2.AutoSize = true;
            lblPredictAngle2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblPredictAngle2.ForeColor = Color.RoyalBlue;
            lblPredictAngle2.Location = new Point(149, 66);
            lblPredictAngle2.Name = "lblPredictAngle2";
            lblPredictAngle2.Size = new Size(40, 17);
            lblPredictAngle2.TabIndex = 21;
            lblPredictAngle2.Text = "0.00";
            // 
            // lblRealAngle2
            // 
            lblRealAngle2.AutoSize = true;
            lblRealAngle2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblRealAngle2.ForeColor = Color.RoyalBlue;
            lblRealAngle2.Location = new Point(116, 27);
            lblRealAngle2.Margin = new Padding(2, 0, 2, 0);
            lblRealAngle2.Name = "lblRealAngle2";
            lblRealAngle2.Size = new Size(40, 17);
            lblRealAngle2.TabIndex = 20;
            lblRealAngle2.Text = "0.00";
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Font = new Font("나눔고딕", 10.125F);
            lblTrainStatus.Location = new Point(16, 140);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(36, 17);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "상태:";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Font = new Font("나눔고딕", 10.125F);
            lblPredictAngle.Location = new Point(15, 66);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(72, 17);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "예측 조향각";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Font = new Font("Microsoft Sans Serif", 10.125F);
            lblRealAngle.Location = new Point(12, 27);
            lblRealAngle.Margin = new Padding(2, 0, 2, 0);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(72, 17);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "실제 조향각";
            // 
            // grpTrainProgress
            // 
            grpTrainProgress.Controls.Add(lblProgressPercent);
            grpTrainProgress.Controls.Add(lblProgress);
            grpTrainProgress.Controls.Add(progressBarTrain);
            grpTrainProgress.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainProgress.Location = new Point(467, 98);
            grpTrainProgress.Margin = new Padding(2);
            grpTrainProgress.Name = "grpTrainProgress";
            grpTrainProgress.Padding = new Padding(2);
            grpTrainProgress.Size = new Size(309, 86);
            grpTrainProgress.TabIndex = 29;
            grpTrainProgress.TabStop = false;
            grpTrainProgress.Text = "학습 진행률";
            // 
            // lblProgressPercent
            // 
            lblProgressPercent.AutoSize = true;
            lblProgressPercent.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgressPercent.Location = new Point(204, 23);
            lblProgressPercent.Margin = new Padding(2, 0, 2, 0);
            lblProgressPercent.Name = "lblProgressPercent";
            lblProgressPercent.Size = new Size(25, 15);
            lblProgressPercent.TabIndex = 25;
            lblProgressPercent.Text = "0%";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblProgress.Location = new Point(3, 23);
            lblProgress.Margin = new Padding(2, 0, 2, 0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(46, 15);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "진행률:";
            // 
            // progressBarTrain
            // 
            progressBarTrain.Location = new Point(47, 23);
            progressBarTrain.Name = "progressBarTrain";
            progressBarTrain.Size = new Size(151, 14);
            progressBarTrain.TabIndex = 24;
            // 
            // grpTrainControl
            // 
            grpTrainControl.Controls.Add(btnTrain);
            grpTrainControl.Controls.Add(btnStopTask);
            grpTrainControl.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainControl.Location = new Point(363, 6);
            grpTrainControl.Margin = new Padding(2);
            grpTrainControl.Name = "grpTrainControl";
            grpTrainControl.Padding = new Padding(2);
            grpTrainControl.Size = new Size(309, 70);
            grpTrainControl.TabIndex = 28;
            grpTrainControl.TabStop = false;
            grpTrainControl.Text = "학습 제어";
            // 
            // btnTrain
            // 
            btnTrain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnTrain.BackColor = Color.DodgerBlue;
            btnTrain.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnTrain.ForeColor = Color.White;
            btnTrain.Image = Properties.Resources.icons8_play_30__2_;
            btnTrain.ImageAlign = ContentAlignment.MiddleLeft;
            btnTrain.Location = new Point(21, 30);
            btnTrain.Margin = new Padding(3, 2, 3, 2);
            btnTrain.Name = "btnTrain";
            btnTrain.Padding = new Padding(8, 0, 0, 0);
            btnTrain.Size = new Size(96, 25);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "   학습 시작";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // btnStopTask
            // 
            btnStopTask.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnStopTask.BackColor = Color.White;
            btnStopTask.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStopTask.ForeColor = Color.Red;
            btnStopTask.Image = Properties.Resources.icons8_stop_30__1_;
            btnStopTask.ImageAlign = ContentAlignment.MiddleLeft;
            btnStopTask.Location = new Point(166, 30);
            btnStopTask.Margin = new Padding(4);
            btnStopTask.Name = "btnStopTask";
            btnStopTask.Padding = new Padding(8, 0, 0, 0);
            btnStopTask.Size = new Size(96, 25);
            btnStopTask.TabIndex = 25;
            btnStopTask.Text = "   학습 중지";
            btnStopTask.UseVisualStyleBackColor = false;
            btnStopTask.Click += btnStopTask_Click;
            // 
            // grpTestImage
            // 
            grpTestImage.Controls.Add(picTestImage);
            grpTestImage.Controls.Add(btnSelectTestImage);
            grpTestImage.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTestImage.Location = new Point(10, 98);
            grpTestImage.Margin = new Padding(2);
            grpTestImage.Name = "grpTestImage";
            grpTestImage.Padding = new Padding(2);
            grpTestImage.Size = new Size(431, 84);
            grpTestImage.TabIndex = 27;
            grpTestImage.TabStop = false;
            grpTestImage.Text = "테스트 이미지";
            // 
            // picTestImage
            // 
            picTestImage.BackColor = Color.Black;
            picTestImage.Location = new Point(201, 10);
            picTestImage.Margin = new Padding(2);
            picTestImage.Name = "picTestImage";
            picTestImage.Size = new Size(147, 65);
            picTestImage.TabIndex = 15;
            picTestImage.TabStop = false;
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Font = new Font("나눔고딕", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnSelectTestImage.Location = new Point(12, 26);
            btnSelectTestImage.Margin = new Padding(3, 2, 3, 2);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(153, 28);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "테스트 이미지 선택";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            btnSelectTestImage.Click += btnSelectTestImage_Click;
            // 
            // grpTrainSetting
            // 
            grpTrainSetting.Controls.Add(btnModelTest);
            grpTrainSetting.Controls.Add(btnSelectModel);
            grpTrainSetting.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpTrainSetting.Location = new Point(9, 6);
            grpTrainSetting.Margin = new Padding(2);
            grpTrainSetting.Name = "grpTrainSetting";
            grpTrainSetting.Padding = new Padding(2);
            grpTrainSetting.Size = new Size(431, 86);
            grpTrainSetting.TabIndex = 26;
            grpTrainSetting.TabStop = false;
            grpTrainSetting.Text = "학습 설정";
            // 
            // btnModelTest
            // 
            btnModelTest.Font = new Font("나눔고딕", 10.875F);
            btnModelTest.Image = (Image)resources.GetObject("btnModelTest.Image");
            btnModelTest.ImageAlign = ContentAlignment.MiddleLeft;
            btnModelTest.Location = new Point(144, 29);
            btnModelTest.Margin = new Padding(3, 2, 3, 2);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Padding = new Padding(12, 0, 0, 0);
            btnModelTest.Size = new Size(231, 33);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "모델 테스트 실행";
            btnModelTest.UseVisualStyleBackColor = true;
            btnModelTest.Click += btnModelTest_Click;
            // 
            // btnSelectModel
            // 
            btnSelectModel.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnSelectModel.Location = new Point(9, 22);
            btnSelectModel.Margin = new Padding(2);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(89, 25);
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
            lblLog.Location = new Point(701, 8);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(32, 17);
            lblLog.TabIndex = 21;
            lblLog.Text = "로그";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLog.Location = new Point(545, 28);
            txtLog.Margin = new Padding(2);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(452, 139);
            txtLog.TabIndex = 20;
            txtLog.Text = "준비 완료...\r\n";
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadConfig.Image = (Image)resources.GetObject("btnLoadConfig.Image");
            btnLoadConfig.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoadConfig.Location = new Point(12, 24);
            btnLoadConfig.Margin = new Padding(2);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Padding = new Padding(8, 0, 0, 0);
            btnLoadConfig.Size = new Size(190, 45);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "   설정 파일 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // lblConfigPath
            // 
            lblConfigPath.AutoSize = true;
            lblConfigPath.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblConfigPath.Location = new Point(12, 76);
            lblConfigPath.Margin = new Padding(2, 0, 2, 0);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(92, 17);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "설정 경로: 없음";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // lstFrames
            // 
            lstFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(4, 38);
            lstFrames.Margin = new Padding(2);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(228, 412);
            lstFrames.TabIndex = 0;
            // 
            // groupFrameList
            // 
            groupFrameList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupFrameList.Location = new Point(757, 95);
            groupFrameList.Margin = new Padding(2);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Padding = new Padding(3, 2, 3, 2);
            groupFrameList.Size = new Size(237, 517);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // picFrame
            // 
            picFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(19, 19);
            picFrame.Margin = new Padding(2);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(593, 351);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            picFrame.Click += picFrame_Click;
            // 
            // btnFirst
            // 
            btnFirst.Anchor = AnchorStyles.Bottom;
            btnFirst.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFirst.Location = new Point(68, 394);
            btnFirst.Margin = new Padding(3, 2, 3, 2);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(57, 32);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Anchor = AnchorStyles.Bottom;
            btnPrev.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(153, 394);
            btnPrev.Margin = new Padding(3, 2, 3, 2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(57, 32);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Anchor = AnchorStyles.Bottom;
            btnPlayStop.BackColor = Color.DodgerBlue;
            btnPlayStop.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnPlayStop.ForeColor = Color.White;
            btnPlayStop.Image = Properties.Resources.icons8_play_30;
            btnPlayStop.ImageAlign = ContentAlignment.MiddleLeft;
            btnPlayStop.Location = new Point(238, 394);
            btnPlayStop.Margin = new Padding(3, 2, 3, 2);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Padding = new Padding(12, 0, 0, 0);
            btnPlayStop.Size = new Size(123, 32);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "재생/정지";
            btnPlayStop.UseVisualStyleBackColor = false;
            btnPlayStop.Click += btnPlayStop_Click;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom;
            btnNext.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(408, 394);
            btnNext.Margin = new Padding(3, 2, 3, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(57, 32);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            btnLast.Anchor = AnchorStyles.Bottom;
            btnLast.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLast.Location = new Point(493, 394);
            btnLast.Margin = new Padding(3, 2, 3, 2);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(57, 32);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // trackFrame
            // 
            trackFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackFrame.Location = new Point(37, 452);
            trackFrame.Margin = new Padding(3, 2, 3, 2);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(445, 45);
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
            groupTubNavigator.Location = new Point(251, 95);
            groupTubNavigator.Margin = new Padding(2);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Padding = new Padding(3, 2, 3, 2);
            groupTubNavigator.Size = new Size(645, 517);
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
            lblCurrentFrame2.Location = new Point(53, 421);
            lblCurrentFrame2.Margin = new Padding(2, 0, 2, 0);
            lblCurrentFrame2.Name = "lblCurrentFrame2";
            lblCurrentFrame2.Size = new Size(30, 15);
            lblCurrentFrame2.TabIndex = 12;
            lblCurrentFrame2.Text = "0 / 0";
            // 
            // lblCurrentFrame
            // 
            lblCurrentFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCurrentFrame.AutoSize = true;
            lblCurrentFrame.Font = new Font("나눔고딕", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblCurrentFrame.Location = new Point(45, 394);
            lblCurrentFrame.Margin = new Padding(2, 0, 2, 0);
            lblCurrentFrame.Name = "lblCurrentFrame";
            lblCurrentFrame.Size = new Size(70, 15);
            lblCurrentFrame.TabIndex = 11;
            lblCurrentFrame.Text = "현재 프레임\r\n";
            // 
            // chkAutoPlay
            // 
            chkAutoPlay.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chkAutoPlay.Appearance = Appearance.Button;
            chkAutoPlay.FlatStyle = FlatStyle.Flat;
            chkAutoPlay.Font = new Font("맑은 고딕", 7.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            chkAutoPlay.Location = new Point(521, 402);
            chkAutoPlay.Margin = new Padding(2);
            chkAutoPlay.Name = "chkAutoPlay";
            chkAutoPlay.Size = new Size(68, 21);
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
            lblSpeed.Location = new Point(425, 387);
            lblSpeed.Margin = new Padding(2, 0, 2, 0);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(54, 13);
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
            cmbPlaySpeed.Location = new Point(424, 408);
            cmbPlaySpeed.Margin = new Padding(2);
            cmbPlaySpeed.Name = "cmbPlaySpeed";
            cmbPlaySpeed.Size = new Size(63, 23);
            cmbPlaySpeed.TabIndex = 7;
            // 
            // rdoLocal
            // 
            rdoLocal.AutoSize = true;
            rdoLocal.Checked = true;
            rdoLocal.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 129);
            rdoLocal.Location = new Point(21, 25);
            rdoLocal.Name = "rdoLocal";
            rdoLocal.Size = new Size(148, 22);
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
            groupBox2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBox2.Location = new Point(23, 33);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(919, 52);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "서버 연결 설정";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDisconnect.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnDisconnect.Image = (Image)resources.GetObject("btnDisconnect.Image");
            btnDisconnect.ImageAlign = ContentAlignment.MiddleLeft;
            btnDisconnect.Location = new Point(812, 16);
            btnDisconnect.Margin = new Padding(2);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Padding = new Padding(5, 0, 0, 0);
            btnDisconnect.Size = new Size(87, 26);
            btnDisconnect.TabIndex = 32;
            btnDisconnect.Text = "   연결 끊기";
            btnDisconnect.TextAlign = ContentAlignment.MiddleRight;
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // lblStatus2
            // 
            lblStatus2.AutoSize = true;
            lblStatus2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblStatus2.ForeColor = Color.Green;
            lblStatus2.Location = new Point(829, 25);
            lblStatus2.Margin = new Padding(2, 0, 2, 0);
            lblStatus2.Name = "lblStatus2";
            lblStatus2.Size = new Size(44, 17);
            lblStatus2.TabIndex = 31;
            lblStatus2.Text = "연결됨";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(772, 25);
            lblStatus.Margin = new Padding(2, 0, 2, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(46, 17);
            lblStatus.TabIndex = 30;
            lblStatus.Text = "상태：";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(734, 21);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 22);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 29;
            pictureBox1.TabStop = false;
            // 
            // picUser
            // 
            picUser.Image = Properties.Resources.icons8_user_301;
            picUser.Location = new Point(511, 21);
            picUser.Margin = new Padding(2);
            picUser.Name = "picUser";
            picUser.Size = new Size(31, 22);
            picUser.SizeMode = PictureBoxSizeMode.Zoom;
            picUser.TabIndex = 28;
            picUser.TabStop = false;
            // 
            // lblUser2
            // 
            lblUser2.AutoSize = true;
            lblUser2.Font = new Font("Microsoft Sans Serif", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblUser2.ForeColor = Color.Blue;
            lblUser2.Location = new Point(608, 25);
            lblUser2.Margin = new Padding(2, 0, 2, 0);
            lblUser2.Name = "lblUser2";
            lblUser2.Size = new Size(32, 17);
            lblUser2.TabIndex = 27;
            lblUser2.Text = "없음";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(551, 25);
            lblUser.Margin = new Padding(2, 0, 2, 0);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(48, 17);
            lblUser.TabIndex = 0;
            lblUser.Text = "사용자:";
            // 
            // panel5
            // 
            panel5.BackColor = Color.LightGray;
            panel5.Location = new Point(484, 11);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(1, 35);
            panel5.TabIndex = 26;
            // 
            // chkUseVenv
            // 
            chkUseVenv.AutoSize = true;
            chkUseVenv.Checked = true;
            chkUseVenv.CheckState = CheckState.Checked;
            chkUseVenv.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkUseVenv.Location = new Point(339, 25);
            chkUseVenv.Name = "chkUseVenv";
            chkUseVenv.Size = new Size(109, 22);
            chkUseVenv.TabIndex = 25;
            chkUseVenv.Text = "가상환경 적용";
            chkUseVenv.UseVisualStyleBackColor = true;
            // 
            // rdoRemote
            // 
            rdoRemote.AutoSize = true;
            rdoRemote.Font = new Font("Microsoft Sans Serif", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rdoRemote.Location = new Point(202, 25);
            rdoRemote.Name = "rdoRemote";
            rdoRemote.Size = new Size(92, 22);
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
            groupBoxDataLoad.Font = new Font("Microsoft Sans Serif", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 129);
            groupBoxDataLoad.Location = new Point(20, 95);
            groupBoxDataLoad.Margin = new Padding(2);
            groupBoxDataLoad.Name = "groupBoxDataLoad";
            groupBoxDataLoad.Padding = new Padding(2);
            groupBoxDataLoad.Size = new Size(278, 241);
            groupBoxDataLoad.TabIndex = 23;
            groupBoxDataLoad.TabStop = false;
            groupBoxDataLoad.Text = "데이터로드";
            groupBoxDataLoad.Enter += groupBoxDataLoad_Enter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Silver;
            panel2.Location = new Point(26, 257);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(218, 1);
            panel2.TabIndex = 24;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(962, 494);
            Controls.Add(panel2);
            Controls.Add(groupBoxDataLoad);
            Controls.Add(groupBox2);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Margin = new Padding(2);
            MinimumSize = new Size(967, 505);
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
        private TabPage tabGraph;
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
        private CheckedListBox lstTrash;
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