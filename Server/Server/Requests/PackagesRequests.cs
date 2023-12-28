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
using System.Collections.Concurrent;

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
            int responseCode = 200;

            Console.WriteLine(fullinfo);

            responseHTML += "<html> <body> \n";
            responseHTML += $"<h1> {requesttype} Buying Package </h1>";
            responseHTML += "\n FullBody: " + fullinfo;
            responseHTML += "\n Token: " + token;



            if (requesttype == "POST")
            {
                // - 5 coind + 5 cards to stack
                UserRequests ur = new UserRequests();
                UserEndpoint user = ur.GetUserByToken(token);

                if (user != null)
                {
                    responseHTML += "\n Found User by token";
                    if (CheckUserAbleToBuy(user))
                    {
                        responseHTML += "\n Able to Buy Package";
                        BuyPackage(user, ref responseHTML);
                        responseHTML += "\n Bought Package";
                    }
                    else
                    {
                        responseHTML += "\n UNABLE to Buy Package, too little coins";
                        responseCode = 500;
                    }
                }
                else
                {
                    responseHTML += "\n Couldnt find User by token";
                    responseCode = 400;
                }
            }
            else
                responseCode = 400;


            responseHTML += "\n</body> </html>";
            response.UniqueResponse(writer, responseCode, description, responseHTML);
        }

       

        public bool CheckUserAbleToBuy(UserEndpoint user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.Coins < 5)
            {
                return false;
            }


            return true;
        }


        public void BuyPackage(UserEndpoint user, ref string html)
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
            GeneralRequests.AddParameterWithValue(command, "@id", DbType.Int32, user.Id);


            command.ExecuteNonQuery();



            // Add Package to Userstack
            CardPackages packagee = new CardPackages();
            PackageEndPoint newPackage = new PackageEndPoint();
               newPackage.packageCard = packagee.CreatePackageDB();

            CardRequests cr = new CardRequests();
            int DeckCount = cr.CheckIfUserHasDeckCards(user);
            bool todeck = false;

            if (DeckCount == 0)
            {
                todeck = true;
            }

            Console.WriteLine("ADDING TO STACK"); 
            for (int i = 0; i < newPackage.packageCard.Count; i++)
            {
                if (todeck && i == 4)
                {
                    todeck = false;
                }
                Console.WriteLine($"add card number {i}"); 
                AddCardtoCardTable(newPackage.packageCard[i], user.Id, todeck);
                html += $"\n {newPackage.packageCard[i].PrintCard()}";
            }

        }


        public void AddCardtoCardTable(Card newCard, int userid, bool deck)
        {
            var connString = "Host=localhost; Username=postgres; Password=postgres; Database=mydb";
            using IDbConnection connection = new NpgsqlConnection(connString);
            connection.Open();

            // command
            using IDbCommand command = connection.CreateCommand();
            string query = "INSERT INTO cards (id, card_name, card_type, card_element, card_damage, card_description, user_id, card_indeck) VALUES(@id, @card_name, @card_type, @card_element, @card_damage, @card_description, @user_id, @card_indeck);";
            command.CommandText = query;

            // parameters
            IDFactory factory = new IDFactory();
            string id = factory.generateID();

            string description = "" + newCard.Description;

            GeneralRequests.AddParameterWithValue(command, "@id", DbType.String, id);
            GeneralRequests.AddParameterWithValue(command, "@card_name", DbType.String, newCard.Name);
            GeneralRequests.AddParameterWithValue(command, "@card_type", DbType.String, newCard.CardType);
            GeneralRequests.AddParameterWithValue(command, "@card_element", DbType.String, newCard.ElementType);
            GeneralRequests.AddParameterWithValue(command, "@card_damage", DbType.Int32, newCard.Damage);
            GeneralRequests.AddParameterWithValue(command, "@card_description", DbType.String, description);
            GeneralRequests.AddParameterWithValue(command, "@user_id", DbType.Int32, userid);
            GeneralRequests.AddParameterWithValue(command, "@card_indeck", DbType.Boolean, deck);

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
