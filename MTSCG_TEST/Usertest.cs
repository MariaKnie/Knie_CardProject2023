using Knie_CardProject2023;
using NUnit.Framework;

namespace MTSCG_TEST
{
    public class Tests
    {
        private User max;

        [SetUp]
        public void Setup() //variables
        {
             max = new User("Max", "Pw", 0, 0);
        }

        [Test]
        public void Test_UserName()
        {
            // Pattern AAA
            // Arrange
            string expectedValue = "Max";
            //Act
            string actualValue = max.Username;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
            Assert.Pass();
            //Assert.Fail();
        }
    }
}