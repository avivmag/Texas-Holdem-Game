using Backend.User;
using Obser;
using System.Net.Sockets;

namespace SL
{
	public interface SLInterface
	{
        object spectateActiveGame(int userId, int gameID);
        object GetGameForPlayers(int userId, int gameID);
        object joinGame(int userId, int gameID, int seatIndex);
        object leaveGame(SystemUser user, int gameID);
        
        object editUserProfile(int userId, string name, string password, string email, string avatar, int amount);

        object findAllActiveAvailableGames();
        object filterActiveGamesByPlayerName(string name);
        object filterActiveGamesByPotSize(int? size);
        object filterActiveGamesByGamePreferences(object pref);
        object filterActiveGamesByGamePreferences(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague, int minRank, int maxRank);
        object filterActiveGamesByGamePreferences(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague);
        //List<object> filterActiveGamesByGamePreferences(GamePreferences pref);
        //List<object> filterActiveGamesByGamePreferences(GameTypePolicy gamePolicy, int buyInPolicy, int startingChipsAmount, int MinimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed);
        object getAllGames();
        

        //object createGame(int gameCreatorId, object pref);
        object createGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague);

        object Login(string user, string password);
		object Register(string user, string password, string email, string userImage);
		object Logout(int userId);

        object getUserByName(string name);
        object getUserById(int userId);
        object getGameById(int gameId);
        //void replayGame(int gameId);
        //ReturnMessage addLeague(int minRank, int maxRank, string name);
        //ReturnMessage removeLeague(League league);
        //      League getLeagueByName(string name);
        //      League getLeagueById(Guid leagueId);
        //ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId);

        #region game
        object Bet(int gameId, int playerUserId, int coins);
        object AddMessage(int gameId, int playerIndex, string messageText);
        object Fold(int gameId, int playerIndex);
        object Check(int gameId, int playerIndex);
        object playGame(int gameId);
        object GetGameState(int gameId);
        object GetPlayer(int gameId, int playerIndex);
        object GetPlayerCards(int gameId, int userId);
        //object GetShowOff(int gameId);
        void SubscribeToGameState(ObserverAbstract<TcpClient> client, int gameID);
        void SubscribeToGameChatPlayers(ObserverAbstract<TcpClient> client, int gameID);
        void SubscribeToGameChatSpectators(ObserverAbstract<TcpClient> client, int gameID);
        #endregion



        //For test purpose methods
        object getAllUsers();
        object removeUser(int userId);
        object removeGame(int gameId);
    }
}
