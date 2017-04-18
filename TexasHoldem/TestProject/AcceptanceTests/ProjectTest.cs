using System.Collections.Generic;

namespace TestProject
{
    public class ProjectTest
    {
        private Bridge bridge;

        public ProjectTest()
        {
            this.bridge = Driver.getBridge();
          
        }
        public object register(string username, string password)
        {
           return  this.bridge.register(username,password);
        }
        public object login(string username, string password)
        {
           return this.bridge.login(username, password);
        }
        public object getUserbyName(string username)
        {
            return this.bridge.getUserbyName(username);
        }
        public bool isUserExist(string username, string password)
        {
            return this.bridge.isUserExist(username, password);
        }
        public bool checkActiveGame(string statusGame)
        {
            return this.bridge.checkActiveGame(statusGame);
        }
        public bool logoutUser(string game, string user)
        {
            return this.bridge.logoutUser(game, user);
        }
        public object editProfile(string username)
        {
            return this.bridge.editProfile(username);
        }
        public bool editImage(string img)
        {
            return this.bridge.editImage(img);
        }
        public bool editName(string name)
        {
            return this.bridge.editName(name);
        }
        public bool editEmail(string email)
        {
            return this.bridge.editEmail(email);
        }

        public object creatGame(string gameDefinitions)
        {
            return this.bridge.creatGame(gameDefinitions);
        }
        public bool isLogin(string username)
        {
            return this.bridge.isLogin(username);
        }
        public bool isGameDefOK(string gameDefinithions)
        {
            return this.bridge.isGameDefOK(gameDefinithions);
        }
        public bool addPlayerToGame(string username, string game)
        {
            return this.bridge.addPlayerToGame(username, game);
        }
        public  object selectGametoJoin(string game)
        {
            return this.bridge.selectGametoJoin(game);
        }
        public bool checkAvailibleSeats(string game)
        {
            return this.bridge.checkAvailibleSeats(game);
        }
        public bool spectateActiveGame(string game)
        {
            return this.bridge.spectateActiveGame(game);
        }
        public bool exitGame(string game)
        {
            return this.bridge.exitGame(game);
        }
        public int removeUserFromGame(string user, string game)
        {
            return this.bridge.removeUserFromGame(user, game);
        }
        public object selectGameToReplay(string game)
        {
            return this.bridge.selectGameToReplay(game);
        }
        public bool isWatchingReplay(string game)
        {
            return this.bridge.isWatchingReplay(game);
        }
        public bool saveTurn(string game)
        {
            return this.bridge.saveTurn(game);
        }
       
        public List<string> findAllActive()
        {
            return this.bridge.findAllActive();
        }
        public List<string> filterByCriteria(string criteria)
        {
            return this.bridge.filterByCriteria(criteria);
        }
        public bool storeGameData()
        {
            return this.bridge.storeGameData();
        }
        public bool isGameOver(string game, string username)
        {
            return this.bridge.isGameOver(game, username);
        }
        public object joinLeaguePerPoints(int points)
        {
            return this.bridge.joinLeaguePerPoints(points);
        }
        public bool bet(int amount)
        {
            return this.bridge.bet(amount);
        }
        public bool updatePot(int amount)
        {
            return this.bridge.updatePot(amount);
        }
        public bool updateStatePlayer(string statePlayer, int amount)
        {
            return this.bridge.updateStatePlayer(statePlayer, amount);
        }
        public bool raise(int amount)
        {
            return this.bridge.raise(amount);
        }
        public bool call(int amount)
        {
            return this.bridge.call(amount);
        }
        public bool fold()
        {
            return this.bridge.fold();
        }
        public bool check()
        {
            return this.bridge.check();
        }

    } 

}
