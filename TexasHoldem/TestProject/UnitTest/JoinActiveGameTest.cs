//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SL;
//using Backend.User;
//using System.Collections.Generic;
//using Moq;
//using DAL;
//using Backend.Game;

//namespace TestProject
//{
//    [TestClass]
//    public class JoinActiveGameTest
//    {
//        SLInterface sl;

//        [TestInitialize]
//        public void SetUp()
//        {

//            var userList = new List<SystemUser>
//            {
//                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
//                new SystemUser("Gili", "123123", "email1", "image1", 0),
//                new SystemUser("Or", "111111", "email2", "image2", 700),
//                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
//            };


//            //var leagues = new List<League>
//            //{
//            //    new League(0, 1000, "Starter League"),
//            //    new League(1000, 2000, "Experienced League")
//            //};

//            var gamesList = new List<TexasHoldemGame>
//            {
//                new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
//                new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
//                new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
//                new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
//                new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
//                new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
//            };

//            Mock<DALInterface> dalMock = new Mock<DALInterface>();
//            dalMock.Setup(x => x.getAllUsers()).Returns(userList);
//            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
//            dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList.Find(g => (g.gameId == i)));
//            dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
//            this.sl = new SLImpl(dalMock.Object);
//        }

//        [TestMethod]
//        public void joinSuccessTest()
//        {
//            SystemUser user = sl.getUserById(0);

//            Assert.IsTrue(sl.joinActiveGame(user, sl.getAllGames()[4].gameId).success);
//        }

//        [TestMethod]
//        public void joinSuccessLeagueGameTest()
//        {
//            SystemUser user = sl.getUserById(2);
//            var m = sl.joinActiveGame(user, sl.getAllGames()[4].gameId);
//            Assert.IsTrue(m.success);
//        }

//        [TestMethod]
//        public void joinFailesNoSeatsTest()
//        {
//            SystemUser user2 = sl.getUserById(2);
//            sl.joinActiveGame(user2, 2);

//            SystemUser user3 = sl.getUserById(3);
//            sl.joinActiveGame(user3,3);

//            SystemUser user0 = sl.getUserById(0);
//            Assert.IsFalse(sl.spectateActiveGame(user0, 1).success);
//        }

//        [TestMethod]
//        public void joinFailesNoMoneyTest()
//        {
//            SystemUser user1 = sl.getUserById(1);
//            Assert.IsFalse(sl.spectateActiveGame(user1, 1).success);
//        }

//        [TestMethod]
//        public void joinFailsLeagueGameTest()
//        {
//            SystemUser user = sl.getUserById(0);
//            Assert.IsFalse(sl.joinActiveGame(user, 5).success);
//        }

//        [TestMethod]
//        public void joinFailsAlreadyPlayTest()
//        {
//            SystemUser user = sl.getUserById(0);
//            sl.joinActiveGame(user, 3);
//            Assert.IsFalse(sl.joinActiveGame(user, 3).success);
//        }

//        [TestMethod]
//        public void joinFailsAlreadySpectatingTest()
//        {
//            SystemUser user = sl.getUserById(0);
//            var m = sl.spectateActiveGame(user, 2);
//            Assert.IsFalse(sl.joinActiveGame(user, 2).success);
//        }

//        [TestMethod]
//        public void joinFailsGameNoExistsTest()
//        {
//            SystemUser user = sl.getUserById(0);
//            Assert.IsFalse(sl.joinActiveGame(user, 1000).success);
//        }
//    }
//}
