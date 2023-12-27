using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knie_CardProject2023.Logic
{
    public class Game
    {



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



        public static int CompareCards(List<User> usersList, int rounds)
        {
            Random rnd = new Random();
            int num = rnd.Next(usersList[0].Deck.Cards.Count);


            rnd = new Random();
            int num2 = rnd.Next(usersList[1].Deck.Cards.Count);
            int won;
            int lost;
            int toRemove;
            bool draw = false;
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
            else if (usersList[0].Deck.Cards[num].Damage < usersList[1].Deck.Cards[num2].Damage)
            {
                
                won = 1;
                lost = 0;
                toRemove = num;
            }
            else // Draw
            {
                won = -1;
                lost = -1;
                toRemove = -1;
                draw = true;
            }

            if (draw)
            {
                Console.WriteLine($"DRAW!: Cards have the same Damage,  round {rounds} !! \n       Deck-Cards stay the same!");

            }
            else
            {
                Console.WriteLine($"User {usersList[won].Username}: Won round {rounds} !! \n       Deck-Cards [{usersList[won].Deck.Cards.Count}] +1");
                Console.WriteLine($"User {usersList[lost].Username}: Lost.  \n       Deck-Cards [{usersList[lost].Deck.Cards.Count}] -1");
                Card temp = usersList[lost].Deck.Cards[toRemove];
                usersList[lost].Deck.Cards.RemoveAt(toRemove);

                usersList[won].Deck.Cards.Add(temp);
                usersList[won].Wins++;
                usersList[lost].Loses++;
            }

            return won;

        }

        public static void FillDecks(List<User> usersList)
        {
            Console.WriteLine($"\nFilling Deck");
            for (int i = 0; i < usersList.Count; i++)
            {
                Console.WriteLine($"User [{i}] {usersList[i].Username}");

                usersList[i].Deck.Fill_Deck(usersList[i].Stack);

            }

            //usersList[0].Stack.PrintStack();
            //usersList[1].Stack.PrintStack();

        }


        public static void GameLoop(List<User> usersList, int rounds)
        {

            Console.WriteLine($"\nGame Start!");
            Console.WriteLine($"First Round: {rounds}");

            while (true)//Game Loop
            {
                Game.CompareCards(usersList, rounds);

                if (Game.GameOver(usersList) == 0 || rounds == 100)
                {
                    break;
                }

                rounds++;
                Console.WriteLine($"\n\nNext Round: {rounds}");
            }
        }
    }
}
