using Hillel_homework_1;

namespace MSTests
{
    [TestClass]
    public class CalculatorMSTests
    {
        private static ConsoleCalc _consoleCalc;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
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
        [DataRow("2+2", "2+2")]
        [DataRow("2 + 2", "2+2")]
        [DataRow("    100   ", "100")]
        public void Test_ReadAndCalculate_Output(string inputText, string expectedOutpu)
        {
            string outputText = _consoleCalc.ReadAndCalculate(inputText);

            Assert.AreEqual(expectedOutpu, outputText);
        }

        [TestMethod]
        [DataRow("2+2", "2+2")]
        [DataRow("999.999", "999.999")]
        public void Test_ConsoleIO_IO(string inputText, string expectedRezult)
        {
            TextReader textIn = new StringReader(inputText);
            Console.SetIn(textIn);
            StringWriter stringWriter = new();
            Console.SetOut(stringWriter);

            _consoleCalc.ConsoleIO();

            var outputStrings = stringWriter.ToString().Split("\r\n");
            StringAssert.Equals(outputStrings[0], expectedRezult);
        }
    }
}