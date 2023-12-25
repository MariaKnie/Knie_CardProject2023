using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023
{
    internal class SpellCard : Card
    {


        public override void Attack()
        {
            base.Attack();
        }

        public override SpellCard GenerateCard()
        {

            Random rnd = new Random();
            int num = rnd.Next(3);

            SpellCard newCard = new SpellCard();
            newCard.CardType = "SpellCard";
            if (num == 0)
            {
                //Water
                newCard.name = "WaterSpell";
                newCard.damage = 40;
                newCard.element_type = "Water";
            }
            else if (num == 1)
            {
                //Fire
                newCard.name = "FireSpell";
                newCard.damage = 90;
                newCard.element_type = "Fire";
            }
            else if (num == 2)
            {
                //Normal
                newCard.name = "NormalSpell";
                newCard.damage = 30;
                newCard.element_type = "Normal";
            }
            


            Console.WriteLine($" Added Spell-Card: \n  Name: {newCard.name}\n  Damage: {newCard.damage}\n  Element: {newCard.element_type}");

            return newCard;
        }
    }
}
