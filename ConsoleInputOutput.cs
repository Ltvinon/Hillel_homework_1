namespace Hillel_homework_1
{
    public class ConsoleInputOutput : IInputOutput
    {
        public string ReadOneLine()
        {
            return Console.ReadLine();
        }

        public void WriteOneLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}