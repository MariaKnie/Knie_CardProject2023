using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023.Server
{
    internal class HTTP_Response
    {


        public StreamWriter GetResponse(StreamWriter writer, int num)
        {

            return writer;
        }

        public StreamWriter OkResponse(StreamWriter writer)
        {

            UniqueResponse(writer, 200, "OK", "<html> <body> <h1>  Good Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter NoBodyRequest(StreamWriter writer)
        {

            UniqueResponse(writer, 200, "No Body", "<html> <body> <h1>   No Body Request! </h1> </body> </html>");

            return writer;
        }
        public StreamWriter BadResponse(StreamWriter writer)
        {

            UniqueResponse(writer, 400, "Bad", "<html> <body> <h1> Bad Request! </h1> </body> </html>");
            return writer;
        }



        public StreamWriter UniqueResponse(StreamWriter writer, int number, string HttpDes, string body )
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 " + number + " " + HttpDes);
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine(body);

            return writer;
        }

    }
}
