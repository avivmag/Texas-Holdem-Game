using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestBlinds
    {
        private Player p1;
        private Player p2;
        private Player p3;
        private GamePreferences gp;
        private TexasHoldemGame game;

        [TestInitialize]
        public void SetUp()
        {
            p1 = new Player(1, 20, 20);
            p2 = new Player(2, 30, 30);
            p3 = new Player(3, 40, 40);
            gp = new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 30, 50, 10, 2, 10, true);
            game = new TexasHoldemGame(1, gp);
            game.joinGame(p1);
            game.joinGame(p2);
            game.joinGame(p3);

        }

        [TestMethod]
        public void TestSmallBlindFirst()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Out.WriteLine(game.players[i].id);
            }

            game.currentDealer = 0;
            Assert.IsTrue(game.setSmallBlind() == 1);
        }

        [TestMethod]
        public void TestSmallBlindLast()
        {
            game.currentDealer = 2;
            Assert.IsTrue(game.setSmallBlind() == 0);
        }

        [TestCleanup]
        public void TearDown()
        {
            game.leaveGame(p1);
            game.leaveGame(p2);
            game.leaveGame(p3);
        }
    }
}
