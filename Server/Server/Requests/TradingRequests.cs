using Knie_CardProject2023;
using Knie_CardProject2023.Server;
using Npgsql;
using Server.Server.EndPoints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Server.Requests
{
    internal class TradingRequests
    {
        public async Task TradingsRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Tranding Request {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            string subpath_user = userInfo?["subpath"];
            int responseCode = 200;
            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Tranding Request </h1>";
            responseHTML += "\n Token: " + token;
            responseHTML += "\n FullBody: " + userInfo?["body"];
            responseHTML += "\n Subpath: " + subpath_user;

            UserRequests uRq = new UserRequests();
            UserEndpoint user = uRq.GetUserByToken(token);


            if (user != null) // user found
            {
                if (requesttype == "GET")
                {
                    responseHTML += "\n Get Trade Data \n\n";
                    responseHTML += GetAllTrades();
                }
                else if (requesttype == "POST")
                {
                    if (fullinfo.Length > 0) // change user data
                    {
                        responseHTML += "\n Add Trade";

                        Dictionary<string, string> InfoToChange_Dic = new Dictionary<string, string>();
                        string temp = fullinfo.ToString();
                        temp = temp.Substring(1);
                        temp = temp.Remove(temp.Length - 1);
                        Console.WriteLine("User Data Snipping starting!");
                        Console.WriteLine(temp);
                        try
                        {
                            string[] parts1 = temp.Split(",");
                            for (int i = 0; i < parts1.Count(); i++)
                            {
                                string key = "";
                                string value = "";
                                string[] parts2 = parts1[i].Split(":", 2);
                                parts2[0] = parts2[0].ToLower();
                                for (int b = 0; b < parts2.Count(); b++)
                                {
                                    parts2[b] = parts2[b].Trim();

                                    if (parts2[0].ToLower() != "mindmg")
                                    {                                 
                                        parts2[b] = parts2[b].Remove(parts2[b].Length - 1);
                                        parts2[b] = parts2[b].Substring(1);
                                    }

                                    if (b == 0)
                                    {
                                        key = parts2[b];
                                    }
                                    else if (b == 1)
                                    {
                                        value = parts2[b];
                                    }
                                }
                                InfoToChange_Dic.Add(key, value);
                                Console.WriteLine($" \nUser Snip {i} : \nKey: " + InfoToChange_Dic.ElementAt(InfoToChange_Dic.Count - 1).Key + "\nValue: " + InfoToChange_Dic.ElementAt(InfoToChange_Dic.Count - 1).Value);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error in Snipping User Data " + e.Message);
                            responseHTML += "\n Error in Snipping User Data";
                            responseHTML += "\n</body> </html>";
                            response.UniqueResponse(writer, 400, description, responseHTML);
                            return;
                        }

                        bool canAdd = true;
                        if (InfoToChange_Dic.ContainsKey("cardtotrade"))
                        {
                            canAdd = !SeeIfTradeIsINDB(InfoToChange_Dic["cardtotrade"]);

                            CardRequests crq = new CardRequests();

                            // see if card really belongs to tbe aledged user
                            Card myCard = crq.GetUserSpecificCards(InfoToChange_Dic["cardtotrade"], user.Id, false, true); // my card, not in deck
                            if (myCard.Name == null)
                            {
                                canAdd = false;
                                responseHTML += "\nCard couldnt be found";
                                responseHTML += "\nMake sure card is not in deck";
                                responseHTML += "\n</body> </html>";
                                responseCode = 400;
                                response.UniqueResponse(writer, responseCode, description, responseHTML);
                                return;
                            }
                            Console.WriteLine("Bool = " + canAdd);
                        }
                        else
                        {
                            canAdd = false;
                        }

                        if (canAdd) // card belongs to user and is not in deck
                        {
                            TradeEndpoint newtrade = new TradeEndpoint();
                            newtrade.id = InfoToChange_Dic["id"];
                            newtrade.card_id = InfoToChange_Dic["cardtotrade"];
                            newtrade.card_ForType = InfoToChange_Dic["type"];
                            newtrade.user_id = user.Id;
                            try
                            {
                                newtrade.card_mindmg = Int32.Parse(InfoToChange_Dic["mindmg"]);
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine(e.Message);
                                responseHTML += "Error with Int Conversion";
                                responseHTML += "\n</body> </html>";
                                responseCode = 400;
                                response.UniqueResponse(writer, responseCode, description, responseHTML);
                                return;
                            }
                            AddNewTrade(newtrade);
                        }
                        else
                        {
                            responseHTML += "\n Card is already in db";
                        }
                    }
                    else
                    {
                        responseHTML += "\nNo Data to change";
                    }
                }
                else
                {
                    responseCode = 400;
                }
            }
            else
            {
                responseHTML += "\n Couldnt find User by token";
                responseCode = 400;
            }
            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
        }
        public async Task SpecificTradingsRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Specific Tranding Request {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            string subpath_user = userInfo?["subpath"];
            int responseCode = 200;
            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Specific Tranding Request </h1>";
            responseHTML += "\n Token: " + token;
            responseHTML += "\n FullBody: " + userInfo?["body"];
            responseHTML += "\n Subpath: " + subpath_user;

            UserRequests uRq = new UserRequests();
            UserEndpoint user = uRq.GetUserByToken(token);


            if (user != null)  // see if token is used by the right user
            {
                if (requesttype == "POST")
                {
                    responseHTML += "\n Get Trade Data \n\n";
                    TradeEndpoint tradeToGet = GetSpecificTrade(subpath_user);

                    if (tradeToGet.id != null && tradeToGet.user_id != user.Id) // if trade could be found and is not the same as author
                    {
                        if (fullinfo.Length > 0) // if body is filled
                        {
                            responseHTML += "\n Swap Trade";

                            fullinfo = fullinfo.Trim();

                            string card_id = fullinfo.Substring(1);
                            card_id = card_id.Remove(card_id.Length - 1);

                            CardRequests crq = new CardRequests();
                            Card myCard = crq.GetUserSpecificCards(card_id, user.Id, false, true); // my card, not in deck

                            if (myCard.Name != null) // card could be found
                            {
                                bool canTrade = true;
                                canTrade = CheckTradeParameter(tradeToGet, myCard);

                                if (canTrade)
                                {
                                    Console.WriteLine("In trading!");
                                    Console.WriteLine(tradeToGet.user_id);
                                    Console.WriteLine(tradeToGet.card_id);
                                    Console.WriteLine(user.Id);
                                    Console.WriteLine(myCard.Id);

                                    crq.AddCardToPlayerStack(myCard.Id, tradeToGet.user_id); // my card to trader
                                    crq.AddCardToPlayerStack(tradeToGet.card_id, user.Id); // traderscard to me

                                    responseHTML += "\n Swapped Cards!";

                                    DeleteSpecificTrade(tradeToGet.id);
                                    responseHTML += "\n Deleted Trade!";
                                }
                                else
                                {
                                    responseHTML += "\nParameters dont match!";
                                }
                            }
                            else
                            {
                                responseHTML += "\nCard couldnt be found";
                                responseHTML += "\nMake sure card is not in deck";
                            }
                        }
                        else
                        {
                            responseHTML += "\nNo Data in Body";
                        }
                    }
                    else
                    {
                        responseHTML += "\nTrade not found Or Author of Trade";
                    }
                }
                else if (requesttype == "DELETE")
                {
                    responseHTML += "\n Get Trade Data \n\n";
                    TradeEndpoint tradeToGet = GetSpecificTrade(subpath_user);

                    if (tradeToGet.id != null && tradeToGet.user_id == user.Id) // if trade could be found and is same as author
                    {
                        DeleteSpecificTrade(tradeToGet.id);
                        responseHTML += "\n Author Deleted Trade!";
                    }  
                }
                else
                {
                    responseHTML += "\n Wrong Method";
                    responseCode = 400;
                }
            }
            else
            {
                responseHTML += "\n Couldnt find User by token";
                responseCode = 400;
            }
            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
        }

        public bool SeeIfTradeIsINDB(string card_id)
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
            GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, card_id);

            var count = command.ExecuteScalar();

            if (count == null)
            {
                Console.WriteLine("AN ERROR OCCURED");
                return true;
            }

            if ((Int64)count > 0)
            {
                Console.WriteLine("card_id exists in the database.");
                return true;
            }
            else
            {
                Console.WriteLine("card_id does not exist in the database.");
                return false;
            }
        }
        public void AddNewTrade(TradeEndpoint newTrade)
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "INSERT INTO trades (id, card_id, card_fortype, card_mindmg, user_id) VALUES(@id, @card_id, @card_fortype, @card_mindmg, @user_id);";
            command.CommandText = query;

            GeneralRequests.AddParameterWithValue(command, "@id", DbType.String, newTrade.id);
            GeneralRequests.AddParameterWithValue(command, "@card_id", DbType.String, newTrade.card_id);
            GeneralRequests.AddParameterWithValue(command, "@card_fortype", DbType.String, newTrade.card_ForType);
            GeneralRequests.AddParameterWithValue(command, "@card_mindmg", DbType.Int32, newTrade.card_mindmg);
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, newTrade.user_id);

            command.ExecuteNonQuery();
        }
        public string GetAllTrades()
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM trades;";
            command.CommandText = query;

            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\n{\n\"Trades\":{\n";
            while (reader.Read())
            {
                count++;
                TradeEndpoint readTrade = new()
                {
                    id = (string)reader["id"],
                    card_ForType = (string)reader["card_fortype"],
                    card_mindmg = (int)reader["card_mindmg"],
                    user_id = (int)reader["user_id"]
                };

                jsonToSendBack += JsonSerializer.Serialize<TradeEndpoint>(readTrade);
            }
            jsonToSendBack += "\n}\n}\n";
            return jsonToSendBack;
        }
        public TradeEndpoint GetSpecificTrade(string tradeId)
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT * FROM trades WHERE id = @id;";
            command.CommandText = query;


            GeneralRequests.AddParameterWithValue(command, "@id", DbType.String, tradeId);

            using IDataReader reader = command.ExecuteReader();
            TradeEndpoint readTrade = new TradeEndpoint();
            while (reader.Read())
            {
                readTrade = new()
                {
                    id = (string)reader["id"],
                    card_id = (string)reader["card_id"],
                    card_ForType = (string)reader["card_fortype"],
                    card_mindmg = (int)reader["card_mindmg"],
                    user_id = (int)reader["user_id"]
                };
            }
            readTrade.PrintTrade();
            return readTrade;
        }
        public void DeleteSpecificTrade(string tradeId)
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "DELETE FROM trades WHERE id = @id;";
            command.CommandText = query;

            GeneralRequests.AddParameterWithValue(command, "@id", DbType.String, tradeId);

           command.ExecuteNonQuery();
        }
        public bool CheckTradeParameter(TradeEndpoint toget, Card myCard)
        {
            if (toget.card_ForType == myCard.CardType && toget.card_mindmg <= myCard.Damage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
