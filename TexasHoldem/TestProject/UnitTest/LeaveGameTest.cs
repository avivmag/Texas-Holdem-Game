using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
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
                new League(0, 1000, "Starter League"),
                new League(1000, 2000, "Experienced League")
            };

            var gamesList = new List<TexasHoldemGame>
            {
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new TexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
                new TexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
            };

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList.Find(g => (g.gameId == i)));
            dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
            this.bl = new BLImpl(dalMock.Object);
        }

        [TestMethod]
        public void LeaveSpectatorSuccessTest()
        {
            TexasHoldemGame game = bl.getGameById(bl.getAllGames()[3].gameId);
            Player spec = new Player(0);
            game.joinSpectate(spec);
            game.leaveGameSpectator(spec);
            CollectionAssert.AreEqual(game.spectators,new List<Player> { });
        }

        [TestMethod]
        public void LeavePlayerSuccessTest()
        {
            TexasHoldemGame game = bl.getGameById(bl.getAllGames()[3].gameId);
            Player p = new Player(0,100,2);
            var actualPlayers = game.players.ToString();
            var m = game.joinGame(p);
            game.leaveGamePlayer(p);
            Assert.AreEqual(game.players.ToString(), actualPlayers);
        }
    }
}
