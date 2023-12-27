using Knie_CardProject2023.Logic;
using Knie_CardProject2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTSCG_TEST
{
    internal class BattteLogicTests
    {
        private User max;
        private User tom;
        private Monstercard monster_Card_1 = new Monstercard();
        private Monstercard monster_Card_2 = new Monstercard();
        private SpellCard spell_Card_1 = new SpellCard();
        private SpellCard spell_Card_2 = new SpellCard();

        [SetUp]
        public void Setup() //variables
        {
            max = new User("Max", "Pw", 0, 0);
            tom = new User("Tom", "Pw", 0, 0);
            monster_Card_1.GenerateCard();
            monster_Card_2.GenerateCard();
            spell_Card_1.GenerateCard();
            spell_Card_2.GenerateCard();

        }

        [Test]
        public void Test_CardDamageComparison_Spell_First()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 0;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_CardDamageComparison_Spell_Second()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 15;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_CardDamageComparison_Spell_Draw()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = -1;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_SpellCards_GoIntoWinnerDeck()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = max.Deck.Cards.Count+1;
            Game.CompareCards(players, 0);
            int actualValue = max.Deck.Cards.Count;

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_SpellCards_LeaveLoserDeck()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = tom.Deck.Cards.Count -1;
            Game.CompareCards(players, 0);
            int actualValue = tom.Deck.Cards.Count;

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        //######################## Monster Cards

        [Test]
        public void Test_CardDamageComparison_Monster_First()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = "Fire";
            monster_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 0;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_CardDamageComparison_Monster_Second()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 10;
            monster_Card_2.Damage = 15;
            monster_Card_1.ElementType = "Fire";
            monster_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_CardDamageComparison_Monster_Draw()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 10;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = "Fire";
            monster_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = -1;
            int actualValue = Game.CompareCards(players, 0);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_MonsterCards_GoIntoWinnerDeck()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = "Fire";
            monster_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = max.Deck.Cards.Count+1;
            Game.CompareCards(players, 0);
            int actualValue = max.Deck.Cards.Count;


            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_MonsterCards_LeaveLosersDeck()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = "Fire";
            monster_Card_2.ElementType = "Fire";

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = tom.Deck.Cards.Count - 1;
            Game.CompareCards(players, 0);
            int actualValue = tom.Deck.Cards.Count;


            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
            //Assert.Fail();
        }
    }
}
