﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Backend.User;

namespace Database
{
    public interface IDB
    {

        DataTable uploadSystemUser();
        string getEnterMessage(string stringCommand);
        //void editUserName(int userID, string newData);
        bool isUserExist(string name);
        bool RegisterUser(string UserName, string password, string email, string image);
        bool EditUserById(int? Id, string UserName, string password, string email, string image, int? money, int? rank);
        int Login(string UserName, string password);
        List<SystemUser> getAllSystemUsers();
        SystemUser getUserById(int Id);
        SystemUser getUserByName(string name);
        SystemUser getUserByEmail(string email);
        bool deleteUser(int Id);
        bool deleteUsers();
    }
}
