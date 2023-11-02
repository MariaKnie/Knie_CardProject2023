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


        public StreamWriter GetResponse(StreamWriter writer ,int num)
        {
            
            return writer;
        }

        public StreamWriter OkResponse(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> Good Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter NoBodyRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 NoBody");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> No Body Request! </h1> </body> </html>");

            return writer;
        }
        public StreamWriter BadResponse(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 400 Bad");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> Bad Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter UserRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 User");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> User Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter PackagesRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 Packages");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> Packages Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter SessionRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 Session");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> Session Request! </h1> </body> </html>");

            return writer;
        }


        public StreamWriter PackageResponse(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 PackageResponse");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> PackageResponse Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter CardsRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 CardResponse");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> CardsRequest Request! </h1> </body> </html>");

            return writer;
        }



        public StreamWriter DeckRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 DeckRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> DeckRequest Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter DeckPlainRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 DeckPlainRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> DeckPlainRequest Request! </h1> </body> </html>");

            return writer;
        }


        public StreamWriter StatsRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 StatsRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> StatsRequest Request! </h1> </body> </html>");

            return writer;
        }
        public StreamWriter ScoreboardRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 ScoreboardRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> ScoreboardRequest Request! </h1> </body> </html>");

            return writer;
        }


        public StreamWriter BattleRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 BattleRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> BattleRequest Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter TransactionsPackagesRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 TransactionsPackagesRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> TransactionsPackagesRequest Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter TradingsRequest(StreamWriter writer)
        {
            //write the HTTP response
            writer.WriteLine("HTTP/1.1 200 TradingsRequest");
            writer.WriteLine("Content-Type: text/html; charset=utf-8");
            writer.WriteLine();
            writer.WriteLine("<html> <body> <h1> TradingsRequest Request! </h1> </body> </html>");

            return writer;
        }
    }
}
