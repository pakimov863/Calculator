using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

/*
-//  1) Новый экран вывода данных
-//  2) Новый экран вывода истории
-//  3) Доработка меню, настроек и подобное
*/

namespace Calculator
{
    public partial class MainForm : Form
    {
        public string panelmode = "norm";
        private byte plotside = 0;
        public byte gradusmode = 0;
        public int SSmode = 10;
        public double plotmultiplier = 1;
        public PlotForm plotter;

        public MainForm()
        {
            InitializeComponent();
            ChangeModeTo(panelmode);
            ChangeSSTo(SSmode);
            ChangeGradusTo(gradusmode);
        }

        private void PreProcess(ref string str)
        {
            str = "(" + str.ToUpper() + ")";
            str = str.Replace("PI", Convert.ToString(Math.PI));
            //str = str.Replace("E", Convert.ToString(Math.E));

            if (str.Contains("%"))
            {
                //⋮=+%, ⋯=-%, ⋰=*%, ⋱=/%
                string[] str_mas = str.Split('%');
                int rbr = 0;
                for (int i = 0; i < str_mas.Length - 1; i++)
                {
                    string tmp = "";
                    for(int j=str_mas[i].Length-1; j>=0; j--)
                    {
                        if(("+-*/").Contains(str_mas[i][j]) && (rbr==0))
                        {
                            str_mas[i] = str_mas[i].Substring(0, j + 1) + '%' + tmp;
                            break;
                        }
                        else
                        {
                            if (str_mas[i][j] == ')') rbr++;
                            if (str_mas[i][j] == '(') rbr--;
                            tmp = str_mas[i][j] + tmp;
                        }
                    }
                }
                str = "";
                for (int i = 0; i < str_mas.Length; i++)
                {
                    str += str_mas[i];
                }
                str = str.Replace("+%", "⋮");
                str = str.Replace("-%", "⋯");
                str = str.Replace("*%", "⋰");
                str = str.Replace("/%", "⋱");
            }

            /*string symbols = "+|-|*|/|^|⋮|⋯|⋰|⋱|SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|!";
            if (str.Contains("-"))
            {
                string[] tstr = str.Split('-');
                for (int i = 0; i < tstr.Length; i++)
                {
                    if(tstr[i] == "" || )
                }
            }*/

            /*if(str.Contains("(") || str.Contains(")"))
            {
                int lbr = 0;
                for(int i=0; i < str.Length; i++)
                {
                    if (str[i] == '(') lbr++;
                    if (str[i] == ')') lbr--;
                }
                if (lbr != 0) str = "";
            }

            /*if (str.Contains("-"))
            {
                string[] str_mas = str.Split('-');
                string tmp = str_mas[0];
                for (int i = 0; i < str_mas.Length - 1; i++)
                {//(15-4)-8
                    if(!"0123456789)".Contains(str_mas.Last())) tmp += str_mas[i + 1];
                    else tmp += "-" + str_mas[i + 1];
                }
                str = tmp;
            }*/
            str = str.Replace(".", ",");
            
            //if(str.StartsWith("+")) str.
        }

        private string PostProcess(Double str)
        {
            if (Double.IsNaN(str)) return "NaN";
            if (Double.IsInfinity(str)) return "Inf";
            Double result = Math.Round(str, 5);
            return Convert.ToString(result);
        }

