﻿using Knie_CardProject2023;
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
using System.Threading;
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


        public static Dictionary<int, List<User>> playerLobbylist = new Dictionary<int, List<User>>();
        public static Dictionary<int, List<List<string>>> deckcard_ids = new Dictionary< int, List<List<string>>>();
        public static Dictionary<int, Dictionary<string, string>> GameLog = new Dictionary< int, Dictionary<string, string>>();
        static int currentLobbycount = 0;
        static Mutex mutex_Playerlist = new Mutex();
        static Mutex mutex_GameLog = new Mutex();
        static Mutex mutex_Deckids = new Mutex();
        // would need a mutex list for better performance;
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
            bool battle = false;
            int posinPlayList = 0;

            Console.WriteLine("Lobbycount : " + playerLobbylist.Count);

            if (requesttype == "GET")
            {
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                User Carduser = new User(user.Username, user.Password, user.Wins, user.Loses, user.Id, user.Matches, user.Elo);
                if (user != null)
                {
                    responseHTML += "\n Looking for Game Lobby";
                    bool foundSpot = false;

                    List<Card> PlayerDeck = new List<Card>();
                    ur.GetPlayerDeckCards(ref Carduser);

                    mutex_Playerlist.WaitOne();
                    for (int i = 0; i < playerLobbylist.Count; i++)
                    {
                        if (playerLobbylist.ElementAt(i).Value.Count() == 1) // found free spot
                        {
                            foundSpot = true;
                            posinPlayList = playerLobbylist.ElementAt(i).Key; // get lobby number
                            playerLobbylist[posinPlayList].Add(Carduser); // add user

                            responseHTML += "\n Found Spot!";
                            Console.WriteLine("Found Spot!");

                            if (playerLobbylist[posinPlayList].Count() == 2)  // can play
                            {
                                battle = true;
                            }                            
                            break;
                        }                     
                    }             

                    if (!foundSpot) // No spots open
                    {
                        foundSpot = true;
                        posinPlayList = currentLobbycount++; // neew lobby count

                        playerLobbylist.Add(posinPlayList, new List<User>()); // open lobby
                        playerLobbylist[posinPlayList].Add(Carduser); // add user

                        GameLog.Add(posinPlayList, new Dictionary<string, string>()); // open log
                        GameLog[posinPlayList].Add("Battle", "null"); // add log

                        responseHTML += "\n Single Playeer, Waiting for Players";
                        Console.WriteLine("Single Player");
                    }
                    mutex_Playerlist.ReleaseMutex();

                    if (battle) // start battle, last player to join stats battle
                    {
                        List<User> playersOfRound = playerLobbylist[posinPlayList]; // get user in lobby
                        GetPlayerDecksIDS(playersOfRound, posinPlayList); // get Player card ids

                        responseHTML += "\n Battle Begin!";
                        int status = Game.GameLoop(playersOfRound, 0); // Actual Battle

                        if (status < 0) // Game return status, errorcodes
                        {
                            if (status == -1) // draw
                            {
                                GameLog[posinPlayList].Add("Log", $"\nDraw!");
                            }
                        }
                        else // there is a winner
                        {
                            Console.WriteLine($"STATUS {status}!");
                            Console.WriteLine($"Player {playersOfRound[status].Username} Won!");

                            GameLog[posinPlayList].Add("Log", $"\nPlayer {playersOfRound[status].Username} Won!");
                            UpdatePlaylistAfterBattle(playersOfRound, status, posinPlayList); // update cards and user stats
                        }

                        GameLog[posinPlayList]["Battle"] = "done"; // cue for first player
                    }

                    while (GameLog[posinPlayList]["Battle"] != "done") //waiting for second player
                    {
                        //Console.WriteLine("\n Waiting for Player");
                    }
                    responseHTML += GameLog[posinPlayList]["Log"]; // players get battle log
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
                }
            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);

            // Clear list and clos lobby
            mutex_Playerlist.WaitOne();
            mutex_Deckids.WaitOne();
            mutex_GameLog.WaitOne();
            if (GameLog[posinPlayList].ContainsKey("Ended"))
            {
                GameLog[posinPlayList]["Ended"] = "2";

                // clear
                playerLobbylist[posinPlayList].Clear();
                GameLog[posinPlayList].Clear();
                deckcard_ids[posinPlayList].Clear();

                // remove
                playerLobbylist.Remove(posinPlayList);
                GameLog.Remove(posinPlayList);
                deckcard_ids.Remove(posinPlayList);
            }
            else
            {
                GameLog[posinPlayList].Add("Ended", "1");
            }
            mutex_Playerlist.ReleaseMutex();
            mutex_Deckids.ReleaseMutex();
            mutex_GameLog.ReleaseMutex();

            Console.WriteLine("Lobbycount : " + playerLobbylist.Count);
        }


        public void GetPlayerDecksIDS(List<User> playersOfRound, int pos)
        {
            mutex_Deckids.WaitOne();
            deckcard_ids.Add( pos, new List<List<string>>());// for current lobby
            for (int p = 0; p < playersOfRound.Count; p++)
            {
                deckcard_ids[pos].Add(new List<string>());// for every player a list of ids

                for (int c = 0; c < playersOfRound[p].Deck.Cards.Count; c++)
                {
                    deckcard_ids[pos][p].Add(playersOfRound[p].Deck.Cards[c].Id);
                    deckcard_ids[pos][p].Add(playersOfRound[p].Deck.Cards[c].Id);
                }
            }
            mutex_Deckids.ReleaseMutex();
        }
        public void UpdatePlaylistAfterBattle(List<User> playersOfRound, int status, int posinLobby)
        {
            for (int p = 0; p < playersOfRound.Count; p++) // take over cards
            {
                if (status == p)
                {
                    playersOfRound[p].Wins++;

                }
                else
                {
                    playersOfRound[p].Loses++;
                }

                for (int c = 0; c < playersOfRound[p].Deck.Cards.Count; c++)
                {
                    if (!deckcard_ids[posinLobby][p].Contains(playersOfRound[p].Deck.Cards[c].Id))
                    {
                        ChangeCardToPlayer((playersOfRound[p].Deck.Cards[c].Id), playersOfRound[p].Id);
                    }
                }
            }
        }



        public void ChangeCardToPlayer(string card_id, int user_id)
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
