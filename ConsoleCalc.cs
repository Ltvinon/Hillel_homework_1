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
                //Строка содержащая мат. выражение для последующего вычисления.
                string rezult = ReadAndCalculate(Console.ReadLine());

                Console.WriteLine($"{rezult}");
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

        public string ReadAndCalculate(string inputText)
        {
            //Удаление всех пробелов из входной строки.
            inputText = Regex.Replace(inputText, @"\s+", "");

            //Проверка наличия недопустимых символов во входной строке.
            if (new Regex(@"[^\+\-\*\/\.\(\)\s\d]").IsMatch(inputText))
            {
                throw new CalcInvalidInputException();
            }

            return inputText;
        }
    }
}