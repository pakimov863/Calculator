using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// Класс работы с бинарными выражениями. Плюс конвертеры в основные СС.
    /// Доступны операции: NOT, AND, OR, IMPL, EQV, MOD2, SHEFF, PIRS.
    /// </summary>
    static class BinaryArithmetic
    {
        #region Основные функции
        /// <summary>
        /// Проводит вычисления в автоматическом режиме
        /// </summary>
        /// <param name="expression">Логическое выражение для подсчета</param>
        /// <returns>Результат вычислений(одномерный массив-строка)</returns>
        public static string CalcToResult(string expression)
        {
            List<Token> rpn = GetRPN(expression);
            Dictionary<string, bool> variables = GetVariables(rpn);
            int count = (int)Math.Pow(2, variables.Count);
            string result = "{[";
            for (int i = 0; i < count; i++)
            {
                AddVariableData(i + count, variables);
                result += (Calculate(rpn, variables) ? "1|" : "0|");
            }
            return result.Substring(0, result.Length - 1) + "]}";
        }

        /// <summary>
        /// Создание обратной польской записи для выражения
        /// </summary>
        /// <param name="expression">Математическое выражение</param>
        /// <returns>Список токенов в виде обратной польской записи</returns>
        public static List<Token> GetRPN(string expression)
        {
            MatchCollection collection = Regex.Matches(expression, @"\(|\)|[A-F]|¬|∨|∧|⨁|_|↓|→|↔");

            Regex variables = new Regex(@"[A-F]");
            Regex operations = new Regex(@"¬|∨|∧|→|↔|⨁|_|↓");
            Regex brackets = new Regex(@"\(|\)");
            string[] priority = { "¬", "∧", "∨", "⨁", "_", "↓", "→", "↔" };

            Stack<string> stack = new Stack<string>();
            List<Token> list = new List<Token>();
            foreach (Match match in collection)
            {
                Match temp = variables.Match(match.Value);
                if (temp.Success)
                {
                    list.Add(new Token(temp.Value, TokenType.Variable)); continue;
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
            while (stack.Count != 0)
                list.Add(new Token(stack.Pop(), TokenType.Operation));

            return list;
        }
        
        /// <summary>
        /// Вычисление выражения по обратной польской записи
        /// </summary>
        /// <param name="rpn">Польская запись выражения</param>
        /// <param name="variables">Словарь с текущими значениями переменных в выражении</param>
        /// <returns>Результат вычислений</returns>
        public static bool Calculate(List<Token> rpn, Dictionary<string, bool> variables)
        {
            Stack<bool> result = new Stack<bool>();
            foreach (Token token in rpn)
            {
                if (token.Type == TokenType.Variable) result.Push(variables[token.Value]);
                if (token.Type == TokenType.Operation)
                {
                    bool a, b;
                    switch (token.Value)
                    {
                        case "¬": //NOT
                            result.Push(!result.Pop());
                            break;
                        case "∧": //AND
                            result.Push(result.Pop() & result.Pop());
                            break;
                        case "∨": //OR
                            result.Push(result.Pop() | result.Pop());
                            break;
                        case "→": //IMP
                            result.Push(result.Pop() | !result.Pop());
                            break;
                        case "↔": //EQV
                            result.Push(!(result.Pop() ^ result.Pop()));
                            break;
                        case "⨁": //MOD2
                            a = result.Pop();
                            b = result.Pop();
                            result.Push((!a & b) | (a & !b));
                            break;
                        case "_": //SHEFF
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(!(a & b));
                            break;
                        case "↓": //PIRS
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(!(a | b));
                            break;
                    }
                }
            }
            return result.Pop();
        }
        
        /// <summary>
        /// Создание словаря переменных в выражении и их значений. + сортировка
        /// </summary>
        /// <param name="rpn">Польская запись выражения</param>
        /// <returns>Словарь переменных выражения</returns>
        public static Dictionary<string, bool> GetVariables(List<Token> rpn)
        {
            string[] variables = rpn.Where(x => x.Type == TokenType.Variable).Distinct().Select(x => x.Value).Cast<string>().ToArray();
            //Sort
            string tmp = "";
            for(int j = 1; j < variables.Length; j++)
            {
                for(int i = 0; i < variables.Length; i++)
                {
                    if ((int)Convert.ToChar(variables[i]) > (int)Convert.ToChar(variables[j]) && i < j)
                    {
                        tmp = variables[i]; variables[i] = variables[j]; variables[j] = tmp;
                    }
                }
            }
            //End sort
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach (string variable in variables)
                dictionary[variable] = false;
            return dictionary;
        }

        /// <summary>
        /// Конвертирует число в 2СС и записывает посимвольно как значение переменных
        /// </summary>
        /// <param name="value">Число-значение пеменных</param>
        /// <param name="variables">Словарь переменных выражения</param>
        public static void AddVariableData(int value, Dictionary<string, bool> variables)
        {
            string binary = Convert.ToString(value, 2);
            for (int i = 1; i < binary.Length; i++)
                variables[variables.ElementAt(i - 1).Key] = binary[i] == '0' ? false : true;
        }

        #endregion

        #region Конвертеры: X->10 + 10->X + конвертация выражения
        /// <summary>
        /// Конвертация строки в различные системы счисления
        /// </summary>
        /// <param name="inputstring">Исходная строка</param>
        /// <param name="fromss">Система, из которой переводим</param>
        /// <param name="toss">Система, в которую переводим</param>
        /// <returns></returns>
        public static string ConvertStringToSS(string inputstring, int fromss, int toss)
        {
            if (fromss == toss) return inputstring;
            if (fromss < 2 && fromss > 16 && toss < 2 && toss > 16) return "ERR";

            MatchCollection collection = Regex.Matches(inputstring, @"\(|\)|[A-F]|[0-9]|\,|\+|\-|\*|\/");

            Regex variables = new Regex(@"[A-F]|[0-9]|\,");
            Regex operations = new Regex(@"\+|\-|\*|\/|\(|\)");

            string resultstring = "";

            bool fl = false; //AB3 -> A B 3
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
                        resultstring += ConvertFrom10ToX(ConvertFromXTo10(t_str, fromss), toss);
                        t_str = "";
                        fl = false;
                    }
                }

                temp = operations.Match(match.Value);
                if (temp.Success) resultstring += temp.Value;
            }
            
            if (fl)
            {
                resultstring += ConvertFrom10ToX(ConvertFromXTo10(t_str, fromss), toss);
                t_str = "";
                fl = false;
            }

            return resultstring;
        }

        /// <summary>
        /// Перевод из произвольной системы счисления в 10
        /// </summary>
        /// <param name="inputstring">Исходная строка</param>
        /// <param name="x">Система счисления</param>
        /// <returns></returns>
        public static string ConvertFromXTo10(string inputstring, int x)
        {
            if (x < 2 || x > 16) return "ERR";
            if (x == 10) return inputstring;

            double result = 0;
            int k;
            if (!inputstring.Contains(',')) k = inputstring.Length - 1;
            else k = inputstring.IndexOf(',') - 1;

            for (int i = 0; i < inputstring.Length; i++)
            {
                if (inputstring[i] == ',') continue;
                double tvar;
                if (inputstring[i] == 'A') tvar = 10;
                else if (inputstring[i] == 'B') tvar = 11;
                else if (inputstring[i] == 'C') tvar = 12;
                else if (inputstring[i] == 'D') tvar = 13;
                else if (inputstring[i] == 'E') tvar = 14;
                else if (inputstring[i] == 'F') tvar = 15;
                else tvar = Convert.ToDouble(Char.GetNumericValue(inputstring[i]));
                tvar *= Math.Pow(x, k--);
                result += tvar;
            }
            return Convert.ToString(result);
        }

        /// <summary>
        /// Перевод из 10 в произвольную систему счисления
        /// </summary>
        /// <param name="inputstring">Исходная строка</param>
        /// <param name="x">Система счисления</param>
        /// <returns></returns>
        public static string ConvertFrom10ToX(string inputstring, int x)
        {
            if (x < 2 || x > 16) return "ERR";
            if (x == 10) return inputstring;

            string result = "";
            double inputnumber_c = Math.Truncate(Convert.ToDouble(inputstring));
            double inputnumber_d = Convert.ToDouble(inputstring) - inputnumber_c;

            if (Math.Abs(inputnumber_c) > x - 1)
            {
                while (Math.Abs(inputnumber_c) > x - 1)
                {
                    int tvar = Convert.ToInt32(inputnumber_c % x);
                    if (tvar == 10) result = "A" + result;
                    else if (tvar == 11) result = "B" + result;
                    else if (tvar == 12) result = "C" + result;
                    else if (tvar == 13) result = "D" + result;
                    else if (tvar == 14) result = "E" + result;
                    else if (tvar == 15) result = "F" + result;
                    else result = tvar.ToString() +result;

                    inputnumber_c = Math.Truncate(Convert.ToDouble(inputnumber_c / x));
                }
                if (inputnumber_c == 10) result = "A" + result;
                else if (inputnumber_c == 11) result = "B" + result;
                else if (inputnumber_c == 12) result = "C" + result;
                else if (inputnumber_c == 13) result = "D" + result;
                else if (inputnumber_c == 14) result = "E" + result;
                else if (inputnumber_c == 15) result = "F" + result;
                else result = inputnumber_c.ToString() + result;
            }
            else
            {
                if (inputnumber_c == 10) result = "A";// + result;
                else if (inputnumber_c == 11) result = "B";// + result;
                else if (inputnumber_c == 12) result = "C";// + result;
                else if (inputnumber_c == 13) result = "D";// + result;
                else if (inputnumber_c == 14) result = "E";// + result;
                else if (inputnumber_c == 15) result = "F";// + result;
                else result = Convert.ToString(inputnumber_c);
            }

            if (inputnumber_d != 0)
            {
                result += ",";

                for (int i = 0; i < 8; i++)
                {
                    inputnumber_d *= x;
                    int tvar = Convert.ToInt32(Math.Truncate(inputnumber_d));
                    if (tvar == 10) result += "A";
                    else if (tvar == 11) result += "B";
                    else if (tvar == 12) result += "C";
                    else if (tvar == 13) result += "D";
                    else if (tvar == 14) result += "E";
                    else if (tvar == 15) result += "F";
                    else result += tvar.ToString();

                    inputnumber_d -= Math.Truncate(inputnumber_d);
                }
            }
            /*while (Math.Abs(inputnumber) > x-1)
            {
                result = Convert.ToString((inputnumber % x)) + result;
                inputnumber = Convert.ToInt32(Math.Truncate(Convert.ToDouble(inputnumber / x)));
            }*/

            return result;
        }
        #endregion
    }
}
