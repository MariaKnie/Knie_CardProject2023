using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server.Requests
{
    internal class GeneralRequests
    {
        HTTP_Response response = new HTTP_Response();
        public StreamWriter SessionRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "Session", "<html> <body> <h1> Session Request! </h1> </body> </html>");
            return writer;
        }






        public StreamWriter StatsRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "StatsRequest", "<html> <body> <h1> StatsRequest Request! </h1> </body> </html>");
            return writer;
        }
        public StreamWriter ScoreboardRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "ScoreboardRequest", "<html> <body> <h1> ScoreboardRequest Request! </h1> </body> </html>");
            return writer;
        }


        public StreamWriter BattleRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "BattleRequest", "<html> <body> <h1> BattleRequest Request! </h1> </body> </html>");
            return writer;
        }

        public StreamWriter TradingsRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "TradingsRequest", "<html> <body> <h1> TradingsRequest Request! </h1> </body> </html>");
            return writer;
        }
    
    }
}