        private void ChangeSSTo(int newSS)
        {
            switch(newSS)
            {
                case 16:
                    toolStripSSMode.Text = "HEX";
                    radioSS16.Checked = true;
                    button00_P.Enabled = true;
                    button01_P.Enabled = true;
                    button02_P.Enabled = true;
                    button03_P.Enabled = true;
                    button04_P.Enabled = true;
                    button05_P.Enabled = true;
                    button06_P.Enabled = true;
                    button07_P.Enabled = true;
                    button08_P.Enabled = true;
                    button09_P.Enabled = true;
                    buttonA_P.Enabled = true;
                    buttonB_P.Enabled = true;
                    buttonC_P.Enabled = true;
                    buttonD_P.Enabled = true;
                    buttonE_P.Enabled = true;
                    buttonF_P.Enabled = true;
                    break;
                case 10:
                default:
                    toolStripSSMode.Text = "DEC";
                    radioSS10.Checked = true;
                    button00_P.Enabled = true;
                    button01_P.Enabled = true;
                    button02_P.Enabled = true;
                    button03_P.Enabled = true;
                    button04_P.Enabled = true;
                    button05_P.Enabled = true;
                    button06_P.Enabled = true;
                    button07_P.Enabled = true;
                    button08_P.Enabled = true;
                    button09_P.Enabled = true;
                    buttonA_P.Enabled = false;
                    buttonB_P.Enabled = false;
                    buttonC_P.Enabled = false;
                    buttonD_P.Enabled = false;
                    buttonE_P.Enabled = false;
                    buttonF_P.Enabled = false;
                    break;
                case 8:
                    toolStripSSMode.Text = "OCT";
                    radioSS8.Checked = true;
                    button00_P.Enabled = true;
                    button01_P.Enabled = true;
                    button02_P.Enabled = true;
                    button03_P.Enabled = true;
                    button04_P.Enabled = true;
                    button05_P.Enabled = true;
                    button06_P.Enabled = true;
                    button07_P.Enabled = true;
                    button08_P.Enabled = false;
                    button09_P.Enabled = false;
                    buttonA_P.Enabled = false;
                    buttonB_P.Enabled = false;
                    buttonC_P.Enabled = false;
                    buttonD_P.Enabled = false;
                    buttonE_P.Enabled = false;
                    buttonF_P.Enabled = false;
                    break;
                case 2:
                    toolStripSSMode.Text = "BIN";
                    radioSS2.Checked = true;
                    button00_P.Enabled = true;
                    button01_P.Enabled = true;
                    button02_P.Enabled = false;
                    button03_P.Enabled = false;
                    button04_P.Enabled = false;
                    button05_P.Enabled = false;
                    button06_P.Enabled = false;
                    button07_P.Enabled = false;
                    button08_P.Enabled = false;
                    button09_P.Enabled = false;
                    buttonA_P.Enabled = false;
                    buttonB_P.Enabled = false;
                    buttonC_P.Enabled = false;
                    buttonD_P.Enabled = false;
                    buttonE_P.Enabled = false;
                    buttonF_P.Enabled = false;
                    break;
            }
        }

        private void ChangeGradusTo(int newgradus)
        {
            switch(newgradus)
            {
                case 0:
                default:
                    toolStripGradMode.Text = "DEG";
                    break;
                case 1:
                    toolStripGradMode.Text = "RAD";
                    break;
                case 2:
                    toolStripGradMode.Text = "GRAD";
                    break;
            }
        }

