using Server.Server;
using Server.Server.Requests;
using Server.Server.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
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
        public async static void ReadRequest(StreamWriter writer, StreamReader reader)
        {
            //read requests
            string? line = "";
            int contentLength = 0;

            Console.WriteLine();

            string ForConsole = "\nREQUEST";

            line = reader.ReadLine();
            ForConsole += "\nFULL COMMAND: " + line;

            string[] requestparts = line.Split(' ');
            string Http_type = requestparts[0];
            string path = requestparts[1];
            ForConsole += "\n- HTTP: " + Http_type;
            ForConsole += "\n- PATH: " + path;
            string[] requestQuery = path.Split('?');
            string[] requestSubPath = requestQuery[0].Split('/');


            string token = HeaderHandler.ReadHeader(ref contentLength, ref writer, ref reader, ref line);
            string body = BodyHandler.BodyRead(ref contentLength, ref writer, ref reader);
            ForConsole += "\n- BODY: " + body;

            HTTP_Response response = new HTTP_Response();
            UserRequests users = new UserRequests();
            CardRequests cards = new CardRequests();
            PackagesRequests packages = new PackagesRequests();
            GeneralRequests general = new GeneralRequests();

            Dictionary<string, string> userInfo = new Dictionary<string, string>();
            userInfo.Add("token", token);
            userInfo.Add("body", body);
            if (requestSubPath.Length >2)
            {
                userInfo.Add("subpath", requestSubPath[2]);
            }
            else
            userInfo.Add("subpath", "-none-");

            Dictionary<string, Func<Task>> headers = new Dictionary<string, Func<Task>>();

            headers.Add("/users", () => users.UserRequest(writer, Http_type, userInfo));
            headers.Add("/users/", () => users.UserSpecificRequest(writer, Http_type, userInfo));

            headers.Add("/packages", () => packages.PackagesRequest(writer, Http_type));
            headers.Add("/cards", () => cards.CardsRequest(writer, Http_type));
            headers.Add("/deck", () => cards.DeckRequest(writer, Http_type));
            headers.Add("/deck?format=plain", () => cards.DeckPlainRequest(writer, Http_type));

            headers.Add("/sessions", () => general.SessionRequest(writer, Http_type, userInfo));
            headers.Add("/stats", () => general.StatsRequest(writer, Http_type));
            headers.Add("/scoreboard", () => general.ScoreboardRequest(writer, Http_type));
            headers.Add("/battles", () => general.BattleRequest(writer, Http_type));

            headers.Add("/transactions", () => packages.TransactionsPackagesRequest(writer, Http_type, userInfo));
            headers.Add("/transactions/packages", () => packages.TransactionsPackagesRequest(writer, Http_type, userInfo)); ;
            headers.Add("/transactions/", () => packages.SpecificTransactionsPackagesRequest(writer, Http_type));
            headers.Add("/tradings", () => general.TradingsRequest(writer, Http_type));
            headers.Add("/tradings/", () => general.SpecificTradingsRequest(writer, Http_type));


            if (headers.ContainsKey(path)) //specifics routes wont work  -> users/
            {
                ForConsole += "\nFUNCTION: " + path;
               await headers[path]();
            }
            else if (headers.ContainsKey("/"+ requestSubPath[1] + "/")) //specific User muss noch name übergeben
            {
                //specfic user
                ForConsole += "\nFUNCTION: " + "/" +  requestSubPath[1] + "/";
                await headers["/" + requestSubPath[1] + "/"]();
            }
            else
            {
               response.UniqueResponse(writer, 400, "Unknown Command: " + requestSubPath[0] + requestSubPath[1], $"<h1> unkown { requestSubPath[0] } { requestSubPath[1]} </h1>");
            }


            Console.WriteLine(ForConsole + "\n");

        }
    }
}
