using NEAT.NEAT;
using NEAT.Utils;
using System;
using System.Windows.Forms;

namespace NEAT
{
    public partial class frmMain : Form
    {
        NeatManager _neat;
        String _game;
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
            _game = (String)cmbGames.Items[cmbGames.SelectedIndex];

            btnPlay.Enabled = true;
            btnTrain.Enabled = true;
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            game2048 game2048 = new game2048();
            game2048.StartPosition = FormStartPosition.CenterScreen;
            game2048.Show();
            game2048.Focus();
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            _neat = new NeatManager(_game);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            GameManager.load(_game);
        }
    }
}
