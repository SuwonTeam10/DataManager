using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataManager
{
    public partial class LoginForm : Form
    {
        // 외부(Form1)에서 가져갈 로그인 정보
        public string Host => txtHost.Text.Trim();
        public string User => txtUser.Text.Trim();
        public string Pass => txtPass.Text.Trim();

        // 저장할 파일 이름 지정
        private readonly string saveFilePath = "login_info.txt";

        public LoginForm()
        {
            InitializeComponent();

            // 디자인 창 필요 없이 여기서 강제로 이벤트 연결! 
            txtHost.Enter += txtHost_Enter;
            txtHost.Leave += txtHost_Leave;
            txtHost.KeyDown += txtHost_KeyDown;

            txtUser.Enter += txtUser_Enter;
            txtUser.Leave += txtUser_Leave;
            txtUser.KeyDown += txtUser_KeyDown;

            txtPass.Enter += txtPass_Enter;
            txtPass.Leave += txtPass_Leave;
            txtPass.KeyDown += txtPass_KeyDown;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            lbErrorMsg.Visible = false;

            if (File.Exists(saveFilePath))
            {
                string[] info = File.ReadAllLines(saveFilePath);
                if (info.Length >= 2)
                {
                    txtHost.Text = info[0];
                    txtHost.ForeColor = Color.Black;
                    txtHost.UseSystemPasswordChar = true;

                    txtUser.Text = info[1];
                    txtUser.ForeColor = Color.Black;

                    chkSaveInfo.Checked = true;
                }
            }
            else
            {
                SetWatermark(txtHost, "IP주소");
                SetWatermark(txtUser, "아이디");
                SetWatermark(txtPass, "비밀번호");
            }
        }
// 워터마크
        private void SetWatermark(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Silver;
            txt.UseSystemPasswordChar = false;
        }

        private void RemoveWatermark(TextBox txt, string placeholder, bool isPassword)
        {
            if (txt.Text == placeholder)
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
                if (isPassword) txt.UseSystemPasswordChar = true;
            }
        }

        // 텍스트박스 클릭/포커스 이동 이벤트 (자동 연결)
        private void txtHost_Enter(object sender, EventArgs e) => RemoveWatermark(txtHost, "IP주소", true);
        private void txtHost_Leave(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtHost.Text)) SetWatermark(txtHost, "IP주소"); }
        private void txtHost_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtUser.Focus(); } }

        private void txtUser_Enter(object sender, EventArgs e) => RemoveWatermark(txtUser, "아이디", false);
        private void txtUser_Leave(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtUser.Text)) SetWatermark(txtUser, "아이디"); }
        private void txtUser_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtPass.Focus(); } }

        private void txtPass_Enter(object sender, EventArgs e) => RemoveWatermark(txtPass, "비밀번호", true);
        private void txtPass_Leave(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtPass.Text)) SetWatermark(txtPass, "비밀번호"); }
        private void txtPass_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnConnect.PerformClick(); } }

        // IP 보기 버튼
        private void btnShowHost_Click_1(object sender, EventArgs e)
        {
            if (txtHost.Text != "IP주소") txtHost.UseSystemPasswordChar = !txtHost.UseSystemPasswordChar;
        }

        // 비번 보기 버튼
        private void btnShowPass_Click_1(object sender, EventArgs e)
        {
            if (txtPass.Text != "비밀번호") txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;
        }

        // IP 지우기 버튼
        private void btnClearHost_Click_1(object sender, EventArgs e)
        {
            SetWatermark(txtHost, "IP주소");
            txtHost.Focus();
        }

        // 아이디 지우기 버튼
        private void btnClearUser_Click_1(object sender, EventArgs e)
        {
            SetWatermark(txtUser, "아이디");
            txtUser.Focus();
        }

        // 비번 지우기 버튼
        private void btnClearPass_Click_1(object sender, EventArgs e)
        {
            SetWatermark(txtPass, "비밀번호");
            txtPass.Focus();
        }

        // 로그인 버튼
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            lbErrorMsg.Visible = false;

            if (string.IsNullOrWhiteSpace(Host) || Host == "IP주소" ||
                string.IsNullOrWhiteSpace(User) || User == "아이디" ||
                string.IsNullOrWhiteSpace(Pass) || Pass == "비밀번호")
            {
                lbErrorMsg.Text = "정보를 모두 입력해주세요!";
                lbErrorMsg.Visible = true;
                return;
            }

            btnConnect.Enabled = false;
            btnConnect.Text = "연결 중...";

            await System.Threading.Tasks.Task.Delay(500);

            if (chkSaveInfo.Checked)
            {
                File.WriteAllText(saveFilePath, $"{Host}\n{User}");
            }
            else if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 더블클릭해서 생겼지만 안 쓰는 애들 (에러 방지)
        private void txtHost_TextChanged(object sender, EventArgs e) { }
        private void txtUser_TextChanged(object sender, EventArgs e) { }
        private void txtPass_TextChanged(object sender, EventArgs e) { }
        private void chkSaveInfo_CheckedChanged(object sender, EventArgs e) { }
        private void lbErrorMsg_Click(object sender, EventArgs e) { }
    }
}