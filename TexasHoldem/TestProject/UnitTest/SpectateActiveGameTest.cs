using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.User;

namespace TestProject
{
    [TestClass]
    public class SpectateActiveGameTest
    {

        BLInterface bl = new BLImpl();

        [TestMethod]
        public void spectateSuccessTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsTrue(bl.spectateActiveGame(user,0).success);
        }

        [TestMethod]
        public void spectateFailesPreferencesTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsFalse(bl.spectateActiveGame(user, 1).success);
        }

        [TestMethod]
        public void spectateFailesAlreadySpectateTest()
        {
            SystemUser user = bl.getUserById(3);
            bl.spectateActiveGame(user, 0);
            Assert.IsFalse(bl.spectateActiveGame(user, 0).success);
        }

        [TestMethod]
        public void spectateFailesAlreadyPlayTest()
        {
            SystemUser user = bl.getUserById(3);
            bl.joinActiveGame(user, 0);
            Assert.IsFalse(bl.spectateActiveGame(user, 0).success);
        }

        [TestMethod]
        public void spectateFailsGameNoExistsTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsFalse(bl.spectateActiveGame(user, 1000).success);
        }
    }
}
