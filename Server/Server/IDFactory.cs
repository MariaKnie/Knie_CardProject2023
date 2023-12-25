using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class IDFactory
    {
        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string generateID(string sourceUrl)
        {
            return string.Format("{0}_{1:N}", sourceUrl, Guid.NewGuid());
        }
    }
}
