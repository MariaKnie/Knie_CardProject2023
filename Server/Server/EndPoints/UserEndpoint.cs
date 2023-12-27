using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class UserEndpoint
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public int Age { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public int Coins { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }

    }
}
