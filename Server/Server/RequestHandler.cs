using Server.Server;
using Server.Server.Requests;
using Server.Server.UserRequests;
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
            string? line = "";
            int contentLength = 0;

            Console.WriteLine();
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
            UserRequests users = new UserRequests();
            CardRequests cards = new CardRequests();
            PackagesRequests packages = new PackagesRequests();
            GeneralRequests general = new GeneralRequests();
            Dictionary<string, StreamWriter> headers = new Dictionary<string, StreamWriter>();
            headers.Add("", response.OkResponse(writer));
            headers.Add("/", response.OkResponse(writer));

            headers.Add("/users", users.UserRequest(writer, Http_type));
            headers.Add("/users/", users.UserSpecificRequest(writer, Http_type));

            headers.Add("/packages ", packages.PackagesRequest(writer));
            headers.Add("/cards", cards.CardsRequest(writer,Http_type));
            headers.Add("/deck", cards.DeckRequest(writer, Http_type));
            headers.Add("/deck?format=plain", cards.DeckPlainRequest(writer, Http_type));

            headers.Add("/sessions", general.SessionRequest(writer));
            headers.Add("/stats", general.StatsRequest(writer));
            headers.Add("/scoreboard", general.ScoreboardRequest(writer));
            headers.Add("/battles", general.BattleRequest(writer));

            headers.Add("/transactions/packages", packages.TransactionsPackagesRequest(writer));
            headers.Add("/tradings", general.TradingsRequest(writer));




            //   HTTP_Response response = new HTTP_Response();

            if (headers.ContainsKey(requestSubPath[1])) //specifics routes wont work  -> users/
            {
                writer = headers[requestSubPath[1]];
            }
            else if (requestSubPath[1].Contains("users/")) //specific User muss noch name übergeben
            {
                writer = headers["/users/"];
            }
            else
            {
                response.BadResponse(writer);
            }


            Console.WriteLine();

        }
    }
}
