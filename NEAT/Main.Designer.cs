namespace NEAT
{
    partial class Main
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
            this.cbFeedback = new System.Windows.Forms.CheckBox();
            this.lNGenerations = new System.Windows.Forms.Label();
            this.nGenerations = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nGenerations)).BeginInit();
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
            this.btnTrain.Enabled = false;
            this.btnTrain.Location = new System.Drawing.Point(12, 139);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(150, 23);
            this.btnTrain.TabIndex = 3;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(12, 189);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(150, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // rtxtInformation
            // 
            this.rtxtInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtInformation.Location = new System.Drawing.Point(168, 12);
            this.rtxtInformation.Name = "rtxtInformation";
            this.rtxtInformation.ReadOnly = true;
            this.rtxtInformation.Size = new System.Drawing.Size(660, 337);
            this.rtxtInformation.TabIndex = 6;
            this.rtxtInformation.Text = "";
            // 
            // cbFeedback
            // 
            this.cbFeedback.AutoSize = true;
            this.cbFeedback.Enabled = false;
            this.cbFeedback.Location = new System.Drawing.Point(15, 332);
            this.cbFeedback.Name = "cbFeedback";
            this.cbFeedback.Size = new System.Drawing.Size(111, 17);
            this.cbFeedback.TabIndex = 8;
            this.cbFeedback.Text = "Display Feedback";
            this.cbFeedback.UseVisualStyleBackColor = true;
            this.cbFeedback.CheckedChanged += new System.EventHandler(this.cbFeedback_CheckedChanged);
            // 
            // lNGenerations
            // 
            this.lNGenerations.AutoSize = true;
            this.lNGenerations.Location = new System.Drawing.Point(12, 97);
            this.lNGenerations.Name = "lNGenerations";
            this.lNGenerations.Size = new System.Drawing.Size(116, 13);
            this.lNGenerations.TabIndex = 10;
            this.lNGenerations.Text = "Number of Generations";
            // 
            // nGenerations
            // 
            this.nGenerations.Location = new System.Drawing.Point(12, 113);
            this.nGenerations.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nGenerations.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nGenerations.Name = "nGenerations";
            this.nGenerations.Size = new System.Drawing.Size(150, 20);
            this.nGenerations.TabIndex = 11;
            this.nGenerations.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 361);
            this.Controls.Add(this.nGenerations);
            this.Controls.Add(this.lNGenerations);
            this.Controls.Add(this.cbFeedback);
            this.Controls.Add(this.rtxtInformation);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnSelectGame);
            this.Controls.Add(this.cmbGames);
            this.Controls.Add(this.lblGame);
            this.MinimumSize = new System.Drawing.Size(650, 400);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI NEAT - Control Panel";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nGenerations)).EndInit();
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
        private System.Windows.Forms.CheckBox cbFeedback;
        private System.Windows.Forms.Label lNGenerations;
        private System.Windows.Forms.NumericUpDown nGenerations;
    }
}

