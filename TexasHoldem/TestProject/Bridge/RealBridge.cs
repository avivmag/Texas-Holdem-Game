using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade;
using Backend.Game;
using Backend.User;
using SL;

namespace TestProject.Bridge
{
    public class RealBridge : IBridge
    {
        private SLInterface sl;

        public RealBridge()
        {
            sl = new SLImpl();
        }

        public object register(string username, string password, string email, string picture)
        {
            return sl.Register(username, password, email, picture);
        }

        public object login(string username, string password)
        {
            return sl.Login(username, password);
        }

        public object logout(int userId)
        {
            return sl.Logout(userId);
        }

        public object editProfile(int userId, string username, string password, string email, string picture, int moneyAmount)
        {
            return sl.editUserProfile(userId, username, password, email, picture, moneyAmount);
        }

        //public bool editEmail(string email)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool editImage(string img)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool editName(string name)
        //{
        //    throw new NotImplementedException();
        //}

        public object getUserbyName(string username)
        {
            return sl.getUserByName(username);
        }

        public bool isLogin(string username)
        {
            List<SystemUser> allUsers = (List<SystemUser>)sl.getAllUsers();
            foreach (SystemUser user in allUsers)
                if (user.name == username)
                    return true;
            return false;
        }

        public bool isUserExist(string username, string password)
        {
            var user = sl.getUserByName(username);
            if (user == null)
                return false;
            return true;
        }
        

        





        public object creatGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            return sl.createGame(gameCreator, gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, MinimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague);
        }

        public object addPlayerToGame(int userId, int gameId)
        {
            return sl.joinActiveGame(userId, gameId);
        }

        public object selectGametoJoin(string game)
        {
            throw new NotImplementedException();
        }
        
        public bool checkActiveGame(string statusGame)
        {
            throw new NotImplementedException();
        }
        
        public bool isGameDefOK(string gameDefinithions)
        {
            throw new NotImplementedException();
        }

        public bool checkAvailibleSeats(string game)
        {
            throw new NotImplementedException();
        }

        public object spectateActiveGame(int userId, int gameId)
        {
            return sl.spectateActiveGame(userId, gameId);
        }

        public bool exitGame(string game)
        {
            throw new NotImplementedException();
        }

        public object removeUserFromGame(SystemUser user, int game)
        {
            return sl.leaveGame(user, game);
        }

        public bool isGameOver(int gameId)
        {
            object game = sl.GetGameState(gameId);
            if (game != null && !((TexasHoldemGame)game).active)
                return true;
            return false;
        }




        public object findAllActive()
        {
            return sl.findAllActiveAvailableGames();
        }

        public string filterByCriteria(string criteria)
        {
            throw new NotImplementedException();
        }




        public bool canBet(TexasHoldemGame game, SystemUser user, int amount)
        {
            return game.gamePreferences.canPerformGameActions(game,user, amount, "Bet").success;
        }

        public bool canRaise(TexasHoldemGame game, SystemUser user, int amount)
        {
            return game.gamePreferences.canPerformGameActions(game, user, amount, "Raise").success;
        }

        public bool canCall(TexasHoldemGame game, SystemUser user, int amount)
        {
            return game.gamePreferences.canPerformGameActions(game, user, amount, "Call").success;
        }

        public bool fold()
        {
            throw new NotImplementedException();
        }

        public bool check()
        {
            throw new NotImplementedException();
        }

        public bool updatePot(int amount)
        {
            throw new NotImplementedException();
        }

        public bool updateStatePlayer(string statePlayer, int amount)
        {
            throw new NotImplementedException();
        }

        public bool betBigBlind(int amount)
        {
            throw new NotImplementedException();
        }

        public bool betSmallBlind(int amount)
        {
            throw new NotImplementedException();
        }



        public bool removeUser(int userId)
        {
            return (bool)sl.removeUser(userId);
        }


        public bool removeGame(int gameId)
        {
            return (bool)sl.removeGame(gameId);
        }

    }
}
