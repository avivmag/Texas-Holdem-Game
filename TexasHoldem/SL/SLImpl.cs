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

		public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar)
		{
			throw new NotImplementedException();
		}

		public List<TexasHoldemGame> filterActiveGamesByGamePreferences(int? gamePolicy, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed)
		{
			throw new NotImplementedException();
		}

        public ReturnMessage createGame(int gameCreator, int gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed)
        {
            throw new NotImplementedException();
        }


        public List<TexasHoldemGame> filterActiveGamesByPlayerName(string name)
		{
			throw new NotImplementedException();
		}

		public List<TexasHoldemGame> filterActiveGamesByPotSize(int size)
		{
			throw new NotImplementedException();
		}

		public List<TexasHoldemGame> findAllActiveAvailableGames()
		{
			throw new NotImplementedException();
		}

		public ReturnMessage joinActiveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
		}

		public ReturnMessage leaveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
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
			return bl.Register(user, password, email, userImage);
		}

		public ReturnMessage Register(string user, string password)
		{
			throw new NotImplementedException();
		}

		public ReturnMessage spectateActiveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
		}
	}
}
