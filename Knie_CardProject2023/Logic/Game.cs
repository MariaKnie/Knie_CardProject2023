using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            if (usersList[0].Deck.Cards.Count > 0 && usersList[1].Deck.Cards.Count > 0) // if both players have cards left continue
            {
                return 1;
            }
            else // one has no more cards
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
        public static string PrintRound(List<User> usersList, int num_0, int num_1)
        {
            string printround = "";
            string stars = "\n****************************************************";
            string player_0 = $"\nUser {usersList[0].Username}: \nCard [{num_0}]: ";
            string player_1 = $"\nUser {usersList[1].Username}: \nCard [{num_1}]: ";
            string versus = " \nV E R S U S ";

            printround += stars;

            printround += player_0;
            printround +=  usersList[0].Deck.Cards[num_0].PrintCard();

            printround += versus;

            printround += player_1;
            printround += usersList[1].Deck.Cards[num_1].PrintCard();

            printround += stars;

            Console.WriteLine(printround);
            return printround;
        }

        public static int CompareCards(List<User> usersList, int rounds, ref string battlelog) // Damage Comparison
        {
            Random rnd = new Random();
            int num_0 = rnd.Next(usersList[0].Deck.Cards.Count); // random card of first player
            rnd = new Random();
            int num_1 = rnd.Next(usersList[1].Deck.Cards.Count);// random card of second player

            List<Card> cards = new List<Card>(); // Cards usd in Battle
            cards.Add(usersList[0].Deck.Cards[num_0]);
            cards.Add(usersList[1].Deck.Cards[num_1]);

            float effect1 = 1; // effect Multiplyer for first Player
            float effect2 = 1;// effect Multiplyer for second Player

            float dmgPlayer1 = usersList[0].Deck.Cards[num_0].Damage; // damage of first Player
            float dmgPlayer2 = usersList[1].Deck.Cards[num_1].Damage; // damage of second Player

            // Game stats
            int won;
            int lost;
            int toRemove;
            bool draw = false;

           battlelog += PrintRound(usersList, num_0, num_1); // Prints username and card for round

            CheckEffect(cards, ref effect1, ref effect2, ref battlelog); // check if Multiplyers need to change
            SpecialityEffect(cards, ref effect1, ref effect2);// check if Specialties Apply

            // effects apply
            dmgPlayer1 = usersList[0].Deck.Cards[num_0].Damage * effect1; 
            dmgPlayer2 = usersList[1].Deck.Cards[num_1].Damage * effect2;


            //Check Who Won
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

            if (draw) // Both same Damage
            {
                string drawtext = $"\nDRAW!: Cards have the same Damage, round {rounds} !! \n       Deck-Cards stay the same!";
                battlelog += drawtext;
                Console.WriteLine(drawtext);

            }
            else // there is a winner
            {
                string wonText = $"\nUser {usersList[won].Username}: Won round {rounds} !! \n       Deck-Cards [{usersList[won].Deck.Cards.Count}] +1";
                string lostText = $"\nUser {usersList[lost].Username}: Lost.  \n       Deck-Cards [{usersList[lost].Deck.Cards.Count}] -1";
                battlelog += wonText;
                battlelog += lostText;

                Console.WriteLine(wonText);
                Console.WriteLine(lostText);

                Card temp = usersList[lost].Deck.Cards[toRemove];
                usersList[lost].Deck.Cards.RemoveAt(toRemove);

                usersList[won].Deck.Cards.Add(temp);            
            }
            return won;
        }
      
        public static void CheckEffect(List<Card> cards, ref float effect1, ref float effect2, ref string battlelog)
        {
            string dmgbefore = $"\nDamage: {cards[0].Damage} VS {cards[1].Damage} ";
            battlelog += dmgbefore;
            Console.WriteLine(dmgbefore);

            if (cards.Count ==2 && cards[0].CardType == "Spell" || cards[1].CardType == "Spell") // if any is spellcard
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].ElementType == Enum_ElementTypes.Water.ToString()) //  winner
                    {
                        ChangeEffect(i, cards, Enum_ElementTypes.Fire, ref effect1, ref effect2, 2f, 0.5f);
                    }
                    else if (cards[i].ElementType == Enum_ElementTypes.Electro.ToString()) // winner
                    {
                        // electrocuted 
                        ChangeEffect(i, cards, Enum_ElementTypes.Water, ref effect1, ref effect2, 2f, 0.25f);
                        // Fire explosion
                        ChangeEffect(i, cards, Enum_ElementTypes.Fire, ref effect1, ref effect2, 0.75f, 0.6f);
                    }
                }
                string dmgnew = $"\nNew Damage: {cards[0].Damage * effect1} VS {cards[1].Damage * effect2} ";
                battlelog += dmgnew;
                Console.WriteLine(dmgnew);
            }
            string effectText = $"\nEffect1: {effect1} \nEffect2: {effect2}";
            battlelog += effectText; 
            Console.WriteLine(effectText);
        }  
        public static void ChangeEffect(int i, List<Card> cards, Enum_ElementTypes element_type, ref float effect1, ref float effect2, float winner, float loser)
        {
            if (i < cards.Count - 1 && cards[i + 1].ElementType == element_type.ToString()) //loser at index 1
            {
                effect1 = winner;
                effect2 = loser;
            }
            else if (i > 0 && cards[i - 1].ElementType == element_type.ToString())//loser at index 0
            {
                effect1 = loser;
                effect2 = winner;
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
        public static void CheckNames(List<Card> cards, ref float effect1, ref float effect2, string winner, string loser)
        {
            for (int i = 0; i < cards.Count; i++) // Go through cards
            {
                if (cards[i].Name.Contains(winner))// if card is winner
                {
                    //if other card loser
                    if (i < cards.Count - 1 && cards[i + 1].Name.Contains(loser)) // loser at index 1
                    {
                        effect1 = 1;
                        effect2 = 0;
                    }
                    else if (i > 0 && cards[i - 1].Name.Contains(loser))// loser at index 0
                    {
                        effect1 = 0;
                        effect2 = 1;

                    }
                }
            }
        }
        public static void FillDecks(List<User> usersList, ref string battlelog)
        {
            string decktext = $"\nFilling Deck";
            battlelog += decktext;
            Console.WriteLine(decktext);
            for (int i = 0; i < usersList.Count; i++) // go through userList
            {
                string usertext = $"\nUser [{i}] {usersList[i].Username}";
                battlelog += usertext;
                Console.WriteLine(usertext);
                usersList[i].Deck.Fill_Deck(usersList[i].Stack);
            }
            //usersList[0].Stack.PrintStack();
            //usersList[1].Stack.PrintStack();
        }


        public static int GameLoop(List<User> usersList, int rounds, ref string battlelog)
        {
            string startText = $"\nGame Start!\n First Round: {rounds}";
            battlelog += startText;
            Console.WriteLine(startText);
            int winner = -2;

            while (true)//Game Loop stops at one user having no cards, or round 100 being reached
            {
                winner = Game.CompareCards(usersList, rounds, ref battlelog); // compar cards and get winner or draw

                if (Game.GameOver(usersList) == 0 || rounds == 100) // end condition
                {
                    if (rounds == 100 && Game.GameOver(usersList) != 0) // no winner in round 100
                    {
                        winner = -1; // no winner
                    }
                    break;
                }
                rounds++;
                string nextround = $"\n\nNext Round: {rounds}";
                battlelog += nextround;
                Console.WriteLine(nextround);
            }
            return winner;
        }
    }
}
