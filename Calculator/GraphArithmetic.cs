using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    /// <summary>
    /// Класс работы с графиками. Рисует график функции на картинке учитывая масштаб.
    /// Возможны функции вида: Y= ..... (они же f(x)) 
    /// </summary>
    class GraphArithmetic
    {
        #region Рисование
        /// <summary>
        /// Изображает на картинке оси, сетку, числа и график по точкам
        /// </summary>
        /// <param name="expression">Выражение-функция</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <param name="YFrom">Верхняя граница Y</param>
        /// <param name="YTo">Нижняя граница Y</param>
        /// <param name="mult">Модификатор масштаба: (0 1 2 ..)*X</param>
        /// <returns>Bitmap с изображением</returns>
        public static Bitmap GetPlot(string expression, double XFrom, double XTo, double YFrom, double YTo, double mult, byte side = 0)
        {
            Bitmap bmp = new Bitmap(Convert.ToInt32(XTo - XFrom + 20), Convert.ToInt32(YFrom - YTo + 20));
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            int plotseparate = 15;
            double plotmult = plotseparate * mult;
            if (plotmult == 0) plotmult = plotseparate;

            DrawGrid(bmp, XFrom, XTo, YFrom, YTo, side, plotseparate);
            DrawAxis(bmp, XFrom, XTo, YFrom, YTo, side);
            DrawNumbers(bmp, XFrom, XTo, YFrom, YTo, mult, side, plotseparate);

            List<MyPoint> Points = GetPoints(expression, XFrom / plotmult, XTo / plotmult, 0.5 / mult/*((1 / mult > 1) ? 1 : 1 / mult)*/);
            if (Points != null) DrawPlot(bmp, Points, XFrom, XTo, YFrom, YTo, mult, side, plotseparate);

            /*Bitmap bmp = new Bitmap(Convert.ToInt32(XTo - XFrom + 20), Convert.ToInt32(YFrom - YTo + 20));
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            DrawGrid(bmp, XFrom, XTo, YFrom, YTo);
            DrawAxis(bmp, XFrom, XTo, YFrom, YTo);
            DrawNumbers(bmp, XFrom, XTo, YFrom, YTo, mult);

            if(expression != "")
            {
                List<MyPoint> Points = GetPoints(expression, XFrom, XTo);
                if (Points != null) DrawPlot(bmp, Points, XFrom, XTo, YFrom, YTo, mult);
            }*/

            /*int plotseparate = 15;
            byte plotside = 0;
            double plotmult = plotseparate * mult;
            if (plotmult == 0) plotmult = plotseparate;

            DrawGrid(bmp, XFrom, XTo, YFrom, YTo, plotside, plotseparate);
            DrawAxis(bmp, XFrom, XTo, YFrom, YTo, plotside);
            DrawNumbers(bmp, XFrom, XTo, YFrom, YTo, mult, plotside, plotseparate);

            List<MyPoint> Points = GetPoints(expression, XFrom / plotmult, XTo / plotmult, ((1 / mult > 1) ? 1 : 1 / mult));
            if (Points != null) DrawPlot(bmp, Points, XFrom, XTo, YFrom, YTo, mult, plotside, plotseparate);*/

            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// Рисует график по точкам на картинке (!)
        /// </summary>
        /// <param name="BMP">Bitmap для рисования</param>
        /// <param name="points">Список точек</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <param name="YFrom">Верхняя граница Y</param>
        /// <param name="YTo">Нижняя граница Y</param>
        /// <param name="mult">Модификатор масштаба: (0 1 2 ..)*X</param>
        /// <param name="side">Расположение отступов: [TopLeft=0], BotLeft=1, TopRight=2, BotRight=3</param>
        /// <param name="separate">Шаг сетки: [default=15]</param>
        public static void DrawPlot(Bitmap BMP, List<MyPoint> points, double XFrom, double XTo, double YFrom, double YTo, double mult, byte side = 0, int separate = 15)
        {
            Graphics g = Graphics.FromImage(BMP);
            double width = XTo - XFrom;
            double height = YFrom - YTo;
            double plotmult = separate * mult;
            if (plotmult == 0) plotmult = separate;

            for (int i = 0; i < points.Count - 1; i++)
                //if (points[i].Y + YFrom > -5 && points[i + 1].Y + YFrom < height+5) 
                if (!(Double.IsInfinity(points[i].Y) || Double.IsNaN(points[i].Y) || Double.IsInfinity(points[i + 1].Y) || Double.IsNaN(points[i + 1].Y)))
                    switch (side)
                    {
                        case 0:
                        default:
                            g.DrawLine(new Pen(Color.Blue, 1), Convert.ToInt32(points[i].X * plotmult - XFrom + 20), Convert.ToInt32(-points[i].Y * plotmult + YFrom + 20), Convert.ToInt32(points[i + 1].X * plotmult - XFrom + 20), Convert.ToInt32(-points[i + 1].Y * plotmult + YFrom + 20));
                            break;
                        case 1:
                            g.DrawLine(new Pen(Color.Blue, 1), Convert.ToInt32(points[i].X * plotmult - XFrom + 20), Convert.ToInt32(-points[i].Y * plotmult + YFrom - 20), Convert.ToInt32(points[i + 1].X * plotmult - XFrom + 20), Convert.ToInt32(-points[i + 1].Y * plotmult + YFrom - 20));
                            break;
                        case 2:
                            g.DrawLine(new Pen(Color.Blue, 1), Convert.ToInt32(points[i].X * plotmult - XFrom - 20), Convert.ToInt32(-points[i].Y * plotmult + YFrom + 20), Convert.ToInt32(points[i + 1].X * plotmult - XFrom - 20), Convert.ToInt32(-points[i + 1].Y * plotmult + YFrom + 20));
                            break;
                        case 3:
                            g.DrawLine(new Pen(Color.Blue, 1), Convert.ToInt32(points[i].X * plotmult - XFrom - 20), Convert.ToInt32(-points[i].Y * plotmult + YFrom - 20), Convert.ToInt32(points[i + 1].X * plotmult - XFrom - 20), Convert.ToInt32(-points[i + 1].Y * plotmult + YFrom - 20));
                            break;
                    }

            g.Dispose();
        }

        /// <summary>
        /// Рисует оси на картинке
        /// </summary>
        /// <param name="BMP">Bitmap для рисования</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <param name="YFrom">Верхняя граница Y</param>
        /// <param name="YTo">Нижняя граница Y</param>
        /// <param name="side">Расположение отступов: [TopLeft=0], BotLeft=1, TopRight=2, BotRight=3</param>
        public static void DrawAxis(Bitmap BMP, double XFrom, double XTo, double YFrom, double YTo, byte side = 0)
        {
            Graphics g = Graphics.FromImage(BMP);
            double width = XTo - XFrom;
            double height = YFrom - YTo;

            switch(side)
            {
                case 0:
                default:
                    if(XFrom < 0 && XTo > 0)
                        g.DrawLine(new Pen(Color.Black, 3), Convert.ToInt32(0 - XFrom+20), 20, Convert.ToInt32(0 - XFrom+20), Convert.ToInt32(height+20));
                    if (YFrom > 0 && YTo <0)
                        g.DrawLine(new Pen(Color.Black, 3), 20, Convert.ToInt32(YFrom+20), Convert.ToInt32(width+20), Convert.ToInt32(YFrom+20));
                    break;
                case 1:
                    if(XFrom < 0 && XTo > 0)
                        g.DrawLine(new Pen(Color.Black, 3), Convert.ToInt32(0 - XFrom+20), 0, Convert.ToInt32(0 - XFrom+20), Convert.ToInt32(height-20));

                    if (YFrom > 0 && YTo <0)
                        g.DrawLine(new Pen(Color.Black, 3), 20, Convert.ToInt32(YFrom-20), Convert.ToInt32(width+20), Convert.ToInt32(YFrom-20));
                    break;
                case 2:
                    if(XFrom < 0 && XTo > 0)
                        g.DrawLine(new Pen(Color.Black, 3), Convert.ToInt32(0 - XFrom-20), 20, Convert.ToInt32(0 - XFrom-20), Convert.ToInt32(height+20));
                    if (YFrom > 0 && YTo <0)
                        g.DrawLine(new Pen(Color.Black, 3), 0, Convert.ToInt32(YFrom+20), Convert.ToInt32(width-20), Convert.ToInt32(YFrom+20));
                    break;
                case 3:
                    if(XFrom < 0 && XTo > 0)
                        g.DrawLine(new Pen(Color.Black, 3), Convert.ToInt32(0 - XFrom-20), 0, Convert.ToInt32(0 - XFrom-20), Convert.ToInt32(height-20));
                    if (YFrom > 0 && YTo <0)
                        g.DrawLine(new Pen(Color.Black, 3), 0, Convert.ToInt32(YFrom-20), Convert.ToInt32(width-20), Convert.ToInt32(YFrom-20));
                    break;
            }
            
            g.Dispose();
        }

        /// <summary>
        /// Рисует сетку на картинке
        /// </summary>
        /// <param name="BMP">Bitmap для рисования</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <param name="YFrom">Верхняя граница Y</param>
        /// <param name="YTo">Нижняя граница Y</param>
        /// <param name="side">Расположение отступов: [TopLeft=0], BotLeft=1, TopRight=2, BotRight=3</param>
        /// <param name="separate">Шаг сетки: [default=15]</param>
        public static void DrawGrid(Bitmap BMP, double XFrom, double XTo, double YFrom, double YTo, byte side = 0, int separate = 15)
        {
            Graphics g = Graphics.FromImage(BMP);
            double width = XTo - XFrom;
            double height = YFrom - YTo;

            switch (side)
            {
                case 0:
                default:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), Convert.ToInt32(i - XFrom + 20), 20, Convert.ToInt32(i - XFrom + 20), Convert.ToInt32(height + 20));
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), 20, Convert.ToInt32(i + YFrom + 20), Convert.ToInt32(width + 20), Convert.ToInt32(i + YFrom + 20));
                    }
                    break;
                case 1:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), Convert.ToInt32(i - XFrom + 20), 0, Convert.ToInt32(i - XFrom + 20), Convert.ToInt32(height-20));
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), 20, Convert.ToInt32(i + YFrom-20), Convert.ToInt32(width + 20), Convert.ToInt32(i + YFrom-20));
                    }
                    break;
                case 2:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), Convert.ToInt32(i - XFrom-20), 20, Convert.ToInt32(i - XFrom-20), Convert.ToInt32(height + 20));
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), 0, Convert.ToInt32(i + YFrom + 20), Convert.ToInt32(width-20), Convert.ToInt32(i + YFrom + 20));
                    }
                    break;
                case 3:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), Convert.ToInt32(i - XFrom-20), 0, Convert.ToInt32(i - XFrom-20), Convert.ToInt32(height-20));
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        if (i % separate == 0) g.DrawLine(new Pen(Color.LightGray, 1), 0, Convert.ToInt32(i + YFrom-20), Convert.ToInt32(width-20), Convert.ToInt32(i + YFrom-20));
                    }
                    break;
            }

            g.Dispose();
        }

        /// <summary>
        /// Рисует числа в месте отступов
        /// </summary>
        /// <param name="BMP">Bitmap для рисования</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <param name="YFrom">Верхняя граница Y</param>
        /// <param name="YTo">Нижняя граница Y</param>
        /// <param name="mult">Модификатор масштаба: (0 1 2 ..)*X</param>
        /// <param name="side">Расположение отступов: [TopLeft=0], BotLeft=1, TopRight=2, BotRight=3</param>
        /// <param name="separate">Шаг сетки: [default=15]</param>
        public static void DrawNumbers(Bitmap BMP, double XFrom, double XTo, double YFrom, double YTo, double mult, byte side = 0, int separate = 15)
        {
            Graphics g = Graphics.FromImage(BMP);
            double width = XTo - XFrom;
            double height = YFrom - YTo;
            double plotmult = separate * mult;
            if (plotmult == 0) plotmult = separate;

            switch (side)
            {
                case 0:
                default:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        string itext = (i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(i - XFrom + 15), 3);
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(i - XFrom + 14), 5);
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 6), Brushes.Green, Convert.ToInt32(i - XFrom + 13), 6);
                            if (itext.Length == 4) g.DrawString(itext, new Font("Arial", 5), Brushes.Green, Convert.ToInt32(i - XFrom + 12), 8);
                            if (itext.Length >= 5) g.DrawString(itext, new Font("Arial", 3), Brushes.Green, Convert.ToInt32(i - XFrom + 12), 10);
                        }
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        string itext = (-i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.ToString().Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, 5, Convert.ToInt32(i + YFrom + 14));
                            if (itext.ToString().Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 3, Convert.ToInt32(i + YFrom + 15));
                            if (itext.ToString().Length == 3) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 2, Convert.ToInt32(i + YFrom + 15));
                            if (itext.ToString().Length >= 4) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 0, Convert.ToInt32(i + YFrom + 15));
                        }
                    }
                    break;
                case 1:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        string itext = (i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(i - XFrom + 15), Convert.ToInt32(height + 3-20));
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(i - XFrom + 14), Convert.ToInt32(height + 5-20));
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 6), Brushes.Green, Convert.ToInt32(i - XFrom + 13), Convert.ToInt32(height + 6-20));
                            if (itext.Length == 4) g.DrawString(itext, new Font("Arial", 5), Brushes.Green, Convert.ToInt32(i - XFrom + 12), Convert.ToInt32(height + 8-20));
                            if (itext.Length >= 5) g.DrawString(itext, new Font("Arial", 3), Brushes.Green, Convert.ToInt32(i - XFrom + 12), Convert.ToInt32(height + 10-20));
                        }
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        string itext = (-i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, 5, Convert.ToInt32(i + YFrom - 7-20));
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 3, Convert.ToInt32(i + YFrom - 6-20));
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 2, Convert.ToInt32(i + YFrom - 6-20));
                            if (itext.Length >= 4) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, 0, Convert.ToInt32(i + YFrom - 6-20));
                        }
                    }
                    break;
                case 2:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        string itext = (i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(i - XFrom - 4-20), 3);
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(i - XFrom - 6-20), 5);
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 6), Brushes.Green, Convert.ToInt32(i - XFrom - 8-20), 6);
                            if (itext.Length == 4) g.DrawString(itext, new Font("Arial", 5), Brushes.Green, Convert.ToInt32(i - XFrom - 10-20), 8);
                            if (itext.Length >= 5) g.DrawString(itext, new Font("Arial", 3), Brushes.Green, Convert.ToInt32(i - XFrom - 12-20), 10);
                        }
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        string itext = (-i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(width + 5-20), Convert.ToInt32(i + YFrom + 13));
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 3-20), Convert.ToInt32(i + YFrom + 14));
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 2-20), Convert.ToInt32(i + YFrom + 14));
                            if (itext.Length >= 4) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 0-20), Convert.ToInt32(i + YFrom + 14));
                        }
                    }
                    break;
                case 3:
                    for (double i = XFrom; i <= XTo; i++)
                    {
                        string itext = (i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(i - XFrom - 4-20), Convert.ToInt32(height + 3-20));
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(i - XFrom - 6 - 20), Convert.ToInt32(height + 5 - 20));
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 6), Brushes.Green, Convert.ToInt32(i - XFrom - 8 - 20), Convert.ToInt32(height + 6 - 20));
                            if (itext.Length == 4) g.DrawString(itext, new Font("Arial", 5), Brushes.Green, Convert.ToInt32(i - XFrom - 10 - 20), Convert.ToInt32(height + 8 - 20));
                            if (itext.Length >= 5) g.DrawString(itext, new Font("Arial", 3), Brushes.Green, Convert.ToInt32(i - XFrom - 12 - 20), Convert.ToInt32(height + 10 - 20));
                        }
                    }
                    for (double i = -YFrom; i <= -YTo; i++)
                    {
                        string itext = (-i / plotmult).ToString();
                        if (i % separate == 0)
                        {
                            if (itext.Length == 1) g.DrawString(itext, new Font("Arial", 8), Brushes.Green, Convert.ToInt32(width + 5 - 20), Convert.ToInt32(i + YFrom - 7 - 20));
                            if (itext.Length == 2) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 3 - 20), Convert.ToInt32(i + YFrom - 6 - 20));
                            if (itext.Length == 3) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 2 - 20), Convert.ToInt32(i + YFrom - 6 - 20));
                            if (itext.Length >= 4) g.DrawString(itext, new Font("Arial", 7), Brushes.Green, Convert.ToInt32(width + 0 - 20), Convert.ToInt32(i + YFrom - 6 - 20));
                        }
                    }
                    break;
            }

            g.Dispose();
        }

        #endregion

        #region Получение точек
        /// <summary>
        /// Вычисляет значения функции на промежутке XFrom-XTo
        /// </summary>
        /// <param name="expression">Выражение-функция</param>
        /// <param name="XFrom">Левая граница X</param>
        /// <param name="XTo">Правая граница X</param>
        /// <returns>Список полученных точек</returns>
        public static List<MyPoint> GetPoints(string expression, double XFrom, double XTo, double XStep = 1)
        {
            string[] expr_split = expression.Split('=');
            List<MyPoint> points = new List<MyPoint>();

            if (expression == "") return null;
            if (expr_split.Length != 2) return null;
            if (expr_split[1] == "") return null;

            for (double i = XFrom; i <= XTo; i += XStep) //область изменения X
            {
                string temp = "";
                if(i<0)temp = expr_split[1].Replace("X", "(0"+i.ToString()+")");
                else temp = expr_split[1].Replace("X", i.ToString());
                List<Token> RPN1 = Arithmetic.GetRPN(temp);
                points.Add(new MyPoint(i, Arithmetic.Calculate(ref RPN1, 1)));
            }
            return points;
        }

        #endregion
    }
}