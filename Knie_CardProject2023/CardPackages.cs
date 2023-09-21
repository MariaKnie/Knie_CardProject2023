using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    internal class CardPackages
    {

        private int max_HandOutSize = 5;

        public CardStack BuyPackage(CardStack userstack )
        {

            for (int i = 0; i < max_HandOutSize; i++)
            {
                Random rnd = new Random();
                int num = rnd.Next(2);

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

                return userstack;
        }
    }
}
