using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Knie_CardProject2023
{
    public class User
    {
        private static int nextUserid = 1;

        private int id;
        private string username;
        private string password;
        private int wins;
        private int loses;
        private int coins = 20;
        private int elo = 100;
        private int matches;


        private CardDeck deck = new CardDeck();
        private CardStack stack = new CardStack();

        public User(string username, string password, int wins, int loses)
        {
            this.id = nextUserid++; 
            this.username = username;
            this.password = password;
            this.wins = wins;
            this.loses = loses;

        }
        public User(string username, string password, int wins, int loses, int id, int matches, int eLO)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.wins = wins;
            this.loses = loses;
            this.matches = matches;
            this.elo = eLO;
        }
        public User(string username, string password, int wins, int loses, int id, int matches, CardDeck deck, CardStack stack, int eLO)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.wins = wins;
            this.loses = loses;
            this.matches = matches;
            this.deck = deck;
            this.stack = stack;
            this.elo = eLO;
        }

        public int ELO
        {
            get { return elo; }
            set { elo = value; }
        }

        public int Matches
        {
            get { return matches; }
            set { matches = value; }
        }
        public int Id
        {
            get { return id; }
            set { }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public int Wins
        {
            get { return wins; }
            set { wins++; }
        }
        public int Loses
        {
            get { return loses; }
            set { loses++; }
        }

        public int Coins
        {
            get { return coins; }
            set { coins = value; }
        }

        public CardDeck Deck
        {
            get { return deck; }
            set { }
        }
        public CardStack Stack
        {
            get { return stack; }
            set { }
        }

        public void DrawCard()
        {

        }
        public void RemoveCard()
        {

        }
        public void TradeCard()
        {

        }

    }
}
