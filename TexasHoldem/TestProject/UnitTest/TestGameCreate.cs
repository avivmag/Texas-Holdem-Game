//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SL;
//using Backend.Game;
//using DAL;
//using Moq;
//using Backend.User;
//using Backend;
//using System.Collections.Generic;

//namespace TestProject.UnitTest
//{
//    [TestClass]
//    public class TestGameCreate
//    {

//        SLInterface sl;

//        [TestInitialize]
//        public void SetUp()
//        {
//            var usersList = new List<SystemUser>
//            {
//                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
//                new SystemUser("Gili", "123123", "email1", "image1", 0),
//                new SystemUser("Or", "111111", "email2", "image2", 700),
//                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
//            };

//            Mock<DALInterface> dalMock = new Mock<DALInterface>();
//            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int id) => usersList.Find(u => u.id == id));
//            dalMock.Setup(x => x.addGame(It.IsAny<TexasHoldemGame>())).Returns(new ReturnMessage(true, null));
//            this.sl = new SLImpl(dalMock.Object);
//        }

//        [TestMethod]
//        public void TestCreateGame()
//        {
//            var user = sl.getUserById(0);
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                15000,
//                120,
//                4,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(user.id, pref);

//            Assert.IsTrue(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithNonExistantUser()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                15000,
//                120,
//                4,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(95, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithNegativeBuyIn()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                -100,
//                15000,
//                120,
//                4,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(0, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithNegativeChipsCount()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                -15000,
//                120,
//                4,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(0, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithMinimalBetHigherThanChipsCount()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                15,
//                120,
//                4,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(0, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithOneMinPlayer()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                15000,
//                120,
//                1,
//                8,
//                true);
//            var gameCreationMessage = sl.createGame(0, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }

//        [TestMethod]
//        public void TestCreateGameWithMaxPlayersLowerThanMinPlayers()
//        {
//            GamePreferences pref = new GamePreferences(
//                GamePreferences.GameTypePolicy.limit,
//                100,
//                15000,
//                120,
//                4,
//                3,
//                true);
//            var gameCreationMessage = sl.createGame(0, pref);

//            Assert.IsFalse(gameCreationMessage.success);
//        }
//    }
//}
