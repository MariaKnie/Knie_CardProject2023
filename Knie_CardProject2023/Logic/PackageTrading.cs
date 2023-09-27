using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023.Logic
{
    internal class PackageTrading
    {
        public static void BuyPackage(User user)
        {
            user.Coins -= 5;
            CardPackages package = new CardPackages();
            int count;
            package.CreatePackage(user.Stack, out count);
            user.Stack.PrintStack();
        }
    }
}
