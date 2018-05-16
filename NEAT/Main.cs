using NEAT.NEAT;
using NEAT.Utils;
using System;
using System.Threading;
using System.Windows.Forms;

namespace NEAT
{
    public partial class Main : Form
    {
        NeatManager _neat;
        Thread _neatThread;
        ChartForm _chartForm;
        String _game;

        public Main()
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
            btnTrain.Text = "Training";
            btnTrain.Enabled = false;
            btnPlay.Enabled = false;
            cbFeedback.Enabled = false;
            if (_chartForm != null)
            {
                _chartForm.Hide();
                _chartForm.Dispose();
            }

            //_neatThread = new Thread(new ThreadStart(trainAI));
            //_neatThread.Start();
            trainAI();

            btnTrain.Text = "Train";
            btnTrain.Enabled = true;
            btnPlay.Enabled = true;
            cbFeedback.Enabled = true;
        }

        private void trainAI()
        {
            _neat = new NeatManager(_game);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            GameManager.load(_game);
        }

        private void cbFeedback_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFeedback.Checked)
            {
                _chartForm.Hide();
                _chartForm.Dispose();
            }
            else
            {
                _chartForm = new ChartForm();
                _chartForm.StartPosition = FormStartPosition.CenterParent;
                _chartForm.Show();
            }
        }
    }
}
