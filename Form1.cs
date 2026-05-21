namespace DataManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lblConfigPath_Click(object sender, EventArgs e)
        {

        }

        private void groupTubNavigator_Enter(object sender, EventArgs e)
        {

        }

        private void lblLog_Click(object sender, EventArgs e)
        {

        }

        private void lblTubPath_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "설정 파일 선택";
            ofd.Filter = "모든 파일 (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string configPath = ofd.FileName;

                lblConfigPath.Text = "설정 경로: " + configPath;
            }
            else
            {
                lblConfigPath.Text = "설정 파일을 불러오지 못했습니다.";
            }

        }

        private void btnLoadTub_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Tub 폴더 선택";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string tubPath = fbd.SelectedPath;

                string manifestPath = Path.Combine(tubPath, "manifest.json");
                string catalogPath = Path.Combine(tubPath, "catalog_0.catalog");

                if (File.Exists(manifestPath) && File.Exists(catalogPath))
                {
                    lblTubPath.Text = "Tub 경로: " + tubPath;
                }
                else
                {
                    lblTubPath.Text = "Tub 파일을 찾을 수 없습니다.";
                }
            }
            else
            {
                lblTubPath.Text = "Tub 불러오기를 취소했습니다.";
            }

        }
    }
}
