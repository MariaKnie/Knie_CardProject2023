using Knie_CardProject2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTSCG_TEST
{
    internal class CardTests
    {
        private Monstercard monster_Card_1 = new Monstercard();
        private Monstercard monster_Card_2 = new Monstercard();
        private SpellCard spell_Card_1 = new SpellCard();
        private SpellCard spell_Card_2 = new SpellCard();



        [SetUp]
        public void Setup() //variables
        {

            monster_Card_1.GenerateCard();
            monster_Card_2.GenerateCard();
            spell_Card_1.GenerateCard();
            spell_Card_2.GenerateCard();

        }


        [Test]
        public void Test_CardStats_Name()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Name = "Card";

            //Act
            string expectedValue = "Card";
            string actualValue = spell_Card_1.Name;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);

            Assert.Pass();
        }

        [Test]
        public void Test_CardStats_Damage()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15f;

            //Act
            float expectedValue = 15f;
            float actualValue = spell_Card_1.Damage;


            //Assert
            Assert.AreEqual(expectedValue, actualValue);


            Assert.Pass();
        }

        [Test]
        public void Test_CardStats_Element()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.ElementType = "Fire";

            //Act
            string expectedValue2 = "Fire";
            string actualValue2 = spell_Card_1.ElementType;

            //Assert
            Assert.AreEqual(expectedValue2, actualValue2);

            Assert.Pass();
        }
    }
}
