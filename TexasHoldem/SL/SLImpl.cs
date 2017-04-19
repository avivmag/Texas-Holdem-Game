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

		public Message editUserProfile(int userId, string name, string password, string email, string avatar)
		{
			throw new NotImplementedException();
		}

		public List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref)
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

		public Message joinActiveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
		}

		public Message leaveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
		}

		public Message Login(string user, string password)
		{
			Message m = bl.Login(user, password);
			if (m.success)
				mySystemUser = bl.getUserByName(user);

			return m;
		}

		public Message Logout()
		{
			Message m = bl.Logout(mySystemUser.name);
			if (m.success)
				mySystemUser = null;

			return m;
		}

		public Message Register(string user, string password, string email, string userImage)
		{
			return bl.Register(user, password, email, userImage);
		}

		public Message Register(string user, string password)
		{
			throw new NotImplementedException();
		}

		public Message spectateActiveGame(SystemUser user, int gameId)
		{
			throw new NotImplementedException();
		}
	}
}
