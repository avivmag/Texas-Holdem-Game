using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestGameCreate
    {
        BLInterface bl = new BLImpl();

        [TestMethod]
        public void TestCreateGame()
        {
            var user = bl.getUserById(0);
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                15000,
                120,
                4,
                8,
                true);
            var gameCreationMessage = bl.createGame(user.id, pref);

            Assert.IsTrue(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithNonExistantUser()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                15000,
                120,
                4,
                8,
                true);
            var gameCreationMessage = bl.createGame(95, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithNegativeBuyIn()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                -100,
                15000,
                120,
                4,
                8,
                true);
            var gameCreationMessage = bl.createGame(0, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithNegativeChipsCount()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                -15000,
                120,
                4,
                8,
                true);
            var gameCreationMessage = bl.createGame(0, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithMinimalBetHigherThanChipsCount()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                15,
                120,
                4,
                8,
                true);
            var gameCreationMessage = bl.createGame(0, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithOneMinPlayer()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                15000,
                120,
                1,
                8,
                true);
            var gameCreationMessage = bl.createGame(0, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }

        [TestMethod]
        public void TestCreateGameWithMaxPlayersLowerThanMinPlayers()
        {
            GamePreferences pref = new GamePreferences(
                GamePreferences.GameTypePolicy.limit,
                100,
                15000,
                120,
                4,
                3,
                true);
            var gameCreationMessage = bl.createGame(0, pref);

            Assert.IsFalse(gameCreationMessage.success);
        }
    }
}
