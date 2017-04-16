namespace BL
{
	interface BLInterface
	{
        void spectateActiveGame(SystemUser user, Game game);
        void joinActiveGame(SystemUser user, Game game);
        void leaveGame(SystemUser user, Game game);
	}
}
