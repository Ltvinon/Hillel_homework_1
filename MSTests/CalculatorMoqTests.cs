using Hillel_homework_1;
using System.Globalization;
using Moq;
using System.Security.Cryptography.X509Certificates;

namespace MSTests
{
    [TestClass]
    public class CalculatorMoqTests
    {
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
        }

        [TestMethod]
        [DataRow("2+2", 4, false, "", "Result: 4")]
        [DataRow("999.999",999.999, false, "", "Result: 999.999")]
        [DataRow("773", 773, true, " - is prime number", "Result: 773 - is prime number")]
        public void Test_ConsoleCalcIO_Output(
            string inputText,
            double inputNumber, bool expecterCheckRezult, string expectedInfoString, 
            string expectedOutputRezult)
        {
            var mockIO = new Mock<IInputOutput>();
            var mockNCheck = new Mock<INumberChecker>();
            mockIO.Setup(x => x.ReadOneLine()).Returns(inputText);
            mockNCheck.Setup(x => x.Check(inputNumber, out expectedInfoString)).Returns(expecterCheckRezult);

            var consoleCalc = new ConsoleCalc(mockIO.Object, mockNCheck.Object);

            consoleCalc.ConsoleCalcIO();

            mockIO.Verify(x => x.WriteOneLine(expectedOutputRezult));
        }
    }
}
