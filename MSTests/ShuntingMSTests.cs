using Hillel_homework_1;
using System.Text.RegularExpressions;

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

        [TestMethod]
        [DataRow(new string[] { "(", "3", "+", ")", "+", "8" }, "(3+)+8")]
        [DataRow(new string[] { ".8", "*", "(", "3.68", "+", "0.86", ")" }, ".8*(3.68+0.86)")]
        public void Test_SplitInputString_Output(string[] expectedRezult, string inputText)
        {
            var rezult = _calcFromString.SplitInputString(inputText);
            CollectionAssert.AreEqual(expectedRezult, rezult);
        }

        [TestMethod]
        [DataRow("(.3+)+8")]
        [DataRow("(+3)+0.8")]
        [DataRow("3(.3+8)")]
        [DataRow("(3+0.6)(3+.8)")]
        [DataRow("3+()")]
        [ExpectedException(typeof(ParenthesisException))]
        public void Test_MathExpressionWritingCheck_ExpectedParenthesisException(string inputText)
        {
            var tokens = _calcFromString.Tokenize(_calcFromString.SplitInputString(inputText));

            _calcFromString.MathExpressionWritingCheck(tokens);
        }

        [TestMethod]
        [DataRow("-*3")]
        [DataRow("--3")]
        [DataRow("-+3")]
        [ExpectedException(typeof(OperationSignAtLineStartException))]
        public void Test_MathExpressionWritingCheck_ExpectedOperationSignAtLineStartException(string inputText)
        {
            var tokens = _calcFromString.Tokenize(_calcFromString.SplitInputString(inputText));

            _calcFromString.MathExpressionWritingCheck(tokens);
        }

        [TestMethod]
        [DataRow("3*3-")]
        [DataRow("3-3-")]
        [DataRow("3+3-")]
        [ExpectedException(typeof(OperationSignAtLineEndException))]
        public void Test_MathExpressionWritingCheck_ExpectedOperationSignAtLineEndException(string inputText)
        {
            var tokens = _calcFromString.Tokenize(_calcFromString.SplitInputString(inputText));

            _calcFromString.MathExpressionWritingCheck(tokens);
        }

        [TestMethod]
        [DataRow("3*--3")]
        [DataRow("3-+3")]
        [DataRow("3/*3")]
        [ExpectedException(typeof(SeveralOperationInARowException))]
        public void Test_MathExpressionWritingCheck_ExpectedSeveralOperationInARowException(string inputText)
        {
            var tokens = _calcFromString.Tokenize(_calcFromString.SplitInputString(inputText));

            _calcFromString.MathExpressionWritingCheck(tokens);
        }
    }
}