        private void ChangeModeTo(string newmode)
        {
            switch (newmode)
            {
                case "norm":
                default:
                    panelNormal.Visible = true;
                    panelNormal.Location = new Point(12, 167);
                    panelEngeneer.Visible = false;
                    panelEngeneer.Location = new Point(-500, 167);
                    toolStripGradMode.Visible = true;
                    panelGrad.Visible = true;
                    panelGrad.Location = new Point(12, 116);
                    panelProgram.Visible = false;
                    panelProgram.Location = new Point(-500, 167);
                    toolStripSSMode.Visible = false;
                    panelSS.Visible = false;
                    panelSS.Location = new Point(-500, 116);
                    panelMatrix.Visible = false;
                    panelMatrix.Location = new Point(-500, 167);
                    panelGraph.Visible = false;
                    panelGraph.Location = new Point(-500, 167);
                    newmode = "norm";
                    break;
                case "engen":
                    panelNormal.Visible = false;
                    panelNormal.Location = new Point(-500, 167);
                    panelEngeneer.Visible = true;
                    panelEngeneer.Location = new Point(12, 167);
                    toolStripGradMode.Visible = true;
                    panelGrad.Visible = true;
                    panelGrad.Location = new Point(12, 116);
                    panelProgram.Visible = false;
                    panelProgram.Location = new Point(-500, 167);
                    toolStripSSMode.Visible = false;
                    panelSS.Visible = false;
                    panelSS.Location = new Point(-500, 116);
                    panelMatrix.Visible = false;
                    panelMatrix.Location = new Point(-500, 167);
                    panelGraph.Visible = false;
                    panelGraph.Location = new Point(-500, 167);
                    break;
                case "prog":
                    panelNormal.Visible = false;
                    panelNormal.Location = new Point(-500, 167);
                    panelEngeneer.Visible = false;
                    panelEngeneer.Location = new Point(-500, 167);
                    toolStripGradMode.Visible = false;
                    panelGrad.Visible = false;
                    panelGrad.Location = new Point(-500, 116);
                    panelProgram.Visible = true;
                    panelProgram.Location = new Point(12, 167);
                    toolStripSSMode.Visible = true;
                    panelSS.Visible = true;
                    panelSS.Location = new Point(12, 116);
                    panelMatrix.Visible = false;
                    panelMatrix.Location = new Point(-500, 167);
                    panelGraph.Visible = false;
                    panelGraph.Location = new Point(-500, 167);
                    break;
                case "matr":
                    panelNormal.Visible = false;
                    panelNormal.Location = new Point(-500, 167);
                    panelEngeneer.Visible = false;
                    panelEngeneer.Location = new Point(-500, 167);
                    toolStripGradMode.Visible = true;
                    panelGrad.Visible = true;
                    panelGrad.Location = new Point(12, 116);
                    panelProgram.Visible = false;
                    panelProgram.Location = new Point(-500, 167);
                    toolStripSSMode.Visible = false;
                    panelSS.Visible = false;
                    panelSS.Location = new Point(-500, 116);
                    panelMatrix.Visible = true;
                    panelMatrix.Location = new Point(12, 167);
                    panelGraph.Visible = false;
                    panelGraph.Location = new Point(-500, 167);
                    break;
                case "graph":
                    panelNormal.Visible = false;
                    panelNormal.Location = new Point(-500, 167);
                    panelEngeneer.Visible = false;
                    panelEngeneer.Location = new Point(-500, 167);
                    toolStripGradMode.Visible = true;
                    panelGrad.Visible = true;
                    panelGrad.Location = new Point(12, 116);
                    panelProgram.Visible = false;
                    panelProgram.Location = new Point(-500, 167);
                    toolStripSSMode.Visible = false;
                    panelSS.Visible = false;
                    panelSS.Location = new Point(-500, 116);
                    panelMatrix.Visible = false;
                    panelMatrix.Location = new Point(-500, 167);
                    panelGraph.Visible = true;
                    panelGraph.Location = new Point(12, 167);
                    break;
            }
            buttonHistory.Text = "<<";
            buttonHistory_Click(new Object(), new EventArgs());
        }

        //Кнопки
        ///////////////////////////////////////////////////////////////////////////////

        private void buttonDigit_Click(object sender, EventArgs e)
        {
            int csel = ScreenBox.SelectionStart;
            if (ScreenBox.Text.Length != 0 && csel != 0)
            {
                ScreenBox.Text = ScreenBox.Text.Substring(0, csel) + ((Button)sender).Text + ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
            }
            else
            {
                ScreenBox.Text = ((Button)sender).Text + ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
            }
            csel += ((Button)sender).Text.Length ;
            ScreenBox.Focus();
            ScreenBox.Select(csel, 0);
        }

        private void buttonFunct_Click(object sender, EventArgs e)
        {
            int csel = ScreenBox.SelectionStart;
            if (ScreenBox.Text.Length != 0 && csel != 0)
            {
                ScreenBox.Text = ScreenBox.Text.Substring(0, csel) + ((Button)sender).Text+"(" + ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
            }
            else
            {
                ScreenBox.Text = ((Button)sender).Text +"("+ ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
            }
            csel += ((Button)sender).Text.Length+1;
            ScreenBox.Focus();
            ScreenBox.Select(csel, 0);
        }

