using System;
using System.Windows.Forms;

namespace NEAT
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
            InfoManager.setTextBox(rtxtInformation);
            InfoManager.addLine("System Loaded");
            InfoManager.clearLine();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbGames.SelectedIndex = 0;
        }

        private void btnSelectGame_Click(object sender, EventArgs e)
        {
            int game = cmbGames.SelectedIndex;
            String gameName = (String)cmbGames.Items[game];

            GameManager.load(gameName);
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            game2048 game2048 = new game2048();
            game2048.StartPosition = FormStartPosition.CenterScreen;
            game2048.Show();
            game2048.Focus();
        }
    }
}
