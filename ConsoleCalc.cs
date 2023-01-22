using System.Globalization;
using System.Text.RegularExpressions;

namespace Hillel_homework_1
{
    public class ConsoleCalc
    {
        private IInputOutput _inputOutput;
        private INumberChecker _primeChecker;

        public ConsoleCalc(IInputOutput inputOutput, INumberChecker primeChecker)
        {
            _inputOutput = inputOutput;
            _primeChecker = primeChecker;
        }

        public void RunCalc()
        {
            _inputOutput.WriteOneLine($"One line calculator.{Environment.NewLine}" +
                $"Enter full math expresion using this allowed operation: \"+\", \"-\", \"*\", \"/\"." +
                $"{Environment.NewLine}You can also use parentheses: \"(\", \")\" and unary minus.");

            //Бесконечный цикл для повторных вычислений.
            while (true)
            {
                ConsoleCalcIO();
            }
        }

        public void ConsoleCalcIO()
        {
            try
            {
                var resultValue = ReadAndCalculate(_inputOutput.ReadOneLine());
                var resultString = $"Result: {resultValue}";
                //Проверка на простое число и добавление инфы, если была успешной
                if (_primeChecker.Check(resultValue))
                {
                    resultString += " - is prime number";
                }
                //Вывод результата вычислений(если оно было успешно) в консоль
                _inputOutput.WriteOneLine(resultString);
            }
            catch (Exception ex)
            {
                //Вывод сообщения об ошибке, полученной во время проверки правильности записи мат. выражения или его вычисления.
                _inputOutput.WriteOneLine($"Error: {ex.Message}");
            }
            finally
            {
                _inputOutput.WriteOneLine($"#################################{Environment.NewLine}Enter new expression.");
            }
        }

        public double ReadAndCalculate(string inputText)
        {
            //Удаление всех пробелов из входной строки.
            inputText = Regex.Replace(inputText, @"\s+", "");

            //Проверка наличия недопустимых символов во входной строке.
            if (new Regex(@"[^\+\-\*\/\.\(\)\s\d]").IsMatch(inputText))
            {
                throw new CalcInvalidInputException();
            }

            //Создание екземпляра класса калькулятора.
            CalculatorFormString shuntingYard = new CalculatorFormString();

            //Вызов метода вычисления .
            return shuntingYard.Compute(inputText);
        }
    }
}