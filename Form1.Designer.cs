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
            tabPage2 = new TabPage();
            tabPage1 = new TabPage();
            btnSelectTestImage = new Button();
            btnSelectModel = new Button();
            btnModelTest = new Button();
            btnTrain = new Button();
            lblProgress = new Label();
            progressTrain = new ProgressBar();
            lblTrainStatus = new Label();
            lblRealAngle = new Label();
            lblPredictAngle = new Label();
            lblLog = new Label();
            txtLog = new TextBox();
            chkAbnormalAngle = new CheckBox();
            chkMissingImage = new CheckBox();
            chkThrottleZero = new CheckBox();
            btnFilter = new Button();
            lblRange = new Label();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            lblTrastList = new Label();
            btnEmptyTrash = new Button();
            btnRestore = new Button();
            btnReloadTub = new Button();
            lstTrash = new ListBox();
            btnDelete = new Button();
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
            groupConfigLoader.Location = new Point(12, 12);
            groupConfigLoader.Name = "groupConfigLoader";
            groupConfigLoader.Size = new Size(584, 86);
            groupConfigLoader.TabIndex = 1;
            groupConfigLoader.TabStop = false;
            groupConfigLoader.Text = "Config Loader";
            // 
            // lblConfigPath
            // 
            lblConfigPath.AutoSize = true;
            lblConfigPath.Location = new Point(288, 38);
            lblConfigPath.Name = "lblConfigPath";
            lblConfigPath.Size = new Size(209, 32);
            lblConfigPath.TabIndex = 1;
            lblConfigPath.Text = "Config path: none";
            lblConfigPath.Click += lblConfigPath_Click;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Location = new Point(6, 38);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(244, 42);
            btnLoadConfig.TabIndex = 0;
            btnLoadConfig.Text = "설정 열기";
            btnLoadConfig.UseVisualStyleBackColor = true;
            // 
            // groupTubLoader
            // 
            groupTubLoader.Controls.Add(lblTubPath);
            groupTubLoader.Controls.Add(btnLoadTub);
            groupTubLoader.Location = new Point(671, 12);
            groupTubLoader.Name = "groupTubLoader";
            groupTubLoader.Size = new Size(582, 86);
            groupTubLoader.TabIndex = 2;
            groupTubLoader.TabStop = false;
            groupTubLoader.Text = "Tub Loader";
            // 
            // lblTubPath
            // 
            lblTubPath.AutoSize = true;
            lblTubPath.Location = new Point(291, 38);
            lblTubPath.Name = "lblTubPath";
            lblTubPath.Size = new Size(209, 32);
            lblTubPath.TabIndex = 2;
            lblTubPath.Text = "Config path: none";
            // 
            // btnLoadTub
            // 
            btnLoadTub.Location = new Point(6, 38);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(244, 42);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "Tub 열기";
            btnLoadTub.UseVisualStyleBackColor = true;
            // 
            // groupFrameList
            // 
            groupFrameList.Controls.Add(lstFrames);
            groupFrameList.Location = new Point(21, 104);
            groupFrameList.Name = "groupFrameList";
            groupFrameList.Size = new Size(182, 336);
            groupFrameList.TabIndex = 3;
            groupFrameList.TabStop = false;
            groupFrameList.Text = "프레임 목록";
            // 
            // lstFrames
            // 
            lstFrames.FormattingEnabled = true;
            lstFrames.Location = new Point(15, 51);
            lstFrames.Name = "lstFrames";
            lstFrames.Size = new Size(141, 228);
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
            groupTubNavigator.Location = new Point(266, 104);
            groupTubNavigator.Name = "groupTubNavigator";
            groupTubNavigator.Size = new Size(721, 447);
            groupTubNavigator.TabIndex = 4;
            groupTubNavigator.TabStop = false;
            groupTubNavigator.Text = "프레임 탐색기";
            groupTubNavigator.Enter += groupTubNavigator_Enter;
            // 
            // trackFrame
            // 
            trackFrame.Location = new Point(68, 369);
            trackFrame.Maximum = 100;
            trackFrame.Name = "trackFrame";
            trackFrame.Size = new Size(596, 90);
            trackFrame.TabIndex = 6;
            trackFrame.TickFrequency = 10;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(579, 316);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(85, 47);
            btnLast.TabIndex = 5;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(473, 316);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(76, 47);
            btnNext.TabIndex = 4;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPlayStop
            // 
            btnPlayStop.Location = new Point(269, 316);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Size = new Size(158, 47);
            btnPlayStop.TabIndex = 3;
            btnPlayStop.Text = "Play/Stop";
            btnPlayStop.UseVisualStyleBackColor = true;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(149, 316);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(76, 47);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(35, 316);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(85, 47);
            btnFirst.TabIndex = 1;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            // 
            // picFrame
            // 
            picFrame.BackColor = Color.Black;
            picFrame.BorderStyle = BorderStyle.FixedSingle;
            picFrame.Location = new Point(35, 51);
            picFrame.Name = "picFrame";
            picFrame.Size = new Size(653, 244);
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
            groupDataView.Location = new Point(1011, 118);
            groupDataView.Name = "groupDataView";
            groupDataView.Size = new Size(285, 373);
            groupDataView.TabIndex = 5;
            groupDataView.TabStop = false;
            groupDataView.Text = "데이터 정보";
            // 
            // chkShowPredictAngle
            // 
            chkShowPredictAngle.AutoSize = true;
            chkShowPredictAngle.Location = new Point(23, 313);
            chkShowPredictAngle.Name = "chkShowPredictAngle";
            chkShowPredictAngle.Size = new Size(230, 36);
            chkShowPredictAngle.TabIndex = 4;
            chkShowPredictAngle.Text = "예측 조향각 표시";
            chkShowPredictAngle.UseVisualStyleBackColor = true;
            // 
            // chkShowRealAngle
            // 
            chkShowRealAngle.AutoSize = true;
            chkShowRealAngle.Location = new Point(23, 260);
            chkShowRealAngle.Name = "chkShowRealAngle";
            chkShowRealAngle.Size = new Size(230, 36);
            chkShowRealAngle.TabIndex = 3;
            chkShowRealAngle.Text = "실제 조향각 표시";
            chkShowRealAngle.UseVisualStyleBackColor = true;
            // 
            // lblThrottle
            // 
            lblThrottle.AutoSize = true;
            lblThrottle.Location = new Point(23, 166);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(119, 32);
            lblThrottle.TabIndex = 2;
            lblThrottle.Text = "속도: 0.00";
            // 
            // lblAngle
            // 
            lblAngle.AutoSize = true;
            lblAngle.Location = new Point(23, 104);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(143, 32);
            lblAngle.TabIndex = 1;
            lblAngle.Text = "조향각: 0.00";
            // 
            // lblFrame
            // 
            lblFrame.AutoSize = true;
            lblFrame.Location = new Point(23, 47);
            lblFrame.Name = "lblFrame";
            lblFrame.Size = new Size(177, 32);
            lblFrame.TabIndex = 0;
            lblFrame.Text = "프레임: 000000";
            // 
            // groupTimeline
            // 
            groupTimeline.Controls.Add(lvTimeline);
            groupTimeline.Location = new Point(18, 557);
            groupTimeline.Name = "groupTimeline";
            groupTimeline.Size = new Size(1220, 138);
            groupTimeline.TabIndex = 6;
            groupTimeline.TabStop = false;
            groupTimeline.Text = "Thumbnail Timeline";
            // 
            // lvTimeline
            // 
            lvTimeline.Location = new Point(9, 58);
            lvTimeline.MultiSelect = false;
            lvTimeline.Name = "lvTimeline";
            lvTimeline.Size = new Size(1205, 58);
            lvTimeline.TabIndex = 0;
            lvTimeline.UseCompatibleStateImageBehavior = false;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabCleaner);
            tabMain.Controls.Add(tabPage2);
            tabMain.Controls.Add(tabPage1);
            tabMain.Location = new Point(18, 701);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1285, 301);
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
            tabCleaner.Location = new Point(8, 46);
            tabCleaner.Name = "tabCleaner";
            tabCleaner.Padding = new Padding(3);
            tabCleaner.Size = new Size(1269, 247);
            tabCleaner.TabIndex = 0;
            tabCleaner.Text = "Cleaner";
            tabCleaner.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(8, 46);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1269, 247);
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
            tabPage1.Location = new Point(8, 46);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1269, 247);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Train/Test";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSelectTestImage
            // 
            btnSelectTestImage.Location = new Point(294, 16);
            btnSelectTestImage.Name = "btnSelectTestImage";
            btnSelectTestImage.Size = new Size(221, 46);
            btnSelectTestImage.TabIndex = 14;
            btnSelectTestImage.Text = "Select Test Image";
            btnSelectTestImage.UseVisualStyleBackColor = true;
            // 
            // btnSelectModel
            // 
            btnSelectModel.Location = new Point(121, 16);
            btnSelectModel.Name = "btnSelectModel";
            btnSelectModel.Size = new Size(166, 46);
            btnSelectModel.TabIndex = 13;
            btnSelectModel.Text = "Select Model";
            btnSelectModel.UseVisualStyleBackColor = true;
            // 
            // btnModelTest
            // 
            btnModelTest.Location = new Point(538, 16);
            btnModelTest.Name = "btnModelTest";
            btnModelTest.Size = new Size(164, 46);
            btnModelTest.TabIndex = 12;
            btnModelTest.Text = "Model Test";
            btnModelTest.UseVisualStyleBackColor = true;
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(17, 16);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(98, 46);
            btnTrain.TabIndex = 11;
            btnTrain.Text = "Train";
            btnTrain.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(19, 134);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(118, 32);
            lblProgress.TabIndex = 17;
            lblProgress.Text = "Progress: ";
            // 
            // progressTrain
            // 
            progressTrain.Location = new Point(143, 134);
            progressTrain.Name = "progressTrain";
            progressTrain.Size = new Size(376, 40);
            progressTrain.TabIndex = 16;
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Location = new Point(19, 79);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(158, 32);
            lblTrainStatus.TabIndex = 15;
            lblTrainStatus.Text = "Status: Ready";
            // 
            // lblRealAngle
            // 
            lblRealAngle.AutoSize = true;
            lblRealAngle.Location = new Point(572, 79);
            lblRealAngle.Name = "lblRealAngle";
            lblRealAngle.Size = new Size(188, 32);
            lblRealAngle.TabIndex = 19;
            lblRealAngle.Text = "Real Angle: 0.00";
            // 
            // lblPredictAngle
            // 
            lblPredictAngle.AutoSize = true;
            lblPredictAngle.Location = new Point(572, 136);
            lblPredictAngle.Name = "lblPredictAngle";
            lblPredictAngle.Size = new Size(243, 32);
            lblPredictAngle.TabIndex = 18;
            lblPredictAngle.Text = "Predicted Angle: 0.00";
            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(840, 25);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(54, 32);
            lblLog.TabIndex = 21;
            lblLog.Text = "Log";
            // 
            // txtLog
            // 
            txtLog.BackColor = SystemColors.ButtonHighlight;
            txtLog.Location = new Point(835, 72);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(431, 39);
            txtLog.TabIndex = 20;
            txtLog.Text = "Donkey ready...";
            // 
            // chkAbnormalAngle
            // 
            chkAbnormalAngle.AutoSize = true;
            chkAbnormalAngle.Location = new Point(406, 180);
            chkAbnormalAngle.Name = "chkAbnormalAngle";
            chkAbnormalAngle.Size = new Size(226, 36);
            chkAbnormalAngle.TabIndex = 13;
            chkAbnormalAngle.Text = "Abnormal angle ";
            chkAbnormalAngle.UseVisualStyleBackColor = true;
            // 
            // chkMissingImage
            // 
            chkMissingImage.AutoSize = true;
            chkMissingImage.Location = new Point(406, 138);
            chkMissingImage.Name = "chkMissingImage";
            chkMissingImage.Size = new Size(210, 36);
            chkMissingImage.TabIndex = 12;
            chkMissingImage.Text = "Missing image ";
            chkMissingImage.UseVisualStyleBackColor = true;
            // 
            // chkThrottleZero
            // 
            chkThrottleZero.AutoSize = true;
            chkThrottleZero.Location = new Point(406, 96);
            chkThrottleZero.Name = "chkThrottleZero";
            chkThrottleZero.Size = new Size(251, 36);
            chkThrottleZero.TabIndex = 11;
            chkThrottleZero.Text = "Remove throttle=0";
            chkThrottleZero.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(406, 23);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(164, 52);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "Set Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Location = new Point(13, 96);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(154, 32);
            lblRange.TabIndex = 9;
            lblRange.Text = "Range: 0 ~ 0";
            // 
            // btnSetRight
            // 
            btnSetRight.Location = new Point(188, 23);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(164, 52);
            btnSetRight.TabIndex = 8;
            btnSetRight.Text = "Set Right";
            btnSetRight.UseVisualStyleBackColor = true;
            // 
            // btnSetLeft
            // 
            btnSetLeft.Location = new Point(13, 23);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(169, 52);
            btnSetLeft.TabIndex = 7;
            btnSetLeft.Text = "Set Left";
            btnSetLeft.UseVisualStyleBackColor = true;
            // 
            // lblTrastList
            // 
            lblTrastList.AutoSize = true;
            lblTrastList.Location = new Point(1067, 10);
            lblTrastList.Name = "lblTrastList";
            lblTrastList.Size = new Size(115, 32);
            lblTrastList.TabIndex = 19;
            lblTrastList.Text = "Trash List";
            // 
            // btnEmptyTrash
            // 
            btnEmptyTrash.Location = new Point(750, 96);
            btnEmptyTrash.Name = "btnEmptyTrash";
            btnEmptyTrash.Size = new Size(161, 49);
            btnEmptyTrash.TabIndex = 18;
            btnEmptyTrash.Text = "Empty Trash";
            btnEmptyTrash.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(898, 26);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(124, 49);
            btnRestore.TabIndex = 17;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnReloadTub
            // 
            btnReloadTub.Location = new Point(889, 180);
            btnReloadTub.Name = "btnReloadTub";
            btnReloadTub.Size = new Size(162, 49);
            btnReloadTub.TabIndex = 16;
            btnReloadTub.Text = "Reload Tub";
            btnReloadTub.UseVisualStyleBackColor = true;
            // 
            // lstTrash
            // 
            lstTrash.FormattingEnabled = true;
            lstTrash.Location = new Point(1069, 45);
            lstTrash.Name = "lstTrash";
            lstTrash.Size = new Size(169, 196);
            lstTrash.TabIndex = 15;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(750, 26);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(124, 49);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1308, 1014);
            Controls.Add(tabMain);
            Controls.Add(groupTimeline);
            Controls.Add(groupDataView);
            Controls.Add(groupTubNavigator);
            Controls.Add(groupFrameList);
            Controls.Add(groupTubLoader);
            Controls.Add(groupConfigLoader);
            Name = "Form1";
            Text = "Form1";
            groupConfigLoader.ResumeLayout(false);
            groupConfigLoader.PerformLayout();
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
