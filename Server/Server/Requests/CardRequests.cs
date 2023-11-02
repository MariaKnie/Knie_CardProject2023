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
        HTTP_Response response = new HTTP_Response();

        public StreamWriter CardsRequest(StreamWriter writer, string requesttype)
        {


            if (requesttype == "GET")
            {

            }
            else if (requesttype == "POST")
            {

            }
            else if (requesttype == "PUT")
            {

            }
            else if (requesttype == "DEL")
            {

            }


            response.UniqueResponse(writer, 200, "CardResponse", "<html> <body> <h1> CardResponse Request! </h1> </body> </html>");
            return writer;
        }



        public StreamWriter DeckRequest(StreamWriter writer, string requesttype)
        {


            response.UniqueResponse(writer, 200, "CardResponse", "<html> <body> <h1> CardResponse Request! </h1> </body> </html>");
            return writer;
        }

        public StreamWriter DeckPlainRequest(StreamWriter writer, string requesttype)
        {

            response.UniqueResponse(writer, 200, "DeckPlainRequest", "<html> <body> <h1> DeckPlainRequest Request! </h1> </body> </html>");
            return writer;
        }
    }
}
