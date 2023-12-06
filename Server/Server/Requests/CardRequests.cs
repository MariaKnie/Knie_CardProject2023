using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class CardRequests
    {
        public async Task CardsRequest(StreamWriter writer, string requesttype)
        {

            HTTP_Response response = new HTTP_Response();
            string description = $"CardResponse {requesttype}";
            string responseHTML = "";
            if (requesttype == "GET")
            {
                responseHTML = $"<html> <body> \n<h1> GET CardResponse Request! </h1>";
                responseHTML += "\n</body> </html>";

            }
            else if (requesttype == "POST")
            {
                responseHTML = $"<html> <body> \n<h1> POST CardResponse Request! </h1>";
                responseHTML += "\n</body> </html>";
            }
            else if (requesttype == "DEL")
            {
                responseHTML = $"<html> <body> \n<h1> DEL CardResponse Request! </h1>";
                responseHTML += "\n</body> </html>";
            }

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