        // Вывод истории в увеличенном варианте
        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if(buttonHistory.Text == ">>")
            {
                HistoryBox1.Location = new Point(402, 30);
                HistoryBox1.Size = new Size(200, 346);
                switch (panelmode)
                {
                    case "norm":
                    default:
                        panelNormal.Location = new Point(12, 146);
                        break;
                    case "engen":
                        panelEngeneer.Location = new Point(12, 146);
                        break;
                    case "prog":
                        panelProgram.Location = new Point(12, 146);
                        break;
                    case "matr":
                        panelMatrix.Location = new Point(12, 146);
                        break;
                    case "graph":
                        panelGraph.Location = new Point(12, 146);
                        break;
                }
                buttonHistory.Size = new Size(27, 24);
                panelModes.Location = new Point(198, 116);
                this.Size = new Size(630, 427);
                //HistoryBox.Location = 402, 30
                //HistoryBox.Size = 200, 346
                //panelStandart.Location = 12, 146
                //buttonHistory.Size = 27, 24
                //panelModes.Location = 198, 116
                //this.Size = 630, 427
                buttonHistory.Text = "<<";
            }
            else
            {
                HistoryBox1.Location = new Point(183, 116);
                HistoryBox1.Size = new Size(183, 48);
                switch (panelmode)
                {
                    case "norm":
                    default:
                        panelNormal.Location = new Point(12, 167);
                        break;
                    case "engen":
                        panelEngeneer.Location = new Point(12, 167);
                        break;
                    case "prog":
                        panelProgram.Location = new Point(12, 167);
                        break;
                    case "matr":
                        panelMatrix.Location = new Point(12, 167);
                        break;
                    case "graph":
                        panelGraph.Location = new Point(12, 167);
                        break;
                }
                buttonHistory.Size = new Size(27, 48);
                panelModes.Location = new Point(12, 139);
                this.Size = new Size(424, 448);
                //HistoryBox.Location = 183, 116
                //HistoryBox.Size = 183, 47
                //panelStandart.Location = 12, 166
                //buttonHistory.Size = 27, 47
                //panelModes.Location = 12, 139
                //this.Size = 424, 450|447
                buttonHistory.Text = ">>";
            }
        }

        // Бекспейс
        private void buttonBack_Click(object sender, EventArgs e)
        {
            /*
            Dim csel As Integer = TextBox1.SelectionStart
            If (TextBox1.Text.Length <> 0 And csel <> 0) Then
                TextBox1.Text = TextBox1.Text.Substring(0, csel - 1) + TextBox1.Text.Substring(csel, TextBox1.Text.Length - csel)
                csel -= 1
            End If
            TextBox1.Focus()
            TextBox1.Select(csel, 0)
            */
            int csel = ScreenBox.SelectionStart;
            if (ScreenBox.Text.Length != 0 && csel != 0)
            {
                ScreenBox.Text = ScreenBox.Text.Substring(0, csel - 1) + ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
                csel -= 1;
            }
            ScreenBox.Focus();
            ScreenBox.Select(csel, 0);
        }

        // Очистка экрана
        private void buttonClr_Click(object sender, EventArgs e)
        {
            if(buttonClr.Text == "CLR")
            {
                ScreenBox.Clear();
            }
            else
            {
                HistoryBox1.Clear();
            }
            
        }

