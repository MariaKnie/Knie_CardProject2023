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
            int responseCode = 200;

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}Show All User Cards </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);
                if (user != null) // If user found
                {
                    responseHTML += "\n Found User by token";
                    int cardCount = CheckIfUserHasCards(user);

                    if (cardCount >= 0) // if able to retrive cards at all
                    {
                        if (cardCount > 0) // cards in Acc
                        {
                            responseHTML += "\n Able to Show cards";
                            responseHTML += ShowAllUserCards(user);
                            responseHTML += "\n Showed Cards";
                        }
                        else// no cards Acc
                        {
                            responseHTML += "\n UNABLE to Show cards, No Cards in Account";
                            responseCode = 500;
                        }
                    }
                    else // Couldnt retrive cards, error while query
                    {
                        responseHTML += "\n Error While trying to read cards";
                        responseCode = 400;
                    }
                }
                else // User wrong
                {
                    responseHTML += "\n Couldnt find User by token";
                    responseCode = 400;
                }
            }
            else
            {
                responseHTML += "\n Method wrong";
                responseCode = 400;
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
        }
        public async Task DeckRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {

            HTTP_Response response = new HTTP_Response();
            string description = $"User Deck Request{requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            int responseCode = 200;

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

                if (cardCount >= 0) // if able to retrive cards at all
                {
                    if (cardCount > 0) // cards in acc
                    {
                        responseHTML += "\n Able to Show Deckcards";
                        responseHTML += ShowAllUserDeckCards(user);
                        responseHTML += "\n Showed DeckCards";
                    }
                    else // no cards in acc
                    {
                        responseHTML += "\n UNABLE to DeckShow cards, No Cards in Account";
                        responseCode = 500;
                    }
                }
                else // error while trying to retrive cards 
                {
                    responseHTML += "\n Error While trying to read cards";
                    responseCode = 400;
                }
            }
            else if (requesttype == "PUT")
            {
                // change deck cards
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                List<string> NewDeckIDs = new List<string>();
                string temp = fullinfo.ToString();
                temp = temp.Substring(1);
                temp = temp.Remove(temp.Length - 1);
                Console.WriteLine("Card Snipping starting!");
                Console.WriteLine(temp);

                // seperating Data
                try
                {
                    string[] parts1 = temp.Split(",");
                    if (parts1.Length != 4) // wrong count
                    {
                        responseHTML += "Deck needs to be exactly 4 cards! You put " + parts1.Length;
                        responseHTML += "\n</body> </html>";
                        response.UniqueResponse(writer, 400, description, responseHTML);
                        return;
                    }

                    for (int i = 0; i < 4; i++)
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
                    responseHTML += "\n Too little cards";
                    responseHTML += "\n</body> </html>";
                    response.UniqueResponse(writer, 400, description, responseHTML);
                    return;
                }

                if (user != null)
                {
                    responseHTML += "\n Found User by token";

                    List<string> cardCount = GetAllDeckIds(user);
                    for (int i = 0; i < cardCount.Count; i++)
                    {
                        if (SeeIfCardIsInTradings(cardCount[i])) // if card is up for trading fail
                        {
                            responseHTML += "\n ERROR \nCard:" + cardCount[i] + " is put up for trading!";
                            responseHTML += "\n</body> </html>";
                            response.UniqueResponse(writer, 400, description, responseHTML);
                            return;

                        }
                        Card temp2 =  GetUserSpecificCards(cardCount[i], user.Id); // see if card belongs to user
                        if (temp2.Name == null)
                        {
                            responseHTML += "\n ERROR \nCard:" + cardCount[i] + " not found!";
                            responseHTML += "\n</body> </html>";
                            response.UniqueResponse(writer, 400, description, responseHTML);
                            return;
                        }
                    }
                    if (cardCount.Count >= 0)
                    {
                        if (cardCount.Count > 0)
                        {
                            responseHTML += "\n Able to Switch";
                            PullOutOfDeck(cardCount);
                            responseHTML += "\n Pulled old out";
                            PutIntoDeck(NewDeckIDs);
                            responseHTML += "\n Put new in";
                        }
                        else
                        {
                            responseHTML += "\n UNABLE to read Deck cards, No Cards in Account";
                            responseCode = 500;
                        }
                    }
                    else
                    {
                        responseHTML += "\n Error While trying to read Deckcards";
                        responseCode = 400;
                    }
                }
                else
                {
                    responseHTML += "\n Couldnt get User";
                    responseCode = 400;
                }
            }
            else
            {
                responseHTML += "\n Method wrong";
                responseCode = 400;
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
        }
        public async Task DeckPlainRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"User Deck Request{requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            int responseCode = 200;

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} User Deck Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;


            if (requesttype == "GET")
            {
                // show deck cards plain
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);
                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                }
                int cardCount = CheckIfUserHasDeckCards(user);

                if (cardCount >= 0) // Could Retrive Cards
                {
                    if (cardCount > 0) // cards in acc
                    {
                        responseHTML += "\n Able to Show Deckcards";
                        responseHTML += ShowAllUserDeckCardsPlain(user);
                        responseHTML += "\n Showed DeckCards";
                    }
                    else// no cards in acc
                    {
                        responseHTML += "\n UNABLE to DeckShow cards, No Cards in Account";
                        responseCode = 500;
                    }
                }
                else// Error while trying to Retrive Cards
                {
                    responseHTML += "\n Error While trying to read cards";
                    responseCode = 400;
                }
            }
            else
            {
                responseHTML += "\n Wrong Method";
                responseCode = 400;
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
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
                    readCard.Description = (string)reader["card_description"];
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

                    readCard.Description = (string)reader["card_description"];
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

                    readCard.Description = (string)reader["card_description"];
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

        public void PullOutOfDeck(List<string> ids)
        {
            List<string> cardids = new List<string>();

            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            for (int i = 0; i < ids.Count; i++) // For each Id in List
            {

                using IDbCommand command = connection.CreateCommand();
                string query = "UPDATE cards SET card_indeck = false WHERE id = @card_id";
                command.CommandText = query;

                // parameters
                GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, ids[i]);

                command.ExecuteNonQuery();
            }
        }
        public void PutIntoDeck(List<string> ids)
        {
            List<string> cardids = new List<string>();

            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            for (int i = 0; i < ids.Count; i++) // for each id in List
            {
                using IDbCommand command = connection.CreateCommand();
                string query = "UPDATE cards SET card_indeck = true WHERE id = @card_id";
                command.CommandText = query;

                // parameters
                GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, ids[i]);

                command.ExecuteNonQuery();
            }
        }

        // Used in Battle
        public bool CheckEnoughDeckCards(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM cards WHERE user_id = @user_id AND card_indeck = true;";

            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("DeckCards couldnt be found");
                return false;
            }
            else
            {
                Console.WriteLine("Deck found!");
                int count = (int)(long)result;
                if (count < 4)
                {
                    Console.WriteLine("Too Little Cards");
                    return false;
                }
                else
                {
                    return true;
                }
            }
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
        public void AddCardToPlayerStack(string card_id, int user_id)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "UPDATE cards SET user_id = @user_id, card_indeck = false WHERE id = @card_id;";

            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user_id);
            GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, card_id);

            command.ExecuteNonQuery();
        }
        public bool SeeIfCardIsInTradings(string cardId)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM trades WHERE card_id = @card_id;";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, cardId);

            var count = command.ExecuteScalar();

            if (count == null)
            {
                Console.WriteLine("AN ERROR OCCURED");
                return true;
            }


            if ((Int64)count > 0)
            {
                Console.WriteLine("CardId exists in the database.");
                return true;
            }
            else
            {
                Console.WriteLine("CardId does not exist in the database.");
                return false;
            }
        }


        // For Trade
        public Card GetUserSpecificCards(string card_id, int user_id)
        {
            PackageEndPoint packageEndPoint = new PackageEndPoint();
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM cards WHERE id = @id AND card_indeck = false AND user_id = @user_id";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@id", DbType.String, card_id);
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user_id);

            using IDataReader reader = command.ExecuteReader();
            Console.WriteLine("READING CARD FROM PLAYER " + card_id);

            SpellCard card = new SpellCard();

            while (reader.Read())
            {
                card = new()
                {
                    Id = (string)reader["id"],
                    Name = (string)reader["card_name"],
                    Damage = (int)reader["card_damage"],
                    CardType = (string)reader["card_type"],
                    ElementType = (string)reader["card_element"],
                    Description = ""
                };

                int columnIndex = reader.GetOrdinal("card_description");
                if (!reader.IsDBNull(columnIndex))
                {

                    card.Description = (string)reader["card_description"];
                }
                card.PrintCard();
            }
            return card;
        }
    }
}
