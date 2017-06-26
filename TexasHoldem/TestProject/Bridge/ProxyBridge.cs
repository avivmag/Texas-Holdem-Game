using System.Collections.Generic;
using Backend.Game;
using Backend.User;
using System.Drawing;

namespace TestProject
{
    class ProxyBridge : IBridge
    {
        private IBridge real;

        public ProxyBridge()
        {
            real = null;
        }
        public void setRealBridge(IBridge implementation)
        {
            if (real == null)
                real = implementation;
        }
        public object register(string username, string password, string email, Image picture)
        {
            if (real != null)
                return real.register(username, password, email, picture);
            if (real != null)
                return real.register(username, password, email, picture);
            else if (username.Equals("Hadas") && password.Equals("1234"))
                return username;
              return "";
        }
        public object login(string username, string password)
        {
            if (real != null)
                return real.login(username, password);
            if (username.Equals("Hadas") && password.Equals("1234"))
                return username;
              return "";
        }
        public object logout(int userId)
        {
            if (real != null)
                return real.logout(userId);
            return 1;
        }
        public object getUserbyName(string username)
        {
            if (real != null)
                return real.getUserbyName(username);
            if (username.Equals("Hadas"))
                return username;
            return "";
        }
        public bool isUserExist(string username, string password)
        {
            if (real != null)
                return real.isUserExist(username, password);
            return (username.Equals("Hadas") && password.Equals("1234"));   
        }
        public bool checkActiveGame(string statusGame)
        {
            if (real != null)
                return real.checkActiveGame(statusGame);
            return (statusGame.Equals("Active"));
        }
        public object editProfile(int userId, string username, string password, string email, Image picture, int amount)
        {
            if (real != null)
                return real.editProfile(userId, username, password, email, picture, amount);
            if (username.Equals("Hadas"))
                return username;
            return null;
        }
        //public bool editImage(string img)
        //{
        //    return (img.Equals("img"));
        //}
        //public bool editName(string name)
        //{
        //    if (real != null)
        //        return real.betSmallBlind(amount);
        //    return (name.Equals("Hadas"));
        //}
        //public bool editEmail(string email)
        //{
        //    if (real != null)
        //        return real.editEmail(email);
        //    return (email.Equals("gmail@gmail.com"));          
        //}
        public object creatGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            if (real != null)
                return real.creatGame(gameCreator, gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, MinimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague);

            return "can't create";
        }
        public bool isLogin(string username)
        {
            if (real != null)
                return real.isLogin(username);
            return (username.Equals("Hadas"));
        }
        public bool isGameDefOK(string gameDefinithions)
        {
            if (real != null)
                return real.isGameDefOK(gameDefinithions);
            return (gameDefinithions.Equals("def"));
         
        }
        public object addPlayerToGame(int userId, int gameId,int seat)
        {
            if (real != null)
                return real.addPlayerToGame(userId, gameId,seat);
            return true;
            // return (username.Equals("Hadas") && game.Equals("Texas1"));
        }
        public object selectGametoJoin(string game)
        {
            if (real != null)
                return real.selectGametoJoin(game);
            if (game.Equals("Texas1"))
                return game;
            return null;
        }
        public bool checkAvailibleSeats(string game)
        {
            if (real != null)
                return real.checkAvailibleSeats(game);
            return (game.Equals("Texas1"));
        }
        public object spectateActiveGame(int userId, int gameId)
        {
            if (real != null)
                return real.spectateActiveGame(userId, gameId);
            return true;
            //return (game.Equals("Texas1"));
        }
        public bool exitGame(string game)
        {
            if (real != null)
                return real.exitGame(game);
            return (game.Equals("Texas1"));
        }
        public object removeUserFromGame(SystemUser user, int gameId)
        {
            if (real != null)
                return real.removeUserFromGame(user, gameId);
            return 1;
        }
        
        public object findAllActive()
        {
            if (real != null)
                return real.findAllActive();
            string active = "Active games";
            return active;
        }
        public string filterByCriteria(string criteria)
        {
            if (real != null)
                return real.filterByCriteria(criteria);
            if (criteria.Equals("points"))
                return criteria;
               return "";
        }
        //public bool storeGameData()
        //{
        //    return true;
        //}
        public bool isGameOver(int gameId)
        {
            if (real != null)
                return real.isGameOver(gameId);
            return true;
            //return (game.Equals("Texas1"));
        }

        //public object joinLeaguePerPoints(int points)
        //{
        //    if (points == 100)
        //        return "league #1";
        //    return "league #10";
        //}

        public bool updatePot(int amount)
        {
            if (real != null)
                return real.updatePot(amount);
            return (amount!=0 );
        }
        public bool updateStatePlayer(string statePlayer, int amount)
        {
            if (real != null)
                return real.updateStatePlayer(statePlayer, amount);
            return (statePlayer.Equals("Player Bet") && amount > 0);
        }
        public bool canBet(TexasHoldemGame game, int amount)
        {
            if (real != null)
                return real.canBet(game, amount);
            return (amount>0);
        }
        public bool canRaise(TexasHoldemGame game, int amount)
        {
            if (real != null)
                return real.canRaise(game,amount);
            return (amount > 0);
        }
        public bool canCall(TexasHoldemGame game, int amount)
        {
            if (real != null)
                return real.canCall(game,amount);
            return (amount > 0);
        }
        public bool fold()
        {
            if (real != null)
                return real.fold();
            return true;
        }
        public bool check()
        {
            if (real != null)
                return real.check();
            return true;
        }
        public bool betSmallBlind(int amount)
        {
            if (real != null)
                return real.betSmallBlind(amount);
            return (amount > 0);
        }
        public bool betBigBlind(int amount)
        {
            if (real != null)
                return real.betBigBlind(amount);
            return ((amount % 2) == 0);
        }


        public bool removeUser(int userId)
        {
            if (real != null)
                return real.removeUser(userId);
            return true;
        }
        public bool removeGame(int gameId)
        {
            if (real != null)
                return real.removeGame(gameId);
            return true;
        }
        //public bool isLeagueExist(string criteria)
        //{
        //    return (criteria.Equals("Points"));
        //}

        //public bool setCriteriaForNewLeague(string criteria)
        //{
        //    return (criteria.Equals("new league"));
        //}
        //public bool PlayersWithCriteria(string criteria, string players)
        //{
        //    return (!criteria.Equals("Points") && players.Equals("Alufim"));
        //}
    }
}
