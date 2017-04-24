using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Backend.Game;
using Backend.User;
using BL;

namespace SL
{
	class SLImpl : SLInterface
	{
		private SystemUser mySystemUser;
		private BLInterface bl;

		public SLImpl()
		{
			bl = new BLImpl();
		}

        public ReturnMessage createGame(int gameCreator, int gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed)
        {
            return null;
        }

        public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar)
		{
            return bl.editUserProfile(userId, name, password, email, avatar);
		}

        public List<TexasHoldemGame> filterActiveGamesByGamePreferences(int? gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed)
        {
            throw new NotImplementedException();
        }

        public List<TexasHoldemGame> filterActiveGamesByPlayerName(string name)
		{
            return null;
        }

		public List<TexasHoldemGame> filterActiveGamesByPotSize(int size)
		{
            return null;
        }

		public List<TexasHoldemGame> findAllActiveAvailableGames()
		{
            return null;
        }

		public ReturnMessage joinActiveGame(SystemUser user, int gameId)
		{
            return null;
        }

		public ReturnMessage leaveGame(SystemUser user, int gameId)
		{
            return null;
        }

		public ReturnMessage Login(string user, string password)
		{
			ReturnMessage m = bl.Login(user, password);
			if (m.success)
				mySystemUser = bl.getUserByName(user);

			return m;
		}

		public ReturnMessage Logout()
		{
			ReturnMessage m = bl.Logout(mySystemUser.name);
			if (m.success)
				mySystemUser = null;

			return m;
		}

		public ReturnMessage Register(string user, string password, string email, string userImage)
		{
            return null;
        }

		public ReturnMessage Register(string user, string password)
		{
            return null;
        }

		public ReturnMessage spectateActiveGame(SystemUser user, int gameId)
		{
            return null;
		}
	}
}
