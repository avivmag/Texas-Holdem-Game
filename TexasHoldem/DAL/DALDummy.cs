using Backend.Game;
using Backend.User;

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
            userDummies[0] = new SystemUser("Hadas","Aa123456",1000);
            userDummies[1] = new SystemUser("Gili", "123123",0);
            userDummies[2] = new SystemUser("Or", "111111", 700);
            for (int i = 0; i < 2; i++)
                userDummies[i].id = i;

            gameDummies = new TexasHoldemGame[4];
            gameDummies[0] = new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true));
            gameDummies[1] = new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false));
            gameDummies[2] = new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true));
            gameDummies[3] = new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false));
            for (int i = 0; i < 4; i++)
                gameDummies[i].id = i;

            playerDummies = new Player[4];
            playerDummies[0] = new Player(0,100,userDummies[0].rank);
            playerDummies[1] = new Player(1, 0, userDummies[1].rank);
            playerDummies[2] = new Player(2, 200, userDummies[2].rank);
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

        SystemUser DALInterface.getUserById(int userID)
        {
            for (int i = 0; i < userDummies.Length; i++)
                if (userDummies[i].id == userID)
                    return userDummies[i];
            return null;
        }
    }
}
