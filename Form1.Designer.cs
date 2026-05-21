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
            groupConfigLoader = new GroupBox();
            lblConfigPath = new Label();
            btnLoadConfig = new Button();
            groupTubLoader = new GroupBox();
            lblTubPath = new Label();
            btnLoadTub = new Button();
            groupFrameList = new GroupBox();
            lstFrames = new ListBox();
            groupTubNavigator = new GroupBox();
            trackFrame = new TrackBar();
            btnLast = new Button();
            btnNext = new Button();
            btnPlayStop = new Button();
            btnPrev = new Button();
            btnFirst = new Button();
            picFrame = new PictureBox();
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
            tabPage2 = new TabPage();
            tabPage1 = new TabPage();
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
            groupConfigLoader.SuspendLayout();
            groupTubLoader.SuspendLayout();
            groupFrameList.SuspendLayout();
            groupTubNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackFrame).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picFrame).BeginInit();
            groupDataView.SuspendLayout();
            groupTimeline.SuspendLayout();
            tabMain.SuspendLayout();
            tabCleaner.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // groupConfigLoader
            // 
            groupConfigLoader.Controls.Add(lblConfigPath);
            groupConfigLoader.Controls.Add(btnLoadConfig);
            groupConfigLoader.Location = new Point(8, 8);
            groupConfigLoader.Margin = new Padding(2);
            groupConfigLoader.Name = "groupConfigLoader";
            groupConfigLoader.Padding = new Padding(2);
            groupConfigLoader.Size = new Size(375, 54);
            groupConfigLoader.TabIndex = 1;
            groupConfigLoader.TabStop = false;
            groupConfigLoader.Text = "Config Loader";
            // 
            // lblConfigPath
            // 
            lblConfigPath.Location = new Point(185, 24);
            lblConfigPath.Margin = new Padding(2, 0, 2, 0);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(134, 20);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "Config path: none";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Location = new Point(4, 24);
            btnLoadConfig.Margin = new Padding(2);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(157, 26);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "설정 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // groupTubLoader
            // 
            groupTubLoader.Controls.Add(lblTubPath);
            groupTubLoader.Controls.Add(btnLoadTub);
            groupTubLoader.Location = new Point(431, 8);
            groupTubLoader.Margin = new Padding(2);
            groupTubLoader.Name = "groupTubLoader";
            groupTubLoader.Padding = new Padding(2);
            groupTubLoader.Size = new Size(374, 54);
            groupTubLoader.TabIndex = 2;
            groupTubLoader.TabStop = false;
            groupTubLoader.Text = "Tub Loader";
            // 
            // lblTubPath
            // 
            lblTubPath.AutoSize = true;
            lblTubPath.Location = new Point(187, 24);
            lblTubPath.Margin = new Padding(2, 0, 2, 0);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(134, 20);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Config path: none";
            // 
            // btnLoadTub
            // 
            btnLoadTub.Location = new Point(4, 24);
            btnLoadTub.Margin = new Padding(2);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(157, 26);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "Tub 열기";
            btnLoadTub.UseVisualStyleBackColor = true;
            // 
            // groupFrameList
            // 
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Location = new Point(14, 65);
            groupFrameList.Margin = new Padding(2);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Padding = new Padding(2);
            groupFrameList.Size = new Size(117, 210);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // lstFrames
            // 
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(10, 32);
            lstFrames.Margin = new Padding(2);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(92, 144);
            lstFrames.TabIndex = 0;
            // 
            // groupTubNavigator
            // 
            groupTubNavigator.Controls.Add(trackFrame);
            groupTubNavigator.Controls.Add(btnLast);
            groupTubNavigator.Controls.Add(btnNext);
            groupTubNavigator.Controls.Add(btnPlayStop);
            groupTubNavigator.Controls.Add(btnPrev);
            groupTubNavigator.Controls.Add(btnFirst);
            groupTubNavigator.Controls.Add(picFrame);
            groupTubNavigator.Location = new Point(171, 65);
            groupTubNavigator.Margin = new Padding(2);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Padding = new Padding(2);
            groupTubNavigator.Size = new Size(464, 279);
            groupTubNavigator.TabIndex = 4;
            groupTubNavigator.TabStop = false;
            groupTubNavigator.Text = "프레임 탐색기";
            groupTubNavigator.Enter += groupTubNavigator_Enter;
            // 
            // trackFrame
            // 
            trackFrame.Location = new Point(44, 231);
            trackFrame.Margin = new Padding(2);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(383, 56);
            trackFrame.TabIndex = 6;
            trackFrame.TickFrequency = 10;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(372, 198);
            btnLast.Margin = new Padding(2);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(55, 29);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(304, 198);
            btnNext.Margin = new Padding(2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(49, 29);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Location = new Point(173, 198);
            btnPlayStop.Margin = new Padding(2);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Size = new Size(102, 29);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "Play/Stop";
            btnPlayStop.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(96, 198);
            btnPrev.Margin = new Padding(2);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(49, 29);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(22, 198);
            btnFirst.Margin = new Padding(2);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(55, 29);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // picFrame
            // 
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(22, 32);
            picFrame.Margin = new Padding(2);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(420, 153);
            picFrame.SizeMode = PictureBoxSizeMode.Zoom;
            picFrame.TabIndex = 0;
            picFrame.TabStop = false;
            // 
            // groupDataView
            // 
            groupDataView.Controls.Add(chkShowPredictAngle);
            groupDataView.Controls.Add(chkShowRealAngle);
            groupDataView.Controls.Add(lblThrottle);
            groupDataView.Controls.Add(lblAngle);
            groupDataView.Controls.Add(lblFrame);
            groupDataView.Location = new Point(650, 74);
            groupDataView.Margin = new Padding(2);
            groupDataView.Name = "groupDataView";
            groupDataView.Padding = new Padding(2);
            groupDataView.Size = new Size(183, 233);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // chkShowPredictAngle
            // 
            chkShowPredictAngle.AutoSize = true;
            chkShowPredictAngle.Location = new Point(15, 196);
            chkShowPredictAngle.Margin = new Padding(2);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(146, 24);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Location = new Point(15, 162);
            chkShowRealAngle.Margin = new Padding(2);
            chkShowRealAngle.Name = "chkShowRealAngle";
            chkShowRealAngle.Size = new Size(146, 24);
            chkShowRealAngle.TabIndex = 3;
            chkShowRealAngle.Text = "실제 조향각 표시";
            chkShowRealAngle.UseVisualStyleBackColor = true;
            // 
            // lblThrottle
            // 
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(15, 104);
            lblThrottle.Margin = new Padding(2, 0, 2, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(74, 20);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도: 0.00";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(15, 65);
            lblAngle.Margin = new Padding(2, 0, 2, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(89, 20);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각: 0.00";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Location = new Point(15, 29);
            lblFrame.Margin = new Padding(2, 0, 2, 0);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(110, 20);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임: 000000";
            // 
            // groupTimeline
            // 
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Location = new Point(12, 348);
            groupTimeline.Margin = new Padding(2);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Padding = new Padding(2);
            groupTimeline.Size = new Size(784, 86);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "Thumbnail Timeline";
            // 
            // lvTimeline
            // 
            lvTimeline.Location = new Point(6, 36);
            lvTimeline.Margin = new Padding(2);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(776, 38);
            lvTimeline.TabIndex = 0;
            lvTimeline.UseCompatibleStateImageBehavior = false;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabCleaner);
            tabMain.Controls.Add(tabPage2);
            tabMain.Controls.Add(tabPage1);
            tabMain.Location = new Point(12, 438);
            tabMain.Margin = new Padding(2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(826, 188);
            tabMain.TabIndex = 7;
            // 
            // tabCleaner
            // 
            tabCleaner.Controls.Add(lblTrastList);
            tabCleaner.Controls.Add(btnEmptyTrash);
            tabCleaner.Controls.Add(btnRestore);
            tabCleaner.Controls.Add(btnReloadTub);
            tabCleaner.Controls.Add(lstTrash);
            tabCleaner.Controls.Add(btnDelete);
            tabCleaner.Controls.Add(chkAbnormalAngle);
            tabCleaner.Controls.Add(chkMissingImage);
            tabCleaner.Controls.Add(chkThrottleZero);
            tabCleaner.Controls.Add(btnFilter);
            tabCleaner.Controls.Add(lblRange);
            tabCleaner.Controls.Add(btnSetRight);
            tabCleaner.Controls.Add(btnSetLeft);
            tabCleaner.Location = new Point(4, 29);
            tabCleaner.Margin = new Padding(2);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(2);
            tabCleaner.Size = new Size(818, 155);
            tabCleaner.TabIndex = 0;
            tabCleaner.Text = "Cleaner";
            tabCleaner.UseVisualStyleBackColor = true;
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Location = new Point(686, 6);
            lblTrastList.Margin = new Padding(2, 0, 2, 0);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(72, 20);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "Trash List";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Location = new Point(482, 60);
            btnEmptyTrash.Margin = new Padding(2);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(104, 31);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "Empty Trash";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(577, 16);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(80, 31);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Location = new Point(572, 112);
            btnReloadTub.Margin = new Padding(2);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(104, 31);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Reload Tub";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(687, 28);
            lstTrash.Margin = new Padding(2);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(110, 124);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(482, 16);
            btnDelete.Margin = new Padding(2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 31);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Location = new Point(261, 112);
            chkAbnormalAngle.Margin = new Padding(2);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(146, 24);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "Abnormal angle ";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Location = new Point(261, 86);
            chkMissingImage.Margin = new Padding(2);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(135, 24);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "Missing image ";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Location = new Point(261, 60);
            chkThrottleZero.Margin = new Padding(2);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(159, 24);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "Remove throttle=0";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(261, 14);
            btnFilter.Margin = new Padding(2);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(105, 32);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "Set Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Location = new Point(8, 60);
            lblRange.Margin = new Padding(2, 0, 2, 0);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(97, 20);
            lblRange.TabIndex = 9;
            lblRange.Text = "Range: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Location = new Point(121, 14);
            btnSetRight.Margin = new Padding(2);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(105, 32);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "Set Right";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Location = new Point(8, 14);
            btnSetLeft.Margin = new Padding(2);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(109, 32);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "Set Left";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(818, 155);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Graph";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(lblLog);
            tabPage1.Controls.Add(txtLog);
            tabPage1.Controls.Add(lblRealAngle);
            tabPage1.Controls.Add(lblPredictAngle);
            tabPage1.Controls.Add(lblProgress);
            tabPage1.Controls.Add(progressTrain);
            tabPage1.Controls.Add(lblTrainStatus);
            tabPage1.Controls.Add(btnSelectTestImage);
            tabPage1.Controls.Add(btnSelectModel);
            tabPage1.Controls.Add(btnModelTest);
            tabPage1.Controls.Add(btnTrain);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(818, 155);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Train/Test";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(540, 16);
            lblLog.Margin = new Padding(2, 0, 2, 0);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(34, 20);
            lblLog.TabIndex = 21;
            lblLog.Text = "Log";
            // 
            // txtLog
            // 
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Location = new Point(537, 45);
            txtLog.Margin = new Padding(2);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(278, 26);
            txtLog.TabIndex = 20;
            txtLog.Text = "Donkey ready...";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Location = new Point(368, 49);
            lblRealAngle.Margin = new Padding(2, 0, 2, 0);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(118, 20);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "Real Angle: 0.00";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Location = new Point(368, 85);
            lblPredictAngle.Margin = new Padding(2, 0, 2, 0);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(153, 20);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "Predicted Angle: 0.00";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(12, 84);
            lblProgress.Margin = new Padding(2, 0, 2, 0);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(74, 20);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "Progress: ";
            // 
            // progressTrain
            // 
            progressTrain.Location = new Point(92, 84);
            progressTrain.Margin = new Padding(2);
            progressTrain.Name = "progressTrain";
            progressTrain.Size = new Size(242, 25);
            progressTrain.TabIndex = 16;
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Location = new Point(12, 49);
            lblTrainStatus.Margin = new Padding(2, 0, 2, 0);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(99, 20);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "Status: Ready";
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Location = new Point(189, 10);
            btnSelectTestImage.Margin = new Padding(2);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(142, 29);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "Select Test Image";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            // 
            // btnSelectModel
            // 
            btnSelectModel.Location = new Point(78, 10);
            btnSelectModel.Margin = new Padding(2);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(107, 29);
            btnSelectModel.TabIndex = 13;
            btnSelectModel.Text = "Select Model";
            btnSelectModel.UseVisualStyleBackColor = true;
            // 
            // btnModelTest
            // 
            btnModelTest.Location = new Point(346, 10);
            btnModelTest.Margin = new Padding(2);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Size = new Size(105, 29);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "Model Test";
            btnModelTest.UseVisualStyleBackColor = true;
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(11, 10);
            btnTrain.Margin = new Padding(2);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(63, 29);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 634);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Controls.Add(groupTubLoader);
            Controls.Add(groupConfigLoader);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            groupConfigLoader.ResumeLayout(false);
            groupTubLoader.ResumeLayout(false);
            groupTubLoader.PerformLayout();
            groupFrameList.ResumeLayout(false);
            groupTubNavigator.ResumeLayout(false);
            groupTubNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackFrame).EndInit();
            ((System.ComponentModel.ISupportInitialize)picFrame).EndInit();
            groupDataView.ResumeLayout(false);
            groupDataView.PerformLayout();
            groupTimeline.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabCleaner.ResumeLayout(false);
            tabCleaner.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupConfigLoader;
        private Label lblConfigPath;
        private Button btnLoadConfig;
        private GroupBox groupTubLoader;
        private Label lblTubPath;
        private Button btnLoadTub;
        private GroupBox groupFrameList;
        private ListBox lstFrames;
        private GroupBox groupTubNavigator;
        private TrackBar trackFrame;
        private Button btnLast;
        private Button btnNext;
        private Button btnPlayStop;
        private Button btnPrev;
        private Button btnFirst;
        private PictureBox picFrame;
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
        private TabPage tabPage2;
        private TabPage tabPage1;
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
    }
}
