using Server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023.Server
{
    internal class RequestHandler
    {
        public static void Serverthread(TcpClient clientSocket)
        {


            using var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };//using bc flushing
            using var reader = new StreamReader(clientSocket.GetStream());//using bc flushing
            ReadRequest(writer, reader);




        }
        public static void ReadRequest(StreamWriter writer, StreamReader reader)
        {
            //read requests
            string? line;
            int contentLength = 0;

            Console.WriteLine("\nREQUEST");

            line = reader.ReadLine();
            string[] requestparts = line.Split(' ');
            string Http_type = requestparts[0];
            string path = requestparts[1];
            Console.WriteLine("- HTTP: " + Http_type);
            Console.WriteLine("- PATH: " + path);
            string[] requestQuery = path.Split('?');
            string[] requestSubPath = requestQuery[0].Split('/');


            string token = HeaderHandler.ReadHeader(contentLength, writer, reader, line);
            string body = BodyHandler.BodyRead(contentLength, writer, reader);

            HTTP_Response response = new HTTP_Response();
            Dictionary<string, StreamWriter> headers = new Dictionary<string, StreamWriter>();
            headers.Add("", response.OkResponse(writer));
            headers.Add("/", response.OkResponse(writer));

            headers.Add("/users", response.UserRequest(writer));
            headers.Add("/users/altenhof", response.UserRequest(writer));
            headers.Add("/users/kienboec", response.UserRequest(writer));

            headers.Add("/packages ", response.PackagesRequest(writer));
            headers.Add("/cards", response.CardsRequest(writer));
            headers.Add("/deck", response.DeckRequest(writer));
            headers.Add("/deck?format=plain", response.DeckPlainRequest(writer));

            headers.Add("/sessions", response.SessionRequest(writer));
            headers.Add("/stats", response.StatsRequest(writer));
            headers.Add("/scoreboard", response.ScoreboardRequest(writer));
            headers.Add("/battles", response.BattleRequest(writer));

            headers.Add("/transactions/packages", response.TransactionsPackagesRequest(writer));
            headers.Add("/tradings", response.TradingsRequest(writer));




         //   HTTP_Response response = new HTTP_Response();

            if (headers.ContainsKey(requestSubPath[1]))
            {
               writer = headers[requestSubPath[1]];
            }
            else
            {
                response.BadResponse(writer);
            }


            //switch (requestSubPath[1])
            //{
            //    case "users":
            //        response.UserRequest(writer);
            //        break;
            //    case "packages":
            //        response.PackagesRequest(writer);
            //        break;
            //    case "sessions":
            //        response.SessionRequest(writer);
            //        break;
            //    default:
            //        response.OkResponse(writer);
            //        break;
            //}     

        }
    }
}
