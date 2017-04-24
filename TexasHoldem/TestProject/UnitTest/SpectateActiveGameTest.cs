using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.User;
using Backend;
using Moq;
using DAL;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class SpectateActiveGameTest
    {

        BLInterface bl;

        [TestInitialize]
        public void SetUp()
        {
            var usersList = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.registerUser(It.IsAny<SystemUser>())).Returns(new ReturnMessage(true, null));
            dalMock.Setup(x => x.logUser(It.IsAny<string>())).Returns(new ReturnMessage(true, null));
            dalMock.Setup(x => x.logOutUser(It.IsAny<string>())).Returns(new ReturnMessage(true, null));
            dalMock.Setup(x => x.getUserByName(It.IsAny<string>())).Returns((string name) => usersList.Find(u => u.name == name));
            this.bl = new BLImpl(dalMock.Object);
        }

        [TestMethod]
        public void spectateSuccessTest()
        {
            SystemUser user = bl.getUserById(0);
            Assert.IsTrue(bl.spectateActiveGame(user,2).success);
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
