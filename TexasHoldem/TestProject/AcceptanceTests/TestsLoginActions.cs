using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Backend.User;
using Backend.Game;

namespace TestProject.AcceptanceTests
{
    [TestClass]
    public class TestsLoginActions :ProjectTest
    {
        SystemUser hadas;
        SystemUser other;
        TexasHoldemGame gameSpectate;
        TexasHoldemGame gameCantSpectate;

        [TestInitialize]
        public void SetUp()
        {
            object objUser= register("Hadas2", "1234", "shidlhad", "1");
            Assert.IsNotNull(objUser);
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            hadas = (SystemUser)objUser;

            object objOther = register("Gil2", "1234", "gilabadi89", "2");
            Assert.IsNotNull(objOther);
            Assert.IsInstanceOfType(objOther, typeof(SystemUser));
            other = (SystemUser)objOther;

            object objGame = creatGame(hadas.id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(objGame);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            gameSpectate = (TexasHoldemGame)objGame;

            object objGame2 = creatGame(hadas.id, "Limit", 1000, 1000, 1000, 100, 2, 9, false, false);
            Assert.IsNotNull(objGame2);
            Assert.IsInstanceOfType(objGame2, typeof(TexasHoldemGame));
            gameCantSpectate = (TexasHoldemGame)objGame2;
        }
        [TestMethod]
        public void TestEditProfileUserName()
        {
            editProfile(hadas.id, "shid", "1234", "shidlhad", "1", 100);
            Assert.AreEqual(hadas.name,"shid");
            editProfile(hadas.id, "Hadas2", "1234", "shidlhad", "1", 100);
        }

        [TestMethod]
        public void TestEditProfilePassword()
        {
            editProfile(hadas.id, "Hadas2", "12345", "shidlhad", "1", 100);
            Assert.AreEqual(hadas.password,"12345");
        }

        [TestMethod]
        public void TestEditProfileEmail()
        {
            editProfile(hadas.id, "Hadas2", "1234", "shidl", "1", 100);
            Assert.AreEqual(hadas.email,"shidl");
        }

        [TestMethod]
        public void TestEditProfilePicture()
        {
            editProfile(hadas.id, "Hadas2", "1234", "shidl", "123", 100);
            Assert.IsNotNull(hadas.userImage,"123");
        }

        [TestMethod]
        public void TestEditProfileUserNameExists()
        {
            editProfile(hadas.id, "Gil2", "1234", "shidlhad", "1", 100);
            Assert.AreEqual(hadas.name,"Hadas2");
        }

        [TestMethod]
        public void TestCreatGame()
        {
            object obj = creatGame(hadas.id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(creatGame(hadas.id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false));
            Assert.IsInstanceOfType(obj, typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestjoinExistingGame()
        {
            hadas.money = 100000;
            Assert.IsNotNull(addPlayerToGame(hadas.id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameTwice()
        {
            hadas.money = 100000;
            Assert.IsNotNull(addPlayerToGame(hadas.id, gameSpectate.gameId, 1));
            Assert.IsNull(addPlayerToGame(hadas.id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoMoney()
        {
            Assert.IsNull(addPlayerToGame(hadas.id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoUser()
        {
            Assert.IsNull(addPlayerToGame(0, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoGame()
        {
            Assert.IsNull(addPlayerToGame(hadas.id, 1000, 1));
        }

        [TestMethod]
        public void TestSpectateActiveGame()
        {
            object ans = spectateActiveGame(hadas.id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestSpectateActiveGameCantSpectate()
        {
            Assert.IsNull(spectateActiveGame(hadas.id, gameCantSpectate.gameId));
        }

        [TestMethod]
        public void TestSpectateActiveGameCantSpectateTwice()
        {
            object ans = spectateActiveGame(hadas.id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
            Assert.IsNull(spectateActiveGame(hadas.id, gameSpectate.gameId));
        }

        [TestMethod]
        public void TestSpectateActiveGameTwoSpectators()
        {
            object ans = spectateActiveGame(hadas.id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
            ans = spectateActiveGame(other.id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
        }

        [TestCleanup]
        public void TearDown()
        {
            removeGame(gameSpectate.gameId);
            removeGame(gameCantSpectate.gameId);
            logout(hadas.id);
            logout(other.id);
            removeUser(hadas.id);
            removeUser(other.id);
        }
    }
}
