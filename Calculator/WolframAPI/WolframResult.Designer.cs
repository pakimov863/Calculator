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
            this.panelLoading = new System.Windows.Forms.Panel();
            this.pictureLoadAnim = new System.Windows.Forms.PictureBox();
            this.timerAnim = new System.Windows.Forms.Timer(this.components);
            this.panelContent = new System.Windows.Forms.Panel();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPodsFound = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelExecTime = new System.Windows.Forms.Label();
            this.panelLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoadAnim)).BeginInit();
            this.panelContent.SuspendLayout();
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
            // pictureLoadAnim
            // 
            this.pictureLoadAnim.BackColor = System.Drawing.SystemColors.Control;
            this.pictureLoadAnim.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureLoadAnim.Location = new System.Drawing.Point(3, 3);
            this.pictureLoadAnim.Name = "pictureLoadAnim";
            this.pictureLoadAnim.Size = new System.Drawing.Size(120, 120);
            this.pictureLoadAnim.TabIndex = 0;
            this.pictureLoadAnim.TabStop = false;
            // 
            // timerAnim
            // 
            this.timerAnim.Interval = 200;
            this.timerAnim.Tick += new System.EventHandler(this.timerAnim_Tick);
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.AutoScroll = true;
            this.panelContent.Controls.Add(this.labelExecTime);
            this.panelContent.Controls.Add(this.label2);
            this.panelContent.Controls.Add(this.labelPodsFound);
            this.panelContent.Controls.Add(this.label1);
            this.panelContent.Controls.Add(this.textBoxOutput);
            this.panelContent.Location = new System.Drawing.Point(12, 12);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(510, 337);
            this.panelContent.TabIndex = 1;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(3, 3);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(504, 315);
            this.textBoxOutput.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ветвей найдено:";
            // 
            // labelPodsFound
            // 
            this.labelPodsFound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPodsFound.AutoSize = true;
            this.labelPodsFound.Location = new System.Drawing.Point(100, 321);
            this.labelPodsFound.Name = "labelPodsFound";
            this.labelPodsFound.Size = new System.Drawing.Size(13, 13);
            this.labelPodsFound.TabIndex = 2;
            this.labelPodsFound.Text = "0";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Время разбора/выполнения:";
            // 
            // labelExecTime
            // 
            this.labelExecTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelExecTime.AutoSize = true;
            this.labelExecTime.Location = new System.Drawing.Point(329, 321);
            this.labelExecTime.Name = "labelExecTime";
            this.labelExecTime.Size = new System.Drawing.Size(40, 13);
            this.labelExecTime.TabIndex = 3;
            this.labelExecTime.Text = "0.0000";
            // 
            // WolframResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 361);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelLoading);
            this.Name = "WolframResult";
            this.Text = "WolframResult";
            this.Load += new System.EventHandler(this.WolframResult_Load);
            this.Resize += new System.EventHandler(this.WolframResult_Resize);
            this.panelLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLoadAnim)).EndInit();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.PictureBox pictureLoadAnim;
        private System.Windows.Forms.Timer timerAnim;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPodsFound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelExecTime;
    }
}