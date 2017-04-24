using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Moq;
using DAL;
using Backend.User;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class EditUserProfileTest
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

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getAllUsers()).Returns(userList);
            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            this.bl = new BLImpl(dalMock.Object);
        }

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
