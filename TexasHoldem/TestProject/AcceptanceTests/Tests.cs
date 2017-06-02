using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Backend.User;
using Backend.Game;
using Backend;

namespace TestProject
{
    [TestClass]
    public class Tests : ProjectTest    {


        SystemUser hadas;
        SystemUser other;
        TexasHoldemGame game;


        [TestInitialize]
        public void SetUp()
        {
            object objUser = register("Hadas", "1234", "shidlhad", "1");
            Assert.IsNotNull(objUser);
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            hadas = (SystemUser)objUser;

            object objOther = register("Gil", "1234", "gilabadi89", "2");
            Assert.IsNotNull(objOther);
            Assert.IsInstanceOfType(objOther, typeof(SystemUser));
            other = (SystemUser)objOther;

            object objGame = creatGame(hadas.id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(objGame);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            game = (TexasHoldemGame)objGame;
        }

        [TestMethod]
        public void TestLogin()
        {
            Assert.IsNotNull(logout(hadas.id));
            Assert.IsNotNull(login("Hadas", "1234"));

        }

        [TestMethod]
        public void TestLogout()
        {
            Assert.IsNotNull(logout(hadas.id));
        }

        [TestMethod]
        public void TestLogoutFail()
        {
            hadas.money = 100000;
            Assert.IsNotNull(addPlayerToGame(hadas.id, game.gameId));
            Assert.IsFalse(((ReturnMessage)logout(hadas.id)).success);
        }
        
        [TestMethod]
        public void TestLeaveGame()
        {
            hadas.money = 100000;
            Assert.IsNotNull(addPlayerToGame(hadas.id, game.gameId));
            Assert.IsFalse(((ReturnMessage)logout(hadas.id)).success);
            Assert.IsNotNull(removeUserFromGame(hadas, game.gameId));
            Assert.IsTrue(((ReturnMessage)logout(hadas.id)).success);
        }

        [TestCleanup]
        public void TearDown()
        {
            removeGame(game.gameId);
            logout(hadas.id);
            logout(other.id);
            removeUser(hadas.id);
            removeUser(other.id);
        }
    }
}
