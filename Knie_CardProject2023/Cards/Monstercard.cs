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

        public Monstercard()
        {

        }
        public Monstercard(string id, string name, float damage, string card_type, string element_type, string description)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;
            this.card_type = card_type;
            this.element_type = element_type;
            this.description = description;
        }
        public override Monstercard GenerateCard()
        {
            Random rnd = new Random();
            int num = rnd.Next(7);

            Monstercard newCard = new Monstercard();
            newCard.CardType = "Monster";
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
                newCard.name = "Goblin";
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
                newCard.name = "Ork";
                newCard.damage = 50;
                newCard.element_type = "Normal";
            }
            else if (num == 4)
            {
                //Knights
                newCard.name = "Knight";
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
                newCard.name = "FireElve";
                newCard.damage = 20;
                newCard.element_type = "Fire";
            }

            Console.WriteLine($" Added Monster-Card: \n  Name: {newCard.name}\n  Damage: {newCard.damage}\n  Element: {newCard.element_type}"); 
            return newCard;
        }
    }
}
