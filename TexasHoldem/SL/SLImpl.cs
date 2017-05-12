using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Backend.Game;
using Backend.User;
using BL;
using static Backend.Game.GamePreferences;

namespace SL
{
	public class SLImpl : SLInterface
	{
		private BLInterface bl;

		public SLImpl()
		{
			bl = new BLImpl();
		}

        public TexasHoldemGame createGame(int gameCreator, GameTypePolicy gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed)
        {
            return bl.createGame(gameCreator,gamePolicy,buyInPolicy,startingChipsAmount,MinimalBet,minPlayers,maxPlayers,isSpectatingAllowed);
        }

        public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar)
		{
            return bl.editUserProfile(userId, name, password, email, avatar);
		}

        public List<TexasHoldemGame> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed)
        {
            return bl.filterActiveGamesByGamePreferences(gamePolicy,buyInPolicy,startingChipsAmount,MinimalBet,minPlayers,maxPlayers,isSpectatingAllowed);
        }

        public List<TexasHoldemGame> filterActiveGamesByPlayerName(string name)
		{
            return bl.filterActiveGamesByPlayerName(name);
        }

		public List<TexasHoldemGame> filterActiveGamesByPotSize(int? size)
		{
            return bl.filterActiveGamesByPotSize(size);
        }

		public List<TexasHoldemGame> findAllActiveAvailableGames()
		{
            return bl.findAllActiveAvailableGames();
        }

        public TexasHoldemGame getGameById(int gameId)
        {
            return bl.getGameById(gameId);
        }

        public SystemUser getUserByName(string userName)
        {
            return bl.getUserByName(userName);
        }

        public ReturnMessage joinActiveGame(SystemUser user, int gameId)
		{
            return bl.joinActiveGame(user, gameId);
        }

		public ReturnMessage leaveGame(Player player, int gameId)
		{
            return bl.leaveGame(player, gameId);
        }

		public ReturnMessage Login(string user, string password)
		{
			return bl.Login(user, password);
		}

		public ReturnMessage Logout(string name)
		{
			return bl.Logout(name);
		}

		public ReturnMessage Register(string user, string password, string email, string userImage)
		{
            return bl.Register(user, password, email, userImage);
        }

		public ReturnMessage spectateActiveGame(SystemUser user, int gameId)
		{
            return bl.spectateActiveGame(user, gameId);
		}
	}
}
