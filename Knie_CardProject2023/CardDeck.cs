using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    public class CardDeck
    {
        List<Card> card_deck= new List<Card>();
        private int max_HandOutSize = 4;

        public List<Card> Cards
        {
            get { return card_deck; }
            set { }
        }

        public virtual void Fill_Deck(CardStack user_stack)
        {
            CardStack tempStack = new CardStack();
            for (int i = 0; i < user_stack.Cards.Count; i++)
            {
                tempStack.Cards.Add(user_stack.Cards[i]);
            }

            int cardsselected = 0;


            while (cardsselected < max_HandOutSize)
            {
                tempStack.PrintStack();
                Console.WriteLine($"\nDeck Count: {card_deck.Count + 1}/{max_HandOutSize}");
                Console.WriteLine($"Choose Card 0-{tempStack.Cards.Count-1}: ");
                int num = Convert.ToInt32(Console.ReadLine());
                card_deck.Add(tempStack.Cards[num]);
                cardsselected++;
                tempStack.Cards.RemoveAt(num);

                
            }
            Console.WriteLine("\n--------------------------------\n");
        }
    }
}
