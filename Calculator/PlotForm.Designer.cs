namespace Calculator
{
    partial class PlotForm
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
            this.buttonDeleteRefresh = new System.Windows.Forms.Button();
            this.buttonZoomPlus = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonZoomMinus = new System.Windows.Forms.Button();
            this.buttonZoomNorm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDeleteRefresh
            // 
            this.buttonDeleteRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteRefresh.Location = new System.Drawing.Point(406, 130);
            this.buttonDeleteRefresh.Name = "buttonDeleteRefresh";
            this.buttonDeleteRefresh.Size = new System.Drawing.Size(148, 23);
            this.buttonDeleteRefresh.TabIndex = 1;
            this.buttonDeleteRefresh.Text = "Удалить";
            this.buttonDeleteRefresh.UseVisualStyleBackColor = true;
            this.buttonDeleteRefresh.Click += new System.EventHandler(this.buttonDeleteRefresh_Click);
            // 
            // buttonZoomPlus
            // 
            this.buttonZoomPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomPlus.Location = new System.Drawing.Point(406, 188);
            this.buttonZoomPlus.Name = "buttonZoomPlus";
            this.buttonZoomPlus.Size = new System.Drawing.Size(40, 40);
            this.buttonZoomPlus.TabIndex = 3;
            this.buttonZoomPlus.Text = "+";
            this.buttonZoomPlus.UseVisualStyleBackColor = true;
            this.buttonZoomPlus.Click += new System.EventHandler(this.buttonZoomPlus_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(406, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(148, 124);
            this.checkedListBox1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // buttonZoomMinus
            // 
            this.buttonZoomMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomMinus.Location = new System.Drawing.Point(406, 234);
            this.buttonZoomMinus.Name = "buttonZoomMinus";
            this.buttonZoomMinus.Size = new System.Drawing.Size(40, 40);
            this.buttonZoomMinus.TabIndex = 5;
            this.buttonZoomMinus.Text = "-";
            this.buttonZoomMinus.UseVisualStyleBackColor = true;
            this.buttonZoomMinus.Click += new System.EventHandler(this.buttonZoomMinus_Click);
            // 
            // buttonZoomNorm
            // 
            this.buttonZoomNorm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonZoomNorm.Location = new System.Drawing.Point(406, 280);
            this.buttonZoomNorm.Name = "buttonZoomNorm";
            this.buttonZoomNorm.Size = new System.Drawing.Size(40, 40);
            this.buttonZoomNorm.TabIndex = 6;
            this.buttonZoomNorm.Text = "=";
            this.buttonZoomNorm.UseVisualStyleBackColor = true;
            this.buttonZoomNorm.Click += new System.EventHandler(this.buttonZoomNorm_Click);
            // 
            // PlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 400);
            this.Controls.Add(this.buttonZoomNorm);
            this.Controls.Add(this.buttonZoomMinus);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.buttonZoomPlus);
            this.Controls.Add(this.buttonDeleteRefresh);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PlotForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Графики";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlotForm_FormClosing);
            this.Resize += new System.EventHandler(this.PlotForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonDeleteRefresh;
        private System.Windows.Forms.Button buttonZoomPlus;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button buttonZoomMinus;
        private System.Windows.Forms.Button buttonZoomNorm;
    }
}