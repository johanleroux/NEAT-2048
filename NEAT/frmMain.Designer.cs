namespace NEAT
{
    partial class frmMain
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
            this.lblGame = new System.Windows.Forms.Label();
            this.cmbGames = new System.Windows.Forms.ComboBox();
            this.btnSelectGame = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.rtxtInformation = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblGame
            // 
            this.lblGame.AutoSize = true;
            this.lblGame.Location = new System.Drawing.Point(12, 9);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(77, 13);
            this.lblGame.TabIndex = 0;
            this.lblGame.Text = "Select a Game";
            // 
            // cmbGames
            // 
            this.cmbGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGames.FormattingEnabled = true;
            this.cmbGames.Items.AddRange(new object[] {
            "2048",
            "Minesweeper",
            "Snake"});
            this.cmbGames.Location = new System.Drawing.Point(12, 25);
            this.cmbGames.Name = "cmbGames";
            this.cmbGames.Size = new System.Drawing.Size(150, 21);
            this.cmbGames.Sorted = true;
            this.cmbGames.TabIndex = 1;
            // 
            // btnSelectGame
            // 
            this.btnSelectGame.Location = new System.Drawing.Point(12, 52);
            this.btnSelectGame.Name = "btnSelectGame";
            this.btnSelectGame.Size = new System.Drawing.Size(150, 23);
            this.btnSelectGame.TabIndex = 2;
            this.btnSelectGame.Text = "Select";
            this.btnSelectGame.UseVisualStyleBackColor = true;
            this.btnSelectGame.Click += new System.EventHandler(this.btnSelectGame_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(12, 107);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(150, 23);
            this.btnTrain.TabIndex = 3;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 136);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(150, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // rtxtInformation
            // 
            this.rtxtInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtInformation.Location = new System.Drawing.Point(261, 12);
            this.rtxtInformation.Name = "rtxtInformation";
            this.rtxtInformation.ReadOnly = true;
            this.rtxtInformation.Size = new System.Drawing.Size(350, 337);
            this.rtxtInformation.TabIndex = 6;
            this.rtxtInformation.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 361);
            this.Controls.Add(this.rtxtInformation);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnSelectGame);
            this.Controls.Add(this.cmbGames);
            this.Controls.Add(this.lblGame);
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI NEAT - Control Panel";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGame;
        private System.Windows.Forms.ComboBox cmbGames;
        private System.Windows.Forms.Button btnSelectGame;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.RichTextBox rtxtInformation;
    }
}

