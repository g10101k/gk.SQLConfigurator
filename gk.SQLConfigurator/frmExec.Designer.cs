namespace gk.SQLConfigurator
{
    partial class frmExec
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
            this.svgImage = new System.Windows.Forms.PictureBox();
            this.bar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).BeginInit();
            this.SuspendLayout();
            // 
            // svgImage
            // 
            this.svgImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.svgImage.Location = new System.Drawing.Point(0, 25);
            this.svgImage.Name = "svgImage";
            this.svgImage.Size = new System.Drawing.Size(735, 449);
            this.svgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.svgImage.TabIndex = 1;
            this.svgImage.TabStop = false;
            // 
            // bar
            // 
            this.bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bar.Location = new System.Drawing.Point(0, 0);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(241, 24);
            this.bar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.bar.TabIndex = 0;
            // 
            // frmExec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 24);
            this.ControlBox = false;
            this.Controls.Add(this.bar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmExec";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmExec";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExec_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmExec_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.PictureBox svgImage;
        public System.Windows.Forms.ProgressBar bar;
    }
}