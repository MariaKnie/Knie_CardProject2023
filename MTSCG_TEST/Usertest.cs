using Knie_CardProject2023;
using Knie_CardProject2023.Logic;
using NUnit.Framework;
using System.Net.Http.Headers;

namespace MTSCG_TEST
{
    public class UserTests
    {
        private User max;
        private User tom;
      

        [SetUp]
        public void Setup() //variables
        {
             max = new User("Max", "Pw", 0, 0);
             tom = new User("Tom", "Pw", 0, 0);
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
            //Assert.That(actualValue, Is.EqualTo(expectedValue));
            Assert.Pass();
        }

        [Test]
        public void Test_IdUnsetable()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = max.Id ;
            //Act
            max.Id = 100;
            int actualValue = max.Id;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Wins()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = max.Wins +1;
            //Act
            max.Wins++ ;
            int actualValue = max.Wins;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Wins_CantBeSubtracted()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = max.Wins + 1;
            //Act
            max.Wins--;
            int actualValue = max.Wins;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Loses()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = max.Loses + 1;
            //Act
            max.Loses++;
            int actualValue = max.Loses;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Loses_CantBeSubtracted()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = max.Loses +1;
            //Act
            max.Loses--;
            int actualValue = max.Loses;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Coins_Plus()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = 100;
            //Act
            max.Coins = 100;
            int actualValue = max.Coins;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }
        [Test]
        public void Test_Coins_Minus()
        {
            // Pattern AAA
            // Arrange
            int expectedValue = 50;
            //Act
            max.Coins = 100;
            max.Coins -=50;
            int actualValue = max.Coins;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }
    }
}