using Knie_CardProject2023;
using Knie_CardProject2023.Logic;
using Knie_CardProject2023.Server;
using Microsoft.VisualBasic;
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
    internal class GeneralRequests
    {
        public static void AddParameterWithValue(IDbCommand command, string paramName, DbType dbType, object value)
        {
            command.Parameters.Add(new NpgsqlParameter(paramName, dbType) { Value = value });
        }
        public async Task SessionRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {

            HTTP_Response response = new HTTP_Response();
            string description = $"Session Request {requesttype}";
            string responseHTML = "";

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}  Login User Request </h1>";

            string fullinfo = userInfo?["body"];
            UserEndpoint userToEndpoint = JsonSerializer.Deserialize<UserEndpoint>(fullinfo);
            responseHTML += $"\n Username: {userToEndpoint.Username}";
            responseHTML += $"\n Password: {userToEndpoint.Password}";

            if (requesttype == "GET")
            {

            }
            else if (requesttype == "POST")

            { // check if token for user is active 
              // if no active then login
              // if active then deny

                int id = UserLoginDataCheck(userToEndpoint);
                if (id >= 0)
                {
                    // Login Data is correct
                    responseHTML += "\n UserData Correct";

                    if (!UserLoginTokenCheck(id))
                    {
                        responseHTML += "\n User able to login";
                        // User isnt logged in yet
                        AddTokenForUser(id, userToEndpoint);
                        responseHTML += "\n User logged in, Token Set";
                    }
                    else
                    {

                        responseHTML += "\n User already Logged in!";
                    }
                }
                else
                {
                    responseHTML += "\n UserData Incorrect";
                }

            }
            else if (requesttype == "DEL")
            {

            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);

        }


        int UserLoginDataCheck(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT id FROM users WHERE Username = @Username AND  Password = @Password;";
            command.CommandText = query;

            // parameters
            //command.Parameters.AddWithValue("@Username", username);
            AddParameterWithValue(command, "@Username", DbType.String, user.Username);
            AddParameterWithValue(command, "@Password", DbType.String, user.Password);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("User couldnt be found");
                return -1;
            }

            Console.WriteLine("User found!");
            int id = (int)result;

            return id;
        }
        bool UserLoginTokenCheck(int id)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT token FROM tokens WHERE user_id = @id";
            command.CommandText = query;

            // parameters
            //command.Parameters.AddWithValue("@Username", username);
            AddParameterWithValue(command, "@id", DbType.Int32, id);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("User token not active yet");
                return false;
            }

            Console.WriteLine("User token already active");
            return true;
        }

        public void AddTokenForUser(int id, UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "INSERT INTO tokens (token, user_id) VALUES (@token, @user_id)";
            command.CommandText = query;

            // parameters
            //command.Parameters.AddWithValue("@Username", username);
            string token = user.Username + "-mtcgToken";
            AddParameterWithValue(command, "@token", DbType.String, token);
            AddParameterWithValue(command, "@user_id", DbType.Int32, id);

            command.ExecuteNonQuery();
        }

        public void DeleteTokens()
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "DELETE FROM tokens;";
            command.CommandText = query;

            command.ExecuteNonQuery();
        }


        public async Task ScoreboardRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Scoreboard Request{requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Scoreboard Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                if (user != null)
                {

                    responseHTML += $"\nGetting Scoreboard \n";
                    responseHTML += GetPlayerScoreboard(user);
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }
        public string GetPlayerScoreboard(UserEndpoint user)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT username, wins FROM users ORDER BY wins;";
            command.CommandText = query;


            using IDataReader reader = command.ExecuteReader();
            int count = 0;
            string jsonToSendBack = "\nUser Score Board\n";
            while (reader.Read())
            {
                count++;
                UserEndpoint user_Read = new()
                {
                    Username = (string)reader["username"],
                    Wins = (int)reader["wins"]
                };

                jsonToSendBack += $"\nPlace: {count}, User: {user_Read.Username}, Wins: {user_Read.Wins}";

            }

            return jsonToSendBack;
        }


        public static List<List<User>> playerlist = new List<List<User>>();
        public static List<List<string>> deckcard_ids = new List<List<string>>();

        public async Task BattleRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Battle Request {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Battle Request </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "GET")
            {
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                User Carduser = new User(user.Username, user.Password, user.Wins, user.Loses, user.Id);
                if (user != null)
                {
                    responseHTML += "\n Looking for Game";
                    bool foundSpot = false;
                    bool battle = false;

                    List<Card> PlayerDeck = new List<Card>();
                    ur.GetPlayerDeckCards(ref Carduser);

                    int posinPlayList = 0;


                    // Game.GameLoop(usersList, 0);

                    if (playerlist.Count < 1) // first player
                    {
                        playerlist.Add(new List<User>());
                        playerlist[0].Add(Carduser);
                        responseHTML += "\n First Playeer, Waiting for Players";
                    }
                    else
                    {


                        for (int i = 0; i < playerlist.Count; i++)
                        {
                            if (playerlist[i].Count() < 2)
                            {
                                foundSpot = true;
                                responseHTML += "\n Found Spot!";

                                playerlist[i].Add(Carduser);
                                posinPlayList = i;
                                if (playerlist[i].Count() == 2)  // can play
                                {
                                    battle = true;
                                }
                            }
                            else
                            {
                                responseHTML += "\n Waiting for Player";
                            }
                            break;
                        }
                    }

                    if (!foundSpot)
                    {
                        playerlist.Add(new List<User>());
                        playerlist[playerlist.Count - 1].Add(Carduser);
                        responseHTML += "\n Waiting for Player";
                    }

                    if (battle) // start battle
                    {
                        List<User> playersOfRound = playerlist[posinPlayList];

                        for (int p = 0; p < playersOfRound.Count; p++)
                        {
                            deckcard_ids.Add(new List<string>());// for every player a list of ids
                            for (int c = 0; c < playersOfRound[p].Deck.Cards.Count; c++)
                            {
                                deckcard_ids[deckcard_ids.Count - 1].Add(playersOfRound[p].Deck.Cards[c].Id);
                            }
                        }

                        responseHTML += "\n Battle Begin!";

                        Game.GameLoop(playersOfRound, 0); // Actual Battle

                        for (int p = 0; p < playersOfRound.Count; p++) // take over cards
                        {
                            for (int c = 0; c < playersOfRound[p].Deck.Cards.Count; c++)
                            {
                                if (!deckcard_ids[p].Contains(playersOfRound[p].Deck.Cards[c].Id))
                                {
                                    ChangeCardToPlayer((playersOfRound[p].Deck.Cards[c].Id), playersOfRound[p].Id);
                                }
                            }
                        }

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


        public void ChangeCardToPlayer(string card_id, int user_id)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "UPDATE cards SET user_id = @user_id WHERE id = @card_id;";

            command.CommandText = query;

            // parameters
            AddParameterWithValue(command, "@user_id", DbType.Int32, user_id);
            AddParameterWithValue(command, "@card_id", DbType.String, card_id);

            command.ExecuteNonQuery();
        }


        public async Task TradingsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"TradingsRequest {requesttype}", $"<html> <body> <h1> {requesttype} TradingsRequest Request! </h1> </body> </html>");

        }
        public async Task SpecificTradingsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"Specific TradingsRequest {requesttype}", $"<html> <body> <h1> {requesttype} Specific TradingsRequest Request! </h1> </body> </html>");

        }

    }
}
