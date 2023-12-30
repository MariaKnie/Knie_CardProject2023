using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using Server.Server.Requests;

namespace Knie_CardProject2023.Server
{
    internal class ServerLoop
    {
        static void Main(string[] args)
        {
            int port = 10001;
            Console.WriteLine($"Our first simple HTTP Server http://localhost:/{port}");

            var httpServer = new TcpListener(IPAddress.Loopback, port);
            httpServer.Start();

            List<Thread> listofThreads = new List<Thread>();

            GeneralRequests resetTokens = new GeneralRequests();
            resetTokens.DeleteTokens();

            while (true) // Server Loop
            {
                //for every client a thread
                var clientSocket = httpServer.AcceptTcpClient(); //blokierende function, bis client kommt, wird ein client erzeugt
                listofThreads.Add(new(() => RequestHandler.Serverthread(clientSocket)));
                listofThreads[listofThreads.Count - 1]?.Start();
            }

            //writer.Flush();
            //writer.Close();

            //join all before closing
            JoinAllThreads(listofThreads);
        }

        static void JoinAllThreads(List<Thread> listofThreads)
        {
            for (int i = 0; i < listofThreads.Count; i++)
            {
                listofThreads[i]?.Join();
            }
        }




    }
}

