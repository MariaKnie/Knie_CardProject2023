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

            string name = userInfo?["subpath"];

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype}  Specific User Reques </h1>";
            responseHTML += $"\n Username: {name}";
            responseHTML += "\n Token: " + userInfo?["token"];
            responseHTML += "\n FullBody: " + userInfo?["subpath"];

            if (requesttype == "GET")
            {
              //change data of user
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
                    id = (int)reader["id"],
                    Username = (string)reader["username"],
                    Password = (string)reader["password"],
                    age = 0,
                    description = "",
                    coins = (int)reader["coins"]

                };

                int columnIndex = reader.GetOrdinal("age");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.description = (string)reader["age"];
                }
                columnIndex = reader.GetOrdinal("description");
                if (!reader.IsDBNull(columnIndex))
                {

                    user.description = (string)reader["description"];
                }

            }

            return user;
        }
    }
    
}
