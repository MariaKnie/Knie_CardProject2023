using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    public class CardPackages
    {
        private int max_HandOutSize = 5;
        public void CreatePackage(CardStack userstack, out int cardscreated)
        {
            for (int i = 0; i < max_HandOutSize; i++) // create cards for max handout size
            {
                Random rnd = new Random();
                int num = rnd.Next(2);

                // 50% 50% chance if Spell or Mosnter Card
                if (num == 1)
                {
                    //Monsercard
                    Monstercard newcard = new Monstercard();
                    newcard = newcard.GenerateCard();
                    userstack.Cards.Add(newcard);
                }
                else
                {
                    //SpellCard
                    SpellCard newcard = new SpellCard();
                    newcard = newcard.GenerateCard();
                    userstack.Cards.Add(newcard);
                }

                userstack.Cards[userstack.Cards.Count - 1].PrintCard();
            }
            cardscreated = max_HandOutSize;
            Console.WriteLine(cardscreated.ToString());

        }

        public List<Card> CreatePackageDB()
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < 5; i++)
            {
                Random rnd = new Random();
                int num = rnd.Next(2);

                if (num == 1)
                {
                    //Monsercard
                    Monstercard newcard = new Monstercard();
                    newcard = newcard.GenerateCard();
                    cards.Add(newcard);
                }
                else
                {
                    //SpellCard
                    SpellCard newcard = new SpellCard();
                    newcard = newcard.GenerateCard();
                    cards.Add(newcard);
                }
                cards[cards.Count - 1].PrintCard(); // print card stats
            }
            return cards;
        }
    }
}
