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
            //Разбитие входной строки на масив подстрок (из операций, чисел, скобок) используя патерн регулярного выражения.
            var numbsAndOperationArray = Regex.Split(inputString, @"([\(\)\+\-\*\/])");

            var tokensList = Tokenize(numbsAndOperationArray);
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
    }
}