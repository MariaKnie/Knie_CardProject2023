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


        public async Task PackageResponse(StreamWriter writer, string Http_type)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"PackageResponse {Http_type}", $"<html> <body> <h1> {Http_type} PackageResponse Request! </h1> </body> </html>");

        }

        public async Task PackagesRequest(StreamWriter writer, string Http_type)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"Packages {Http_type}", $"<html> <body> <h1> {Http_type} Packages Request! </h1> </body> </html>");
        }

        public async Task TransactionsPackagesRequest(StreamWriter writer, string Http_type)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"TransactionsPackagesRequest {Http_type}", $"<html> <body> <h1> {Http_type} TransactionsPackagesRequest Request! </h1> </body> </html>");

        }

        public async Task SpecificTransactionsPackagesRequest(StreamWriter writer, string Http_type)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $" Specific TransactionsPackagesRequest {Http_type}", $"<html> <body> <h1> {Http_type} Specific TransactionsPackagesRequest Request! </h1> </body> </html>");

        }

    }
}
