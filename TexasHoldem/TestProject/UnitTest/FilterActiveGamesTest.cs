using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;
using System.Collections.Generic;
using DAL;
using Backend.User;
using Moq;

namespace TestProject
{
    [TestClass]
    public class FilterActiveGamesTest
    {
        BLInterface bl;
        [TestInitialize]
        public void SetUp()
        {
            var userList = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };


            var leagues = new List<League>
            {
                new League(0, 1000, "hi"),
                new League(1000, 2000, "bye")
            };

            var gamesList = new List<TexasHoldemGame>
            {                                             
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new TexasHoldemGame(2, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new TexasHoldemGame(2, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new TexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
                new TexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
            };
            
            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList[i]);
            this.bl = new BLImpl(dalMock.Object);
        }
        [TestMethod]
        public void filterActiveGamesByPlayerNameSuccessTest()
        {
            SystemUser user2 = bl.getUserById(2);
            var m = bl.joinActiveGame(user2, 3);

            CollectionAssert.AreNotEqual(bl.filterActiveGamesByPlayerName("Hadas"),new List<TexasHoldemGame>());
        }

        [TestMethod]
        public void filterActiveGamesByPlayerNameTwoGamesTest()
        {
            SystemUser user2 = bl.getUserById(2);
            var m = bl.joinActiveGame(user2, 3);

            var m2 = bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPlayerName("Hadas").Count, 3);
        }

        [TestMethod]
        public void filterActiveGamesByPlayerNameTwoGamesFailsTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPlayerName("Shaked").Count, 0);
        }

        [TestMethod]
        public void filterActiveGamesByPotSizeTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPotSize(0).Count, 8);
        }

        [TestMethod]
        public void filterActiveGamesByPotSizeFailsTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPotSize(100).Count, 8);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesTest()
        {
            GamePreferences pref = new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true);
            
            Assert.AreEqual(bl.filterActiveGamesByGamePreferences(pref).Count, 1);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesThreeGamesTest()
        {
            GamePreferences pref = new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false);

            Assert.AreEqual(bl.filterActiveGamesByGamePreferences(pref).Count, 3);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesFailsTest()
        {
            GamePreferences pref = new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 1000000, 500, 20, 2, 2, false);

            Assert.AreEqual(bl.filterActiveGamesByGamePreferences(pref).Count, 0);
        }
    }
}
