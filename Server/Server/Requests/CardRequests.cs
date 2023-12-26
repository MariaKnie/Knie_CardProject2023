using Knie_CardProject2023.Server;
using Server.Server.Requests;
using Server.Server.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class CardRequests
    {
        public async Task CardsRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {

            HTTP_Response response = new HTTP_Response();
            string description = $"Show All User Cards {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}Show All User Cards </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "POST")
            {
                // - 5 coind + 5 cards to stack
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                if (true)
                {
                    responseHTML += "\n Able to Show cards";                
                    responseHTML += "\n Showed Cards";
                }
                else
                {
                    responseHTML += "\n UNABLE to Show cards";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }



        public async Task DeckRequest(StreamWriter writer, string requesttype)
        {

            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"CardResponse {requesttype}", $"<html> <body> <h1> {requesttype} CardResponse Request! </h1> </body> </html>");
        }

        public async Task DeckPlainRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"DeckPlainRequest {requesttype}", $"<html> <body> <h1> {requesttype} DeckPlainRequest Request! </h1> </body> </html>");
        }
    }
}
