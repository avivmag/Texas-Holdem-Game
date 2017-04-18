using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;
using System.Collections.Generic;
using DAL;
using Backend.User;

namespace TestProject
{
    //filter active games
    [TestClass]
    public class FindActiveAvailableGamesTest
    {

        BLInterface bl = new BLImpl();
        DALInterface dal = new DALDummy();

        [TestMethod]
        public void findActiveAvailableGamesSuccessTest()
        {
            List<TexasHoldemGame> active = bl.findAllActiveAvailableGames();
            Assert.AreEqual(active.Count, dal.getAllGames().Count);
        }

        [TestMethod]
        public void findActiveAvailableGamesFailTest()
        {
            
            SystemUser user2 = bl.getUserById(2);
            bl.joinActiveGame(user2, 3);

            SystemUser user3 = bl.getUserById(3);
            bl.joinActiveGame(user3, 3);

            List<TexasHoldemGame> active = bl.findAllActiveAvailableGames();

            Assert.AreNotEqual(active.Count, dal.getAllGames().Count);
        }
    }
}
