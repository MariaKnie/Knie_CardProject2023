using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Knie_CardProject2023
{
    public abstract class Card
    {
        protected string name;
        protected float damage;
        protected string card_type;
        protected string element_type;
        protected string description;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public string CardType
        {
            get { return card_type; }
            set { card_type = value; }
        }
        public string ElementType
        {
            get { return element_type; }
            set { element_type = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public virtual void Attack()
        { 
        
        }

        public virtual void PrintCard()
        {
            Console.WriteLine($"Card: \n  Name: {name}\n  Damage: {damage}\n  Element: {element_type}");
        }

        public virtual Card GenerateCard()
        {

            return null; 
        }


    }
}
