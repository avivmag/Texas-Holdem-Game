using System.Collections.Generic;
using Backend.Game;
using Backend.User;

namespace TestProject
{
    public class ProjectTest
    {
        private IBridge bridge;

        public ProjectTest()
        {
            bridge = Driver.getBridge();
          
        }
        public object register(string username, string password, string email, string picture)
        {
           return  bridge.register(username,password, email, picture);
        }
        public object login(string username, string password)
        {
           return bridge.login(username, password);
        }
        public object getUserbyName(string username)
        {
            return bridge.getUserbyName(username);
        }
        public bool isUserExist(string username, string password)
        {
            return bridge.isUserExist(username, password);
        }
        public bool checkActiveGame(string statusGame)
        {
            return bridge.checkActiveGame(statusGame);
        }
        public object logout(int userId)
        {
            return bridge.logout(userId);
        }
        public object editProfile(int userId, string username, string password, string email, string picture, int moneyAmount)
        {
            return bridge.editProfile(userId, username, password, email, picture, moneyAmount);
        }
        //public bool editImage(string img)
        //{
        //    return bridge.editImage(img);
        //}
        //public bool editName(string name)
        //{
        //    return bridge.editName(name);
        //}
        //public bool editEmail(string email)
        //{
        //    return bridge.editEmail(email);
        //}

        public object creatGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            return bridge.creatGame(gameCreator, gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, MinimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague);
        }
        public bool isLogin(string username)
        {
            return bridge.isLogin(username);
        }
        public bool isGameDefOK(string gameDefinithions)
        {
            return bridge.isGameDefOK(gameDefinithions);
        }
        public object addPlayerToGame(int userId, int gameId,int seat)
        {
            return bridge.addPlayerToGame(userId,gameId, seat);
        }
        public object selectGametoJoin(string game)
        {
            return bridge.selectGametoJoin(game);
        }
        public bool checkAvailibleSeats(string game)
        {
            return bridge.checkAvailibleSeats(game);
        }
        public object spectateActiveGame(int userId, int gameId)
        {
            return bridge.spectateActiveGame(userId,gameId);
        }
        public bool exitGame(string game)
        {
            return bridge.exitGame(game);
        }
        public object removeUserFromGame(SystemUser user, int gameId)
        {
            return bridge.removeUserFromGame(user, gameId);
        }
               
        public object findAllActive()
        {
            return bridge.findAllActive();
        }
        public string filterByCriteria(string criteria)
        {
            return bridge.filterByCriteria(criteria);
        }
        public bool isGameOver(int gameId)
        {
            return bridge.isGameOver(gameId);
        }
        
        public bool canBet(TexasHoldemGame game, int amount)
        {
            return bridge.canBet(game, amount);
        }
        public bool updatePot(int amount)
        {
            return bridge.updatePot(amount);
        }
        public bool updateStatePlayer(string statePlayer, int amount)
        {
            return bridge.updateStatePlayer(statePlayer, amount);
        }
        public bool canRaise(TexasHoldemGame game, int amount)
        {
            return bridge.canRaise(game, amount);
        }
        //public bool call(int amount)
        //{
        //    return bridge.canCall(amount);
        //}
        public bool fold()
        {
            return bridge.fold();
        }
        public bool check()
        {
            return bridge.check();
        }
        public bool betSmallBlind(int amount)
        {
            return bridge.betSmallBlind(amount);
        }
        public bool betBigBlind(int amount)
        {
            return bridge.betBigBlind(amount);
        }

        public bool removeUser(int userId)
        {
            return bridge.removeUser(userId);
        }
        public bool removeGame(int gameId)
        {
            return bridge.removeGame(gameId);
        }
        //public bool PlayersWithCriteria(string criteria, string players)
        //{
        //    return this.bridge.PlayersWithCriteria(criteria, players);
        //}
    }   

}
