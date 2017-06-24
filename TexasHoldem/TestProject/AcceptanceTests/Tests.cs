using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Backend.User;
using Backend.Game;
using Backend;
using Database;

namespace TestProject
{
    [TestClass]
    public class Tests : ProjectTest    {

        IDB db;
        TexasHoldemGame game;
        
        [TestInitialize]
        public void SetUp()
        {
            db = new DBImpl();

            bool objUser = db.RegisterUser("Hadas","1234","shidlhad", null);
            Assert.IsTrue(objUser);

            bool objOther = db.RegisterUser("Gil", "1234", "gilabadi89", null);
            Assert.IsTrue(objOther);

            object objGame = creatGame(db.getUserByName("Hadas").id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(objGame);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            game = (TexasHoldemGame)objGame;
        }

        [TestMethod]
        public void TestLogin()
        {
            Assert.IsNotNull(logout(db.getUserByName("Hadas").id));
            Assert.IsNotNull(login("Hadas", "1234"));

        }

        [TestMethod]
        public void TestLogout()
        {
            Assert.IsNotNull(logout(db.getUserByName("Hadas").id));
        }

        [TestMethod]
        public void TestLogoutFail()
        {
            db.EditUserById(db.getUserByName("Hadas").id,null,null,null,null,100000,null,true);
            Assert.IsNotNull(addPlayerToGame(db.getUserByName("Hadas").id, game.gameId,1));
            Assert.IsFalse(((ReturnMessage)logout(db.getUserByName("Hadas").id)).success);
        }
        
        [TestMethod]
        public void TestLeaveGame()
        {
            editProfile(db.getUserByName("Hadas").id, "Hadas", "123", "shilhad", "1", 10000);
            Assert.IsNotNull(addPlayerToGame(db.getUserByName("Hadas").id, game.gameId, 1));
            Assert.IsFalse(((ReturnMessage)logout(db.getUserByName("Hadas").id)).success);
            Assert.IsNotNull(removeUserFromGame(db.getUserByName("Hadas"), game.gameId));
            Assert.IsTrue(((ReturnMessage)logout(db.getUserByName("Hadas").id)).success);
        }

        [TestCleanup]
        public void TearDown()
        {
            removeGame(game.gameId);
            db.deleteUser(db.getUserByName("Hadas").id);
            db.deleteUser(db.getUserByName("Gil").id);
        }
    }
}
