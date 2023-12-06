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

            while ((line = reader.ReadLine()) != null)
            {
                //Console.WriteLine(line); //erste line noch aufspliten und dann htttp protcol, / dann ? dann && || und dann =
                if (line == "")
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
                }
            }

            return "";
        }
    }
}
