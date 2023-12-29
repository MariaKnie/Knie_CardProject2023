using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Knie_CardProject2023.Enums.Card_Enums;
using static System.Net.Mime.MediaTypeNames;

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
            int num_0 = rnd.Next(usersList[0].Deck.Cards.Count);


            rnd = new Random();
            int num_1 = rnd.Next(usersList[1].Deck.Cards.Count);
            int won;
            int lost;
            int toRemove;
            bool draw = false;
            Console.WriteLine("****************************************************");
            Console.WriteLine($"User {usersList[0].Username}: Card [{num_0}]: ");
            usersList[0].Deck.Cards[num_0].PrintCard();
            Console.WriteLine(" V E R S U S ");
            Console.WriteLine($"User {usersList[1].Username}: Card [{num_1}]: ");
            usersList[1].Deck.Cards[num_1].PrintCard();
            Console.WriteLine("****************************************************");

            float effect1 = 1;
            float effect2 = 1;
            List<Card> cards = new List<Card>();
            cards.Add(usersList[0].Deck.Cards[num_0]);
            cards.Add(usersList[1].Deck.Cards[num_1]);

            Console.WriteLine($"Damage: {usersList[0].Deck.Cards[num_0].Damage} VS {usersList[1].Deck.Cards[num_1].Damage} ");

            CheckEffect(cards, ref effect1, ref effect2);
            Console.WriteLine($"Effect1: {effect1} \nEffect2: {effect2}");

            SpecialityEffect(cards, ref effect1, ref effect2);

            float dmgPlayer1 = usersList[0].Deck.Cards[num_0].Damage * effect1;
            float dmgPlayer2 = usersList[1].Deck.Cards[num_1].Damage * effect2;


            if (dmgPlayer1 > dmgPlayer2) // player 1 won
            {
                won = 0;
                lost = 1;
                toRemove = num_1;
            }
            else if (dmgPlayer1 < dmgPlayer2) // player 2 won
            {
                
                won = 1;
                lost = 0;
                toRemove = num_0;
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
            }

            return won;

        }
        public static void CheckEffect(List<Card> cards, ref float effect1, ref float effect2)
        {
            if (cards.Count ==2 && cards[0].CardType == "Spell" || cards[1].CardType == "Spell") // if any is spellcard
            {

                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].ElementType == Enum_ElementTypes.Water.ToString()) //  winner
                    {
                        if (i < cards.Count - 1 && cards[i + 1].ElementType == Enum_ElementTypes.Fire.ToString())
                        {
                            effect1 = 2f;
                            effect2 = 0.5f;
                        }
                        else if (i > 0 && cards[i - 1].ElementType == Enum_ElementTypes.Fire.ToString())
                        {
                            effect1 = 0.5f;
                            effect2 = 2f;
                        }
                    }
                    else if (cards[i].ElementType == Enum_ElementTypes.Electro.ToString()) // winner
                    {
                        // electrocuted 
                        if (i < cards.Count - 1 && cards[i + 1].ElementType == Enum_ElementTypes.Water.ToString())
                        {
                            effect1 = 2f;
                            effect2 = 0.25f;
                        }
                        else if (i > 0 && cards[i - 1].ElementType == Enum_ElementTypes.Water.ToString())
                        {
                            effect1 = 0.25f;
                            effect2 = 2f;
                        }

                        // Fire explosion
                        if (i < cards.Count - 1 && cards[i + 1].ElementType == Enum_ElementTypes.Fire.ToString())
                        {
                            effect1 = 0.75f;
                            effect2 = 0.6f;
                        }
                        else if (i > 0 && cards[i - 1].ElementType == Enum_ElementTypes.Fire.ToString())
                        {
                            effect1 = 0.6f;
                            effect2 = 0.75f;
                        }
                    }
                }
                Console.WriteLine($"New Damage: {cards[0].Damage * effect1} VS {cards[1].Damage * effect2} ");

            }
        }  
        public static void SpecialityEffect(List<Card> cards, ref float effect1, ref float effect2)
        {
            CheckNames(cards, ref effect1, ref effect2, "Dragon", "Goblin");
            CheckNames(cards, ref effect1, ref effect2, "Wizzard", "Ork");
            CheckNames(cards, ref effect1, ref effect2, "Knight", "WaterSpell");
            CheckNames(cards, ref effect1, ref effect2, "Kraken", "Spell");
            CheckNames(cards, ref effect1, ref effect2, "FireElve", "Dragon");
        }

        public static void CheckNames(List<Card> cards, ref float effect1, ref float effect2, string first, string second)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].Name.Contains(first))
            {
                    if (i < cards.Count - 1 && cards[i + 1].Name.Contains(second))
                    {
                        effect1 = 1;
                        effect2 = 0;
                    }
                    else if (i > 0 && cards[i - 1].Name.Contains(second))
                    {
                        effect1 = 0;
                        effect2 = 1;

                    }
                }
            }
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


        public static int GameLoop(List<User> usersList, int rounds)
        {

            Console.WriteLine($"\nGame Start!");
            Console.WriteLine($"First Round: {rounds}");

            int winner = -2;

            while (true)//Game Loop
            {
                winner = Game.CompareCards(usersList, rounds);

                if (Game.GameOver(usersList) == 0 || rounds == 100)
                {
                    if (rounds == 100)
                    {
                        winner = -1;
                    }
                    break;
                }

                rounds++;
                Console.WriteLine($"\n\nNext Round: {rounds}");
            }

            return winner;
        }
    }
}
