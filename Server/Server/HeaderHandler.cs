using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class HeaderHandler
    {

        public static string ReadHeader(ref int contentLength, ref StreamWriter writer, ref StreamReader reader, ref string? line)
        {
            bool isBody = false;
            string bearer = "";

            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") // end of  Header
                {
                    isBody = true;
                    break;
                }

                //Parse the header
                if (!isBody)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && parts[0] == "Content-Length")
                    {
                        contentLength = int.Parse(parts[1].Trim());
                    }
                    if (parts.Length == 2 && parts[0] == "Authorization")
                    {
                        var partsToken = parts[1].TrimStart().Split(' ');
                        bearer = partsToken[1];
                    }
                }
            }

            return bearer;
        }
    }
}
