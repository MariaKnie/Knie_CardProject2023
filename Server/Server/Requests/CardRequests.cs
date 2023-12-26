using Knie_CardProject2023;
using Knie_CardProject2023.Server;
using Npgsql;
using Server.Server.EndPoints;
using Server.Server.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
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
                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                    int cardCount = CheckIfUserHasCards(user);

                    if (cardCount >=0)
                    {
                        if (cardCount > 0)
                        {
                            responseHTML += "\n Able to Show cards";
                            responseHTML += ShowAllUserCards(user);
                            responseHTML += "\n Showed Cards";
                        }
                        else
                        {
                            responseHTML += "\n UNABLE to Show cards, No Cards in Account";
                        }
                    }
                    else
                    {
                        responseHTML += "\n Error While trying to read cards";
                    }
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }

        public int CheckIfUserHasCards(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM cards WHERE user_id = @user_id";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.id);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("User couldnt be found");
                return -1;
            }

            Console.WriteLine("User found!");
            int count = (int)(long)result;
            return count;
        }

        public string ShowAllUserCards(UserEndpoint user)
        {
            PackageEndPoint packageEndPoint = new PackageEndPoint();    
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM cards WHERE user_id = @user_id";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.id);

            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\n{\n\"cards\":{\n";
            while (reader.Read())
            {
                count++;
                CardEndpoint readCard = new(){
                    id = (string)reader["id"],
                    Name = (string)reader["card_name"],
                    Damage = (int)reader["card_damage"],
                    CardType = (string)reader["card_type"],
                    ElementType = (string)reader["card_element"],
                    Description = ""

                };

                int columnIndex = reader.GetOrdinal("card_description");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.description = (string)reader["card_description"];
                }
              
                Console.WriteLine($"CardNumber {count}");
                readCard.PrintCard();
                packageEndPoint.package.Add(readCard);
                jsonToSendBack += JsonSerializer.Serialize<CardEndpoint>(readCard);
            }
            Console.WriteLine();
            jsonToSendBack += "\n}\n}";
            Console.WriteLine(jsonToSendBack);

            return jsonToSendBack;
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
