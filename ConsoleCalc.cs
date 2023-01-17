using System.Globalization;
using System.Text.RegularExpressions;

namespace Hillel_homework_1
{
    public class ConsoleCalc
    {
        public void RunCalc()
        {
            Console.WriteLine($"One line calculator.{Environment.NewLine}" +
                $"Enter full math expresion using this allowed operation: \"+\", \"-\", \"*\", \"/\"." +
                $"{Environment.NewLine}You can also use parentheses: \"(\", \")\" and unary minus.");

            //Бесконечный цикл для повторных вычислений.
            while (true)
            {
                ConsoleIO();
            }
        }

        public void ConsoleIO()
        {
            try
            {
                var resultValue = ReadAndCalculate(Console.ReadLine());
                var resultString = $"Result: {resultValue}";
                //Проверка на простое число и добавление инфы, если была успешной
                if (PrimeChecker.PrimeCheck(resultValue))
                {
                    resultString += " - is prime number";
                }
                //Вывод результата вычислений(если оно было успешно) в консоль
                Console.WriteLine(resultString);
            }
            catch (Exception ex)
            {
                //Вывод сообщения об ошибке, полученной во время проверки правильности записи мат. выражения или его вычисления.
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"#################################{Environment.NewLine}Enter new expression.");
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