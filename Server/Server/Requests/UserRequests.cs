using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server.UserRequests
{
    internal class UserRequests
    {
        HTTP_Response response = new HTTP_Response();

        public StreamWriter UserRequest(StreamWriter writer, string requesttype)
        {

            response.UniqueResponse(writer, 200, "User All GET", "<html> <body> <h1> User Request! </h1> </body> </html>");
            return writer;
        }

        public StreamWriter UserSpecificRequest(StreamWriter writer, string requesttype)
        {

            response.UniqueResponse( writer, 200, "User GET", "<html> <body> <h1> User Request! </h1> </body> </html>");
            return writer;
        }
    }
}
