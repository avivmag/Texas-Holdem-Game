using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using System.Collections.Generic;
using Backend.User;
using Moq;
using DAL;
using Backend;
namespace TestProject.UnitTest
{
	[TestClass]
	public class RegisterTest
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
		public void successTest()
		{
			Assert.IsTrue(bl.Register("Scorpion", "GET OVER HERE!!", "scorpy@winnig.tournament.mk", "deadly skull behind a mask").success);
		}
        
		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(bl.Register("", "P@SSW0RD", "gmail@gmail.com", "none").success);
		}

		[TestMethod]
		public void emptyPasswordTest()
		{
			Assert.IsFalse(bl.Register("NeverPassworded", "", "no@password.com", "ying yang photo").success);
		}

		[TestMethod]
		public void alreadyExistsEmailTest()
		{
			Assert.IsFalse(bl.Register("NeverPassworded", "", "no@password.com", "ying yang photo").success);
		}
	}

}
