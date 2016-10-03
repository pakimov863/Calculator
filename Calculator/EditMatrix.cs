using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class EditMatrix : Form
    {
        private const int MAXSIZE = 20;
        public string result;
        private TextBox[,] matrix;
        private int currentWidth = 3;
        private int currentHeight = 3;
        private char currentName = 'A';

        public EditMatrix(char name)
        {
            InitializeComponent();
            currentName = name;
            label_matrixName.Text = currentName.ToString() ;
            ResizeMatrix();
        }

        public EditMatrix(char name, int columns, int rows)
        {
            InitializeComponent();
            if (columns >= 1 && columns <= MAXSIZE) currentWidth = columns;
            if (rows >= 1 && rows <= MAXSIZE) currentHeight = rows;
            currentName = name;
            label_matrixName.Text = currentName.ToString();
            ResizeMatrix();
        }

        public EditMatrix(char name, string[,] input)
        {
            InitializeComponent();
            if (input.GetLength(0) >= 1 && input.GetLength(0) <= MAXSIZE) currentHeight = input.GetLength(0);
            if (input.GetLength(1) >= 1 && input.GetLength(1) <= MAXSIZE) currentWidth = input.GetLength(1);
            text_matrixHeight.Text = currentHeight.ToString();
            text_matrixWidth.Text = currentWidth.ToString();
            matrix = new TextBox[currentHeight, currentWidth];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = new TextBox();
                    matrix[i, j].Size = new Size(50, 20);
                    matrix[i, j].Location = new Point(3 + j * (50 + 6), 3 + i * (20 + 6));
                    matrix[i, j].TabIndex = i*currentHeight + j;
                    matrix[i, j].Text = input[i, j];
                    panel_matrixView.Controls.Add(matrix[i, j]);
                }
            }
            currentName = name;
            label_matrixName.Text = currentName.ToString();
            ResizeMatrix();
        }

        public EditMatrix(char name, string input)
        {
            InitializeComponent();

            int width = -1;
            int height = -1;
            int ffpos = -1;
            int sspos = -1;
            List<string> line_tmp = new List<string>();
            matrix = null;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '[')
                {
                    if (ffpos != -1)
                    {
                        MessageBox.Show("Ошибка разбора");
                        this.Close();
                    }
                    else ffpos = i;
                }

                if (input[i] == ']')
                {
                    if (ffpos == -1)
                    {
                        MessageBox.Show("Ошибка разбора");
                        this.Close();
                    }
                    else sspos = i;
                }

                if (ffpos != -1 && sspos != -1)
                {
                    line_tmp.Add(input.Substring(ffpos + 1, sspos - ffpos - 1));
                    if (width == -1) width = line_tmp.Last().Split('|').Length;
                    else if (width != line_tmp.Last().Split('|').Length)
                    {
                        MessageBox.Show("Ошибка разбора");
                        this.Close();
                    }
                    ffpos = -1; sspos = -1;
                }
            }

            height = line_tmp.Count;

            if (width >= 1 && width <= MAXSIZE) currentWidth = width;
            if (height >= 1 && height <= MAXSIZE) currentHeight = height;
            matrix = new TextBox[currentHeight, currentWidth];
            for (int ii = 0; ii < height; ii++)
            {
                for (int jj = 0; jj < width; jj++)
                {
                    string[] str_tmp = line_tmp[ii].Split('|');
                    matrix[ii, jj] = new TextBox();
                    matrix[ii, jj].Size = new Size(50, 20);
                    matrix[ii, jj].Location = new Point(3 + jj * (50 + 6), 3 + ii * (20 + 6));
                    matrix[ii, jj].TabIndex = ii * currentHeight + jj;
                    matrix[ii, jj].Text = str_tmp[jj];
                }
            }

            currentName = name;
            label_matrixName.Text = currentName.ToString();
            ResizeMatrix();
        }

        /// <summary>
        /// Конец диалога - DialogResult.OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (matrix != null)
            {
                result = label_matrixName.Text + "#{";

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    string str = "[" + matrix[i, 0].Text;
                    for (int j = 1; j < matrix.GetLength(1); j++)
                    {
                        str += "|" + matrix[i, j].Text;
                    }
                    result += str + "]";
                }

                result += "}";
            }
            this.Close();
        }

        /// <summary>
        /// Конец диалога - DialogResult.Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            result = "";
            this.Close();
        }

        /// <summary>
        /// Ввод нового размера матрицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_matrixSize_TextChanged(object sender, EventArgs e)
        {
            short newSize;
            if (Int16.TryParse(((TextBox)sender).Text, out newSize))
            {
                if (newSize >= 1 && newSize <= MAXSIZE)
                {
                    ((TextBox)sender).BackColor = SystemColors.Window;
                    if (((TextBox)sender).Name == "text_matrixWidth") currentWidth = newSize;
                    else currentHeight = newSize;
                }
                else ((TextBox)sender).BackColor = Color.LightCoral;
            }
            else ((TextBox)sender).BackColor = Color.LightCoral;
        }

        /// <summary>
        /// Изменение размера матрицы
        /// </summary>
        private void ResizeMatrix()
        {
            panel_matrixView.Controls.Clear();
            TextBox[,] tmp = new TextBox[currentHeight, currentWidth];
            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                for (int j = 0; j < tmp.GetLength(1); j++)
                {
                    tmp[i, j] = new TextBox();
                    tmp[i, j].Size = new Size(50, 20);
                    tmp[i, j].Location = new Point(3 + j * (50 + 6), 3 + i * (20 + 6));
                    tmp[i, j].TabIndex = i * currentHeight + j;
                    if(matrix != null)
                    {
                        if (i <= matrix.GetLength(0) - 1 && j <= matrix.GetLength(1) - 1)
                            tmp[i, j].Text = matrix[i, j].Text;
                    }
                    panel_matrixView.Controls.Add(tmp[i, j]);
                }
            }
            matrix = tmp;
        }

        /// <summary>
        /// Перезагрузка матрицы из памяти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            short newSize;
            if (Int16.TryParse(text_matrixWidth.Text, out newSize))
            {
                if (newSize >= 1 && newSize <= MAXSIZE)
                {
                    text_matrixWidth.BackColor = SystemColors.Window;
                    currentWidth = newSize;
                }
                else
                {
                    text_matrixWidth.BackColor = Color.LightCoral;
                    return;
                }
            }
            else
            {
                text_matrixWidth.BackColor = Color.LightCoral;
                return;
            }
            if (Int16.TryParse(text_matrixHeight.Text, out newSize))
            {
                if (newSize >= 1 && newSize <= MAXSIZE)
                {
                    text_matrixHeight.BackColor = SystemColors.Window;
                    currentHeight = newSize;
                }
                else
                {
                    text_matrixHeight.BackColor = Color.LightCoral;
                    return;
                }
            }
            else
            {
                text_matrixHeight.BackColor = Color.LightCoral;
                return;
            }
            ResizeMatrix();
        }

        #region Быстрое заполнение матрицы

        /// <summary>
        /// Пресет - нулевая матрица
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetZeros_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j].Text = "0";
        }

        /// <summary>
        /// Пресет - единичная матрица
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetOnes_Click(object sender, EventArgs e)
        {
            if (currentWidth == currentHeight)
            {
                button_presetZeros_Click(sender, e);
                for (int i = 0; i < matrix.GetLength(0); i++)
                    matrix[i, i].Text = "1";
            }
        }

        /// <summary>
        /// Пресет - произвольная диагональ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetDiag_Click(object sender, EventArgs e)
        {
            if (currentWidth == currentHeight)
            {
                Random rnd = new Random();
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (i == j)
                            matrix[i, j].Text = (Math.Round(rnd.NextDouble() * Math.Pow(10, rnd.Next(0, 3)), 4)).ToString();
                        else matrix[i, j].Text = "0";
                    }
            }
        }

        /// <summary>
        /// Пресет - произвольный верхний угол
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetUpTriangle_Click(object sender, EventArgs e)
        {
            if (currentWidth == currentHeight)
            {
                Random rnd = new Random();
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j < i)
                            matrix[i, j].Text = (Math.Round(rnd.NextDouble() * Math.Pow(10, rnd.Next(0, 3)), 4)).ToString();
                        else matrix[i, j].Text = "0";
                    }
            }
        }

        /// <summary>
        /// Пресет - произвольный нижний угол
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetDownTriangle_Click(object sender, EventArgs e)
        {
            if (currentWidth == currentHeight)
            {
                Random rnd = new Random();
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j > i)
                            matrix[i, j].Text = (Math.Round(rnd.NextDouble() * Math.Pow(10, rnd.Next(0, 3)), 4)).ToString();
                        else matrix[i, j].Text = "0";
                    }
            }
        }

        /// <summary>
        /// Пресет - произвольная матрица
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_presetRnd_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j].Text = (Math.Round(rnd.NextDouble() * Math.Pow(10, rnd.Next(0, 3)), 4)).ToString();
        }

        #endregion

    }
}