        // Начать вычисление
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string inpstr = ScreenBox.Text;
            if (inpstr.Contains("X") || inpstr.Contains("Y"))
            {
                buttonDigit_Click(sender, e);
            }
            else
            {
                string heststr = inpstr;
                PreProcess(ref inpstr);
                string result = "";

                switch (panelmode)
                {
                    case "norm":
                    case "engen":
                    default:
                        //Вычисление на обычной или инженерной панели
                        string sinpstr = "sin(3.1415926535/4)*2/0.64*exp(7)";
                        for (int iii = 0; iii < 200; iii++)
                        {
                            double sr = 0;
                            string nstr = sinpstr;
                            for (int jjj = 0; jjj<iii; jjj++)
                            {
                                nstr += "+" + sinpstr;
                            }
                            for (int kkk = 0; kkk < 7; kkk++)
                            {
                                PreProcess(ref nstr);
                                Stopwatch sw = new Stopwatch();
                                sw.Start();

                                List<Token> RPN1 = Arithmetic.GetRPN(nstr);
                                result = Convert.ToString(Arithmetic.Calculate(ref RPN1, gradusmode));
                                result = PostProcess(Convert.ToDouble(result));

                                sw.Stop();
                                int ts = sw.Elapsed.Milliseconds ;
                                sr += ts;
                                HistoryBox1.AppendText("\r\n" + ts.ToString());
                            }
                            HistoryBox1.AppendText((sr / 7).ToString() + " ");
                        }
                        
                        //HistoryBox1.AppendText("\r\n" + ts.ToString());
                        //if(RPN1!=null) RPN1.Clear();
                        break;
                    case "prog":
                        //Вычисление на програмной панели
                        if (radioLogicExpr.Checked)
                        {
                            List<Token> RPN2 = BinaryArithmetic.GetRPN(inpstr);
                            Dictionary<string, bool> variables = BinaryArithmetic.GetVariables(RPN2);
                            int count = (int)Math.Pow(2, variables.Count);
                            result = "{[";
                            for (int i = 0; i < count; i++)
                            {
                                BinaryArithmetic.AddVariableData(i + count, variables);
                                result += (BinaryArithmetic.Calculate(RPN2, variables) ? "1|" : "0|");
                            }
                            result = result.Substring(0, result.Length - 1) + "]}";
                        }
                        else
                        {
                            List<Token> RPN2 = Arithmetic.GetRPN(BinaryArithmetic.ConvertStringToSS(inpstr, SSmode, 10));
                            result = Convert.ToString(Arithmetic.Calculate(ref RPN2, gradusmode));
                            result = BinaryArithmetic.ConvertStringToSS(result, 10, SSmode);
                            if(RPN2!=null)RPN2.Clear();
                        }
                        break;
                    case "matr":
                        //Вычисление на матричной панели
                        Dictionary<string, string> mas = new Dictionary<string, string>();
                        MatrixArithmetic.GetMatrixes(ref inpstr, ref mas);
                        List<Token> RPN3 = MatrixArithmetic.GetRPN(inpstr);
                        result = MatrixArithmetic.Calculate(RPN3, mas);
                        if(RPN3!=null)RPN3.Clear();
                        mas.Clear();
                        break;
                }

                heststr += "=" + result;
                ScreenBox.Text = result;
                if (HistoryBox1.Text == "") HistoryBox1.Text += heststr;
                else HistoryBox1.Text += "\r\n" + heststr;
            }
        }

        // Альтернативный вид кнопок
        private void checkBoxMode_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxMode.Checked)
            {
                //Нормальная панель
                buttonSqrt_N.Text = "Cbrt";
                buttonStep2_N.Text = "^3";
                buttonStepM1_N.Text = "^";
                button00_N.Text = "00";
                buttonClr_N.Text = "HClr";
                //Инженерная панель
                buttonSin.Text = "ASin";
                buttonCos.Text = "ACos";
                buttonTg.Text = "ATg";
                buttonCtg.Text = "ACtg";
                buttonLn.Text = "Lg";
                buttonStep2.Text = "^3";
                buttonSqrt.Text = "Cbrt";
                button00.Text = "00";
                buttonClr.Text = "HClr";
                //Матричная панель
                button00_M.Text = "00";
                buttonClr_M.Text = "HClr";
                //Программная панель
                button00_P.Text = "00";
                buttonClr_P.Text = "HClr";
            }
            else
            {
                //Нормальная панель
                buttonSqrt_N.Text = "Sqrt";
                buttonStep2_N.Text = "^2";
                buttonStepM1_N.Text = "^-1";
                button00_N.Text = "0";
                buttonClr_N.Text = "Clr";
                //Инженерная панель
                buttonSin.Text = "Sin";
                buttonCos.Text = "Cos";
                buttonTg.Text = "Tg";
                buttonCtg.Text = "Ctg";
                buttonLn.Text = "Ln";
                buttonStep2.Text = "^2";
                buttonSqrt.Text = "Sqrt";
                button00.Text = "0";
                buttonClr.Text = "Clr";
                //Матричная панель
                button00_M.Text = "0";
                buttonClr_M.Text = "Clr";
                //Программная панель
                button00_P.Text = "0";
                buttonClr_P.Text = "Clr";
            }
        }

        private void checkCalcMode_P_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                switch (((RadioButton)sender).Name)
                {
                    case "radioMathExpr":
                    default:
                        
                        break;
                    case "radioLogicExpr":
                        SSmode = 16;
                        break;
                }
                ChangeSSTo(SSmode);
            }
        }

        private void toolStripModeChange_Click(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Name)
            {
                case "toolStripMode1":
                default:
                    panelmode = "norm";
                    break;
                case "toolStripMode2":
                    panelmode = "engen";
                    break;
                case "toolStripMode3":
                    panelmode = "prog";
                    break;
                case "toolStripMode4":
                    panelmode = "matr";
                    break;
                case "toolStripMode5":
                    panelmode = "graph";
                    break;
            }

            foreach (Object lines in toolStripModes.DropDownItems )
            {
                if (lines.ToString().Contains("Separator")) break;
                if (((ToolStripMenuItem)lines).Name == ((ToolStripMenuItem)sender).Name) ((ToolStripMenuItem)lines).Checked = true;
                else ((ToolStripMenuItem)lines).Checked = false;
            }

            ChangeModeTo(panelmode);
        }

        private void radioGRAD_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                switch (((RadioButton)sender).Name)
                {
                    case "radioDEG":
                    default:
                        gradusmode = 0;
                        break;
                    case "radioRAD":
                        gradusmode = 1;
                        break;
                    case "radioGRAD":
                        gradusmode = 2;
                        break;
                }
                ChangeGradusTo(gradusmode);
            }
        }

        private void radioSS_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                switch (((RadioButton)sender).Name)
                {
                    case "radioSS16":
                    default:
                        ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 16);
                        SSmode = 16;
                        break;
                    case "radioSS10":
                        ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 10);
                        SSmode = 10;
                        break;
                    case "radioSS8":
                        ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 8);
                        SSmode = 8;
                        break;
                    case "radioSS2":
                        ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 2);
                        SSmode = 2;
                        break;
                }
                ChangeSSTo(SSmode);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lx = e.X;
            ly = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Capture)
            {
                int dx=0, dy=0;
                if (e.X > 20 && e.Y < 20)
                    dx = e.X - lx;
                else if (e.X < 20 && e.Y > 20)
                    dy = e.Y - ly;
                else if (e.X > 20 && e.Y > 20)
                {
                    dx = e.X - lx;
                    dy = e.Y - ly;
                }
                cx -= dx;
                cy += dy;
                lx = e.X;
                ly = e.Y;
                pictureBox1.Invalidate();
            }
        }

        int lx = 0, ly = 0;
        int cx = -166, cy = -102;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (panelmode == "graph")
            {
                List<MyPoint> points = new List<MyPoint>();
                string input = ScreenBox.Text;
                PreProcess(ref input);
                e.Graphics.DrawImageUnscaled(GraphArithmetic.GetPlot(input, cx, cx + 352, cy + 224, cy, plotmultiplier, plotside), 0, 0);
            }
        }

        private void buttonCreatePlot_Click(object sender, EventArgs e)
        {
            cx = -176; cy = -112;
            panelmode = "graph";
            pictureBox1.Invalidate();
            ChangeModeTo(panelmode);
        }

        private void buttonZoomPlus_Click(object sender, EventArgs e)
        {
            plotmultiplier*=2;
            if (plotmultiplier == 0) plotmultiplier = 1;
            pictureBox1.Invalidate();
        }

        private void buttonZoomMinus_Click(object sender, EventArgs e)
        {
            plotmultiplier/=2;
            if (plotmultiplier == 0) plotmultiplier = -1;
            pictureBox1.Invalidate();
        }

        private void buttonZoomNorm_Click(object sender, EventArgs e)
        {
            cx = -176; cy = -112;
            plotmultiplier = 1;
            pictureBox1.Invalidate();
        }

        private void buttonAddPlotWindow_Click(object sender, EventArgs e)
        {
            if (plotter == null || plotter.IsDisposed) plotter = new PlotForm();
            Random rnd = new Random();
            //plotter.AddPlot(rnd.Next().ToString());
            plotter.AddPlot(ScreenBox.Text.ToUpper());
            plotter.Show();
            plotter.Focus();
        }
        
    }
}