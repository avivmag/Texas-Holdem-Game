using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;
using System.Collections.Generic;
using DAL;
using Backend.User;

namespace TestProject
{
    [TestClass]
    public class FilterActiveGamesTest
    {

        BLInterface bl = new BLImpl();
        DALInterface dal = new DALDummy();

        [TestMethod]
        public void filterActiveGamesByPlayerNameSuccessTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            CollectionAssert.AreNotEqual(bl.filterActiveGamesByPlayerName("Hadas"),new List<TexasHoldemGame>());
        }

        [TestMethod]
        public void filterActiveGamesByPlayerNameTwoGamesTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPlayerName("Hadas").Count, 2);
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

            Assert.AreEqual(bl.filterActiveGamesByPotSize(0).Count, 6);
        }

        [TestMethod]
        public void filterActiveGamesByPotSizeFailsTest()
        {
            SystemUser user2 = bl.getUserById(0);
            bl.joinActiveGame(user2, 3);

            bl.joinActiveGame(user2, 0);

            Assert.AreEqual(bl.filterActiveGamesByPotSize(100).Count, 6);
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
