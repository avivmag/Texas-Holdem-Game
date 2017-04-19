using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using Backend;

namespace DAL
{
    public interface DALInterface
    {
        TexasHoldemGame getGameById(int gameID);

		SystemUser getUserById(int userID);

		SystemUser getUserByName(string name);

		List<TexasHoldemGame> getAllGames();

        List<SystemUser> getAllUsers();

        void editUser(SystemUser user);

		Message registerUser(SystemUser user);

		Message logUser(string user);

		Message logOutUser(string user);
	}
}
