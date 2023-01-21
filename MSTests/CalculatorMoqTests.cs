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
        [DataRow("2+2", "Result: 4")]
        [DataRow("999.999", "Result: 999.999")]
        [DataRow("773", "Result: 773 - is prime number")]
        public void Test_ConsoleCalcIO_Output(string inputText, string expectedRezult)
        {
            var mock = new Mock<IInputOutput>();
            mock.Setup(x => x.ReadOneLine()).Returns(inputText);
            var consoleCalc = new ConsoleCalc(mock.Object);

            consoleCalc.ConsoleCalcIO();

            mock.Verify(x => x.WriteOneLine(expectedRezult));
        }
    }
}
