using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;

namespace SL
{
	interface SLInterface
	{
        Message spectateActiveGame(SystemUser user, int gameId);
        Message joinActiveGame(SystemUser user, int gameId);
        Message leaveGame(SystemUser user, int gameId);
        Message editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref);
    }
}
