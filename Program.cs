using System.Text.RegularExpressions;

namespace Hillel_homework_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"One line calculator.{Environment.NewLine}" +
                $"Enter full math expresion using this allowed operation: \"+\", \"-\", \"*\", \"/\"." +
                $"{Environment.NewLine}You can also use parentheses: \"(\", \")\" and unary minus.");
            
            //Бесконечный цикл для повторных вычислений.
            while (true)
            {
                try
                {
                    //Строка содержащая мат. выражение для последующего вычисления.
                    string inputText = Console.ReadLine();

                    //Удаление всех пробелов из входной строки.
                    inputText = Regex.Replace(inputText, @"\s+", "");

                    //Проверка наличия недопустимых символов во входной строке.
                    if (new Regex(@"[^\+\-\*\/\.\(\)\s\d]").IsMatch(inputText))
                    {
                        throw new Exception("Invalid characters in input string");
                    }

                    //Создание екземпляра класса калькулятора.
                    CalculatorFormString shuntingYard = new CalculatorFormString();

                    //Вызов метода вычисления и вывод результата вычислений (если оно было успешно) в консоль.
                    Console.WriteLine($"Result: {shuntingYard.Compute(inputText)}");
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
        }
    }
}