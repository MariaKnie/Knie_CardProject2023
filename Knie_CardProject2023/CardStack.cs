using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    public class CardStack
    {

        private List<Card> stack = new List<Card>();
        private int max_HandOutSize = 5;
        public List<Card> Cards
        {
            get { return stack; }
            set { }
        }
        public void PrintStack()
        {
            for (int i = 0; i < stack.Count; i++) // print each card in stack
            {
                Console.WriteLine($"\nStack Count: {i + 1}");
                Console.WriteLine(stack[i].CardType + "Card");
                Console.WriteLine($"-Card: \n  Name: {stack[i].Name}\n  Damage: {stack[i].Damage}\n  Element: {stack[i].ElementType}");
            }
        }
        public virtual void Fill_Stack()
        {
            for (int i = 0; i < max_HandOutSize; i++)
            {
                Random rnd = new Random();
                int num = rnd.Next(2);

                Console.WriteLine($"\nStack Count: {stack.Count + 1}");
                if (num == 1)
                {
                    //Monsercard
                    Monstercard newcard = new Monstercard();
                    newcard = newcard.GenerateCard();
                    stack.Add(newcard);
                }
                else
                {
                    //SpellCard
                    SpellCard newcard = new SpellCard();
                    newcard = newcard.GenerateCard();
                    stack.Add(newcard);
                }
            }
        }
    }
}
