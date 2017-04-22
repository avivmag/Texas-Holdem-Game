using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.User;

namespace TestProject
{
    [TestClass]
    public class JoinActiveGameTest
    {

        BLInterface bl = new BLImpl();

        [TestMethod]
        public void joinSuccessTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsTrue(bl.joinActiveGame(user,2).success);
        }

        [TestMethod]
        public void joinSuccessLeagueGameTest()
        {
            SystemUser user = bl.getUserById(0);
            var m = bl.joinActiveGame(user, 4);
            Assert.IsTrue(m.success);
        }

        [TestMethod]
        public void joinFailesNoSeatsTest()
        {
            SystemUser user2 = bl.getUserById(2);
            bl.joinActiveGame(user2, 2);

            SystemUser user3 = bl.getUserById(3);
            bl.joinActiveGame(user3,3);

            SystemUser user0 = bl.getUserById(0);
            Assert.IsFalse(bl.spectateActiveGame(user0, 1).success);
        }

        [TestMethod]
        public void joinFailesNoMoneyTest()
        {
            SystemUser user1 = bl.getUserById(1);
            Assert.IsFalse(bl.spectateActiveGame(user1, 1).success);
        }

        [TestMethod]
        public void joinFailsLeagueGameTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsFalse(bl.joinActiveGame(user, 5).success);
        }

        [TestMethod]
        public void joinFailsAlreadyPlayTest()
        {
            SystemUser user = bl.getUserById(0);
            bl.joinActiveGame(user, 3);
            Assert.IsFalse(bl.joinActiveGame(user, 3).success);
        }

        [TestMethod]
        public void joinFailsAlreadySpectatingTest()
        {
            SystemUser user = bl.getUserById(0);
            var m = bl.spectateActiveGame(user, 2);
            Assert.IsFalse(bl.joinActiveGame(user, 2).success);
        }

        [TestMethod]
        public void joinFailsGameNoExistsTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsFalse(bl.joinActiveGame(user, 1000).success);
        }
    }
}
