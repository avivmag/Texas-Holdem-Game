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
        object addPlayerToGame(int userId, int gameId, int seat);
        object selectGametoJoin(string game);
        bool checkActiveGame(string statusGame);
        bool isGameDefOK(string gameDefinithions);
        bool checkAvailibleSeats(string game);
        object spectateActiveGame(int userId, int gameId);
        bool exitGame(string game);
        object removeUserFromGame(SystemUser user, int game);
        bool isGameOver(int gameId);

        object findAllActive();
        string filterByCriteria(string criteria);

        bool canBet(TexasHoldemGame game, int amount);
        bool canRaise(TexasHoldemGame game, int amount);
        bool canCall(TexasHoldemGame game ,int  amount);
        bool fold();
        bool check();
        bool updatePot(int amount);

        bool updateStatePlayer(string statePlayer, int amount);
        bool betSmallBlind(int amount);
        bool betBigBlind(int amount);

        bool removeUser(int userId);
        bool removeGame(int gameId);
    }
}
