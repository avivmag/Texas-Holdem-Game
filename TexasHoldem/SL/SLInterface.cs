using Backend;
using Backend.User;

namespace SL
{
	interface SLInterface
	{
        Message spectateActiveGame(SystemUser user, int gameId);
        Message joinActiveGame(SystemUser user, int gameId);
        Message leaveGame(SystemUser user, int gameId);
	}
}
