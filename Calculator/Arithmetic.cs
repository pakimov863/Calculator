using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// Класс работы с обычными математическими выражениями.
    /// Доступны операции: +, -, *, /, ^, SQRT(), QBRT(), ()XQRT(), ASIN(), SINH(), SIN(), ACOS(), COSH(), COS(), ATG(), TGH(), TG(), ACTG(), CTGH(), CTG(), LN(), LG(), ()LOG(), EXP(), ABS(), !
    /// </summary>
    static class Arithmetic
    {
        #region Основные функции
        /// <summary>
        /// Проводит вычисления в автоматическом режиме
        /// </summary>
        /// <param name="expression">Выражение для подсчета</param>
        /// <param name="deg">Режим меры угла: deg=0, [rad=1], grad=2</param>
        /// <returns>Результат вычислений</returns>
        public static Double CalcToResult(string expression, byte deg=1)
        {
            List<Token> RPN = Arithmetic.GetRPN(expression);
            return Arithmetic.Calculate(ref RPN, deg);
        }

        /// <summary>
        /// Создание обратной польской записи для выражения
        /// </summary>
        /// <param name="expression">Математическое выражение</param>
        /// <returns>Список токенов в виде обратной польской записи</returns>
        public static List<Token> GetRPN(string expression)
        {
            MatchCollection collection = Regex.Matches(expression, @"\(|\)|(\d+(((\.|,)\d+|)+E(\+|-)\d+|(\.|,)\d+|))|\+|\-|_|\*|\/|\^|⋮|⋯|⋰|⋱|SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|\!|ABS|·");
            Regex variables = new Regex(@"\d+(((\.|,)\d+|)+E(\+|-)\d+|(\.|,)\d+|)");
            //(\-?\d+(\.\d{0,})?)
            Regex operations = new Regex(@"\+|\-|_|\*|\/|\^|⋮|⋯|⋰|⋱|SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|\!|ABS|·");
            Regex brackets = new Regex(@"\(|\)");
            //string[] priority = { "SQRT", "QBRT", "XQRT", "ASIN", "SINH", "SIN", "ACOS", "COSH", "COS", "ATG", "TGH", "TG", "ACTG", "CTGH", "CTG", "LN", "LG", "LOG", "EXP", "_", "!", "^", "⋰", "⋱", "⋯", "⋮", "*", "/", "-", "+" };//⋮=+%, ⋯=-%, ⋰=*%, ⋱=/%

            Stack<string> stack = new Stack<string>();
            List<Token> list = new List<Token>();
            bool operleft = true;

            foreach (Match match in collection)
            {
                Match temp = variables.Match(match.Value);
                if (temp.Success)
                {
                    operleft = false;
                    list.Add(new Token(temp.Value, TokenType.Variable));
                    continue;
                }

                temp = brackets.Match(match.Value);
                if (temp.Success)
                {
                    //stack isEmpty() ?
                    operleft = true;
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
                    string tempValue = temp.Value;
                    if (operleft && tempValue == "-") { list.Add(new Token("-1", TokenType.Variable)); tempValue = "·"; }
                    operleft = true;

                    if (stack.Count != 0)
                    {
                        //while (Array.IndexOf(priority, tempValue) >= Array.IndexOf(priority, stack.Peek()))
                        while (GetPriority(tempValue) >= GetPriority(stack.Peek()))
                        {
                            if (stack.Peek() == "(") break;
                            list.Add(new Token(stack.Pop(), TokenType.Operation));
                            if (stack.Count == 0) break;
                        }
                    }
                    stack.Push(tempValue);
                }
            }

            while (stack.Count != 0)
            {
                //проверка | new ?
                list.Add(new Token(stack.Pop(), TokenType.Operation));
            }

            return list;
        }

        public static Double CalcToResult(string expression, Dictionary<string, string> uservars, byte deg = 1)
        {
            List<Token> RPN = Arithmetic.GetRPN(expression, uservars);
            return Arithmetic.Calculate(ref RPN, deg);
        }

        private static int CompareStringsByLength(string x, string y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                else return -1;
            }
            else
            {
                if (y == null) return 1;
                else
                {
                    int retval = x.Length.CompareTo(y.Length);

                    if (retval != 0) return -retval;
                    else return x.CompareTo(y);
                }
            }
        }

        public static List<Token> GetRPN(string expression, Dictionary<string, string> uservars)
        {
            List<string> tmp = new List<string>();
            foreach (string item in uservars.Keys) tmp.Add(item);
            tmp.Sort(CompareStringsByLength);
            string regex_collection = @"\(|\)|(\d+(((\.|,)\d+|)+E(\+|-)\d+|(\.|,)\d+|))|\+|\-|_|\*|\/|\^|⋮|⋯|⋰|⋱|SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|\!|ABS|·";
            string regex_variables = @"(\d+(((\.|,)\d+|)+E(\+|-)\d+|(\.|,)\d+|))";
            foreach (string item in tmp)
            {
                regex_collection += "|" + item.Trim().ToUpper();
                regex_variables += "|" + item.Trim().ToUpper();
            }

            MatchCollection collection = Regex.Matches(expression, regex_collection);
            Regex variables = new Regex(regex_variables);
            Regex operations = new Regex(@"\+|\-|_|\*|\/|\^|⋮|⋯|⋰|⋱|SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|\!|ABS|·");
            Regex brackets = new Regex(@"\(|\)");

            Stack<string> stack = new Stack<string>();
            List<Token> list = new List<Token>();
            bool operleft = true;

            foreach (Match match in collection)
            {
                Match temp = variables.Match(match.Value);
                if (temp.Success)
                {
                    operleft = false;
                    double res;
                    if (Double.TryParse(temp.Value, out res))
                        list.Add(new Token(temp.Value, TokenType.Variable));
                    else
                        if (uservars.ContainsKey(temp.Value)) list.Add(new Token(uservars[temp.Value], TokenType.Variable));
                        else return null;
                    continue;
                }

                temp = brackets.Match(match.Value);
                if (temp.Success)
                {
                    //stack isEmpty() ?
                    operleft = true;
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
                    string tempValue = temp.Value;
                    if (operleft && tempValue == "-") { list.Add(new Token("-1", TokenType.Variable)); tempValue = "·"; }
                    operleft = true;

                    if (stack.Count != 0)
                    {
                        //while (Array.IndexOf(priority, tempValue) >= Array.IndexOf(priority, stack.Peek()))
                        while (GetPriority(tempValue) >= GetPriority(stack.Peek()))
                        {
                            if (stack.Peek() == "(") break;
                            list.Add(new Token(stack.Pop(), TokenType.Operation));
                            if (stack.Count == 0) break;
                        }
                    }
                    stack.Push(tempValue);
                }
            }

            while (stack.Count != 0)
            {
                //проверка | new ?
                list.Add(new Token(stack.Pop(), TokenType.Operation));
            }

            return list;
        }

        /// <summary>
        /// Вычисление выражения по обратной польской записи
        /// </summary>
        /// <param name="rpn">Польская запись выражения</param>
        /// <param name="deg">Режим меры угла: deg=0, [rad=1], grad=2</param>
        /// <returns>Результат вычислений</returns>
        public static Double Calculate(ref List<Token> rpn, byte deg)
        {
            if (rpn == null) return Double.NaN;
            
            Stack<Double> result = new Stack<Double>();
            foreach (Token token in rpn)
            {
                if (token.Type == TokenType.Variable) result.Push(Convert.ToDouble(token.Value));
                if (token.Type == TokenType.Operation)
                {
                    Double a, b, res_tmp;
                    if (token.Value == "SIN" || token.Value == "COS" || token.Value == "TG" || token.Value == "CTG")
                    {
                        a = result.Pop();
                        switch (deg) //from: deg=0, rad=1, grad=2
                        {
                            case 0:
                                a = a * Math.PI / 180;
                                break;
                            case 2:
                                a = a * 0.015708;
                                break;
                        }
                        result.Push(a);
                    }
                    switch (token.Value)
                    {
                        case "!":
                            a = result.Pop();
                            result.Push(Factorial(a));
                            break;
                        case "SQRT":
                            a = result.Pop();
                            result.Push(Math.Sqrt(a));
                            break;
                        case "QBRT":
                            a = result.Pop();
                            result.Push(Math.Pow(a, 1 / 3));
                            break;
                        case "XQRT":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(Math.Pow(a, 1 / b));
                            break;
                        case "ABS":
                            a = result.Pop();
                            result.Push(Math.Abs(a));
                            break;
                        case "ASIN":
                            a = result.Pop();
                            res_tmp = Math.Asin(a);
                            switch (deg) //to: deg=0, rad=1, grad=2
                            {
                                case 0:
                                    res_tmp = res_tmp / (Math.PI / 180);
                                    break;
                                case 2:
                                    res_tmp = res_tmp / 0.015708;
                                    break;
                            }
                            result.Push(res_tmp);
                            break;
                        case "SINH":
                            a = result.Pop();
                            result.Push(Math.Sinh(a));
                            break;
                        case "SIN":
                            a = result.Pop();
                            result.Push(Math.Sin(a));
                            break;
                        case "ACOS":
                            a = result.Pop();
                            res_tmp = Math.Acos(a);
                            switch (deg) //to: deg=0, rad=1, grad=2
                            {
                                case 0:
                                    res_tmp = res_tmp / (Math.PI / 180);
                                    break;
                                case 2:
                                    res_tmp = res_tmp / 0.015708;
                                    break;
                            }
                            result.Push(res_tmp);
                            break;
                        case "COSH":
                            a = result.Pop();
                            result.Push(Math.Cosh(a));
                            break;
                        case "COS":
                            a = result.Pop();
                            result.Push(Math.Cos(a));
                            break;
                        case "ATG":
                            a = result.Pop();
                            res_tmp = Math.Atan(a);
                            switch (deg) //to: deg=0, rad=1, grad=2
                            {
                                case 0:
                                    res_tmp = res_tmp / (Math.PI / 180);
                                    break;
                                case 2:
                                    res_tmp = res_tmp / 0.015708;
                                    break;
                            }
                            result.Push(res_tmp);
                            break;
                        case "TGH":
                            a = result.Pop();
                            result.Push(Math.Tanh(a));
                            break;
                        case "TG":
                            a = result.Pop();
                            result.Push(Math.Tan(a));
                            break;
                        case "ACTG":
                            a = result.Pop();
                            res_tmp = Math.Pow(Math.Atan(a), -1);
                            switch (deg) //to: deg=0, rad=1, grad=2
                            {
                                case 0:
                                    res_tmp = res_tmp / (Math.PI / 180);
                                    break;
                                case 2:
                                    res_tmp = res_tmp / 0.015708;
                                    break;
                            }
                            result.Push(res_tmp);
                            break;
                        case "CTGH":
                            a = result.Pop();
                            result.Push(Math.Pow(Math.Tanh(a), -1));
                            break;
                        case "CTG":
                            a = result.Pop();
                            result.Push(Math.Pow(Math.Tan(a), -1));
                            break;
                        case "LN":
                            a = result.Pop();
                            result.Push(Math.Log(a, Math.E));
                            break;
                        case "LG":
                            a = result.Pop();
                            result.Push(Math.Log10(a));
                            break;
                        case "LOG":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(Math.Log10(a) / Math.Log10(b));
                            break;
                        case "EXP":
                            a = result.Pop();
                            result.Push(Math.Exp(a));
                            break;
                        case "+":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b + a);
                            break;
                        case "-":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b - a);
                            break;
                        case "_":
                            a = result.Pop();
                            result.Push(- a);
                            break;
                        case "*":
                        case "·":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b * a);
                            break;
                        case "/":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b / a);
                            break;
                        case "^":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(Math.Pow(b, a));
                            break;
                        //⋮=+%, ⋯=-%, ⋰=*%, ⋱=/%
                        case "⋮":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b + b * (a / 100));
                            break;
                        case "⋯":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b - b * (a / 100));
                            break;
                        case "⋰":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(b * (a / 100));
                            break;
                        case "⋱":
                            a = result.Pop();
                            b = result.Pop();
                            result.Push(100 * (b / a));
                            break;
                    }
                }
            }
            return result.Pop();
        }

        #endregion

        #region Вспомогательные функции
        /// <summary>
        /// Вычисление факториала числа. Любого числа. Даже с точкой. ОwО
        /// </summary>
        /// <param name="number">Число для вычислений</param>
        /// <returns>Результат</returns>
        public static Double Factorial(Double number)
        {
            double x = number + 1;
            double tmp = (x - 0.5) * Math.Log(x + 4.5) - (x + 4.5);
            double ser = 1.0 + 76.18009173 / (x + 0) - 86.50532033 / (x + 1)
                             + 24.01409822 / (x + 2) - 1.231739516 / (x + 3)
                             + 0.00120858003 / (x + 4) - 0.00000536382 / (x + 5);
            return Math.Exp(tmp + Math.Log(ser * Math.Sqrt(2 * Math.PI)));
        }

        /// <summary>
        /// Возвращает приоритет операции
        /// </summary>
        /// <param name="value">Символ операции</param>
        /// <returns>Число-приоритет</returns>
        private static int GetPriority(string value)
        {
            Regex[] mainpriority = {
                                    new Regex(@"SQRT|QBRT|XQRT|ASIN|SINH|SIN|ACOS|COSH|COS|ATG|TGH|TG|ACTG|CTGH|CTG|LN|LG|LOG|EXP|ABS"),
                                    new Regex(@"_"),
                                    new Regex(@"·"),
                                    new Regex(@"\^|\!"),
                                    new Regex(@"⋮|⋯|⋰|⋱"),
                                    new Regex(@"\*|\/"),
                                    new Regex(@"\+|\-")
                               };
            for (int i = 0; i < mainpriority.Length - 1; i++)
                if (mainpriority[i].Match(value).Success) return i;
            return mainpriority.Length + 1;
        }

        #endregion
    }
}