using Backend.Game;
using Backend.User;

namespace DAL
{
    public interface DALInterface
    {
        TexasHoldemGame getGameById(int gameID);

        SystemUser getUserById(int userID);
    }
}
