using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Hillel_homework_1
{
    internal partial class Program
    {
        static void Main(string[] _)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator= ".",
                }
            };
            ConsoleCalc consoleCalc = new();
            consoleCalc.RunCalc();
        }
    }
}