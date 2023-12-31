﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023.Server
{
    internal class BodyHandler
    {

        public static string BodyRead( ref int contentLength, ref StreamWriter writer, ref StreamReader reader)
        {
            // read the body if exists     
            var datablock = new StringBuilder(200);
            char[] chars = new char[1024];
            int bytesReadtotal = 0;

            while (bytesReadtotal < contentLength)
            {
                var bytesRead = reader.Read(chars, 0, chars.Length);
                bytesReadtotal += bytesRead;
                if (bytesRead == 0)
                {
                    break;
                }

                datablock.Append(chars, 0, bytesRead);
            }

            return datablock.ToString();
        }
    }
}
