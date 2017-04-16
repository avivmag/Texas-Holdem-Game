using System;

public class BLImpl : BLInterface
{
	public BLImpl()
	{
	}

    Game spectateActiveGame(SystemUser user, int gameID)
    {
        TexasHoldemGame existingGame = getGameById(id);
        if (existingGame != null)
        {
            if (existingGame.canSpectate())
            {
                existingGame.addSpectator(new Spectator(user.id));
                user.addSpectatingGame(existingGame);
                return existingGame;
            }
            else throw new Exception("The game cant be spectated.");
        }
        else throw new Exception("Could not find the wanted game to spectate.");
    }

    Game joinActiveGame(SystemUser user, int gameID)
    {
        TexasHoldemGame existingGame = getGameById(id);
        if (existingGame != null)
        {
            if (existingGame.canJoinGame())
            {
                Player p = new Player(existingGame.GamePreferences.JoinCost);
                existingGame.addPlayer(p);
            }
            else throw new Exception("The game is full - can't join the game");
        }
        else throw new Exception("Could not find the wanted game to join");
    }

    void leaveGame(SystemUser user, Game game)
    {
        TexasHoldemGame existingGame = getGameById(id);
        if (existingGame != null)
        {
            existingGame.leaveGame(user);
        }
        else throw new Exception("Could not find the wanted game to leave");
    }
}
