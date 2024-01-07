using Knie_CardProject2023.Logic;
using Knie_CardProject2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Knie_CardProject2023.Enums.Card_Enums;

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
        string battleLog = "";

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
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 0;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_CardDamageComparison_Spell_Second()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 15;
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_CardDamageComparison_Spell_Draw()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = -1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }


        [Test]
        public void Test_SpellCards_GoIntoWinnerDeck()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = max.Deck.Cards.Count+1;
            Game.CompareCards(players, 0, ref battleLog);
            int actualValue = max.Deck.Cards.Count;

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_SpellCards_LeaveLoserDeck()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 15;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = tom.Deck.Cards.Count -1;
            Game.CompareCards(players, 0, ref battleLog);
            int actualValue = tom.Deck.Cards.Count;

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }



        [Test]
        public void Test_Spell_FireWater()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 10;
            spell_Card_2.Damage = 10;
            spell_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Spell_FireWater_draw()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = -1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Spell_FireWater_water_lose()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 26;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_Spell_ElectroWater()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 20;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Electro.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(spell_Card_1);
            tom.Deck.Cards.Add(spell_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 0;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        //######################## Monster Cards

        [Test]
        public void Test_CardDamageComparison_Monster_First()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 0;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_CardDamageComparison_Monster_Second()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 10;
            monster_Card_2.Damage = 15;
            monster_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = 1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_CardDamageComparison_Monster_Draw()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 10;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = -1;
            int actualValue = Game.CompareCards(players, 0, ref battleLog);

            Console.WriteLine(actualValue);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_MonsterCards_GoIntoWinnerDeck()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = max.Deck.Cards.Count+1;
            Game.CompareCards(players, 0, ref battleLog);
            int actualValue = max.Deck.Cards.Count;


            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }

        [Test]
        public void Test_MonsterCards_LeaveLosersDeck()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Damage = 15;
            monster_Card_2.Damage = 10;
            monster_Card_1.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();

            max.Deck.Cards.Clear();
            tom.Deck.Cards.Clear();

            max.Deck.Cards.Add(monster_Card_1);
            tom.Deck.Cards.Add(monster_Card_2);

            List<User> players = new List<User>();
            players.Add(max);
            players.Add(tom);

            //Act
            int expectedValue = tom.Deck.Cards.Count - 1;
            Game.CompareCards(players, 0, ref battleLog);
            int actualValue = tom.Deck.Cards.Count;


            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.Pass();
        }





 // ################## effect calculation
        [Test]
        public void Test_Spell_effect_calculation_WaterFire()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2, ref battleLog);
            //Act
            float expectedValue1 = 2f;
            float expectedValue2 = 0.5f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
        }

        [Test]
        public void Test_Spell_effect_calculation_noEEffect()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2, ref battleLog);
            //Act
            float expectedValue1 = 1f;
            float expectedValue2 = 1f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
        }



        [Test]
        public void Test_Spell_effect_calculation_ElectroWater()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Electro.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2, ref battleLog);
            //Act
            float expectedValue1 = 2f;
            float expectedValue2 = 0.25f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
        }
        [Test]
        public void Test_Spell_effect_calculation_ElectroFire()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Damage = 5;
            spell_Card_2.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Electro.ToString();
            spell_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();
            spell_Card_2.CardType = Enum_CardTypes.Spell.ToString();
            float ef1 = 1f;
            float ef2 = 1f;

            List<Card> cards = new List<Card>();
            cards.Add(spell_Card_1);
            cards.Add(spell_Card_2);

            Game.CheckEffect(cards, ref ef1, ref ef2, ref battleLog);
            //Act
            float expectedValue1 = 0.75f;
            float expectedValue2 = 0.6f;

            Console.WriteLine(ef1);
            Console.WriteLine(ef2);

            //Assert
            Assert.AreEqual(expectedValue1, ef1);
            Assert.AreEqual(expectedValue2, ef2);
            Assert.Pass();
        }


        // Specialties

        [Test]
        public void Test_Speciality_Goblin_Dragon()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = Enum_Monster.Goblin.ToString();
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            monster_Card_1.CardType = Enum_CardTypes.Monster.ToString();

            monster_Card_2.Name = Enum_Monster.Dragon.ToString();
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.CardType = Enum_CardTypes.Monster.ToString();

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
        }

        [Test]
        public void Test_Speciality_Wizzard_Ork()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = Enum_Monster.Ork.ToString(); 
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            monster_Card_1.CardType = Enum_CardTypes.Monster.ToString();

            monster_Card_2.Name = Enum_Monster.Wizzard.ToString();
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.CardType = Enum_CardTypes.Monster.ToString();

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
        }

        [Test]
        public void Test_Speciality_Knights_WaterSpells()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Name = Enum_ElementTypes.Water.ToString() + Enum_CardTypes.Spell.ToString();
            spell_Card_1.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();

            monster_Card_2.Name = Enum_Monster.Knight.ToString();
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.CardType = Enum_CardTypes.Monster.ToString();

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
        }

        [Test]
        public void Test_Speciality_Kraken_Spells()
        {
            // Pattern AAA
            // Arrange
            spell_Card_1.Name = Enum_ElementTypes.Water.ToString() + Enum_CardTypes.Spell.ToString();
            spell_Card_1.Damage = 20;
            spell_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            spell_Card_1.CardType = Enum_CardTypes.Spell.ToString();

            monster_Card_2.Name = Enum_Monster.Kraken.ToString();
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.CardType = Enum_CardTypes.Monster.ToString();

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
        }

        [Test]
        public void Test_Speciality_FireElves_Dragon()
        {
            // Pattern AAA
            // Arrange
            monster_Card_1.Name = Enum_Monster.Dragon.ToString();
            monster_Card_1.Damage = 20;
            monster_Card_1.ElementType = Enum_ElementTypes.Water.ToString();
            monster_Card_1.CardType = Enum_CardTypes.Monster.ToString();

            monster_Card_2.Name = Enum_Monster.FireElve.ToString();
            monster_Card_2.Damage = 20;
            monster_Card_2.ElementType = Enum_ElementTypes.Fire.ToString();
            monster_Card_2.CardType = Enum_CardTypes.Monster.ToString();

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
        }
    }
}
