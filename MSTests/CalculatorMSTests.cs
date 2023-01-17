using Hillel_homework_1;
using System.Globalization;

namespace MSTests
{
    [TestClass]
    public class CalculatorMSTests
    {
        private static ConsoleCalc _consoleCalc;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator= ".",
                }
            };
            _consoleCalc = new ConsoleCalc();
        }

        [TestMethod]
        [DataRow("2+O")]
        [DataRow("#row")]
        [DataRow("two+one")]
        [ExpectedException(typeof(CalcInvalidInputException))]
        public void Test_ReadAndCalculate_UnwatedSymbolsExeption(string inputText)
        {
            _consoleCalc.ReadAndCalculate(inputText);
        }

        [TestMethod]
        [DataRow("2+2", 4)]
        [DataRow("2 + 2", 4)]
        [DataRow("    100   ", 100)]
        [DataRow("100/(5*(2-.4)+2)", 10)]
        public void Test_ReadAndCalculate_Output(string inputText, double expectedOutput)
        {
            double output = _consoleCalc.ReadAndCalculate(inputText);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        [DataRow("2+2", "Result: 4")]
        [DataRow("999.999", "Result: 999.999")]
        [DataRow("773", "Result: 773 - is prime number")]
        public void Test_ConsoleIO_IO(string inputText, string expectedRezult)
        {
            TextReader textIn = new StringReader(inputText);
            Console.SetIn(textIn);
            StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            _consoleCalc.ConsoleIO();

            //Локально тест проходит успешно. Github actions фейлится на нём :`(
            //var outputStrings = stringWriter.ToString().Split("\r\n");
            //Assert.AreEqual(outputStrings[0], expectedRezult);

            //Первая попытка - :`(
            //string result = stringWriter.ToString().Split("\r\n")[0];
            //Assert.AreEqual(result, expectedRezult);

            //Вторая попытка. Озарение. Под линуксом разрыв строки отличается от винды.
            string result = stringWriter.ToString();
            StringAssert.Contains(result, expectedRezult);
        }
    }
}