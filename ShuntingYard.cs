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
            {"/", new Operation(3, OperationName.Division, (x, y) => { if (y == 0) throw new Exception("Division by zero."); return x / y; } ) },
        };

        public CalculatorFormString()
        {
        }

        /// <summary>
        /// Парсинг математического выражения и вычисление результата.
        /// </summary>
        /// <param name="inputString">Математическое выражение в формате строки.</param>
        /// <returns>Результат вычислений.</returns>
        public double Compute(string inputString)
        {
            //Стак временного хранения токенов операторов.
            Stack<Token> operatorsStack = new Stack<Token>();
            //Выходной стак токенов в формате обратной польской записи.
            Stack<Token> outputStack = new Stack<Token>();

            //Разбитие входной строки на масив подстрок (из операций, чисел, скобок) используя патерн регулярного выражения.
            var numbsAndOperationArray = Regex.Split(inputString, @"([\(\)\+\-\*\/])");

            var tokensList = Tokenize(numbsAndOperationArray);
            MathExpressionWritingCheck(tokensList);
            tokensList = UnaryMinusImplementation(tokensList);

            //Shunting Yard алгоритм.
            //https://en.wikipedia.org/wiki/Shunting_yard_algorithm
            foreach (var token in tokensList)
            {
                if (token.GetType() == typeof(Number))
                {
                    outputStack.Push(token);
                }
                else if (token.GetType() == typeof(Operation))
                {
                    while (operatorsStack.TryPeek(out Token lastOperatorToken)
                        && (lastOperatorToken is not Parenthesis //lastOperatorToken.GetType() != typeof(Parenthesis)
                        ||
                        (lastOperatorToken is Parenthesis parenthesis//lastOperatorToken.GetType() == typeof(Parenthesis)
                        && parenthesis.Orientation != ParenthesisOrientation.Left))
                        && lastOperatorToken.Precedence >= token.Precedence)
                    {
                        outputStack.Push(operatorsStack.Pop());
                    }
                    operatorsStack.Push(token);
                }
                else if (token is Parenthesis parenthesis1 && parenthesis1.Orientation == ParenthesisOrientation.Left)
                {
                    operatorsStack.Push(token);
                }
                else if (token is Parenthesis parenthesis2 && parenthesis2.Orientation == ParenthesisOrientation.Right)
                {
                    while (operatorsStack.TryPeek(out Token lastOperatorToken)
                        && (lastOperatorToken is not Parenthesis //lastOperatorToken.GetType() != typeof(Parenthesis)
                        ||
                        (lastOperatorToken is Parenthesis parenthesis3//lastOperatorToken.GetType() == typeof(Parenthesis)
                        && parenthesis3.Orientation != ParenthesisOrientation.Left)))//((Parenthesis)lastOperatorToken).Orientation != ParenthesisOrientation.Left)))
                    {
                        outputStack.Push(operatorsStack.Pop());
                    }
                    if (operatorsStack.Count == 0)
                    {
                        throw new Exception("Mismatched parentheses.");
                    }
                    else if (operatorsStack.Peek() is Parenthesis parenthesis4
                        && parenthesis4.Orientation == ParenthesisOrientation.Left)
                    {
                        _ = operatorsStack.Pop();
                    }
                }
            }
            //Переносим все оставшиеся операции в основной стак.
            while (operatorsStack.Count > 0)
            {
                if (operatorsStack.Peek().GetType() == typeof(Parenthesis))
                {
                    throw new Exception("Mismatched parentheses.");
                }
                outputStack.Push(operatorsStack.Pop());
            }

            //Reverse Polish notation. Массив полученый после выполнения алгоритма Shunting yard.
            //Представлет из себя математическое выражение, разбитое на элементы списка, в котором операнды расположены перед знаками операций.
            var rpn = outputStack.Reverse().ToList();

            //Цикл вычисления конечного результата. Происходит по приципу нахождения в массиве знака операции
            //и выполнение соответсвущего ему вычислений над двумя предшедствующими значениями.
            //Оставшийся элемент в списке является искомым результатом вычислений.
            while (rpn.Count != 1)
            {
                int index = rpn.FindIndex(x => x is Operation);
                ((Number)rpn[index - 1]).Value = ((Operation)rpn[index]).Compute(((Number)rpn[index - 2]).Value, ((Number)rpn[index - 1]).Value);
                rpn.RemoveAt(index);
                rpn.RemoveAt(index - 2);
            }
            return ((Number)rpn[0]).Value;
        }

        /// <summary>
        /// Парсинг массива операций, чисел и других елементов мат. выражения в соответствующие им токены.
        /// </summary>
        /// <param name="numbsAndOperationArray">Массив елементов мат. выражения (числа, операциий и т.д.) в строчном формате.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private List<Token> Tokenize(string[] numbsAndOperationArray)
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
                    throw new Exception("Invalid input character.");
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
        private void MathExpressionWritingCheck(List<Token> tokensList)
        {
            //Знак операции в начале строки (кроме унарного минуса).
            if (tokensList[0] is Operation operation
                && (operation.Name != OperationName.Substraction
                ||
                //Особый вариант проверки для унарного минуса.
                (tokensList[1] is Operation)))
            {
                throw new Exception("Operation sign at the start of the line.");
            }

            //Знак операции в конце строки.
            else if (tokensList[tokensList.Count - 1] is Operation)
            {
                throw new Exception("Operation sign at the end of the line.");
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
                        throw new Exception("Several operation signs in a row");
                    }

                    //Знак операции сразу после открывающей скобки (кроме унарного минуса).
                    else if (tokensList[i - 1] is Parenthesis parenthesis1
                        && parenthesis1.Orientation == ParenthesisOrientation.Left
                        && token.Name != OperationName.Substraction)
                    {
                        throw new Exception("Operation sign right after opening parenthesis.");
                    }

                    //Знак операции сразу перед закрывающей скобкой.
                    else if (i != tokensList.Count - 1
                        && tokensList[i + 1] is Parenthesis parenthesis2
                        && parenthesis2.Orientation == ParenthesisOrientation.Right)
                    {
                        throw new Exception("Operation sign right before closing parenthesis.");
                    }
                }
                else if (tokensList[i] is Parenthesis parenthesis)
                {
                    if (parenthesis.Orientation == ParenthesisOrientation.Left)
                    {
                        //Отсутствует знак операции сразу перед открывающей скобкой.
                        if (tokensList[i - 1] is Number)
                        {
                            throw new Exception("No operation sign before opening parenthesis.");
                        }

                        //Отсутствует знак операции между закрывающей и открывающей скобкой.
                        else if (tokensList[i - 1] is Parenthesis parenthesis2
                            && parenthesis2.Orientation == ParenthesisOrientation.Right)
                        {
                            throw new Exception("No operation sign between closing and opening parentheses or mismatched parentheses.");
                        }
                    }

                    //Пустые скобки
                    else if (parenthesis.Orientation == ParenthesisOrientation.Right
                        && tokensList?[i - 1] is Parenthesis parenthesis3
                        && parenthesis3.Orientation == ParenthesisOrientation.Left)
                    {
                        throw new Exception("Empty parentheses.");
                    }
                }
            }
        }

        /// <summary>
        /// Внедрение унарного минуса.
        /// </summary>
        /// <param name="tokensList">Список токенов операций, чисел и др.</param>
        /// <returns>Возвращает модифицированный список с отрицательными (если был найден унарный минус) числами.</returns>
        private List<Token> UnaryMinusImplementation(List<Token> tokensList)
        {
            List<int> indexesOfMinus = new List<int>();

            //Ищем индексы всех вхождений знака "минус".
            for (int i = 0; i < tokensList.Count; i++)
            {
                var token = tokensList[i];
                if (token.GetType() == typeof(Operation) 
                    && ((Operation)token).Name == OperationName.Substraction)
                {
                    indexesOfMinus.Add(i);
                }
            }
            //Проверка на то, что знак "минус" является унарным.
            foreach (int index in indexesOfMinus.ToList())
            {
                //В случае подтверждения условий "унарности", меняем полярность значения.
                if (index == 0 && tokensList[index + 1].GetType() == typeof(Number))
                {
                    ((Number)tokensList[index + 1]).Value *= -1;
                }
                else if (index > 0
                    && tokensList[index - 1].GetType() != typeof(Number)
                    && tokensList[index + 1].GetType() == typeof(Number))
                {
                    ((Number)tokensList[index + 1]).Value *= -1;
                }
                //Удаляем из списка индексы не унарных минусов.
                else
                {
                    indexesOfMinus.Remove(index);
                }
            }
            //Инвертируем список индексов, чтобы при удалении элементов из списка токенов по индексу избежать сдвигов.
            indexesOfMinus.Reverse();
            //Удаляем из списка токенов все вхождения унарных минусов.
            foreach (int index in indexesOfMinus)
            {
                tokensList.RemoveAt(index);
            }
            return tokensList;
        }
    }


}
