using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;

namespace SL
{
	interface SLInterface
	{
        ReturnMessage spectateActiveGame(SystemUser user, int gameId);
        ReturnMessage joinActiveGame(SystemUser user, int gameId);
        ReturnMessage leaveGame(SystemUser user, int gameId);
        ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(int? gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed);
        ReturnMessage createGame(int gameCreator, int gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed);

		ReturnMessage Login(string user, string password);
		ReturnMessage Register(string user, string password);
		ReturnMessage Logout();
	}
}
