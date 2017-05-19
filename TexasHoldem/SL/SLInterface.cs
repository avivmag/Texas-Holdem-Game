using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using static Backend.Game.GamePreferences;

namespace SL
{
	public interface SLInterface
	{
        object spectateActiveGame(int userId, int gameID);
        object joinActiveGame(int userId, int gameID);
        ReturnMessage leaveGame(SystemUser user, int gameID);
        object editUserProfile(int userId, string name, string password, string email, string avatar);
        object findAllActiveAvailableGames();
        object filterActiveGamesByPlayerName(string name);
        object filterActiveGamesByPotSize(int? size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed);
        List<TexasHoldemGame> getAllGames();
        object createGame(int gameCreatorId, object pref);
        object createGame(int gameCreator, int gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed);

        object Login(string user, string password);
		object Register(string user, string password, string email, string userImage);
		object Logout(int userId);
		SystemUser getUserByName(string name);

		SystemUser getUserById(int userId);
        object getGameById(int gameId);
        void replayGame(int gameId);
        //ReturnMessage addLeague(int minRank, int maxRank, string name);
        //ReturnMessage removeLeague(League league);
        //      League getLeagueByName(string name);
        //      League getLeagueById(Guid leagueId);
        //ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId);

        string raiseBet(int gameId, int playerId, int coins);
    }
}
