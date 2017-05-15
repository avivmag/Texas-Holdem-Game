using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Moq;
using DAL;
using Backend.User;
using Backend;
using System.Collections.Generic;

namespace TestProject.UnitTest
{
	[TestClass]
	public class LoginTest
	{
        SLInterface bl;

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
            dalMock.Setup(x => x.getUserByName(It.IsAny<string>())).Returns((string name) => usersList.Find(u => u.name == name));
            this.bl = new SLImpl(dalMock.Object);
        }
		[TestMethod]
		public void successTest()
		{
            var registerMessage = bl.Login("Or", "111111");
            Assert.IsTrue(registerMessage.success);
		}

		[TestMethod]
		public void alreadyLoggedInTest()
		{
			bl.Register("crash bandicoot", "green boxes are the worst", "crash@bash.bugabugabuga", "hedgehog photo");
			bl.Login("crash bandicoot", "green boxes are the worst");
			Assert.IsFalse(bl.Login("crash bandicoot", "green boxes are the worst").success);
		}

		[TestMethod]
		public void notRegisteredTest()
		{
			bl.Login("a worm from worms world party", "for a donkey, for a donkey, my kindom for a donkey");
			Assert.IsFalse(bl.Login("a worm from worms world party", "for a donkey, for a donkey, my kindom for a donkey").success);
		}

		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(bl.Login("", "Transparancy is overwhelming").success);
		}

		[TestMethod]
		public void emptyPasswordTest()
		{
			Assert.IsFalse(bl.Login("sandro the living dead", "").success);
		}
	}

}
