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
        [DataRow("2+2", "Result: 4", 4, false)]
        [DataRow("999.999", "Result: 999.999", 999.999, false)]
        [DataRow("773", "Result: 773 - is prime number", 773, true)]
        public void Test_ConsoleCalcIO_Output(
            string inputText, string expectedOutputRezult,
            double inputNumber, bool expecterCheckRezult)
        {
            var mockIO = new Mock<IInputOutput>();
            var mockNCheck = new Mock<INumberChecker>();
            mockIO.Setup(x => x.ReadOneLine()).Returns(inputText);
            mockNCheck.Setup(x => x.Check(inputNumber)).Returns(expecterCheckRezult);

            var consoleCalc = new ConsoleCalc(mockIO.Object, mockNCheck.Object);

            consoleCalc.ConsoleCalcIO();

            mockIO.Verify(x => x.WriteOneLine(expectedOutputRezult));
        }
    }
}
