using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using static Backend.Game.GamePreferences;
using Backend.Game.DecoratorPreferences;

namespace SL
{
	public interface SLInterface
	{
        object spectateActiveGame(int userId, int gameID);
        object joinActiveGame(int userId, int gameID);
        object leaveGame(SystemUser user, int gameID);

        object editUserProfile(int userId, string name, string password, string email, string avatar, int amount);

        List<object> findAllActiveAvailableGames();
        List<object> filterActiveGamesByPlayerName(string name);
        List<object> filterActiveGamesByPotSize(int? size);
        List<object> filterActiveGamesByGamePreferences(MustPreferences pref);
        //List<object> filterActiveGamesByGamePreferences(GamePreferences pref);
        //List<object> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed);
        List<object> getAllGames();

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
