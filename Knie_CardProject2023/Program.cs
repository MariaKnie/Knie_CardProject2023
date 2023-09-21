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
                        usersList[i].Coins -= 5;
                        CardPackages package = new CardPackages();
                        usersList[i].Stack = package.BuyPackage(usersList[i].Stack);
                        usersList[i].Stack.PrintStack();
                    }
                    else
                    {
                        break;
                    }

                }    
            }


            

            Console.WriteLine($"\nFilling Deck");
            Console.ReadLine();
            for (int i = 0; i < usersList.Count; i++)
            {
                Console.WriteLine($"User [{i}] {usersList[i].Username}");

                usersList[i].Deck.Fill_Deck(usersList[i].Stack);
                
            }

            //usersList[0].Stack.PrintStack();
            //usersList[1].Stack.PrintStack();


            Console.WriteLine($"\nGame Start!");
            Console.WriteLine($"First Round: {rounds}");

            while (true)
            {
                //Game Loop


                 CompareCards(usersList, rounds);

                if (GameOver(usersList) == 0 || rounds == 100)
                {
                    break;
                }

                rounds++;
                Console.WriteLine($"\n\nNext Round: {rounds}");
            }




       
        }



        public static int GameOver(List<User> usersList)
        {
            int player; 
            if (usersList[0].Deck.Cards.Count > 0 && usersList[1].Deck.Cards.Count > 0)
            {
                return 1;
            }
            else
            {
                if (usersList[0].Deck.Cards.Count <= 0)
                {
                    player = 0;
                }
                else
                    player = 1;
            }

            Console.WriteLine($"\n\nUser {usersList[player].Username}: has no more cards!");
            return 0;
        }


        public static void CompareCards(List<User> usersList, int rounds)
        {
            Random rnd = new Random();
            int num = rnd.Next(usersList[0].Deck.Cards.Count);


            rnd = new Random();
            int num2 = rnd.Next(usersList[1].Deck.Cards.Count);
            int won;
            int lost;
            int toRemove;
            Console.WriteLine("****************************************************");
            Console.WriteLine($"User {usersList[0].Username}: Card [{num}]: ");
            usersList[0].Deck.Cards[num].PrintCard();
            Console.WriteLine(" V E R S U S ");
            Console.WriteLine($"User {usersList[1].Username}: Card [{num2}]: ");
            usersList[1].Deck.Cards[num2].PrintCard();
            Console.WriteLine("****************************************************");

            if (usersList[0].Deck.Cards[num].Damage > usersList[1].Deck.Cards[num2].Damage)
            {
                won = 0;
                lost = 1;
                toRemove = num2;
            }
            else
            {
                won = 1;
                lost = 0;
                toRemove = num;
            }


            Console.WriteLine($"User {usersList[won].Username}: Won round {rounds} !! \n       Deck-Cards [{usersList[won].Deck.Cards.Count}] +1");
            Console.WriteLine($"User {usersList[lost].Username}: Lost.  \n       Deck-Cards [{usersList[lost].Deck.Cards.Count}] -1");

           

            Card temp = usersList[lost].Deck.Cards[toRemove];
            usersList[lost].Deck.Cards.RemoveAt(toRemove);

            usersList[won].Deck.Cards.Add(temp);

        }
    }
}