using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Server.Server.EndPoints
{
    internal class CardEndpoint
    {
        public string id { get; set; }
        public string Name { get; set; }
        public float Damage { get; set; }
        public string CardType { get; set; }
        public string ElementType { get; set; }
        public string Description { get; set; }

        public virtual string PrintCard()
        {
            string stats; 
            if (id != null)
            {

                stats = $"Card: id: {id} \n  Name: {Name}\n  Damage: {Damage}\n  Element: {ElementType}\n Description: {Description}";
            }
            else
                stats = $"Card: Name: {Name}\n  Damage: {Damage}\n  Element: {ElementType}\n Description: {Description}";


            Console.WriteLine(stats);
            return stats;
        }
    }
}
