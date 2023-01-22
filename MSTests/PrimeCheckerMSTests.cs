using Hillel_homework_1;
using System.Globalization;

namespace MSTests

{
    [TestClass]
    public class PrimeCheckerMSTests
    {
        static PrimeChecker _primeChecker;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            _primeChecker = new PrimeChecker();
        }

        [TestMethod]
        [DataRow(1, false, "")]
        [DataRow(213, false, "")]
        [DataRow(-20, false, "")]
        [DataRow(0.7, false, "")]
        [DataRow(7, true, " - is prime number")]
        [DataRow(743, true, " - is prime number")]
        [DataRow(999.999, false, "")]
        public void Test_PrimeChecker_Output(double value, bool expectedResult, string expectedInfoString)
        {
            bool result = _primeChecker.Check(value, out string rezulInfoString);

            Assert.AreEqual(expectedResult, result);
            StringAssert.Equals(rezulInfoString, expectedInfoString);
        }
    }
}
