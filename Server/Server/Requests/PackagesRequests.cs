using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server.Requests
{
    internal class PackagesRequests
    {

        HTTP_Response response = new HTTP_Response();

        public StreamWriter PackageResponse(StreamWriter writer)
        {
            
            response.UniqueResponse(writer, 200, "PackageResponse", "<html> <body> <h1> PackageResponse Request! </h1> </body> </html>");

            return writer;
        }

        public StreamWriter PackagesRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "Packages", "<html> <body> <h1> Packages Request! </h1> </body> </html>");
            return writer;
        }

        public StreamWriter TransactionsPackagesRequest(StreamWriter writer)
        {

            response.UniqueResponse(writer, 200, "TransactionsPackagesRequest", "<html> <body> <h1> TransactionsPackagesRequest Request! </h1> </body> </html>");

            return writer;
        }

    }
}
