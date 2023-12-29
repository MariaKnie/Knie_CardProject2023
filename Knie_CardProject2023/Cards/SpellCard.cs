using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Knie_CardProject2023.Enums.Card_Enums;

namespace Knie_CardProject2023
{
    public class SpellCard : Card
    {
        public SpellCard()
        {

        }
        public SpellCard(string id, string name, float damage, string card_type, string element_type, string description)
        {
            this.id = id;
            this.name = name;
            this.damage = damage;
            this.card_type = card_type;
            this.element_type = element_type;
            this.description = description;
        }

        public override void Attack()
        {
            base.Attack();
        }

        public override SpellCard GenerateCard()
        {

            Random rnd = new Random();
            int enumCount = Enum.GetNames(typeof(Enum_ElementTypes)).Length;
            int num = rnd.Next(enumCount);

            SpellCard newCard = new SpellCard();
            newCard.CardType = Enum_CardTypes.Spell.ToString();
            if (num == 0)
            {
                //Water
                newCard.name = Enum_ElementTypes.Water.ToString() + newCard.CardType;
                newCard.damage = 40;
                newCard.element_type = Enum_ElementTypes.Water.ToString();
            }
            else if (num == 1)
            {
                //Fire
                newCard.name = Enum_ElementTypes.Fire.ToString() + newCard.CardType;
                newCard.damage = 90;
                newCard.element_type = Enum_ElementTypes.Fire.ToString();
            }
            else if (num == 2)
            {
                //Normal
                newCard.name = Enum_ElementTypes.Normal.ToString() + newCard.CardType;
                newCard.damage = 30;
                newCard.element_type = Enum_ElementTypes.Normal.ToString();
            }
            else if (num == 3)
            {
                //Normal
                newCard.name = Enum_ElementTypes.Electro.ToString() + newCard.CardType;
                newCard.damage = 30;
                newCard.element_type = Enum_ElementTypes.Electro.ToString();
            }


            Console.WriteLine($" Added Spell-Card: \n  Name: {newCard.name}\n  Damage: {newCard.damage}\n  Element: {newCard.element_type}");

            return newCard;
        }
    }
}
