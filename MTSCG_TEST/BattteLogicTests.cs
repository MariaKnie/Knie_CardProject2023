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

            monster_Card_1.Name = "-";
            monster_Card_2.Name = "-";
            monster_Card_1.ElementType = "-";
            monster_Card_2.ElementType = "-";
            monster_Card_1.CardType = "-";
            monster_Card_2.CardType = "-";
            monster_Card_1.Damage = 0;
            monster_Card_2.Damage = 0;

            spell_Card_1.Name = "-";
            spell_Card_2.Name = "-";
            spell_Card_1.ElementType = "-";
            spell_Card_2.ElementType = "-";
            spell_Card_1.CardType = "-";
            spell_Card_2.CardType = "-";
            spell_Card_1.Damage = 0;
            spell_Card_2.Damage = 0;

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



        [Test]
        public void Test_Spell_FireWater()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = "Fire";
            spell_Card_2.ElementType = "Water";
            spell_Card_1.CardType = "Spell";
            spell_Card_2.CardType = "Spell";

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
        public void Test_Spell_FireWater_draw()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = "Water";
            spell_Card_2.ElementType = "Fire";
            spell_Card_1.CardType = "Spell";
            spell_Card_2.CardType = "Spell";

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
        public void Test_Spell_FireWater_water_lose()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 26;
            spell_Card_1.ElementType = "Water";
            spell_Card_2.ElementType = "Fire";
            spell_Card_1.CardType = "Spell";
            spell_Card_2.CardType = "Spell";

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





 // ################## effect calculation
        [Test]
        public void Test_Spell_effect_calculation()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = "Water";
            spell_Card_2.ElementType = "Fire";
            spell_Card_1.CardType = "Spell";
            spell_Card_2.CardType = "Spell";
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 2f;
            float expectedValue2 = 0.5f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_Spell_effect_calculation_noEEffect()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = "Water";
            spell_Card_2.ElementType = "Water";
            spell_Card_1.CardType = "Spell";
            spell_Card_2.CardType = "Spell";
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 1f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }


        [Test]
        public void Test_Speciality_Goblin_Dragon()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = "Goblin";
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = "Water";
            monster_Card_1.CardType = "Monster";

            monster_Card_2.Name = "Dragon";
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = "Fire";
            monster_Card_2.CardType = "Monster";

            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(monster_Card_1);
            cards.Add(monster_Card_2);

            Game.SpecialityEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 0f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_Speciality_Wizzard_Ork()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = "Ork";
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = "Water";
            monster_Card_1.CardType = "Monster";

            monster_Card_2.Name = "Wizzard";
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = "Fire";
            monster_Card_2.CardType = "Monster";

            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(monster_Card_1);
            cards.Add(monster_Card_2);

            Game.SpecialityEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 0f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_Speciality_Knights_WaterSpells()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Name = "WaterSpell";
            spell_Card_1.Damage = 20;
            spell_Card_1.ElementType = "Water";
            spell_Card_1.CardType = "Spell";

            monster_Card_2.Name = "Knights";
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = "Fire";
            monster_Card_2.CardType = "Monster";

            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(monster_Card_2);

            Game.SpecialityEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 0f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_Speciality_Kraken_Spells()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Name = "WaterSpell";
            spell_Card_1.Damage = 20;
            spell_Card_1.ElementType = "Water";
            spell_Card_1.CardType = "Spell";

            monster_Card_2.Name = "Kraken";
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = "Fire";
            monster_Card_2.CardType = "Monster";

            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(monster_Card_2);

            Game.SpecialityEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 0f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }

        [Test]
        public void Test_Speciality_FireElves_Dragon()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = "Dragon";
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = "Water";
            monster_Card_1.CardType = "Monster";

            monster_Card_2.Name = "FireElve";
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = "Fire";
            monster_Card_2.CardType = "Monster";

            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(monster_Card_1);
            cards.Add(monster_Card_2);

            Game.SpecialityEffect(cards, ref ef1, ref ef2);
            //Act
            float expectedValue1 = 0f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
            //Assert.Fail();
        }
    }
}
