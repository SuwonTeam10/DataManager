namespace DataManager
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtHost = new TextBox();
            btnConnect = new Button();
            txtUser = new TextBox();
            txtPass = new TextBox();
            btnShowHost = new Button();
            btnShowPass = new Button();
            btnClearHost = new Button();
            btnClearUser = new Button();
            btnClearPass = new Button();
            chkSaveInfo = new CheckBox();
            lbErrorMsg = new Label();
            lblLogin = new Label();
            SuspendLayout();
            // 
            // txtHost
            // 
            txtHost.Font = new Font("맑은 고딕", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtHost.ForeColor = Color.Gray;
            txtHost.Location = new Point(99, 151);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(588, 43);
            txtHost.TabIndex = 0;
            txtHost.Text = "IP주소";
            txtHost.TextChanged += txtHost_TextChanged;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(128, 255, 255);
            btnConnect.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConnect.ForeColor = Color.Blue;
            btnConnect.Location = new Point(336, 324);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(112, 47);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Login";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtUser
            // 
            txtUser.Font = new Font("맑은 고딕", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUser.ForeColor = Color.Gray;
            txtUser.Location = new Point(99, 207);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(588, 43);
            txtUser.TabIndex = 2;
            txtUser.Text = "아이디";
            txtUser.TextChanged += txtUser_TextChanged;
            // 
            // txtPass
            // 
            txtPass.Font = new Font("맑은 고딕", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPass.ForeColor = Color.Gray;
            txtPass.Location = new Point(99, 264);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(588, 43);
            txtPass.TabIndex = 3;
            txtPass.Text = "비밀번호";
            txtPass.TextChanged += txtPass_TextChanged;
            // 
            // btnShowHost
            // 
            btnShowHost.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnShowHost.Location = new Point(500, 151);
            btnShowHost.Name = "btnShowHost";
            btnShowHost.Size = new Size(90, 43);
            btnShowHost.TabIndex = 4;
            btnShowHost.Text = "😎";
            btnShowHost.UseVisualStyleBackColor = true;
            btnShowHost.Click += btnShowHost_Click_1;
            // 
            // btnShowPass
            // 
            btnShowPass.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnShowPass.Location = new Point(500, 264);
            btnShowPass.Name = "btnShowPass";
            btnShowPass.Size = new Size(90, 43);
            btnShowPass.TabIndex = 5;
            btnShowPass.Text = "😎";
            btnShowPass.UseVisualStyleBackColor = true;
            btnShowPass.Click += btnShowPass_Click_1;
            // 
            // btnClearHost
            // 
            btnClearHost.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearHost.Location = new Point(606, 151);
            btnClearHost.Name = "btnClearHost";
            btnClearHost.Size = new Size(81, 43);
            btnClearHost.TabIndex = 6;
            btnClearHost.Text = "X";
            btnClearHost.UseVisualStyleBackColor = true;
            btnClearHost.Click += btnClearHost_Click_1;
            // 
            // btnClearUser
            // 
            btnClearUser.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearUser.Location = new Point(606, 207);
            btnClearUser.Name = "btnClearUser";
            btnClearUser.Size = new Size(81, 43);
            btnClearUser.TabIndex = 7;
            btnClearUser.Text = "X";
            btnClearUser.UseVisualStyleBackColor = true;
            btnClearUser.Click += btnClearUser_Click_1;
            // 
            // btnClearPass
            // 
            btnClearPass.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearPass.Location = new Point(606, 264);
            btnClearPass.Name = "btnClearPass";
            btnClearPass.Size = new Size(81, 43);
            btnClearPass.TabIndex = 8;
            btnClearPass.Text = "X";
            btnClearPass.UseVisualStyleBackColor = true;
            btnClearPass.Click += btnClearPass_Click_1;
            // 
            // chkSaveInfo
            // 
            chkSaveInfo.AutoSize = true;
            chkSaveInfo.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkSaveInfo.Location = new Point(99, 331);
            chkSaveInfo.Name = "chkSaveInfo";
            chkSaveInfo.Size = new Size(193, 34);
            chkSaveInfo.TabIndex = 9;
            chkSaveInfo.Text = "로그인 정보 저장";
            chkSaveInfo.UseVisualStyleBackColor = true;
            chkSaveInfo.CheckedChanged += chkSaveInfo_CheckedChanged;
            // 
            // lbErrorMsg
            // 
            lbErrorMsg.AutoSize = true;
            lbErrorMsg.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbErrorMsg.ForeColor = Color.FromArgb(192, 0, 0);
            lbErrorMsg.Location = new Point(232, 387);
            lbErrorMsg.Name = "lbErrorMsg";
            lbErrorMsg.Size = new Size(322, 32);
            lbErrorMsg.TabIndex = 10;
            lbErrorMsg.Text = "IP와 아이디를 확인해주세요.";
            lbErrorMsg.Visible = false;
            lbErrorMsg.Click += lbErrorMsg_Click;
            // 
            // lblLogin
            // 
            lblLogin.AutoSize = true;
            lblLogin.Font = new Font("맑은 고딕", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLogin.ForeColor = Color.FromArgb(192, 0, 0);
            lblLogin.Location = new Point(280, 37);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(229, 86);
            lblLogin.TabIndex = 11;
            lblLogin.Text = "로그인";
            lblLogin.Visible = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(800, 450);
            Controls.Add(lblLogin);
            Controls.Add(lbErrorMsg);
            Controls.Add(chkSaveInfo);
            Controls.Add(btnClearPass);
            Controls.Add(btnClearUser);
            Controls.Add(btnClearHost);
            Controls.Add(btnShowPass);
            Controls.Add(btnShowHost);
            Controls.Add(txtPass);
            Controls.Add(txtUser);
            Controls.Add(btnConnect);
            Controls.Add(txtHost);
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtHost;
        private Button btnConnect;
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnShowHost;
        private Button btnShowPass;
        private Button btnClearHost;
        private Button btnClearUser;
        private Button btnClearPass;
        private CheckBox chkSaveInfo;
        private Label lbErrorMsg;
        private Label lblLogin;
    }
}