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
        public async Task SessionRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"Session {requesttype}", $"<html> <body> <h1> {requesttype} Session Request! </h1> </body> </html>");
        }






        public async Task StatsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"StatsRequest {requesttype}", $"<html> <body> <h1> {requesttype} StatsRequest Request! </h1> </body> </html>");
        }
        public async Task ScoreboardRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"ScoreboardRequest {requesttype}", $"<html> <body> <h1> {requesttype} ScoreboardRequest Request! </h1> </body> </html>");
        }


        public async Task BattleRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"BattleRequest {requesttype} ", $"<html> <body> <h1> {requesttype} BattleRequest Request! </h1> </body> </html>");
        }

        public async Task TradingsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"TradingsRequest {requesttype}", $"<html> <body> <h1> {requesttype} TradingsRequest Request! </h1> </body> </html>");

        }
        public async Task SpecificTradingsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"Specific TradingsRequest {requesttype}", $"<html> <body> <h1> {requesttype} Specific TradingsRequest Request! </h1> </body> </html>");

        }

    }
}
