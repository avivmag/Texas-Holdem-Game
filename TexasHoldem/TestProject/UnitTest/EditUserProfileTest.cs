using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Moq;
using DAL;
using Backend.User;
using System.Collections.Generic;
using Backend;
using ApplicationFacade;

namespace TestProject
{
    [TestClass]
    public class EditUserProfileTest
    {
        SLInterface sl;
        GameCenter center = GameCenter.getGameCenter();

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

            for (int i = 0; i<4; i++)
            {
                userList[i].id = i;
                center.loggedInUsers.Add(userList[i]);
                //center.login(userList[i].name, userList[i].password);
            }

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getAllUsers()).Returns(userList);
            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            sl = new SLImpl();
        }

        [TestMethod]
        public void successEditUserTest()
        {
            object obj = sl.editUserProfile(0, "Hadas123", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsTrue(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void alreadyExistsUserNameTest()
        {
            object obj = sl.editUserProfile(0, "aviv", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void editUserEmptyUserNameTest()
        {
            object obj = sl.editUserProfile(0, "", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void editUserEmptyPasswordTest()
        {
            object obj = sl.editUserProfile(0, "gil", "", "email7", "image5",100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void alreadyExistsEmailTest()
        {
            object obj = sl.editUserProfile(0, "gil", "1111", "1", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void negativeMoneyTest()
        {
            object obj = sl.editUserProfile(0, "gil", "1111", "email100", "image5", -100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestCleanup]
        public void TearDown()
        {
            center.shutDown();
        }
    }
}
