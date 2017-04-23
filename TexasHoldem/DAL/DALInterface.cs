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

		ReturnMessage registerUser(SystemUser user);

		ReturnMessage logUser(string user);

		ReturnMessage logOutUser(string user);

        ReturnMessage addGame(TexasHoldemGame game);
	}
}
