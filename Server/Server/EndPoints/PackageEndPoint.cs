using Knie_CardProject2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server.EndPoints
{
    internal class PackageEndPoint
    {
        public List<CardEndpoint> package = new List<CardEndpoint>();
        public List<Card> packageCard = new List<Card>();
    }
}
