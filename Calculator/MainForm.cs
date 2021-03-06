﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WolframAlphaAPI;

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
        public int pointsymbols = 5;
        public bool allowwolfram = false;
        public bool allowvariables = false;
        public PlotForm plotter;
        public Dictionary<string, string> savedVariables;
        
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            ChangeModeTo(panelmode);
            ChangeSSTo(SSmode);
            ChangeGradusTo(gradusmode);
            if ((Properties.Settings.Default.WA_LastWipe.Month < DateTime.Now.Month && Properties.Settings.Default.WA_LastWipe.Year < DateTime.Now.Year) || (Properties.Settings.Default.WA_LastWipe > DateTime.Now))
            {
                Properties.Settings.Default.WA_LastWipe = DateTime.Now;
                Properties.Settings.Default.WA_RespSend = 100;
            }
        }

        private void LoadSettings()
        {
            switch (Properties.Settings.Default.StartPanel)
            {
                case 0:
                default:
                    panelmode = "norm";
                    break;
                case 1:
                    panelmode = "engen";
                    break;
                case 2:
                    panelmode = "prog";
                    break;
                case 3:
                    panelmode = "matr";
                    break;
                case 4:
                    panelmode = "graph";
                    break;
            }
            plotside = Convert.ToByte(Properties.Settings.Default.PlotSide);
            toolStripModeSwitch.Checked = Properties.Settings.Default.AutoALT;
        }

        private void PreProcess(ref string str)
        {
            str = str.ToUpper().Trim() ;
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

            if(str.Contains("^"))
            {
                string[] t_str = str.Split('^');
                for (int i = 1; i < t_str.Length; i++)
                {
                    if(t_str[i].StartsWith("-"))
                        t_str[i] = "_" + t_str[i].Substring(1);
                }
                str = String.Join("^", t_str);
            }

            if(str.Contains("(") || str.Contains(")"))
            {
                int lbr = 0;
                for(int i=0; i < str.Length; i++)
                {
                    if (str[i] == '(') lbr++;
                    if (str[i] == ')') lbr--;
                }
                if (lbr != 0) str = "ERR";
            }

            str = str.Replace(".", ",");
        }

        private string PostProcess(Double str)
        {
            if (Double.IsNaN(str)) return "NaN";
            if (Double.IsInfinity(str)) return "Inf";
            Double result = Math.Round(str, pointsymbols);
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
                default:
                    toolStripSSMode.Text = (SSmode == 8) ? "OCT" : SSmode.ToString() + "S";
                    radioSSX.Checked = true;
                    button00_P.Enabled = false;
                    button01_P.Enabled = false;
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
                    if (SSmode > 2)
                    {
                        button00_P.Enabled = true;
                        button01_P.Enabled = true;
                        button02_P.Enabled = true;
                    }
                    if (SSmode > 3) button03_P.Enabled = true;
                    if (SSmode > 4) button04_P.Enabled = true;
                    if (SSmode > 5) button05_P.Enabled = true;
                    if (SSmode > 6) button06_P.Enabled = true;
                    if (SSmode > 7) button07_P.Enabled = true;
                    if (SSmode > 8) button08_P.Enabled = true;
                    if (SSmode > 9) button09_P.Enabled = true;
                    if (SSmode > 10) buttonA_P.Enabled = true;
                    if (SSmode > 11) buttonB_P.Enabled = true;
                    if (SSmode > 12) buttonC_P.Enabled = true;
                    if (SSmode > 13) buttonD_P.Enabled = true;
                    if (SSmode > 14) buttonE_P.Enabled = true;
                    if (SSmode > 15) buttonF_P.Enabled = true;   
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
            csel += ((Button)sender).Text.Length;
            ScreenBox.Focus();
            ScreenBox.Select(csel, 0);
            if (Properties.Settings.Default.AutoALT && checkBoxMode.Checked) checkBoxMode.Checked = false;
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
            if (Properties.Settings.Default.AutoALT && checkBoxMode.Checked) checkBoxMode.Checked = false;
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
            int csel = ScreenBox.SelectionStart;
            if (ScreenBox.Text.Length != 0 && csel != 0)
            {
                ScreenBox.Text = ScreenBox.Text.Substring(0, csel - 1) + ScreenBox.Text.Substring(csel, ScreenBox.Text.Length - csel);
                csel -= 1;
            }
            ScreenBox.Focus();
            ScreenBox.Select(csel, 0);
            if (Properties.Settings.Default.AutoALT && checkBoxMode.Checked) checkBoxMode.Checked = false;
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
            if (Properties.Settings.Default.AutoALT && checkBoxMode.Checked) checkBoxMode.Checked = false;
        }

        // Начать вычисление
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (buttonEnter.Text == "WA=")
            {
                if (ScreenBox.Text.Length > 200 && (DialogResult.Yes != MessageBox.Show("Похоже, у вас слишком длинный.. запрос. Возможно, WolframAlpha обрежет его и вычисления будут неверны. Продолжить?", "Превышена длина запроса", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
                    return ;
                string inpstr = ScreenBox.Text;

                String WolframAlphaApplicationID = Properties.Settings.Default.WA_AppKey;
                if (WolframAlphaApplicationID == "")
                {
                    MessageBox.Show("Неверно задан ключ для WolframAPI. Введите действительный ключ.", "Не задан ключ приложения", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if (Properties.Settings.Default.WA_RespSend >= Properties.Settings.Default.WA_RespLimit)
                {
                    MessageBox.Show("Превышен месячный лимит запросов. Увеличить лимит или отключить контроль можно в настройках.", "Превышен лимит запросов", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                ScreenBox.Clear();
                WolframResult WR = new WolframResult();
                WR.ShowAndCalculate(inpstr, WolframAlphaApplicationID);
                if (HistoryBox1.Text == "") HistoryBox1.Text += "WAQUERY~" + inpstr;
                else HistoryBox1.Text += "\r\n" + inpstr;
            }
            else
            {
                string inpstr = ScreenBox.Text;
                if (inpstr.Contains("X") || inpstr.Contains("Y"))
                {
                    buttonDigit_Click(sender, e);
                }
                else if(inpstr.StartsWith("set") )
                {
                    if(inpstr.Contains("="))
                    {
                        KeyValuePair<string, string> parsedres = new KeyValuePair<string,string>();
                        if (parseVariable(inpstr, ref parsedres))
                        {
                            if (savedVariables == null) savedVariables = new Dictionary<string, string>();
                            if (savedVariables.Keys.Contains(parsedres.Key))
                            {
                                if(DialogResult.No == MessageBox.Show("Переменная с таким именем уже существует. Перезаписать?","Замена", MessageBoxButtons.YesNo,MessageBoxIcon.Question))
                                {
                                    return;
                                }
                            }

                            ScreenBox.Clear();
                            List<Token> RPN1 = Arithmetic.GetRPN(parsedres.Value, savedVariables);
                            string result = savedVariables[parsedres.Key] = PostProcess(Arithmetic.Calculate(ref RPN1, gradusmode));
                            if (RPN1 != null) RPN1.Clear();
                            savedVariables[parsedres.Key] = result;
                            if (HistoryBox1.Text == "") HistoryBox1.Text += parsedres.Key + "=" + result + ((parsedres.Value == result) ? "" : " [" + parsedres.Value + "]");
                            else HistoryBox1.Text += "\r\n" + parsedres.Key + "=" + result + ((parsedres.Value == result) ? "" : " [" + parsedres.Value + "]");
                        }
                    }
                    else buttonDigit_Click(sender, e);
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
                            //List<Token> RPN1 = Arithmetic.GetRPN(inpstr);
                            //result = Convert.ToString(Arithmetic.Calculate(ref RPN1, gradusmode));
                            //result = PostProcess(Convert.ToDouble(result));
                            //if (RPN1 != null) RPN1.Clear();
                            List<Token> RPN1 = Arithmetic.GetRPN(inpstr, savedVariables);
                            result = Convert.ToString(Arithmetic.Calculate(ref RPN1, gradusmode));
                            result = PostProcess(Convert.ToDouble(result));
                            if (RPN1 != null) RPN1.Clear();
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
                                if (RPN2 != null) RPN2.Clear();
                            }
                            break;
                        case "matr":
                            //Вычисление на матричной панели
                            //Dictionary<string, string> mas = new Dictionary<string, string>();
                            //MatrixArithmetic.GetMatrixes(ref inpstr, ref mas);
                            List<Token> RPN3 = MatrixArithmetic.GetRPN(inpstr);
                            //result = MatrixArithmetic.Calculate(RPN3, mas);
                            result = MatrixArithmetic.Calculate(RPN3, savedVariables);
                            if (RPN3 != null) RPN3.Clear();
                            //mas.Clear();
                            break;
                    }

                    heststr += "=" + result;
                    ScreenBox.Text = result;
                    if (HistoryBox1.Text == "") HistoryBox1.Text += heststr;
                    else HistoryBox1.Text += "\r\n" + heststr;
                }
            }
        }

        private bool parseVariable(string input, ref KeyValuePair<string, string> output)
        {
            input = input.ToUpper().Trim();
            string[] mas = input.Split('=');
            if (mas.Length != 2) return false;
            if (!mas[0].StartsWith("SET")) return false;
            mas[0] = mas[0].Substring(3).Trim();
            if (checkReserved(mas[0])) return false;
            foreach (char c in mas[0]) if (!Char.IsLetter(c)) return false;

            mas[1] = mas[1].Trim();

            output = new KeyValuePair<string, string>(mas[0], mas[1]);
            return true;
        }

        private bool checkReserved(string input)
        {
            string[] reserverwords = { "SQRT", "QBRT", "XQRT", "ASIN", "SINH", "SIN", "ACOS", "COSH", "COS", "ATG", "TGH", "TG", "ACTG", "CTGH", "CTG", "LN", "LG", "LOG", "EXP", "ABS", "SET" };
            input = input.ToUpper().Trim();
            foreach (string item in reserverwords) if (input == item) return true;
            return false;
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
                buttonClr_N.Text = "HCLR";
                //Инженерная панель
                buttonSin.Text = "ASin";
                buttonCos.Text = "ACos";
                buttonTg.Text = "ATg";
                buttonCtg.Text = "ACtg";
                buttonLn.Text = "Lg";
                buttonStep2.Text = "^3";
                buttonSqrt.Text = "Cbrt";
                button00.Text = "00";
                buttonClr.Text = "HCLR";
                //Матричная панель
                button00_M.Text = "00";
                buttonClr_M.Text = "HCLR";
                //Программная панель
                button00_P.Text = "00";
                buttonClr_P.Text = "HCLR";
                //Переключение в Wolfram-калькулятор
                if (allowwolfram)
                {
                    buttonEnter_M.Text = "WA=";
                    buttonEnter_N.Text = "WA=";
                    buttonEnter_P.Text = "WA=";
                    buttonEnter.Text = "WA=";
                }
                else
                {
                    buttonEnter_M.Text = "=";
                    buttonEnter_N.Text = "=";
                    buttonEnter_P.Text = "=";
                    buttonEnter.Text = "=";
                }
            }
            else
            {
                //Нормальная панель
                buttonSqrt_N.Text = "Sqrt";
                buttonStep2_N.Text = "^2";
                buttonStepM1_N.Text = "^-1";
                button00_N.Text = "0";
                buttonClr_N.Text = "CLR";
                //Инженерная панель
                buttonSin.Text = "Sin";
                buttonCos.Text = "Cos";
                buttonTg.Text = "Tg";
                buttonCtg.Text = "Ctg";
                buttonLn.Text = "Ln";
                buttonStep2.Text = "^2";
                buttonSqrt.Text = "Sqrt";
                button00.Text = "0";
                buttonClr.Text = "CLR";
                //Матричная панель
                button00_M.Text = "0";
                buttonClr_M.Text = "CLR";
                //Программная панель
                button00_P.Text = "0";
                buttonClr_P.Text = "CLR";
                //Переключение в Wolfram-калькулятор
                if (buttonEnter.Text == "WA=") buttonEnter.Text = "=";
                if (buttonEnter_M.Text == "WA=") buttonEnter_M.Text = "=";
                if (buttonEnter_N.Text == "WA=") buttonEnter_N.Text = "=";
                if (buttonEnter_P.Text == "WA=") buttonEnter_P.Text = "=";
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
            if (sender is RadioButton) { if (!((RadioButton)sender).Checked) return; }
            else return;
            switch (((RadioButton)sender).Name)
            {
                case "radioSS16":
                default:
                    textSSX.Enabled = false;
                    ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 16);
                    SSmode = 16;
                    break;
                case "radioSS10":
                    textSSX.Enabled = false;
                    ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 10);
                    SSmode = 10;
                    break;
                case "radioSSX":
                    textSSX.Enabled = true;
                    short newSS;
                    if (Int16.TryParse(textSSX.Text, out newSS))
                    {
                        if (newSS >= 2 && newSS <= 16)
                        {
                            textSSX.BackColor = SystemColors.Window;
                            ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, newSS);
                            SSmode = newSS;

                            if (newSS == 2 || newSS == 10 || newSS == 16) textSSX.Text = "8";
                        }
                        else
                        {
                            textSSX.BackColor = Color.LightCoral;
                        }
                    }
                    else
                    {
                        textSSX.BackColor = Color.LightCoral;
                    }

                    break;
                case "radioSS2":
                    textSSX.Enabled = false;
                    ScreenBox.Text = BinaryArithmetic.ConvertStringToSS(ScreenBox.Text, SSmode, 2);
                    SSmode = 2;
                    break;
            }
            ChangeSSTo(SSmode);
        }

        private void textSSX_TextChanged(object sender, EventArgs e)
        {
            RadioButton rb = new RadioButton();
            rb.Name = "radioSSX";
            rb.Checked = true;
            radioSS_CheckedChanged(rb, e);
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

        private void toolStripSettings_Click(object sender, EventArgs e)
        {
            SettingsForm SF = new SettingsForm();
            SF.ShowDialog();
        }

        private void toolStripModeSwitch_CheckStateChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoALT = toolStripModeSwitch.Checked ;
            Properties.Settings.Default.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.StartPanelLast)
            {
                switch (panelmode)
                {
                    case "norm":
                    default:
                        Properties.Settings.Default.StartPanel = 0;
                        break;
                    case "engen":
                        Properties.Settings.Default.StartPanel = 1;
                        break;
                    case "prog":
                        Properties.Settings.Default.StartPanel = 2;
                        break;
                    case "matr":
                        Properties.Settings.Default.StartPanel = 3;
                        break;
                    case "graph":
                        Properties.Settings.Default.StartPanel = 4;
                        break;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void toolStripWASwitch_CheckStateChanged(object sender, EventArgs e)
        {
            if(toolStripWASwitch.Checked)
            {
                allowwolfram = true;
                toolStripWA.Visible = true;
                
            }
            else
            {
                allowwolfram = false;
                toolStripWA.Visible = false;
                
            }
            checkBoxMode_CheckedChanged(sender, e);
        }

        private void buttonNewMatr_M_Click(object sender, EventArgs e)
        {
            if (savedVariables == null) savedVariables = new Dictionary<string, string>();
            char i;
            for (i = 'A'; i <= 'N'; i++) if (!savedVariables.ContainsKey(i.ToString())) break;
            EditMatrix em = new EditMatrix(i);
            string[] tmp;
            if(DialogResult.OK == em.ShowDialog(this))
            {
                tmp = em.result.Split('#');
                savedVariables.Add(tmp[0], tmp[1]);

                if (HistoryBox1.Text == "") HistoryBox1.Text += tmp[0] + "=" + tmp[1];
                else HistoryBox1.Text += "\r\n" + tmp[0] + "=" + tmp[1];

                Button tbtn = new Button();
                tbtn.Text = tmp[0];
                buttonDigit_Click(tbtn, e);
            }
            ScreenBox.Focus();
        }

        private void toolStripVarSwitch_CheckStateChanged(object sender, EventArgs e)
        {
            if (toolStripVarSwitch.Checked)
            {
                allowvariables  = true;
            }
            else
            {
                allowwolfram = false;
            }
            checkBoxMode_CheckedChanged(sender, e);
        }
    }
}