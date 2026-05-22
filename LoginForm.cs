using System;
using System.Drawing;
using System.IO; // 파일 저장(기능 6)을 위해 필요
using System.Windows.Forms;

namespace DataManager
{
    public partial class LoginForm : Form
    {
        // 외부(Form1)에서 가져갈 로그인 정보
        public string Host => txtHost.Text.Trim();
        public string User => txtUser.Text.Trim();
        public string Pass => txtPass.Text.Trim();

        // 기능 6: 저장할 파일 이름 지정
        private readonly string saveFilePath = "login_info.txt";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // 기능 6: 폼이 열릴 때 저장된 파일이 있으면 불러오기
            if (File.Exists(saveFilePath))
            {
                string[] info = File.ReadAllLines(saveFilePath);
                if (info.Length >= 2)
                {
                    txtHost.Text = info[0];
                    txtHost.ForeColor = Color.Black;
                    txtHost.UseSystemPasswordChar = true; // IP 마스킹

                    txtUser.Text = info[1];
                    txtUser.ForeColor = Color.Black;

                    chkSaveInfo.Checked = true;
                }
            }
            else
            {
                // 저장된 게 없으면 워터마크 세팅 (기능 1)
                SetWatermark(txtHost, "IP주소");
                SetWatermark(txtUser, "아이디");
                SetWatermark(txtPass, "비밀번호");
            }
        }

        // ==========================================
        // 기능 1 & 3: 워터마크(Placeholder) 및 마스킹 로직
        // ==========================================
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

        private void txtHost_Enter(object sender, EventArgs e) => RemoveWatermark(txtHost, "IP주소", true);
        private void txtHost_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHost.Text)) SetWatermark(txtHost, "IP주소");
        }

        private void txtUser_Enter(object sender, EventArgs e) => RemoveWatermark(txtUser, "아이디", false);
        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text)) SetWatermark(txtUser, "아이디");
        }

        private void txtPass_Enter(object sender, EventArgs e) => RemoveWatermark(txtPass, "비밀번호", true);
        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text)) SetWatermark(txtPass, "비밀번호");
        }


        // ==========================================
        // 기능 3: 보기/숨기기 버튼 로직
        // ==========================================
        private void btnShowHost_Click(object sender, EventArgs e)
        {
            if (txtHost.Text != "IP주소") txtHost.UseSystemPasswordChar = !txtHost.UseSystemPasswordChar;
        }
        private void btnShowPass_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != "비밀번호") txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;
        }


        // ==========================================
        // 기능 4: 모두 지우기 (X) 버튼 로직
        // ==========================================
        private void btnClearHost_Click(object sender, EventArgs e) { txtHost.Text = ""; txtHost.Focus(); }
        private void btnClearUser_Click(object sender, EventArgs e) { txtUser.Text = ""; txtUser.Focus(); }
        private void btnClearPass_Click(object sender, EventArgs e) { txtPass.Text = ""; txtPass.Focus(); }


        // ==========================================
        // 기능 2: 엔터(Enter) 키로 다음 칸 이동 & 로그인
        // ==========================================
        private void txtHost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtUser.Focus(); }
        }
        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; txtPass.Focus(); }
        }
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnConnect.PerformClick(); }
        }


        // ==========================================
        // 기능 7 & 8 & 6: 로그인 시도 로직
        // ==========================================
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            lbErrorMsg.Visible = false; // 에러 메시지 초기화

            // 기능 7: 공백(Trim) 및 비어있는지 검사 (Placeholder 글씨 그대로여도 막음)
            if (string.IsNullOrWhiteSpace(Host) || Host == "IP주소" ||
                string.IsNullOrWhiteSpace(User) || User == "아이디" ||
                string.IsNullOrWhiteSpace(Pass) || Pass == "비밀번호")
            {
                lbErrorMsg.Text = "정보를 모두 입력해주세요!";
                lbErrorMsg.Visible = true;
                return;
            }

            // 기능 8: 버튼 '연결 중...' 처리 및 중복 클릭 방지
            btnConnect.Enabled = false;
            btnConnect.Text = "연결 중...";

            // 약간의 딜레이를 주어 사용자가 '연결 중'을 인식하게 함 (UX 향상)
            await System.Threading.Tasks.Task.Delay(500);

            // 기능 6: 체크박스 체크 시 파일에 저장, 해제 시 파일 삭제
            if (chkSaveInfo.Checked)
            {
                File.WriteAllText(saveFilePath, $"{Host}\n{User}");
            }
            else if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            // 모든 검사 통과, OK 싸인 보내고 창 닫기
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnShowHost_Click_1(object sender, EventArgs e)
        {

        }

        private void btnShowPass_Click_1(object sender, EventArgs e)
        {

        }

        private void btnClearHost_Click_1(object sender, EventArgs e)
        {

        }

        private void btnClearUser_Click_1(object sender, EventArgs e)
        {

        }

        private void btnClearPass_Click_1(object sender, EventArgs e)
        {

        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkSaveInfo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbErrorMsg_Click(object sender, EventArgs e)
        {

        }
    }
}