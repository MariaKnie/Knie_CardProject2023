using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Server.EndPoints
{
    internal class TradeEndpoint
    {
        public string id { get; set; }
        public string card_id { get; set; }
        public string card_ForType { get; set; }
        public int card_mindmg { get; set; }
        public int user_id { get; set; }

        public virtual string PrintTrade()
        {
            string stats = $"Trade: id: {id} \n  card_id: {card_id}\n  card_ForType: {card_ForType}\n  card_mindmg: {card_mindmg}\n user_id: {user_id}";
            Console.WriteLine(stats);
            return stats;
        }
    }
}
