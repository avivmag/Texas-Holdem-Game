using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using static Backend.Game.GamePreferences;

namespace SL
{
	public interface SLInterface
	{
        ReturnMessage spectateActiveGame(SystemUser user, int gameId);
        ReturnMessage joinActiveGame(SystemUser user, int gameId);
        ReturnMessage leaveGame(Player player, int gameId);
        ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int? size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed);
        TexasHoldemGame createGame(int gameCreator, GameTypePolicy gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed);


        ReturnMessage Login(string user, string password);
		ReturnMessage Register(string user, string password, string email, string avatar);
		ReturnMessage Logout(string name);
        TexasHoldemGame getGameById(int gameId);
        SystemUser getUserByName(string name);
    }
}
