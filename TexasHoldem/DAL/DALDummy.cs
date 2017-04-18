using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class DALDummy : DALInterface
    {
        private SystemUser[] userDummies;
        private Player[] playerDummies;
        private Spectator[] spectatorDummies;
        private TexasHoldemGame[] gameDummies;
        

        public DALDummy()
        {
            userDummies = new SystemUser[4];
            userDummies[0] = new SystemUser("Hadas","Aa123456","email0","image0",1000);
            userDummies[1] = new SystemUser("Gili", "123123", "email1","image1", 0);
            userDummies[2] = new SystemUser("Or", "111111", "email2", "image2", 700);
            userDummies[3] = new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500);
            for (int i = 0; i < 4; i++)
                userDummies[i].id = i;

            gameDummies = new TexasHoldemGame[6];
            gameDummies[0] = new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true));
            gameDummies[1] = new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false));
            gameDummies[2] = new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true));
            gameDummies[3] = new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false));
            gameDummies[4] = new LeagueTexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false), 0, 5);
            gameDummies[5] = new LeagueTexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false), 5, 10);
            for (int i = 0; i < 6; i++)
                gameDummies[i].id = i;

            playerDummies = new Player[4];
            playerDummies[0] = new Player(0,100,userDummies[0].rank);
            playerDummies[1] = new Player(1, 0, userDummies[1].rank);
            playerDummies[2] = new Player(2, 200, userDummies[2].rank);
            playerDummies[3] = new Player(3, 200, userDummies[3].rank);
            for (int i = 0; i < 4; i++)
                playerDummies[i].id = i;

        }

        public TexasHoldemGame getGameById(int gameID)
        {
            for (int i = 0; i < gameDummies.Length; i++)
                if (gameDummies[i].id == gameID)
                    return gameDummies[i];
            return null;
        }

        public SystemUser getUserById(int userID)
        {
            for (int i = 0; i < userDummies.Length; i++)
                if (userDummies[i].id == userID)
                    return userDummies[i];
            return null;
        }

        public List<TexasHoldemGame> getAllGames()
        {
            return gameDummies.Cast<TexasHoldemGame>().ToList();
        }

        public List<SystemUser> getAllUsers()
        {
            return userDummies.Cast<SystemUser>().ToList();
        }

        public void editUser(SystemUser user)
        {
            if (user.id < userDummies.Length)
                userDummies[user.id - 1] = user;
        }
    }
}
