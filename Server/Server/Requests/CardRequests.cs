using Knie_CardProject2023;
using Knie_CardProject2023.Server;
using Microsoft.VisualBasic;
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
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);
                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                    int cardCount = CheckIfUserHasCards(user);

                    if (cardCount >= 0)
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
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("Cards couldnt be found");
                return -1;
            }

            Console.WriteLine($"Cards found! Count {(int)(long)result}");
            int count = (int)(long)result;
            return count;
        }

        public int CheckIfUserHasDeckCards(UserEndpoint user)
        {
            if (user == null)
            {
                return -1;
            }
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM cards WHERE user_id = @user_id AND card_indeck = true";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("DeckCards couldnt be found");
                return -1;
            }

            Console.WriteLine("DeckCards found!");
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
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\n{\n\"cards\":{\n";
            while (reader.Read())
            {
                count++;
                CardEndpoint readCard = new()
                {
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

                    user.Bio = (string)reader["card_description"];
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

        public string ShowAllUserDeckCards(UserEndpoint user)
        {
            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM cards WHERE user_id = @user_id AND card_indeck = true";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\n{\n\"deck_cards\":{\n";
            while (reader.Read())
            {
                count++;
                CardEndpoint readCard = new()
                {
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

                    user.Bio = (string)reader["card_description"];
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

        public async Task DeckRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {

            HTTP_Response response = new HTTP_Response();
            string description = $"User Deck Request{requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} User Deck Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                // show deck cards
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);
                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                }
                int cardCount = CheckIfUserHasDeckCards(user);

                if (cardCount >= 0)
                {
                    if (cardCount > 0)
                    {
                        responseHTML += "\n Able to Show Deckcards";
                        responseHTML += ShowAllUserDeckCards(user);
                        responseHTML += "\n Showed DeckCards";
                    }
                    else
                    {
                        responseHTML += "\n UNABLE to DeeckShow cards, No Cards in Account";
                    }
                }
                else
                {
                    responseHTML += "\n Error While trying to read cards";
                }
            }
            else if (requesttype == "PUT")
            {
                // show deck cards
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                List<string> NewDeckIDs = new List<string>();
                string temp = fullinfo.ToString();
                temp = temp.Substring(1);
                temp = temp.Remove(temp.Length - 1);
                Console.WriteLine("Card Snipping starting!");
                Console.WriteLine(temp);
                try
                {
                    string[] parts1 = temp.Split(",");
                    for (int i = 0; i < 5; i++)
                    {
                        parts1[i] = parts1[i].Trim();
                        parts1[i] = parts1[i].Remove(parts1[i].Length - 1);
                        parts1[i] = parts1[i].Substring(1);
                        Console.WriteLine(" Snip : " + parts1[i]);
                        NewDeckIDs.Add(parts1[i]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Too little cards" + e.Message);
                    responseHTML += "\n Too little cards";
                    responseHTML += "\n</body> </html>";
                    response.UniqueResponse(writer, 200, description, responseHTML);
                    return;
                }


                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                }
                List<string> cardCount = GetAllDeckIds(user);

                if (cardCount.Count >= 0)
                {
                    if (cardCount.Count > 0)
                    {
                        responseHTML += "\n Able to Switch";
                        PullOutOfDeckIds(cardCount);
                        responseHTML += "\n Pulled old out";
                        PutIntoDeckIds(NewDeckIDs);
                        responseHTML += "\n put new in";
                    }
                    else
                    {
                        responseHTML += "\n UNABLE to read Deck cards, No Cards in Account";
                    }
                }
                else
                {
                    responseHTML += "\n Error While trying to read Deckcards";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }

        public List<string> GetAllDeckIds(UserEndpoint user)
        {
            List<string> cardids = new List<string>();

            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT id FROM cards WHERE user_id = @user_id AND card_indeck = true;";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            using IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string id = (string)reader["id"];
                cardids.Add(id);
            }
            return cardids;
        }

        public void PullOutOfDeckIds(List<string> ids)
        {
            List<string> cardids = new List<string>();

            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            for (int i = 0; i < ids.Count; i++)
            {

                using IDbCommand command = connection.CreateCommand();
                string query = "UPDATE cards SET card_indeck = false WHERE id = @card_id";
                command.CommandText = query;

                // parameters
                GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, ids[i]);

                command.ExecuteNonQuery();
            }
        }

        public void PutIntoDeckIds(List<string> ids)
        {
            List<string> cardids = new List<string>();

            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            for (int i = 0; i < ids.Count; i++)
            {

                using IDbCommand command = connection.CreateCommand();
                string query = "UPDATE cards SET card_indeck = true WHERE id = @card_id";
                command.CommandText = query;

                // parameters
                GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, ids[i]);

                command.ExecuteNonQuery();
            }
        }


        public async Task DeckPlainRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"User Deck Request{requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} User Deck Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                // show deck cards
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);
                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                }
                int cardCount = CheckIfUserHasDeckCards(user);

                if (cardCount >= 0)
                {
                    if (cardCount > 0)
                    {
                        responseHTML += "\n Able to Show Deckcards";
                        responseHTML += ShowAllUserDeckCardsPlain(user);
                        responseHTML += "\n Showed DeckCards";
                    }
                    else
                    {
                        responseHTML += "\n UNABLE to DeeckShow cards, No Cards in Account";
                    }
                }
                else
                {
                    responseHTML += "\n Error While trying to read cards";
                }
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);

        }


        public string ShowAllUserDeckCardsPlain(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM cards WHERE user_id = @user_id AND card_indeck = true";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\ndeck_cards\n";
            while (reader.Read())
            {
                count++;
                CardEndpoint readCard = new()
                {
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

                    user.Bio = (string)reader["card_description"];
                }

                Console.WriteLine($"CardNumber {count}");
                readCard.PrintCard();
                jsonToSendBack += $"\n\n Card: {count}\n";
                jsonToSendBack += readCard.PrintCard();
            }
            Console.WriteLine();
            Console.WriteLine(jsonToSendBack);

            return jsonToSendBack;
        }
    }
}
