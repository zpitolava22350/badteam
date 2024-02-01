namespace PictureBoxTest {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.canvasPanel = new System.Windows.Forms.PictureBox();
            this.gameRefresh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // canvasPanel
            // 
            this.canvasPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvasPanel.Location = new System.Drawing.Point(0, 0);
            this.canvasPanel.Name = "canvasPanel";
            this.canvasPanel.Size = new System.Drawing.Size(800, 450);
            this.canvasPanel.TabIndex = 0;
            this.canvasPanel.TabStop = false;
            this.canvasPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // gameRefresh
            // 
            this.gameRefresh.Enabled = true;
            this.gameRefresh.Interval = 1;
            this.gameRefresh.Tick += new System.EventHandler(this.gameRefresh_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.canvasPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvasPanel;
        private System.Windows.Forms.Timer gameRefresh;
    }
}

