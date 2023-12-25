using Knie_CardProject2023.Server;
using Knie_CardProject2023;
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
    internal class PackagesRequests
    {


        public async Task PackageResponse(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"PackageResponse {requesttype}", $"<html> <body> <h1> {requesttype} PackageResponse Request! </h1> </body> </html>");

        }

        public async Task PackagesRequest(StreamWriter writer, string requesttype)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $"Packages {requesttype}", $"<html> <body> <h1> {requesttype} Packages Request! </h1> </body> </html>");
        }

        public async Task TransactionsPackagesRequest(StreamWriter writer, string requesttype, Dictionary<string, string> userInfo)
        {
            HTTP_Response response = new HTTP_Response();
            string description = $"Buying Packag {requesttype}";
            string responseHTML = "";
            string fullinfo = userInfo?["body"];
            string token = userInfo?["token"];

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Buying Package </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "POST")
            {
                // - 5 coind + 5 cards to stack

                UserEndpoint user = GetUserByToken(token);

                if (CheckUserAbleToBuy(user))
                {
                    responseHTML += "\n Able to Buy Package";
                    BuyPackage(user.id);
                }
                else
                {
                    responseHTML += "\n UNABLE to Buy Package, too little coins";
                }
            }


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, 200, description, responseHTML);
        }

        static void AddParameterWithValue(IDbCommand command, string paramName, DbType dbType, object value)
        {
            command.Parameters.Add(new NpgsqlParameter(paramName, dbType) { Value = value });
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
            AddParameterWithValue(command, "@token", DbType.String, token);

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
            AddParameterWithValue(command, "@id", DbType.Int32, id);

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user = new()
                {
                    id = (int)reader["id"],
                    Username = (string)reader["username"],
                    Password = (string)reader["password"],
                    age = 0,
                    description ="",
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

        public bool CheckUserAbleToBuy(UserEndpoint user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.coins < 5)
            {
                return false;
            }


            return true;
        }


        public void BuyPackage(int id)
        {

            // Connection
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "UPDATE users SET coins = coins - 5 WHERE id = @id;";
            command.CommandText = query;

            // parameters
            AddParameterWithValue(command, "@id", DbType.Int32, id);


            command.ExecuteNonQuery();



            // Add Package to Userstack
            CardPackages packagee = new CardPackages();
            PackageEndPoint newPackage = new PackageEndPoint();
               newPackage.package = packagee.CreatePackageDB();

            for (int i = 0; i < newPackage.package.Count; i++)
            {
                AddCardtoCardTable(newPackage.package[i], id);  
            }

        }


        public void AddCardtoCardTable(Card newCard, int userid)
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "INSERT INTO cards (id, card_name, card_type, card_element, card_damage, card_description, user_id) VALUES(@id, @card_name, @card_type, @card_element, @card_damage, @card_description, @user_id);";
            command.CommandText = query;

            // parameters
            IDFactory factory = new IDFactory();
            string id = factory.generateID();

            string description = "" + newCard.Description;

            AddParameterWithValue(command, "@id", DbType.String, id);
            AddParameterWithValue(command, "@card_name", DbType.String, newCard.Name);
            AddParameterWithValue(command, "@card_type", DbType.String, newCard.CardType);
            AddParameterWithValue(command, "@card_element", DbType.String, newCard.ElementType);
            AddParameterWithValue(command, "@card_damage", DbType.Int32, newCard.Damage);
            AddParameterWithValue(command, "@card_description", DbType.String, description);
            AddParameterWithValue(command, "@user_id", DbType.Int32, userid);

            newCard.PrintCard();
            command.ExecuteNonQuery();
        }

        public async Task SpecificTransactionsPackagesRequest(StreamWriter writer, string Http_type)
        {
            HTTP_Response response = new HTTP_Response();
            response.UniqueResponse(writer, 200, $" Specific TransactionsPackagesRequest {Http_type}", $"<html> <body> <h1> {Http_type} Specific TransactionsPackagesRequest Request! </h1> </body> </html>");

        }

    }
}
