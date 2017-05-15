using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using System.Collections.Generic;
using Moq;
using DAL;
using Backend.User;
using Backend;

namespace TestProject.UnitTest
{
	[TestClass]
	public class LogOutTest
	{
        SLInterface sl;

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
            this.sl = new SLImpl(dalMock.Object);
        }

        [TestMethod]
		public void successTest()
		{
			sl.Login("Or", "111111");
			Assert.IsTrue(sl.Logout("Or").success);
		}

		[TestMethod]
		public void notLoggedInTest()
		{
			Assert.IsFalse(sl.Logout("an outsider").success);
		}

		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(sl.Logout("").success);
		}

		[TestMethod]
		public void logOutTwiceTest()
		{
			sl.Register("rick roll", "never gonna give you up", "never@gonna.let.you.down", "a picture of something completly not related to rick roll");
			sl.Login("rick roll", "never gonna give you up");
			sl.Logout("rick roll");
			Assert.IsFalse(sl.Logout("rick roll").success);
		}
	}
}