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

        #region Конвертеры: 2,8,16->10 + 10->2,8,16 + конвертация выражения
        private delegate string ConvertAction(string inputstring);
        public static string ConvertStringToSS(string inputstring, int fromss, int toss)
        {
            //Определение делегата конвертации
            ConvertAction ca = null;
            if(fromss == 16)
            {
                if (toss == 16) return inputstring;
                if (toss == 10) ca = ConvertFrom16To10;
                if (toss == 8) ca = ConvertFrom16To8;
                if (toss == 2) ca = ConvertFrom16To2;
            }
            else if(fromss == 10)
            {
                if (toss == 16) ca = ConvertFrom10To16;
                if (toss == 10) return inputstring;
                if (toss == 8) ca = ConvertFrom10To8;
                if (toss == 2) ca = ConvertFrom10To2;
            }
            else if(fromss == 8)
            {
                if (toss == 16) ca = ConvertFrom8To16;
                if (toss == 10) ca = ConvertFrom8To10;
                if (toss == 8) return inputstring;
                if (toss == 2) ca = ConvertFrom8To2;
            }
            else if(fromss == 2)
            {
                if (toss == 16) ca = ConvertFrom2To16;
                if (toss == 10) ca = ConvertFrom2To10;
                if (toss == 8) ca = ConvertFrom2To8;
                if (toss == 2) return inputstring;
            }
            else return "ERR";

            MatchCollection collection = Regex.Matches(inputstring, @"\(|\)|[A-F]|[0-9]|\,|\+|\-|\*|\/");

            Regex variables = new Regex(@"[A-N]|[0-9]|\,");
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
                        resultstring += ca(t_str);
                        t_str = "";
                        fl = false;
                    }
                }

                temp = operations.Match(match.Value);
                if (temp.Success) resultstring += temp.Value;
            }
            
            if (fl)
            {
                resultstring += ca(t_str);
                t_str = "";
                fl = false;
            }

            return resultstring;
        }

        // Основные
        public static string ConvertFrom2To10(string inputstring)
        {
            int result = 0;
            int k = inputstring.Length - 1;
            for (int i = 0; i < inputstring.Length; i++)
            {
                result += Convert.ToInt32(Char.GetNumericValue(inputstring[i])) * Convert.ToInt32(Math.Pow(2, k--)); //k-- --k
            }
            return Convert.ToString(result);
        }

        public static string ConvertFrom8To10(string inputstring)
        {
            double result = 0;
            int k = inputstring.Length - 1;
            for (int i = 0; i < inputstring.Length; i++)
            {
                result += Convert.ToInt32(Char.GetNumericValue(inputstring[i])) * Math.Pow(8, k--);
            }
            return Convert.ToString(result);
        }

        public static string ConvertFrom16To10(string inputstring)
        {
            double result = 0;
            int k = inputstring.Length - 1;
            for (int i = 0; i < inputstring.Length; i++)
            {
                switch (inputstring[i])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        result += Convert.ToInt32(Char.GetNumericValue(inputstring[i])) * Math.Pow(16, k--);
                        break;
                    case 'A':
                        result += 10 * Math.Pow(16, k--);
                        break;
                    case 'B':
                        result += 11 * Math.Pow(16, k--);
                        break;
                    case 'C':
                        result += 12 * Math.Pow(16, k--);
                        break;
                    case 'D':
                        result += 13 * Math.Pow(16, k--);
                        break;
                    case 'E':
                        result += 14 * Math.Pow(16, k--);
                        break;
                    case 'F':
                        result += 15 * Math.Pow(16, k--);
                        break;
                }
            }
            return Convert.ToString(result);
        }

        public static string ConvertFrom10To2(string inputstring)
        {
            string result = "";
            int inputnumber = Convert.ToInt32(inputstring);

            while (inputnumber != 0 && inputnumber != 1)
            {
                result = Convert.ToString((inputnumber % 2)) + result;
                inputnumber = Convert.ToInt32(Math.Truncate(Convert.ToDouble(inputnumber / 2)));
            }

            result = inputnumber + result;

            while (result.Length % 4 !=0)
            {
                result = "0" + result;
            }
            return Convert.ToString(result);
        }

        public static string ConvertFrom10To8(string inputstring)
        {
            string result = "";
            int inputnumber = Convert.ToInt32(inputstring);

            while (Math.Abs(inputnumber) > 7)
            {
                result = Convert.ToString((inputnumber % 8)) + result;
                inputnumber = Convert.ToInt32(Math.Truncate(Convert.ToDouble(inputnumber / 8)));
            }

            return Convert.ToString(inputnumber + result);
        }

        public static string ConvertFrom10To16(string inputstring)
        {
            string result = "";
            int inputnumber = Convert.ToInt32(inputstring);

            while (Math.Abs(inputnumber) > 16)
            {
                switch (Convert.ToString((inputnumber % 16)))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        result = Convert.ToString((inputnumber % 16)) + result;
                        break;
                    case "10":
                        result = "A" + result;
                        break;
                    case "11":
                        result = "B" + result;
                        break;
                    case "12":
                        result = "C" + result;
                        break;
                    case "13":
                        result = "D" + result;
                        break;
                    case "14":
                        result = "E" + result;
                        break;
                    case "15":
                        result = "F" + result;
                        break;
                }
                inputnumber = Convert.ToInt32(Math.Truncate(Convert.ToDouble(inputnumber / 16)));
            }

            switch (Convert.ToString(inputnumber))
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    result = Convert.ToString(inputnumber) + result;
                    break;
                case "10":
                    result = "A" + result;
                    break;
                case "11":
                    result = "B" + result;
                    break;
                case "12":
                    result = "C" + result;
                    break;
                case "13":
                    result = "D" + result;
                    break;
                case "14":
                    result = "E" + result;
                    break;
                case "15":
                    result = "F" + result;
                    break;
            }
            return Convert.ToString(result);
        }

        // Дополнительно
        public static string ConvertFrom16To8(string inputstring)
        {
            return ConvertFrom10To8(ConvertFrom16To10(inputstring));
        }
        
        public static string ConvertFrom16To2(string inputstring)
        {
            return ConvertFrom10To2(ConvertFrom16To10(inputstring));
        }
        
        public static string ConvertFrom8To16(string inputstring)
        {
            return ConvertFrom10To16(ConvertFrom8To10(inputstring));
        }
        
        public static string ConvertFrom8To2(string inputstring)
        {
            return ConvertFrom10To2(ConvertFrom8To10(inputstring));
        }
        
        public static string ConvertFrom2To16(string inputstring)
        {
            return ConvertFrom10To16(ConvertFrom2To10(inputstring));
        }
        
        public static string ConvertFrom2To8(string inputstring)
        {
            return ConvertFrom10To8(ConvertFrom2To10(inputstring));
        }
        
        public static string ConvertNoSS(string inputstring)
        {
            return inputstring;
        }
        
        #endregion
    }
}
