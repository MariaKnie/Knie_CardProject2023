using Knie_CardProject2023;
using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Server.Server.UserRequests
{
    internal class UserRequests
    {

        public async Task UserRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo )
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"General User {requesttype}";
            string responseHTML = "";
            string fullinfo =  userInfo?["body"];

            Console.WriteLine(fullinfo);
            UserEndpoint m = JsonSerializer.Deserialize<UserEndpoint>(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Registered User </h1>";
            responseHTML += $"\n Username: {m.Username} Password: {m.Password}";


            if (requesttype == "GET")
            {
            }
            else if (requesttype == "POST")
            {
            }
            else if (requesttype == "DEL")
            {
            }

            responseHTML += "\n FullBody: " + userInfo?["body"];
            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }

        public async Task UserSpecificRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Specific User {requesttype}";
            string responseHTML = "";

            string name = userInfo?["subpath"];

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}  Specific User Reques </h1>";
            responseHTML += $"\n Username: {name}";

            if (requesttype == "GET")
            {

            }
            else if (requesttype == "POST")
            {

            }
            else if (requesttype == "DEL")
            {

            }

            responseHTML += "\n FullBody: " + userInfo?["subpath"];
            responseHTML += "\n</body> </html>";
            response.UniqueResponse( writer, 200, description, responseHTML);

        }
    }
}
