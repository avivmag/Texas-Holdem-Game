using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;

namespace TestProject
{
    [TestClass]
    public class EditUserProfileTest
    {
        BLInterface bl = new BLImpl();

        [TestMethod]
        public void successTest()
        {
            Assert.IsTrue(bl.editUserProfile(0, "Hadas123", "12345", "email7", "image5").success);
        }

        [TestMethod]
        public void alreadyExistsUserNameTest()
        {
            Assert.IsFalse(bl.editUserProfile(0, "gili", "12345", "email7", "image5").success);
        }

        [TestMethod]
        public void emptyUserNameTest()
        {
            Assert.IsFalse(bl.editUserProfile(0, "", "12345", "email7", "image5").success);
        }

        [TestMethod]
        public void emptyPasswordTest()
        {
            Assert.IsFalse(bl.editUserProfile(0, "gil", "", "email7", "image5").success);
        }

        [TestMethod]
        public void alreadyExistsEmailTest()
        {
            Assert.IsFalse(bl.editUserProfile(0, "gil", "1111", "email2", "image5").success);
        }
    }
}
