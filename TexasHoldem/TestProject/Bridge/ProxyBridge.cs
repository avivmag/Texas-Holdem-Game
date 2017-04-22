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
            if (username == "Hadas" && password == "1234")
                return username;
              return "";
        }
        public object login(string username, string password)
        {
            if (username == "Hadas" && password == "1234")
                return username;
              return "";
        }
        public object getUserbyName(string username)
        {
            if(username == "Hadas")
                return username;
            return "";
        }
        public bool isUserExist(string username, string password)
        {
            if (username == "Hadas" && password == "1234")
                return true;
             return false;
        }
        public bool checkActiveGame(string satusGame)
        {
            if(satusGame == "Active")
                return true;
            return false;
        }
        public bool logoutUser(string game, string user)
        {
            if (game == "notActive" && user == "Hadas")
                return true;
            else 
                return false;
        }
        public object editProfile(string username)
        {
            return null;
        }
        public bool editImage(string img)
        {
            return true;
        }
        public bool editName(string name)
        {
            return true;
        }
        public bool editEmail(string email)
        {
            return true;
        }
        public object creatGame(string gameDefinitions)
        {
            return null;
        }
        public bool isLogin(string username)
        {
            if(username == "Hadas")
                return true;
            return false;
        }
        public bool isGameDefOK(string gameDefinithions)
        {
            return true;
        }
        public bool addPlayerToGame(string username, string game)
        {
            return true;
        }
        public object selectGametoJoin(string game)
        {
            if (game == "Texas1")
                return game;
            return null;
        }
        public bool checkAvailibleSeats(string game)
        {
            if(game == "Texas1")
                return true;
            return false;
        }
        public bool spectateActiveGame(string game)
        {
            return false;
        }
        public bool exitGame(string game)
        {
            if(game == "Texas1")
                return true;
            return false;
        }
        public int removeUserFromGame(string user, string game)
        {
            return 1;
        }
        public object selectGameToReplay(string game)
        {
            if(game == "notActive")
                return game;
            return "";
        }
        public bool isWatchingReplay(string game)
        {
            return true;
        }
        public bool saveTurn(string game)
        {
            if(game == "Texas1" || game == "notActive")
                return true;
            return false;
        }

        public List<string> findAllActive()
        {
            return null;
        }
        public List<string> filterByCriteria(string criteria)
        {
            return null;
        }
        public bool storeGameData()
        {
            return true;
        }
        public bool isGameOver(string game, string username)
        {
            if (game == "Texas1")
                return true;
            return false;
        }
        public object joinLeaguePerPoints(int points)
        {
            if (points == 100)
                return "league #1";
            return "league #10";
        }
        public bool updatePot(int amount)
        {
            return true;
        }
        public bool updateStatePlayer(string statePlayer, int amount)
        {
            return false;
        }
        public bool bet(int amount)
        {
            return false;
        }
        public bool raise(int amount)
        {
            return false;
        }
        public bool call(int amount)
        {
            return false;
        }
        public bool fold()
        {
            return true;
        }
        public bool check()
        {
            return false;
        }
        public bool betSmallBlind(int amount)
        {
            return false;
        }
        public bool betBigBlind(int amount)
        {
            return false;
        }
    }
}
