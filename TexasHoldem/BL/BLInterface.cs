using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;

namespace BL
{
	public interface BLInterface
	{
        Message spectateActiveGame(SystemUser user, int gameID);
        Message joinActiveGame(SystemUser user, int gameID);
        Message leaveGame(Spectator spec, int gameID);
        Message editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref);
        Message createGame(int gameCreatorId, GamePreferences pref);

        Message Login(string user, string password);
		Message Register(string user, string password, string email, string userImage);
		Message Logout(string user);
		SystemUser getUserByName(string name);

		SystemUser getUserById(int userId);
        TexasHoldemGame getGameById(int gameId);
    }
}
