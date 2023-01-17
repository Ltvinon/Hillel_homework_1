using Hillel_homework_1;

namespace MSTests

{
    [TestClass]
    public class PrimeCheckerMSTests
    {
        [TestMethod]
        [DataRow(1, false)]
        [DataRow(213, false)]
        [DataRow(-20, false)]
        [DataRow(0.7, false)]
        [DataRow(7, true)]
        [DataRow(743, true)]
        public void Test_PrimeChecker_Output(double value, bool expectedResult)
        {
            bool result = PrimeChecker.PrimeCheck(value);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
