using Knie_CardProject2023;
using Knie_CardProject2023.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data;
using Npgsql;
using Server.Server.Requests;
using Microsoft.VisualBasic;
using Server.Server.EndPoints;

namespace Server.Server.Requests
{
    internal class UserRequests
    {

        public async Task UserRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"General User {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];

            Console.WriteLine(fullinfo);
            UserEndpoint userToEndpoint = JsonSerializer.Deserialize<UserEndpoint>(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Registered User </h1>";
            responseHTML += $"\n Username: {userToEndpoint.Username} Password: {userToEndpoint.Password}";
            responseHTML += "\n FullBody: " + userInfo?["body"];


            if (requesttype == "GET")
            {
            }
            else if (requesttype == "POST")
            {
                if (SeeIfUserIsINDB(userToEndpoint.Username) == false) // if user isnt already registered
                {
                    responseHTML += "\n Username is AVAILABLE";
                    responseHTML += "\n Rgistration continue.";

                    AddUsertoDb(userToEndpoint);
                    responseHTML += "\n Rgistration Complete!";

                }
                else
                {
                    responseHTML += "\n Username is UNAVAILABLE";
                }
            }
            else if (requesttype == "DEL")
            {
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }

        public async Task UserSpecificRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Specific User {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            string subpath_user = userInfo?["subpath"];
            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Specific User </h1>";
            responseHTML += "\n Token: " + token;
            responseHTML += "\n FullBody: " + userInfo?["body"];
            responseHTML += "\n Subpath: " + subpath_user;

            UserEndpoint user = GetUserByToken(token);

            if (user != null && user.Username == subpath_user)
            {

                if (requesttype == "GET")
                {
                    if (fullinfo.Length > 0) // change user data
                    {
                        responseHTML += "\n Change User Data";

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
                                    parts2[b] = parts2[b].Remove(parts2[b].Length - 1);
                                    parts2[b] = parts2[b].Substring(1);
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
                                Console.WriteLine($" \nUser Snip {i} : \nKey: " + InfoToChange_Dic.ElementAt(InfoToChange_Dic.Count - 1).Key + "\nValue: " + InfoToChange_Dic.ElementAt(InfoToChange_Dic.Count-1).Value);
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


                        ChangeUserData(user, InfoToChange_Dic);

                    }
                    else // get user data
                    {
                        responseHTML += "\n Get User Data \n\n";
                        responseHTML += "\n { \"User\": {";

                        responseHTML += JsonSerializer.Serialize<UserEndpoint>(user);
                        responseHTML += "\n } \n}";
                    }
                }


            }
            else
            {
                responseHTML += "\n Couldnt find User by token";
            }
            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);

        }


        public void ChangeUserData(UserEndpoint user, Dictionary<string, string> newUserdata)
        {

            for (int i = 0; i < newUserdata.Count; i++)
            {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            Console.WriteLine("IN CHANGE\n");
                using IDbCommand command = connection.CreateCommand();
                string col = newUserdata.ElementAt(i).Key;
                string query = "UPDATE users SET " + col + " = @value WHERE id = @id;";

                Console.WriteLine(newUserdata.ElementAt(i).Key + " - " + newUserdata.ElementAt(i).Value);

                command.CommandText = query;

                // parameters
                GeneralRequests.AddParameterWithValue(command, "@id", DbType.Int32, user.Id);
                GeneralRequests.AddParameterWithValue(command, "@value", DbType.String, newUserdata.ElementAt(i).Value);

                command.ExecuteNonQuery();
            }
        }

        public bool SeeIfUserIsINDB(string username)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM users WHERE Username = @Username;";
            command.CommandText = query;

            // parameters
            //command.Parameters.AddWithValue("@Username", username);
            GeneralRequests.AddParameterWithValue(command, "@Username", DbType.String, username);


            var count = command.ExecuteScalar();

            if (count == null)
            {
                Console.WriteLine("AN ERROR OCCURED");
                return true;
            }


            if ((Int64)count > 0)
            {
                Console.WriteLine("Username exists in the database.");
                return true;
            }
            else
            {
                Console.WriteLine("Username does not exist in the database.");
                return false;
            }

        }


        public void AddUsertoDb(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
            command.CommandText = query;

            // parameters
            //IDFactory factory = new IDFactory();
            //string id = factory.generateID();
            //Console.WriteLine($"username = {user.Username}, password = {user.Password} ");
            //AddParameterWithValue(command, "@id", DbType.String, id);
            GeneralRequests.AddParameterWithValue(command, "@username", DbType.String, user.Username);
            GeneralRequests.AddParameterWithValue(command, "@password", DbType.String, user.Password);




            command.ExecuteNonQuery();
        }



        public UserEndpoint GetUserByToken(string token)
        {
            UserEndpoint user = null;

            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT user_id FROM tokens WHERE token = @token";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@token", DbType.String, token);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("User token doesnt exist");
                return null;
            }

            int id = (int)result;
            Console.WriteLine($"User id found = {id}");

            // ----------------------------gets user now by id
            // command
            query = "SELECT * FROM users WHERE id = @id";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@id", DbType.Int32, id);

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = new()
                {
                    Id = (int)reader["id"],
                    Username = (string)reader["username"],
                    Password = (string)reader["password"],
                    Age = 0,
                    Bio = "",
                    Image = "",
                    Coins = (int)reader["coins"],
                    Wins = (int)reader["wins"],
                    Loses = (int)reader["loses"]

                };

                int columnIndex = reader.GetOrdinal("age");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.Age = (int)reader["age"];
                }
                columnIndex = reader.GetOrdinal("bio");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.Bio = (string)reader["bio"];
                }
                columnIndex = reader.GetOrdinal("image");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.Image = (string)reader["image"];
                }

            }

            return user;
        }


        public async Task StatsRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Stats Request {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Stats Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                if (user != null)
                {
                    int matches = user.Wins + user.Loses;
                    float win_perc = 0;
                    float lose_perc = 0;
                    if (matches > 0)
                    {
                        win_perc = ((float)user.Wins / (float)matches) * 100;
                        lose_perc = ((float)user.Loses / (float)matches) * 100;
                    }

                    responseHTML += $"\nWins: {user.Wins}";
                    responseHTML += $"\nLoses: {user.Loses}";

                    responseHTML += $"\nWinChance: {win_perc }%";
                    responseHTML += $"\nLoseChance: {lose_perc}%";
                    responseHTML += $"\nMatches Played: {matches}";
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }



        public void GetPlayerDeckCards(ref User user)
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
            Console.WriteLine("READING CARDS FROM PLAYER " + user.Username); 
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

                if (readCard.CardType == "Spell")
                {
                    SpellCard spellCard = new SpellCard(readCard.id, readCard.Name, readCard.Damage, readCard.CardType, readCard.ElementType, readCard.Description);
                    user.Deck.Cards.Add(spellCard);
                    spellCard.PrintCard();
                }
                else if (readCard.CardType == "Monster")
                {
                    Monstercard spellCard = new Monstercard(readCard.id, readCard.Name, readCard.Damage, readCard.CardType, readCard.ElementType, readCard.Description);
                    user.Deck.Cards.Add(spellCard);
                    spellCard.PrintCard();
                }


            }
            Console.WriteLine();


        }
    }
}
