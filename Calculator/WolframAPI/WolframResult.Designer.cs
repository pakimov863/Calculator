namespace Calculator
{
    partial class WolframResult
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WolframResult));
            this.panelLoading = new System.Windows.Forms.Panel();
            this.timerAnim = new System.Windows.Forms.Timer(this.components);
            this.pictureLoadAnim = new System.Windows.Forms.PictureBox();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoadAnim)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLoading
            // 
            this.panelLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLoading.Controls.Add(this.pictureLoadAnim);
            this.panelLoading.Location = new System.Drawing.Point(12, 12);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Size = new System.Drawing.Size(510, 337);
            this.panelLoading.TabIndex = 0;
            // 
            // timerAnim
            // 
            this.timerAnim.Interval = 200;
            this.timerAnim.Tick += new System.EventHandler(this.timerAnim_Tick);
            // 
            // pictureLoadAnim
            // 
            this.pictureLoadAnim.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureLoadAnim.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureLoadAnim.Location = new System.Drawing.Point(3, 3);
            this.pictureLoadAnim.Name = "pictureLoadAnim";
            this.pictureLoadAnim.Size = new System.Drawing.Size(120, 120);
            this.pictureLoadAnim.TabIndex = 0;
            this.pictureLoadAnim.TabStop = false;
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.AutoScroll = true;
            this.panelContent.Location = new System.Drawing.Point(12, 12);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(510, 337);
            this.panelContent.TabIndex = 1;
            // 
            // WolframResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(534, 361);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelLoading);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WolframResult";
            this.Text = "WolframResult";
            this.Load += new System.EventHandler(this.WolframResult_Load);
            this.Resize += new System.EventHandler(this.WolframResult_Resize);
            this.panelLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoadAnim)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.PictureBox pictureLoadAnim;
        private System.Windows.Forms.Timer timerAnim;
        private System.Windows.Forms.Panel panelContent;
    }
}