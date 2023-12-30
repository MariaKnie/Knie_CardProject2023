using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Knie_CardProject2023.Enums.Card_Enums;

namespace Knie_CardProject2023
{
    public class Monstercard : Card
    {
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
            int enumCount = Enum.GetNames(typeof(Enum_Monster)).Length;
            int num = rnd.Next(enumCount);

            Monstercard newCard = new Monstercard();
            newCard.CardType = Enum_CardTypes.Monster.ToString();
            if (num == 0)
            {
                //Dragon
                newCard.name = Enum_Monster.Dragon.ToString() ;
                newCard.damage = 100;
                newCard.element_type = Enum_ElementTypes.Fire.ToString();
            }
            else if (num == 1)
            {
                //Goblins
                newCard.name = Enum_Monster.Goblin.ToString();
                newCard.damage = 40;
                newCard.element_type = Enum_ElementTypes.Normal.ToString();
            }
            else if (num == 2)
            {
                //Wizzard
                newCard.name = Enum_Monster.Wizzard.ToString();
                newCard.damage = 50;
                newCard.element_type = Enum_ElementTypes.Water.ToString();
            }
            else if (num == 3)
            {
                //Orks
                newCard.name = Enum_Monster.Ork.ToString();
                newCard.damage = 50;
                newCard.element_type = Enum_ElementTypes.Normal.ToString();
            }
            else if (num == 4)
            {
                //Knights
                newCard.name = Enum_Monster.Knight.ToString();
                newCard.damage = 50;
                newCard.element_type = Enum_ElementTypes.Normal.ToString();
            }
            else if (num == 5)
            {
                //Kraken
                newCard.name = Enum_Monster.Kraken.ToString();
                newCard.damage = 70;
                newCard.element_type = Enum_ElementTypes.Normal.ToString();
            }
            else if (num == 6)
            {
                //FireElves
                newCard.name = Enum_Monster.FireElve.ToString();
                newCard.damage = 20;
                newCard.element_type = Enum_ElementTypes.Fire.ToString();
            }

            Console.WriteLine($" Added Monster-Card: \n  Name: {newCard.name}\n  Damage: {newCard.damage}\n  Element: {newCard.element_type}\n  Description: {newCard.description}"); 
            return newCard;
        }
    }
}
