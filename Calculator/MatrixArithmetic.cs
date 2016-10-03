using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// Класс работы с матрицами.
    /// Доступны операции: +, -, *, /, OPR(), T(), ^.
    /// Возможно до 14 разных матриц в выражении.
    /// </summary>
    static class MatrixArithmetic
    {
        #region Основные функции
        /// <summary>
        /// Проводит вычисления в автоматическом режиме
        /// </summary>
        /// <param name="expression">Выражение для подсчета</param>
        /// <returns>Результат вычислений</returns>
        public static string CalcToResult(string expression)
        {
            Dictionary<string, string> mas = new Dictionary<string, string>();
            GetMatrixes(ref expression, ref mas);
            List<Token> RPN = MatrixArithmetic.GetRPN(expression);
            return MatrixArithmetic.Calculate(RPN, mas);
        }

        /// <summary>
        /// Преобразование выражения в буквенный вид, создание словаря матричных строк
        /// </summary>
        /// <param name="expression">Выражение для преобразования</param>
        /// <param name="variables">Словарь для матриц(Если в матрице содержится математическое выражение - оно будет решено)</param>
        /// <returns>Возвращает true, если завершилось успешно</returns>
        public static bool GetMatrixes(ref string expression, ref Dictionary<string, string> variables)
        {
            if (CheckBrackets(ref expression))
            {
                char smb = 'A';
                string expression_copy = expression;
                int fpos = -1;
                int spos = -1;

                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == '{')
                    {
                        if (fpos != -1) return false;
                        else fpos = i;
                    }

                    if (expression[i] == '}')
                    {
                        if (fpos == -1) return false;
                        else spos = i;
                    }

                    if (fpos != -1 && spos != -1)
                    {
                        if (smb <= 'N')
                        {
                            string elem = expression.Substring(fpos + 1, spos - fpos - 1);
                            if (expression_copy.Contains(elem))
                            {
                                expression_copy = expression_copy.Replace("{" + elem + "}", smb.ToString());
                                variables[smb.ToString()] = "{" + elem + "}";
                                smb++;
                            }
                            fpos = -1; spos = -1;
                        }
                        else return false;
                    }
                }
                expression = expression_copy;

                //Вычисление значений в ячейках матриц
                for (int k = 0; k < variables.Count; k++)
                {
                    string[,] matr = RetMatrix(variables.ElementAt(k).Value);
                    for (int i = 0; i < matr.GetLength(0); i++)
                    {
                        for (int j = 0; j < matr.GetLength(1); j++)
                        {
                            matr[i, j] = Convert.ToString(Arithmetic.CalcToResult(matr[i, j]));
                        }
                    }
                    variables[variables.ElementAt(k).Key] = RetLine(matr);
                }
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Создание обратной польской записи для выражения
        /// </summary>
        /// <param name="expression">Математическое выражение</param>
        /// <returns>Список токенов в виде обратной польской записи</returns>
        public static List<Token> GetRPN(string expression)
        {
            MatchCollection collection = Regex.Matches(expression, @"\(|\)|[A-N]|[0-9]|\,|\+|\-|\*|\/|\^|T|OPR");

            Regex variables = new Regex(@"[A-N]|[0-9]|\,");
            Regex operations = new Regex(@"\+|\-|\*|\/|\^|T|OPR");
            Regex brackets = new Regex(@"\(|\)");
            string[] priority = { "OPR", "T", "^", "*", "/", "-", "+" };

            Stack<string> stack = new Stack<string>();
            List<Token> list = new List<Token>();

            bool fl = false;
            string t_str = "";

            foreach (Match match in collection)
            {
                Match temp = variables.Match(match.Value);
                if (temp.Success)
                {
                    if (fl)
                    { //уже сохранялось число
                        t_str += temp.Value;
                    }
                    else
                    { //еще не сохранялось
                        t_str = temp.Value;
                        fl = true;
                    }
                    continue;
                }
                else
                {
                    if (fl)
                    {
                        list.Add(new Token(t_str, TokenType.Variable));
                        t_str = "";
                        fl = false;
                    }
                }

                temp = brackets.Match(match.Value);
                if (temp.Success)
                {
                    if (temp.Value == "(") { stack.Push(temp.Value); continue; }
                    string operation = stack.Pop();
                    while (operation != "(")
                    {
                        list.Add(new Token(operation, TokenType.Operation));
                        operation = stack.Pop();
                    }
                    continue;
                }

                temp = operations.Match(match.Value);
                if (temp.Success)
                {
                    if (stack.Count != 0)
                        while (Array.IndexOf(priority, temp.Value) > Array.IndexOf(priority, stack.Peek()))
                        {
                            if (stack.Peek() == "(") break;
                            list.Add(new Token(stack.Pop(), TokenType.Operation));
                            if (stack.Count == 0) break;
                        }
                    stack.Push(temp.Value);
                }
            }

            if (fl)
            {
                list.Add(new Token(t_str, TokenType.Variable));
                t_str = "";
                fl = false;
            }

            while (stack.Count != 0)
                list.Add(new Token(stack.Pop(), TokenType.Operation));

            return list;
        }

        /// <summary>
        /// Вычисление выражения по обратной польской записи
        /// </summary>
        /// <param name="rpn">Польская запись выражения</param>
        /// <param name="variables">Словарь с матрицами</param>
        /// <returns>Результат вычислений или ERR</returns>
        public static string Calculate(List<Token> rpn, Dictionary<string, string> variables)
        {
            Regex letter = new Regex(@"[A-N]");
            Stack<string> result = new Stack<string>();
            foreach (Token token in rpn)
            {
                if (token.Type == TokenType.Variable)
                {
                    if (letter.IsMatch(token.Value) && token.Value != "OPR") result.Push(variables[token.Value]);
                    else result.Push(token.Value);
                }
                if (token.Type == TokenType.Operation)
                {
                    string a = "";
                    string b = "";
                    switch (token.Value)
                    {
                        case "+":
                            a = result.Pop();
                            b = result.Pop();

                            if (a.Contains("{") && b.Contains("{"))
                            {//если обе - матрицы
                                string[,] amatr = RetMatrix(a);
                                string[,] bmatr = RetMatrix(b);
                                if (amatr.GetLength(0) == bmatr.GetLength(0) && amatr.GetLength(1) == bmatr.GetLength(1))
                                {
                                    string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                    for (int i = 0; i < amatr.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < amatr.GetLength(1); j++)
                                        {
                                            resmatr[i, j] = Convert.ToString(Convert.ToDouble(bmatr[i, j]) + Convert.ToDouble(amatr[i, j]));
                                        }
                                    }
                                    result.Push(RetLine(resmatr));
                                }
                                else return "ERR";
                            }

                            if (a.Contains("{") && !b.Contains("{"))
                            {//если одна матрица
                                string[,] amatr = RetMatrix(a);
                                Double bmatr = Convert.ToDouble(b);
                                string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                for (int i = 0; i < amatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < amatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(bmatr + Convert.ToDouble(amatr[i, j]));
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }
                            if (!a.Contains("{") && b.Contains("{"))
                            {//если одна матрица
                                Double amatr = Convert.ToDouble(a);
                                string[,] bmatr = RetMatrix(b);
                                string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                for (int i = 0; i < bmatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < bmatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(amatr + Convert.ToDouble(bmatr[i, j]));
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }

                            if (!a.Contains("{") && !b.Contains("{"))
                            {//если обе - числа
                                result.Push(Convert.ToString(Convert.ToDouble(b) + Convert.ToDouble(a)));
                            }
                            break;
                        case "-":
                            a = result.Pop();
                            b = result.Pop();

                            if (a.Contains("{") && b.Contains("{"))
                            {//если обе - матрицы
                                string[,] amatr = RetMatrix(a);
                                string[,] bmatr = RetMatrix(b);
                                if (amatr.GetLength(0) == bmatr.GetLength(0) && amatr.GetLength(1) == bmatr.GetLength(1))
                                {
                                    string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                    for (int i = 0; i < amatr.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < amatr.GetLength(1); j++)
                                        {
                                            resmatr[i, j] = Convert.ToString(Convert.ToDouble(bmatr[i, j]) - Convert.ToDouble(amatr[i, j]));
                                        }
                                    }
                                    result.Push(RetLine(resmatr));
                                }
                                else return "ERR";
                            }

                            if (a.Contains("{") && !b.Contains("{"))
                            {//если одна матрица
                                string[,] amatr = RetMatrix(a);
                                Double bmatr = Convert.ToDouble(b);
                                string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                for (int i = 0; i < amatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < amatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(bmatr - Convert.ToDouble(amatr[i, j]));
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }
                            if (!a.Contains("{") && b.Contains("{"))
                            {//если одна матрица
                                Double amatr = Convert.ToDouble(a);
                                string[,] bmatr = RetMatrix(b);
                                string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                for (int i = 0; i < bmatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < bmatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(Convert.ToDouble(bmatr[i, j]) - amatr);
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }

                            if (!a.Contains("{") && !b.Contains("{"))
                            {//если обе - числа
                                result.Push(Convert.ToString(Convert.ToDouble(b) - Convert.ToDouble(a)));
                            }
                            break;
                        case "*":
                            a = result.Pop();
                            b = result.Pop();

                            if (a.Contains("{") && b.Contains("{"))
                            {//если обе - матрицы
                                string[,] amatr = RetMatrix(a);
                                string[,] bmatr = RetMatrix(b);

                                if (bmatr.GetLength(1) == amatr.GetLength(0))
                                {
                                    result.Push(RetLine(MatrixMul(bmatr, amatr)));
                                }
                                else return "ERR";
                            }

                            if (a.Contains("{") && !b.Contains("{"))
                            {//если одна матрица
                                string[,] amatr = RetMatrix(a);
                                Double bmatr = Convert.ToDouble(b);
                                string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                for (int i = 0; i < amatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < amatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(bmatr * Convert.ToDouble(amatr[i, j]));
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }
                            if (!a.Contains("{") && b.Contains("{"))
                            {//если одна матрица
                                Double amatr = Convert.ToDouble(a);
                                string[,] bmatr = RetMatrix(b);
                                string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                for (int i = 0; i < bmatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < bmatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(Convert.ToDouble(bmatr[i, j]) * amatr);
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }

                            if (!a.Contains("{") && !b.Contains("{"))
                            {//если обе - числа
                                result.Push(Convert.ToString(Convert.ToDouble(b) * Convert.ToDouble(a)));
                            }
                            break;
                        case "/":
                            a = result.Pop();
                            b = result.Pop();

                            if (a.Contains("{") && b.Contains("{"))
                            {//если обе - матрицы
                                string[,] amatr = RetMatrix(a);
                                string[,] bmatr = RetMatrix(b);

                                if (bmatr.GetLength(1) == amatr.GetLength(0) && amatr.GetLength(0) == amatr.GetLength(1))
                                {
                                    result.Push(RetLine(MatrixMul(bmatr, MatrixInverse(amatr))));
                                }
                                else return "ERR";
                            }

                            if (a.Contains("{") && !b.Contains("{"))
                            {//если одна матрица
                                string[,] amatr = RetMatrix(a);
                                Double bmatr = Convert.ToDouble(b);
                                string[,] resmatr = new string[amatr.GetLength(0), amatr.GetLength(1)];
                                for (int i = 0; i < amatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < amatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(bmatr / Convert.ToDouble(amatr[i, j]));
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }
                            if (!a.Contains("{") && b.Contains("{"))
                            {//если одна матрица
                                Double amatr = Convert.ToDouble(a);
                                string[,] bmatr = RetMatrix(b);
                                string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                for (int i = 0; i < bmatr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < bmatr.GetLength(1); j++)
                                    {
                                        resmatr[i, j] = Convert.ToString(Convert.ToDouble(bmatr[i, j]) / amatr);
                                    }
                                }
                                result.Push(RetLine(resmatr));
                            }
                            if (!a.Contains("{") && !b.Contains("{"))
                            {//если обе - числа
                                result.Push(Convert.ToString(Convert.ToDouble(b) / Convert.ToDouble(a)));
                            }
                            break;
                        case "^":
                            a = result.Pop();
                            b = result.Pop();

                            if (b.Contains("{") && !a.Contains("{"))
                            {//если матрица
                                string[,] bmatr = RetMatrix(b);
                                if (bmatr.GetLength(0) == bmatr.GetLength(1))
                                {
                                    string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                    if (Math.Abs(Convert.ToDouble(a)) != 1)
                                    {
                                        resmatr = MatrixMul(bmatr, bmatr);
                                        for (int i = 3; i <= Math.Abs(Convert.ToDouble(a)); i++)
                                        {
                                            resmatr = MatrixMul(resmatr, bmatr);
                                        }
                                    }
                                    else resmatr = bmatr;

                                    if (Convert.ToDouble(a) < 0) result.Push(RetLine(MatrixInverse(resmatr)));
                                    else result.Push(RetLine(resmatr));
                                }
                                else if (Convert.ToDouble(a) == 0)
                                {
                                    string[,] resmatr = new string[bmatr.GetLength(0), bmatr.GetLength(1)];
                                    for (int i = 0; i < bmatr.GetLength(0); i++)
                                        resmatr[i, i] = "1";
                                    result.Push(RetLine(resmatr));
                                }
                                else return "ERR";
                            }
                            else if (!b.Contains("{") && !a.Contains("{"))
                            {//если число
                                result.Push(Convert.ToString(Math.Pow(Convert.ToDouble(b), Convert.ToDouble(a))));
                            }
                            else return "ERR in^";
                            break;
                        case "OPR":
                            a = result.Pop();

                            if (a.Contains("{"))
                            {//если матрица
                                string[,] amatr = RetMatrix(a);
                                if (amatr.GetLength(0) == amatr.GetLength(1))
                                {
                                    result.Push(Convert.ToString(GetDeterm(amatr)));
                                }
                                else return "ERR";
                            }

                            if (!a.Contains("{"))
                            {//если число
                                result.Push(a);
                            }
                            break;
                        case "T":
                            a = result.Pop();

                            if (a.Contains("{"))
                            {//если матрица
                                string[,] amatr = RetMatrix(a);

                                result.Push(RetLine(MatrixPonate(amatr)));
                            }

                            if (!a.Contains("{"))
                            {//если число
                                result.Push(a);
                            }
                            break;
                    }
                }
            }
            return result.Pop();
        }

        #endregion

        #region Вспомогательные функции
        /// <summary>
        /// Проверяет корректность выражения по количеству скобок
        /// </summary>
        /// <param name="expression">Выражение для проверки</param>
        /// <returns>Возвращает true, если выражение корректно</returns>
        private static bool CheckBrackets(ref string expression)
        {
            int open1 = 0,clos1 = 0, open2=0, clos2=0;
            for(int i=0; i<expression.Length; i++)
            {
                if (expression[i] == '{') open1++;
                if (expression[i] == '}') clos1++;
                if (expression[i] == '[') open2++;
                if (expression[i] == ']') clos2++;
            }
            if (open1 != clos1 || open2 != clos2) return false;
            else return true;
        }

        /// <summary>
        /// Составляет строковую матрицу из строки
        /// </summary>
        /// <param name="matrexpression">Выражение для преобразования</param>
        /// <returns>Полученная матрица</returns>
        private static string[,] RetMatrix(string matrexpression)
        {
            int width = -1;
            int height = -1;
            int ffpos = -1;
            int sspos = -1;
            List<string> line_tmp = new List<string>();
            string[,] retmatrix = null;

            for (int i = 0; i < matrexpression.Length; i++)
            {
                if (matrexpression[i] == '[')
                {
                    if (ffpos != -1) return null;
                    else ffpos = i;
                }

                if (matrexpression[i] == ']')
                {
                    if (ffpos == -1) return null;
                    else sspos = i;
                }

                if (ffpos != -1 && sspos != -1)
                {
                    line_tmp.Add(matrexpression.Substring(ffpos + 1, sspos - ffpos - 1));
                    if (width == -1) width = line_tmp.Last().Split('|').Length;
                    else if (width != line_tmp.Last().Split('|').Length) return null;
                    ffpos = -1; sspos = -1;
                }
            }

            height = line_tmp.Count;

            retmatrix = new string[height, width];
            for (int ii = 0; ii < height; ii++)
            {
                for (int jj = 0; jj < width; jj++)
                {
                    string[] str_tmp = line_tmp[ii].Split('|');
                    retmatrix[ii, jj] = str_tmp[jj];
                }
            }

            return retmatrix;
        }

        /// <summary>
        /// Составляет строку из строковой матрицы
        /// </summary>
        /// <param name="matrix">Матрица для преобразования</param>
        /// <returns>Полученная строка</returns>
        private static string RetLine(string[,] matrix)
        {
            string result = "{";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string str = "[" + matrix[i, 0];
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    str += "|" + matrix[i, j];
                }
                result += str + "]";
            }

            return result + "}";
        }

        /// <summary>
        /// Инверсия матрицы(возведение в -1 степень)
        /// </summary>
        /// <param name="matrix">Матрица для вычислений</param>
        /// <returns>Полученная матрица</returns>
        private static string[,] MatrixInverse(string[,] matrix)
        {
            string[,] resmatr = new string[matrix.GetLength(0), matrix.GetLength(1)];
            string[,] matrix_copy = new string[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix_copy.GetLength(0); i++)
            {
                for (int j = 0; j < matrix_copy.GetLength(1); j++)
                {
                    matrix_copy[i, j] = Convert.ToString(GetDeterm(GetMinor(matrix,i,j)));
                }
            }

            double determ = GetDeterm(matrix);

            matrix_copy = MatrixPonate(matrix_copy);

            int sign = 1;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i % 2 == 0) sign = 1;
                else sign = -1;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    resmatr[i, j] = Convert.ToString(sign * Convert.ToDouble(matrix_copy[i, j]) / determ);
                    sign = -sign;
                }
            }
            return resmatr;
        }

        /// <summary>
        /// Транспонирование матрицы(Замена строк столбцами и наоборот)
        /// </summary>
        /// <param name="matrix">Матрица для вычислений</param>
        /// <returns>Полученная матрица</returns>
        private static string[,] MatrixPonate(string[,] matrix)
        {
            string[,] resmatr = new string[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    resmatr[j, i] = matrix[i, j];
                }
            }
            return resmatr;
        }

        /// <summary>
        /// Перемножение матриц
        /// </summary>
        /// <param name="matrix1">Первая матрица-множитель</param>
        /// <param name="matrix2">Вторая матрица-множитель</param>
        /// <returns>Полученная матрица</returns>
        private static string[,] MatrixMul(string[,] matrix1,string[,] matrix2)
        {
            string[,] resmatr = new string[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < resmatr.GetLength(0); i++)
            {
                for (int j = 0; j < resmatr.GetLength(1); j++)
                {
                    resmatr[i, j] = "0";
                    for (int k = 0; k < matrix2.GetLength(0); k++)
                    {
                        resmatr[i, j] = Convert.ToString(Convert.ToDouble(resmatr[i, j]) +
                            Convert.ToDouble(matrix2[k, j]) * Convert.ToDouble(matrix1[i, k]));
                    }
                }
            }
            return resmatr;
        }

        /// <summary>
        /// Определитель матрицы 3 порядка (Способом Саррюса)
        /// </summary>
        /// <param name="matrix">Матрица для вычислений</param>
        /// <returns>Результат</returns>
        private static Double GetDeterm3(string[,] matrix)
        {
            if (matrix.GetLength(0) != 3 && matrix.GetLength(1)!=3) return Double.NaN;

            string[,] matrix_tmp = new string[matrix.GetLength(0), matrix.GetLength(1) * 2 - 1];
            for (int i = 0; i < matrix_tmp.GetLength(0); i++)
                for (int j = 0; j < matrix_tmp.GetLength(1); j++)
                {
                    if(j < matrix.GetLength(1))
                        matrix_tmp[i, j] = matrix[i, j];
                    else
                        matrix_tmp[i, j] = matrix[i, j - matrix.GetLength(1)];
                }

            Double result = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Double diag1 = 1;
                Double diag2 = 1;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    diag1 *= Convert.ToDouble(matrix_tmp[i, i+j]);
                    diag2 *= Convert.ToDouble(matrix_tmp[matrix.GetLength(0) - i - 1, i + j]);
                }
                result = result + diag1 - diag2;
            }

            return result;
        }

        /// <summary>
        /// Определитель матрицы(для >4 порядка рекурсивна, подсчет методом разложения по 1 строке)
        /// </summary>
        /// <param name="matrix">Матрица для вычислений</param>
        /// <returns>Результат</returns>
        private static Double GetDeterm(string[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) return Double.NaN;

            if (matrix.GetLength(0) == 1)
            {
                return Convert.ToDouble(matrix[0, 0]);
            }
            else if(matrix.GetLength(0) == 2)
            {
                return Convert.ToDouble(matrix[0, 0]) * Convert.ToDouble(matrix[1, 1]) - Convert.ToDouble(matrix[0, 1]) * Convert.ToDouble(matrix[1, 0]);
            }
            else if (matrix.GetLength(0) == 3)
            {
                return GetDeterm3(matrix);
            }
            else
            {
                double sign = 1, result = 0;

                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    string[,] minor = GetMinor(matrix, 0, i);
                    result += sign * Convert.ToDouble(matrix[0, i]) * GetDeterm(minor);
                    sign = -sign;
                }

                return result;
            }
        }

        /// <summary>
        /// Возвращает минор матрицы(удаление строки и столбца из начальной матрицы)
        /// </summary>
        /// <param name="matrix">Начальная матрица для вычислений</param>
        /// <param name="m">Строка для удаления(индекс)</param>
        /// <param name="n">Стролец для удаления(индекс)</param>
        /// <returns>Полученная матрица</returns>
        private static string[,] GetMinor(string[,] matrix, int m, int n)
        {
            string[,] result = new string[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            int ii = 0, jj = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i != m)
                {
                    jj = 0;
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j != n)
                        {
                            result[ii, jj] = matrix[i, j];
                            jj++;
                        }
                    }
                    ii++;
                }
            }
            return result;
        }

        #endregion

        #region Для WolframAPI
        /// <summary>
        /// Составляет строку из строковой матрицы, используя синтаксис WolframAlpha
        /// </summary>
        /// <param name="matrix">Матрица для преобразования</param>
        /// <returns>Полученная строка</returns>
        public static string WARetLine(string[,] matrix)
        {
            string result = "{";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string str = "{" + matrix[i, 0];
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    str += "," + matrix[i, j];
                }
                result += str + "},";
            }

            return result.Substring(0, result.Length - 1) + "}";
        }

        /// <summary>
        /// Преобразование синтаксиса выражения в синтаксис WolframAlpha
        /// </summary>
        /// <param name="expression">Выражение для преобразования</param>
        /// <returns>Возвращает true, если завершилось успешно</returns>
        public static bool WAConvertExpression(ref string expression)
        {
            if (CheckBrackets(ref expression))
            {
                Dictionary<string, string> variables = new Dictionary<string, string>();
                char smb = 'A';
                string expression_copy = expression;
                int fpos = -1;
                int spos = -1;

                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == '{')
                    {
                        if (fpos != -1) return false;
                        else fpos = i;
                    }

                    if (expression[i] == '}')
                    {
                        if (fpos == -1) return false;
                        else spos = i;
                    }

                    if (fpos != -1 && spos != -1)
                    {
                        if (smb <= 'N')
                        {
                            string elem = expression.Substring(fpos + 1, spos - fpos - 1);
                            if (expression_copy.Contains(elem))
                            {
                                expression_copy = expression_copy.Replace("{" + elem + "}", smb.ToString());
                                variables[smb.ToString()] = "{" + elem + "}";
                                smb++;
                            }
                            fpos = -1; spos = -1;
                        }
                        else return false;
                    }
                }

                //Перенос данных с конвертацией синтаксиса обратно в строку
                for (int k = 0; k < variables.Count; k++)
                {
                    string WASyntaxStr = WARetLine(RetMatrix(variables.ElementAt(k).Value));
                    expression_copy = expression_copy.Replace(variables.ElementAt(k).Key, WASyntaxStr);
                }
                expression = expression_copy;
                return true;
            }
            else return false;
        }

        #endregion

        /*private static string[,] GetMinor(string[,] matrix, int n)
        {
           string[,] result = new string[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];

           for (int i = 1; i < matrix.GetLength(0); i++)
           {
               for (int j = 0; j < n; j++)
                   result[i - 1, j] = matrix[i, j];
               for (int j = n + 1; j < matrix.GetLength(0); j++)
                   result[i - 1, j - 1] = matrix[i, j];
           }
           return result;
        }*/

        //{[1|2|3][4|5|6][7|8|9]}-{[1|2|3][3|5/9|1][0|1|6]}
        //{[1|2|3][4|5|6][7|8|9]}-{[1|2|3][3|5/9|1]}
        //{[1|2|3][4|5|6][7|8|9]}-{[1|2|3][3|5/9|1]}+5*{[1|2|3][4|5|6][7|8|9]}
        //A-B+C
        //OPR({[1|2|3][4|5|6][7|8|9]})
        //OPR({[1|0-2|3][4|0|6][0-7|8|9]})
        //OPR({[3|0-3|0-5|8][0-3|2|4|0-6][2|0-5|0-7|5][0-4|3|5|0-6]})
    }
}
