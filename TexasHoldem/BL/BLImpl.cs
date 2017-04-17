using Backend.User;
using Backend.Game;
using Backend;
using DAL;

public class BLImpl : BL.BLInterface
{
    private DALInterface itsDal;
	public BLImpl()
	{
        itsDal = new DALDummy();
    }

    public Message spectateActiveGame(SystemUser user, int gameID)
    {
        Message m = new Message();
        TexasHoldemGame existingGame = itsDal.getGameById(gameID);
        if (existingGame != null)
        {
            Spectator spec = new Spectator(user.id);
            if (existingGame.joinSpectate(spec))
            {
                user.addSpectatingGame(spec);
            }
            else
            {
                m.success = false;
                m.description = "Couldn't spectate the wanted game because the preferences doesn't allow to spectate.";
            }
        }
        else
        {
            m.success = false;
            m.description = "Couldn't find the wanted game with the id:" + gameID.ToString() + ".";
        }
        return m;
    }

    public Message joinActiveGame(SystemUser user, int gameID)
    {
        Message m = new Message();
        TexasHoldemGame existingGame = itsDal.getGameById(gameID);
        if (existingGame != null)
        {
            Player p = new Player(user.id, existingGame.GamePreferences.BuyInPolicy, user.rank);
            if (user.money < existingGame.GamePreferences.BuyInPolicy)
            {
                if (existingGame.joinGame(p))
                {
                    m.success = true;
                }
                else
                {
                    m.success = false;
                    m.description = "Could not join the game because there are no seats.";
                }
            }
            
        }
        {
            m.success = false;
            m.description = "Couldn't find the wanted game with the id:" + gameID.ToString() + ".";
        }
        return m;
    }

   public Message leaveGame(Spectator spec, int gameID)
    {
        Message m = new Message();
        TexasHoldemGame existingGame = itsDal.getGameById(gameID);
        if (spec.GetType() == typeof(Player))
        {
            Player p = (Player)spec;
            existingGame.leaveGame(p);
            SystemUser user = itsDal.getUserById(spec.systemUserID);
            //TODO: what is the rank changing policy.
            if (p.Tokens > existingGame.GamePreferences.BuyInPolicy)
                user.rank += 1;
            else
                user.rank -= 1;
        }
        else
        {
            existingGame.leaveGame((Player)spec);
        }
        return m;
    }
}
