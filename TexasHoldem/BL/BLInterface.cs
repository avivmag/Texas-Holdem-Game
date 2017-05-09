using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using System;
using static Backend.Game.GamePreferences;

namespace BL
{
	public interface BLInterface
	{
        ReturnMessage spectateActiveGame(SystemUser user, int gameID);
        ReturnMessage joinActiveGame(SystemUser user, int gameID);
        ReturnMessage leaveGame(Player spec, int gameID);
        ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int? size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed);
        List<TexasHoldemGame> getAllGames();
        ReturnMessage createGame(int gameCreatorId, GamePreferences pref);
        TexasHoldemGame createGame(int gameCreator, GameTypePolicy gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed);

        ReturnMessage Login(string user, string password);
		ReturnMessage Register(string user, string password, string email, string userImage);
		ReturnMessage Logout(string user);
		SystemUser getUserByName(string name);

		SystemUser getUserById(int userId);
        TexasHoldemGame getGameById(int gameId);
        void replayGame(int gameId);
		ReturnMessage addLeague(int minRank, int maxRank, string name);
		ReturnMessage removeLeague(League league);
        League getLeagueByName(string name);
        League getLeagueById(Guid leagueId);
		ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId);
    }
}
