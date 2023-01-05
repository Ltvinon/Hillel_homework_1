using System.Globalization;
using System.Text.RegularExpressions;

namespace Hillel_homework_1
{
    /// <summary>
    /// Калькулятор мат выражений, записаных в формате строки.
    /// </summary>
    public class CalculatorFormString
    {
        //Словарь с основными арифметичекими операциями и назначение делагатам вычисления соответсвующего метода.
        private Dictionary<string, Operation> _operatorsDictionary = new Dictionary<string, Operation>
        {
            {"+", new Operation(2, OperationName.Addition, (x, y) => { return x + y; } ) },
            {"-", new Operation(2, OperationName.Substraction, (x, y) => { return x - y; } ) },
            {"*", new Operation(3, OperationName.Multiply, (x, y) => { return x * y; } ) },
            {"/", new Operation(3, OperationName.Division, (x, y) => { if (y == 0) throw new DivideByZeroException("Division by zero."); return x / y; } ) },
        };

        public CalculatorFormString()
        {
        }

        public void TestCompute(string inputString)
        {
            var tokensList = Tokenize(SplitInputString(inputString));
            MathExpressionWritingCheck(tokensList);
        }

        /// <summary>
        ///Разбитие входной строки на масив подстрок (из операций, чисел, скобок) используя патерн регулярного выражения.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>Масив подстрок (из операций, чисел, скобок)</returns>
        public string[] SplitInputString (string inputString)
        {
            return Regex.Split(inputString, @"([\(\)\+\-\*\/])").Where(x => x != String.Empty).ToArray();
        }

        /// <summary>
        /// Парсинг массива операций, чисел и других елементов мат. выражения в соответствующие им токены.
        /// </summary>
        /// <param name="numbsAndOperationArray">Массив елементов мат. выражения (числа, операциий и т.д.) в строчном формате.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Token> Tokenize(string[] numbsAndOperationArray)
        {
            List<Token> tokens = new List<Token>();
            foreach (string futureToken in numbsAndOperationArray)
            {
                if (futureToken == "(")
                {
                    tokens.Add(new Parenthesis(ParenthesisOrientation.Left));
                }
                else if (futureToken == ")")
                {
                    tokens.Add(new Parenthesis(ParenthesisOrientation.Right));
                }
                else if (_operatorsDictionary.TryGetValue(futureToken, out Operation operation))
                {
                    tokens.Add(operation);
                }
                else if (double.TryParse(futureToken, CultureInfo.InvariantCulture, out double numb))
                {
                    tokens.Add(new Number(numb));
                }
                else if (futureToken == "")
                {
                    //Появляется между двух одинаковых операций. Игнорируется.
                }
                else
                {
                    throw new CalcInvalidInputException();
                }
            }
            return tokens;
        }

        /// <summary>
        /// Проверка на ошибки в записи математического выражения 
        /// (кроме тех, которые проверяются в ходе выполнения Shunting yard алгоритма).
        /// </summary>
        /// <param name="tokensList">Список токенов операций, чисел и др.</param>
        /// <exception cref="Exception"></exception>
        public void MathExpressionWritingCheck(List<Token> tokensList)
        {
            //Знак операции в начале строки (кроме унарного минуса).
            if (tokensList[0] is Operation operation
                && (operation.Name != OperationName.Substraction
                ||
                //Особый вариант проверки для унарного минуса.
                (tokensList[1] is Operation)))
            {
                throw new OperationSignAtLineStartException();
            }

            //Знак операции в конце строки.
            else if (tokensList[tokensList.Count - 1] is Operation)
            {
                throw new OperationSignAtLineEndException();
            }

            for (int i = 1; i < tokensList.Count; i++)
            {
                //Token token = tokensList[i];
                if (tokensList[i] is Operation token)
                {
                    //Несколько знаков операций подряд (кроме унарного минуса).
                    if ((i != 0
                        && tokensList[i - 1] is Operation
                        && token.Name != OperationName.Substraction)
                        ||
                        //Особый вариант проверки для унарного минуса.
                        (i > 1
                        && token.Name == OperationName.Substraction
                        && tokensList[i - 1] is Operation
                        && tokensList[i - 2] is Operation))
                    {
                        throw new SeveralOperationInARowException();
                    }

                    //Знак операции сразу после открывающей скобки (кроме унарного минуса).
                    else if (tokensList[i - 1] is Parenthesis parenthesis1
                        && parenthesis1.Orientation == ParenthesisOrientation.Left
                        && token.Name != OperationName.Substraction)
                    {
                        throw new ParenthesisException("Operation sign right after opening parenthesis.");
                    }

                    //Знак операции сразу перед закрывающей скобкой.
                    else if (i != tokensList.Count - 1
                        && tokensList[i + 1] is Parenthesis parenthesis2
                        && parenthesis2.Orientation == ParenthesisOrientation.Right)
                    {
                        throw new ParenthesisException("Operation sign right before closing parenthesis.");
                    }
                }
                else if (tokensList[i] is Parenthesis parenthesis)
                {
                    if (parenthesis.Orientation == ParenthesisOrientation.Left)
                    {
                        //Отсутствует знак операции сразу перед открывающей скобкой.
                        if (tokensList[i - 1] is Number)
                        {
                            throw new ParenthesisException("No operation sign before opening parenthesis.");
                        }

                        //Отсутствует знак операции между закрывающей и открывающей скобкой.
                        else if (tokensList[i - 1] is Parenthesis parenthesis2
                            && parenthesis2.Orientation == ParenthesisOrientation.Right)
                        {
                            throw new ParenthesisException("No operation sign between closing and opening parentheses or mismatched parentheses.");
                        }
                    }

                    //Пустые скобки
                    else if (parenthesis.Orientation == ParenthesisOrientation.Right
                        && tokensList?[i - 1] is Parenthesis parenthesis3
                        && parenthesis3.Orientation == ParenthesisOrientation.Left)
                    {
                        throw new ParenthesisException("Empty parentheses.");
                    }
                }
            }
        }
    }
}