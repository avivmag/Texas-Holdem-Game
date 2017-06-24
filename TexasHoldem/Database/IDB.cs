using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Backend.User;
using System.Drawing;

namespace Database
{
    public interface IDB
    {

        DataTable uploadSystemUser();
        string getEnterMessage(string stringCommand);
        //void editUserName(int userID, string newData);
        bool isUserExist(string name);
        bool RegisterUser(string UserName, string password, string email, Image image);
        bool EditUserById(int? Id, string UserName, string password, string email, string image, int? money, int? rank, bool playedAnotherGame);
        bool EditUserLeaderBoardsById(int? Id, int? highetsCashInAGame, int? totalGrossProfit);
        int Login(string UserName, string password);
        List<SystemUser> getAllSystemUsers();
        SystemUser getUserById(int Id);
        SystemUser getUserByName(string name);
        SystemUser getUserByEmail(string email);
        bool deleteUser(int Id);
        bool deleteUsers();
        List<object> getLeaderboardsByParam(string param);
        List<object> getUsersDetails();
    }
}
