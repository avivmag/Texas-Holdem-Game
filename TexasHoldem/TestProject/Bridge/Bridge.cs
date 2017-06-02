using System.Collections.Generic;
using Backend.Game;
using Backend.User;

namespace TestProject
{
    interface IBridge
    {
        object register(string username, string password, string email, string picture);
        object login(string username, string password);
        object logout(int userId);
        object editProfile(int userId, string username, string password, string email, string picture, int moneyAmount);
        //bool editImage(string img);
        //bool editName(string name);
        //bool editEmail(string email);
        object getUserbyName(string username);
        bool isLogin(string username);
        bool isUserExist(string username, string password);

        object creatGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? MinimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague);
        object addPlayerToGame(int userId, int gameId);
        object selectGametoJoin(string game);
        bool checkActiveGame(string statusGame);
        bool isGameDefOK(string gameDefinithions);
        bool checkAvailibleSeats(string game);
        object spectateActiveGame(int userId, int gameId);
        //TODO: what is the difference?
        bool exitGame(string game);
        object removeUserFromGame(SystemUser user, int game);
        bool isGameOver(int gameId);

        //object selectGameToReplay(string game);
        //bool isWatchingReplay(string game);
        //bool saveTurn(string game);
        object findAllActive();
        string filterByCriteria(string criteria);
        //bool storeGameData();
        //object joinLeaguePerPoints(int points);
        bool canBet(TexasHoldemGame game, SystemUser user, int amount);
        bool canRaise(TexasHoldemGame game, SystemUser user, int amount);
        bool canCall(TexasHoldemGame game, SystemUser user,int  amount);
        bool fold();
        bool check();
        bool updatePot(int amount);
        //statePlayer = bet or raise or call or fold
        bool updateStatePlayer(string statePlayer, int amount);
        bool betSmallBlind(int amount);
        bool betBigBlind(int amount);
        //bool isLeagueExist(string criteria);
        //bool setCriteriaForNewLeague(string criteria);
        //bool PlayersWithCriteria(string criteria, string players);

        bool removeUser(int userId);
        bool removeGame(int gameId);
    }
}
