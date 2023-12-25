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

namespace Server.Server.UserRequests
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

            string name = userInfo?["subpath"];

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}  Specific User Reques </h1>";
            responseHTML += $"\n Username: {name}";
            responseHTML += "\n Token: " + userInfo?["token"];
            responseHTML += "\n FullBody: " + userInfo?["subpath"];

            if (requesttype == "GET")
            {
                // check if token is active 
                // if no active then login
                // if active then deny
            }
            else if (requesttype == "POST")
            {

            }
            else if (requesttype == "DEL")
            {

            }

            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);

        }



        static void AddParameterWithValue(IDbCommand command, string paramName, DbType dbType, object value)
        {
            command.Parameters.Add(new NpgsqlParameter(paramName, dbType) { Value = value });
        }
        public bool SeeIfUserIsINDB(string username)
        {
            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "SELECT COUNT(*) FROM users WHERE Username = @Username";
            command.CommandText = query;

            // parameters
            //command.Parameters.AddWithValue("@Username", username);
            AddParameterWithValue(command, "@Username", DbType.String, username);


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
            AddParameterWithValue(command, "@username", DbType.String, user.Username);
            AddParameterWithValue(command, "@password", DbType.String, user.Password);




            command.ExecuteNonQuery();
        }
    }
    
}
