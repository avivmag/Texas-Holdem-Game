using Backend;
using Backend.Game;
using Backend.User;

namespace BL
{
	public interface BLInterface
	{
        Message spectateActiveGame(SystemUser user, int gameID);
        Message joinActiveGame(SystemUser user, int gameID);
        Message leaveGame(Spectator spec, int gameID);
	}
}
