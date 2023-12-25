using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class UserEndpoint
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public int age { get; set; }
        public string description { get; set; }
        public int coins { get; set; }

    }
}
