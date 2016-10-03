namespace Calculator
{
    partial class EditMatrix
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
            this.panel_matrixView = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label_matrixName = new System.Windows.Forms.Label();
            this.text_matrixWidth = new System.Windows.Forms.TextBox();
            this.text_matrixHeight = new System.Windows.Forms.TextBox();
            this.button_presetZeros = new System.Windows.Forms.Button();
            this.button_presetOnes = new System.Windows.Forms.Button();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.button_presetDiag = new System.Windows.Forms.Button();
            this.button_presetUpTriangle = new System.Windows.Forms.Button();
            this.button_presetDownTriangle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_Rnd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel_matrixView
            // 
            this.panel_matrixView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_matrixView.AutoScroll = true;
            this.panel_matrixView.Location = new System.Drawing.Point(12, 12);
            this.panel_matrixView.Name = "panel_matrixView";
            this.panel_matrixView.Size = new System.Drawing.Size(237, 237);
            this.panel_matrixView.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Матрица: ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ширина:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(252, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Высота:";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(255, 226);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(70, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(331, 226);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label_matrixName
            // 
            this.label_matrixName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_matrixName.AutoSize = true;
            this.label_matrixName.Location = new System.Drawing.Point(328, 15);
            this.label_matrixName.Name = "label_matrixName";
            this.label_matrixName.Size = new System.Drawing.Size(14, 13);
            this.label_matrixName.TabIndex = 9;
            this.label_matrixName.Text = "X";
            // 
            // text_matrixWidth
            // 
            this.text_matrixWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_matrixWidth.Location = new System.Drawing.Point(331, 37);
            this.text_matrixWidth.Name = "text_matrixWidth";
            this.text_matrixWidth.Size = new System.Drawing.Size(70, 20);
            this.text_matrixWidth.TabIndex = 10;
            this.text_matrixWidth.Text = "3";
            this.text_matrixWidth.TextChanged += new System.EventHandler(this.text_matrixSize_TextChanged);
            // 
            // text_matrixHeight
            // 
            this.text_matrixHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.text_matrixHeight.Location = new System.Drawing.Point(331, 63);
            this.text_matrixHeight.Name = "text_matrixHeight";
            this.text_matrixHeight.Size = new System.Drawing.Size(70, 20);
            this.text_matrixHeight.TabIndex = 11;
            this.text_matrixHeight.Text = "3";
            this.text_matrixHeight.TextChanged += new System.EventHandler(this.text_matrixSize_TextChanged);
            // 
            // button_presetZeros
            // 
            this.button_presetZeros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_presetZeros.Location = new System.Drawing.Point(255, 139);
            this.button_presetZeros.Name = "button_presetZeros";
            this.button_presetZeros.Size = new System.Drawing.Size(70, 23);
            this.button_presetZeros.TabIndex = 12;
            this.button_presetZeros.Text = "Нули";
            this.button_presetZeros.UseVisualStyleBackColor = true;
            this.button_presetZeros.Click += new System.EventHandler(this.button_presetZeros_Click);
            // 
            // button_presetOnes
            // 
            this.button_presetOnes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_presetOnes.Location = new System.Drawing.Point(331, 139);
            this.button_presetOnes.Name = "button_presetOnes";
            this.button_presetOnes.Size = new System.Drawing.Size(70, 23);
            this.button_presetOnes.TabIndex = 13;
            this.button_presetOnes.Text = "Единичная";
            this.button_presetOnes.UseVisualStyleBackColor = true;
            this.button_presetOnes.Click += new System.EventHandler(this.button_presetOnes_Click);
            // 
            // button_Refresh
            // 
            this.button_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Refresh.Location = new System.Drawing.Point(255, 89);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(146, 23);
            this.button_Refresh.TabIndex = 14;
            this.button_Refresh.Text = "Обновить";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // button_presetDiag
            // 
            this.button_presetDiag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_presetDiag.Location = new System.Drawing.Point(255, 168);
            this.button_presetDiag.Name = "button_presetDiag";
            this.button_presetDiag.Size = new System.Drawing.Size(70, 23);
            this.button_presetDiag.TabIndex = 12;
            this.button_presetDiag.Text = "Диагональ";
            this.button_presetDiag.UseVisualStyleBackColor = true;
            this.button_presetDiag.Click += new System.EventHandler(this.button_presetDiag_Click);
            // 
            // button_presetUpTriangle
            // 
            this.button_presetUpTriangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_presetUpTriangle.Location = new System.Drawing.Point(255, 197);
            this.button_presetUpTriangle.Name = "button_presetUpTriangle";
            this.button_presetUpTriangle.Size = new System.Drawing.Size(70, 23);
            this.button_presetUpTriangle.TabIndex = 13;
            this.button_presetUpTriangle.Text = "ВТМ";
            this.button_presetUpTriangle.UseVisualStyleBackColor = true;
            this.button_presetUpTriangle.Click += new System.EventHandler(this.button_presetUpTriangle_Click);
            // 
            // button_presetDownTriangle
            // 
            this.button_presetDownTriangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_presetDownTriangle.Location = new System.Drawing.Point(331, 197);
            this.button_presetDownTriangle.Name = "button_presetDownTriangle";
            this.button_presetDownTriangle.Size = new System.Drawing.Size(70, 23);
            this.button_presetDownTriangle.TabIndex = 13;
            this.button_presetDownTriangle.Text = "НТМ";
            this.button_presetDownTriangle.UseVisualStyleBackColor = true;
            this.button_presetDownTriangle.Click += new System.EventHandler(this.button_presetDownTriangle_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Шаблоны:";
            // 
            // button_Rnd
            // 
            this.button_Rnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Rnd.Location = new System.Drawing.Point(331, 168);
            this.button_Rnd.Name = "button_Rnd";
            this.button_Rnd.Size = new System.Drawing.Size(70, 23);
            this.button_Rnd.TabIndex = 12;
            this.button_Rnd.Text = "Произвольно";
            this.button_Rnd.UseVisualStyleBackColor = true;
            this.button_Rnd.Click += new System.EventHandler(this.button_presetRnd_Click);
            // 
            // EditMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 261);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.button_Refresh);
            this.Controls.Add(this.button_presetDownTriangle);
            this.Controls.Add(this.button_presetUpTriangle);
            this.Controls.Add(this.button_Rnd);
            this.Controls.Add(this.button_presetDiag);
            this.Controls.Add(this.button_presetOnes);
            this.Controls.Add(this.button_presetZeros);
            this.Controls.Add(this.text_matrixHeight);
            this.Controls.Add(this.text_matrixWidth);
            this.Controls.Add(this.label_matrixName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel_matrixView);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "EditMatrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Управление матрицей";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_matrixView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label_matrixName;
        private System.Windows.Forms.TextBox text_matrixWidth;
        private System.Windows.Forms.TextBox text_matrixHeight;
        private System.Windows.Forms.Button button_presetZeros;
        private System.Windows.Forms.Button button_presetOnes;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.Button button_presetDiag;
        private System.Windows.Forms.Button button_presetUpTriangle;
        private System.Windows.Forms.Button button_presetDownTriangle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_Rnd;
    }
}