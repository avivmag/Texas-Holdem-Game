using System.Collections.Generic;

namespace TestProject
{
    class ProxyBridge : Bridge
    {
        private Bridge real;

        public ProxyBridge()
        {
            real = null;
        }
        public void setRealBridge(Bridge implementation)
        {
            if (real == null)
                real = implementation;
        }
        public object register(string username, string password)
        {
            if (username.Equals("Hadas") && password.Equals("1234"))
                return username;
              return "";
        }
        public object login(string username, string password)
        {
            if (username.Equals("Hadas") && password.Equals("1234"))
                return username;
              return "";
        }
        public object getUserbyName(string username)
        {
            if(username.Equals("Hadas"))
                return username;
            return "";
        }
        public bool isUserExist(string username, string password)
        {
            return (username.Equals("Hadas") && password.Equals("1234"));
            
        }
        public bool checkActiveGame(string satusGame)
        {
            return (satusGame.Equals("Active"));
        }
        public bool logoutUser(string game, string user)
        {
            return (game.Equals("notActive") && user.Equals("Hadas"));
        }
        public object editProfile(string username)
        {
            if (username.Equals("Hadas"))
                return username;
            return null;
        }
        public bool editImage(string img)
        {
            return (img.Equals("img"));
        }
        public bool editName(string name)
        {
            return (name.Equals("Hadas"));
        }
        public bool editEmail(string email)
        {
            return (email.Equals("gmail@gmail.com"));          
        }
        public object creatGame(string game)
        {
            if (game.Equals("Texas1"))
                return game;
            return "can't create";
        }
        public bool isLogin(string username)
        {
            return (username.Equals("Hadas"));
        }
        public bool isGameDefOK(string gameDefinithions)
        {
            return (gameDefinithions.Equals("def"));
         
        }
        public bool addPlayerToGame(string username, string game)
        {
            return (username.Equals("Hadas") && game.Equals("Texas1"));
        }
        public object selectGametoJoin(string game)
        {
            if (game.Equals("Texas1"))
                return game;
            return null;
        }
        public bool checkAvailibleSeats(string game)
        {
            return (game.Equals("Texas1"));
        }
        public bool spectateActiveGame(string game)
        {
            return (game.Equals("Texas1"));
        }
        public bool exitGame(string game)
        {
            return (game.Equals("Texas1"));
        }
        public int removeUserFromGame(string user, string game)
        {
            return 1;
        }
        public object selectGameToReplay(string game)
        {
            if(game.Equals("notActive"))
                return game;
            return "";
        }
        public bool isWatchingReplay(string game)
        {
            return true;
        }
        public bool saveTurn(string game)
        {
            return (game.Equals("Texas1") || game.Equals("notActive"));
        }

        public string findAllActive()
        {
           string active = "Active games";
            return active;
        }
        public string filterByCriteria(string criteria)
        {
            if (criteria.Equals("points"))
                return criteria;
               return "";
        }
        public bool storeGameData()
        {
            return true;
        }
        public bool isGameOver(string game, string username)
        {
            return (game.Equals("Texas1"));
        }
        public object joinLeaguePerPoints(int points)
        {
            if (points == 100)
                return "league #1";
            return "league #10";
        }
        public bool updatePot(int amount)
        {
            return (amount!=0 );
        }
        public bool updateStatePlayer(string statePlayer, int amount)
        {
            return (statePlayer.Equals("Player Bet") && amount > 0);
        }
        public bool bet(int amount)
        {
            return (amount > 0);
        }
        public bool raise(int amount)
        {
            return (amount > 0);
        }
        public bool call(int amount)
        {
            return (amount > 0);
        }
        public bool fold()
        {
            return true;
        }
        public bool check()
        {
            return true;
            
        }
        public bool betSmallBlind(int amount)
        {
            return (amount > 0);
        }
        public bool betBigBlind(int amount)
        {
            return ((amount % 2) == 0);
        }
        public bool isLeagueExist(string criteria)
        {
            return (criteria.Equals("Points"));
        }

        public bool setCriteriaForNewLeague(string criteria)
        {
            return (criteria.Equals("new league"));
        }
        public bool PlayersWithCriteria(string criteria, string players)
        {
            return (!criteria.Equals("Points") && players.Equals("Alufim"));
        }
    }
}
