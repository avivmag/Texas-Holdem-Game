using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Backend.User;
using Backend.Game;
using System.Collections.Generic;
using Moq;
using DAL;

namespace TestProject
{
    //filter active games
    //find active games
    [TestClass]
    public class LeaveGameGameTest
    {

        SLInterface sl;

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

            //var leagues = new List<League>
            //{
            //    new League(0, 1000, "Starter League"),
            //    new League(1000, 2000, "Experienced League")
            //};

            var gamesList = new List<TexasHoldemGame>
            {
                new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
                new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
            };

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList.Find(g => (g.gameId == i)));
            dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
            this.sl = new SLImpl(dalMock.Object);
        }

        [TestMethod]
        public void LeaveSpectatorSuccessTest()
        {
            TexasHoldemGame game = sl.getGameById(sl.getAllGames()[3].gameId);
            SystemUser user = new SystemUser("Gil", "123123", "adfg", null, 0);
            game.joinSpectate(user);
            game.leaveGameSpectator(user);
            CollectionAssert.AreEqual(game.spectators,new List<SystemUser> { });
        }

        [TestMethod]
        public void LeavePlayerSuccessTest()
        {
            TexasHoldemGame game = sl.getGameById(sl.getAllGames()[3].gameId);
            SystemUser user = new SystemUser("Gil", "123", "gmail", "", 100);
            //Player p = new Player(0,100,2);
            var actualPlayers = game.players.ToString();
            sl.joinActiveGame(user, game.gameId);
            //var m = game.joinGame(user);
            //game.leaveGamePlayer(user);
            sl.leaveGame(user, game.gameId);
            Assert.AreEqual(game.players.ToString(), actualPlayers);
        }
    }
}
