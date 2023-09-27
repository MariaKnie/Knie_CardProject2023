using Knie_CardProject2023.Logic;

namespace Knie_CardProject2023
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int rounds = 1;

            User Tom = new User("Tom", "123", 0, 0);
            User Max = new User("Max", "12345", 0, 0);

            
            List<User> usersList = new List<User>();
            usersList.Add(Tom); 
            usersList.Add(Max);


           


            Console.WriteLine($"Filling Stacks");
            Console.ReadLine();
            for (int i = 0; i < usersList.Count; i++)
            {
                Console.WriteLine($"User [{i}] {usersList[i].Username}" );
                usersList[i].Stack.Fill_Stack();
                Console.ReadLine();


                while (usersList[i].Coins >=5)
                {
                    Console.WriteLine($"{usersList[i].Username}, want to buy more Cards? Y/N");
                    string answer = Console.ReadLine();

                    if (answer == "Y")
                    {
                        PackageTrading.BuyPackage(usersList[i]);
                    }
                    else
                    {
                        break;
                    }
                }    
            } 

            Game.FillDecks(usersList);
            Game.GameLoop(usersList, rounds);
       
        }
    }
}