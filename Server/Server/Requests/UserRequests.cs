﻿using Knie_CardProject2023;
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

        public async Task StatsRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Stats Request {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            int responseCode = 200;
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
                    int matches = user.Matches;
                    float win_perc = 0;
                    float lose_perc = 0;
                    float drawchance = 0;
                    float deciededmatches = 0;
                    if (matches > 0) // to avoid math exeptions
                    {
                        win_perc = ((float)user.Wins / (float)matches) * 100;
                        lose_perc = ((float)user.Loses / (float)matches) * 100;
                        deciededmatches = user.Loses + user.Wins;
                        deciededmatches = (float)user.Matches - deciededmatches;
                        drawchance = ((deciededmatches) / (float)user.Matches) * 100;

                    }
                    // write stats
                    responseHTML += $"\nWins: {user.Wins}";
                    responseHTML += $"\nLoses: {user.Loses}";
                    responseHTML += $"\nDraws: {deciededmatches}";

                    responseHTML += $"\nWinChance: {win_perc}%";
                    responseHTML += $"\nLoseChance: {lose_perc}%";
                    responseHTML += $"\nDrawChance: {drawchance}%";
                    responseHTML += $"\nMatches Played: {user.Matches}";
                    responseHTML += $"\nELO: {user.Elo}";
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
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
        public async Task UserRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"General User {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            int responseCode = 200;

            Console.WriteLine(fullinfo);
            UserEndpoint userToEndpoint = JsonSerializer.Deserialize<UserEndpoint>(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Registered User </h1>";
            responseHTML += $"\n Username: {userToEndpoint.Username} Password: {userToEndpoint.Password}";
            responseHTML += "\n FullBody: " + userInfo?["body"];


            if (requesttype == "POST")
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
        public async Task UserSpecificRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Specific User {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];
            string subpath_user = userInfo?["subpath"];
            int responseCode = 200;
            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Specific User </h1>";
            responseHTML += "\n Token: " + token;
            responseHTML += "\n FullBody: " + userInfo?["body"];
            responseHTML += "\n Subpath: " + subpath_user;

            UserEndpoint user = GetUserByToken(token);

            string temptoken = token.Substring(0, token.Length - 10); // if token matches with / subpath

            if (user != null && temptoken == subpath_user)  // see if token is used by the right user
            {
                if (requesttype == "GET") // get user data
                {
                    responseHTML += "\n Get User Data \n\n";
                    responseHTML += "\n { \"User\": {";

                    responseHTML += JsonSerializer.Serialize<UserEndpoint>(user);
                    responseHTML += "\n } \n}";
                }
                else if (requesttype == "PUT") 
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

                        bool canchange = true;
                        if (InfoToChange_Dic.ContainsKey("username")) 
                        {
                            canchange = !SeeIfUserIsINDB(InfoToChange_Dic["username"]);// if name is free
                        }

                        if (canchange)
                        {
                            ChangeUserData(user, InfoToChange_Dic);
                        }
                        else
                        {
                            responseHTML += "\nUsername is already taken";
                        }
                    }
                    else
                    {
                        responseHTML += "\nNo Data to change";
                    }
                }
                else
                {
                    responseHTML += "\nWrong Method";
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
            string query = "INSERT INTO users (username, password, coins) VALUES (@username, @password, @coins)";
            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@username", DbType.String, user.Username);
            GeneralRequests.AddParameterWithValue(command, "@password", DbType.String, user.Password);
            GeneralRequests.AddParameterWithValue(command, "@coins", DbType.Int32, 20);

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
                    Loses = (int)reader["loses"],
                    Elo = (int)reader["elo"],
                    Matches = (int)reader["matches"]

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
        public void GetUserDeckCards(ref User user)
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
        public void ChangeUserStats(User user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "UPDATE users SET wins = @wins, loses = @loses, matches = @matches, elo = @elo WHERE id = @user_id;";

            command.CommandText = query;

            // parameters
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, user.Id);
            GeneralRequests.AddParameterWithValue(command, "@wins", DbType.Int32, user.Wins);
            GeneralRequests.AddParameterWithValue(command, "@loses", DbType.Int32, user.Loses);
            GeneralRequests.AddParameterWithValue(command, "@elo", DbType.Int32, user.ELO);
            GeneralRequests.AddParameterWithValue(command, "@matches", DbType.Int32, user.Matches);

            command.ExecuteNonQuery();
        }

    }
}
