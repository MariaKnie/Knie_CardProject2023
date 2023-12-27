using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    public class Monstercard : Card
    {


        public override void Attack()
        {
            base.Attack();
        }

        public override Monstercard GenerateCard()
        {
            Random rnd = new Random();
            int num = rnd.Next(7);

            Monstercard newCard = new Monstercard();
            newCard.CardType = "Monstercard";
            if (num == 0)
            {
                //Dragon
                newCard.name = "Dragon";
                newCard.damage = 100;
                newCard.element_type = "Fire";
            }
            else if (num == 1)
            {
                //Goblins
                newCard.name = "Goblins";
                newCard.damage = 40;
                newCard.element_type = "Normal";
            }
            else if (num == 2)
            {
                //Wizzard
                newCard.name = "Wizzard";
                newCard.damage = 50;
                newCard.element_type = "Water";
            }
            else if (num == 3)
            {
                //Orks
                newCard.name = "Orks";
                newCard.damage = 50;
                newCard.element_type = "Normal";
            }
            else if (num == 4)
            {
                //Knights
                newCard.name = "Knights";
                newCard.damage = 50;
                newCard.element_type = "Normal";
            }
            else if (num == 5)
            {
                //Kraken
                newCard.name = "Kraken";
                newCard.damage = 70;
                newCard.element_type = "Normal";
            }
            else if (num == 6)
            {
                //FireElves
                newCard.name = "FireElves";
                newCard.damage = 20;
                newCard.element_type = "Fire";
            }

            Console.WriteLine($" Added Monster-Card: \n  Name: {newCard.name}\n  Damage: {newCard.damage}\n  Element: {newCard.element_type}"); 
            return newCard;
        }
    }
}
