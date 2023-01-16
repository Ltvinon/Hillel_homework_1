using Hillel_homework_1;

namespace MSTests
{
    [TestClass]
    public class ShuntingMSTests
    {
        public static CalculatorFormString _calcFromString;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            _calcFromString = new CalculatorFormString();
        }

        [TestMethod]
        [DataRow(")", ParenthesisOrientation.Right)]
        [DataRow("(", ParenthesisOrientation.Left)]
        public void Test_Tokenize_Parenthesis(string inputText, ParenthesisOrientation orientation)
        {
            var rezult = _calcFromString.Tokenize(new string[] { inputText });

            Assert.IsTrue(rezult[0] is Parenthesis parenthesis && parenthesis.Orientation == orientation);
        }

        [TestMethod]
        [DataRow("+", OperationName.Addition)]
        [DataRow("-", OperationName.Substraction)]
        [DataRow("/", OperationName.Division)]
        [DataRow("*", OperationName.Multiply)]
        public void Test_Tokenize_Operations(string inputText, OperationName opName)
        {
            var rezult = _calcFromString.Tokenize(new string[] { inputText });

            Assert.IsTrue(rezult[0] is Operation operation && operation.Name == opName);
        }

        [TestMethod]
        [DataRow("68", 68)]
        [DataRow("0.68", 0.68)]
        [DataRow(".68", 0.68)]
        public void Test_Tokenize_Number(string inputText, double expectedValue)
        {
            var rezult = _calcFromString.Tokenize(new string[] { inputText });

            Assert.IsTrue(rezult[0] is Number number && number.Value == expectedValue);
        }
    }
}
