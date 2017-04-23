using Backend;
using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using System;

namespace BL
{
	public interface BLInterface
	{
        ReturnMessage spectateActiveGame(SystemUser user, int gameID);
        ReturnMessage joinActiveGame(SystemUser user, int gameID);
        ReturnMessage leaveGame(Spectator spec, int gameID);
        ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar);
        List<TexasHoldemGame> findAllActiveAvailableGames();
        List<TexasHoldemGame> filterActiveGamesByPlayerName(string name);
        List<TexasHoldemGame> filterActiveGamesByPotSize(int size);
        List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref);
        ReturnMessage createGame(int gameCreatorId, GamePreferences pref);

        ReturnMessage Login(string user, string password);
		ReturnMessage Register(string user, string password, string email, string userImage);
		ReturnMessage Logout(string user);
		SystemUser getUserByName(string name);

		SystemUser getUserById(int userId);
        TexasHoldemGame getGameById(int gameId);

		ReturnMessage addLeague(int minRank, int maxRank, string name);
		ReturnMessage removeLeague(Guid leagueId);
        League getLeagueByName(string name);
        League getLeagueById(Guid leagueId);
		ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId);
    }
}
