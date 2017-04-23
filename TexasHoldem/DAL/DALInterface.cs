using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using Backend;
using System;

namespace DAL
{
    public interface DALInterface
    {
        TexasHoldemGame getGameById(int gameID);

		SystemUser getUserById(int userID);

		SystemUser getUserByName(string name);

		List<TexasHoldemGame> getAllGames();

        List<SystemUser> getAllUsers();

        List<League> getAllLeagues();

        void editUser(SystemUser user);

		ReturnMessage registerUser(SystemUser user);

		ReturnMessage logUser(string user);

		ReturnMessage logOutUser(string user);
		
        ReturnMessage addGame(TexasHoldemGame game);

		ReturnMessage addLeague(int minRank, int maxRank, string leagueName);

		ReturnMessage removeLeague(Guid leagueId);

        int getHighestUserId();

		ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId);

    }
}
