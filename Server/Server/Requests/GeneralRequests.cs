using Knie_CardProject2023.Server;
using Npgsql;
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
                if (id >=0)
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

        public async Task StatsRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"StatsRequest {requesttype}", $"<html> <body> <h1> {requesttype} StatsRequest Request! </h1> </body> </html>");
        }
        public async Task ScoreboardRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"ScoreboardRequest {requesttype}", $"<html> <body> <h1> {requesttype} ScoreboardRequest Request! </h1> </body> </html>");
        }


        public async Task BattleRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"BattleRequest {requesttype} ", $"<html> <body> <h1> {requesttype} BattleRequest Request! </h1> </body> </html>");
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
