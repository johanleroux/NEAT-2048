namespace NEAT
{
    partial class game2048
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
            this.pbGameScreen = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbGameScreen
            // 
            this.pbGameScreen.Location = new System.Drawing.Point(9, 9);
            this.pbGameScreen.Margin = new System.Windows.Forms.Padding(0);
            this.pbGameScreen.Name = "pbGameScreen";
            this.pbGameScreen.Size = new System.Drawing.Size(466, 443);
            this.pbGameScreen.TabIndex = 0;
            this.pbGameScreen.TabStop = false;
            // 
            // game2048
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.pbGameScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "game2048";
            this.Text = "Game - 2048";
            this.Load += new System.EventHandler(this.game2048_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.game2048_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameScreen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameScreen;
    }
}