using Backend.User;
using Backend.Game;
using Backend;
using DAL;
using BL;
using System.Collections.Generic;
using System;

public class BLImpl : BLInterface
{
	private DALInterface dal;

	public BLImpl()
	{
		dal = new DALDummy();
	}

	public Message spectateActiveGame(SystemUser user, int gameID)
	{
		Message m = new Message();
		TexasHoldemGame existingGame = dal.getGameById(gameID);
		if (existingGame != null)
		{
			Spectator spec = new Spectator(user.id);
			m = existingGame.joinSpectate(spec);
			if (m.success)
			{
				user.addSpectatingGame(spec);
			}
			return m;
		}
		else
			return new Message(false, "Couldn't find the wanted game with the id:" + gameID.ToString() + ".");
	}

	public Message joinActiveGame(SystemUser user, int gameID)
	{
		TexasHoldemGame existingGame = dal.getGameById(gameID);
		if (existingGame != null)
		{
			if (user.money >= existingGame.GamePreferences.BuyInPolicy)
			{
				Player p = new Player(user.id, existingGame.GamePreferences.BuyInPolicy, user.rank);
				return existingGame.joinGame(p);
			}
			else
				return new Message(false, "Could not join the game because the user dont have enough money to join.");
		}
		else
			return new Message(false, "Couldn't find the wanted game with the id:" + gameID.ToString() + ".");
	}

	public Message leaveGame(Spectator spec, int gameID)
	{
		Message m = new Message();
		TexasHoldemGame existingGame = dal.getGameById(gameID);
		if (spec.GetType() == typeof(Player))
		{
			Player p = (Player)spec;
			existingGame.leaveGame(p);
			SystemUser user = dal.getUserById(spec.systemUserID);
			//TODO: what is the rank changing policy.
			user.money += p.Tokens;
			if (p.Tokens > existingGame.GamePreferences.BuyInPolicy)
				user.rank += 1;
			else
				user.rank -= 1;
			dal.editUser(user);
		}
		else
		{
			existingGame.leaveGame((Player)spec);
		}
		return m;
	}

	public Message editUserProfile(int userId, string name, string password, string email, string avatar)
	{
		Message m = new Message();
		SystemUser user = dal.getUserById(userId);
		List<SystemUser> allUsers = dal.getAllUsers();
		if (name.Equals("") || password.Equals(""))
		{
			m.success = false;
			m.description = "Can't change to empty user name or password.";
			return m;
		}
		foreach (SystemUser u in allUsers) {
			if (u.name.Equals(name, StringComparison.OrdinalIgnoreCase) || u.email.Equals(email, StringComparison.OrdinalIgnoreCase)) //comparing two passwords including cases i.e AbC = aBc
			{
				m.success = false;
				m.description = "Username or email already exists.";
				return m;
			}
		}
		user.name = name;
		user.password = password;
		user.email = email;
		user.userImage = avatar;
		return m;
	}

	public List<TexasHoldemGame> findAllActiveAvailableGames()
	{
		List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
		foreach (TexasHoldemGame g in dal.getAllGames())
		{
			if (g.active && g.AvailableSeats > 0)
				ans.Add(g);
		}

		return ans;
	}

	public List<TexasHoldemGame> filterActiveGamesByPlayerName(string name)
	{
		List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
		foreach (TexasHoldemGame g in dal.getAllGames())
		{
			foreach (Player p in g.players)
			{
				if (dal.getUserById(p.systemUserID).name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					ans.Add(g);
					break;
				}
			}
		}

		return ans;
	}

	public List<TexasHoldemGame> filterActiveGamesByPotSize(int potSize)
	{
		List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
		ans = dal.getAllGames();
		foreach (TexasHoldemGame g in ans)
		{
			if (g.pot > potSize)
			{
				ans.Remove(g);
			}
		}

		return ans;
	}

	public List<TexasHoldemGame> filterActiveGamesByGamePreferences(GamePreferences pref)
	{
		List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
		foreach (TexasHoldemGame g in dal.getAllGames())
		{
			if (g.GamePreferences.Equals(pref))
			{
				ans.Add(g);
			}
		}

		return ans;
	}

	public SystemUser getUserById(int userId)
	{
		return dal.getUserById(userId);
	}

	public SystemUser getUserByName(string name)
	{
		return dal.getUserByName(name);
	}

	public TexasHoldemGame getGameById(int gameId)
	{
		return dal.getGameById(gameId);
	}

	public Message Login(string user, string password)
	{
		if(user == null || password == null || user.Equals("") || password.Equals(""))
			return new Message(false, "all attributes must be filled.");

		SystemUser systemUser = dal.getUserByName(user);
		if (systemUser == null)
			return new Message(false, "user does not exists\\incorrect password mismatched");

		if (systemUser.password.Equals(password))
			return dal.logUser(user);
		else
			return new Message(false, "user does not exists\\incorrect password mismatched");

	}
	public Message Register(string user, string password, string email, string userImage)
	{
		if (user == null || password == null || email == null || userImage == null || user.Equals("") || password.Equals("") || email.Equals("") || userImage.Equals(""))
			return new Message(false, "all attributes must be filled.");

		SystemUser systemUser = dal.getUserByName(user);
		if (systemUser != null)
			return new Message(false, "user name already taken");

		return dal.registerUser(new SystemUser(user, password, email, userImage, 0));
		
	}

	public Message Logout(string user)
	{
		if (user == null || user.Equals(""))
			return new Message(false, "all attributes must be filled.");

		SystemUser systemUser = dal.getUserByName(user);
		if (systemUser == null)
			return new Message(false, "User does not exists");

		if (systemUser.spectators.Count > 0)
			return new Message(false, "you are active in some games, leave them and then log out.");

		return dal.logOutUser(user);
	}
}
