using Backend.Game;
using Backend.User;
using System.Collections.Generic;

namespace DAL
{
    public interface DALInterface
    {
        TexasHoldemGame getGameById(int gameID);

        SystemUser getUserById(int userID);

        List<TexasHoldemGame> getAllGames();

        List<SystemUser> getAllUsers();

        void editUser(SystemUser user);
    }
}
