using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Moq;
//using DAL;
using Backend.User;
using System.Collections.Generic;
using Backend;
using ApplicationFacade;
using Database;

namespace TestProject
{
    [TestClass]
    public class EditUserProfileTest
    {
        SLInterface sl;
        private IDB db;
        GameCenter center = GameCenter.getGameCenter();

        [TestCleanup]
        public void Cleanup()
        {
            for (int i = 0; i < 4; i++)
                db.deleteUser(db.getUserByName("test" + i).id);

            center.shutDown();
        }

        [TestInitialize]
        public void SetUp()
        {
            db = new DBImpl();
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i);
            }
            db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false);
            db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false);
            db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false);
            db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false);


            var userList = new List<SystemUser>
            {
                db.getUserByName("test0"),
                db.getUserByName("test1"),
                db.getUserByName("test2"),
                db.getUserByName("test3")
                //new SystemUser("Hadas", "email0", "image0", 1000),
                //new SystemUser("Gili", "email1", "image1", 0),
                //new SystemUser("Or", "email2", "image2", 700),
                //new SystemUser("Aviv", "email3", "image3", 1500)
            };

            center = GameCenter.getGameCenter();

            ////set users ranks.
            //userList[0].rank = 10;
            //userList[1].rank = 15;
            //userList[2].rank = 20;
            //userList[3].rank = 25;

            //for (int i = 0; i < 4; i++)
            //{
            //    userList[i].id = i;
            //    center.loggedInUsers.Add(userList[i]);
            //    //center.login(userList[i].name, userList[i].password);
            //}


            //Mock<DALInterface> dalMock = new Mock<DALInterface>();
            //dalMock.Setup(x => x.getAllUsers()).Returns(userList);
            //dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            sl = new SLImpl();
        }

        [TestMethod]
        public void successEditUserTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "Hadas123", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsTrue(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void alreadyExistsUserNameTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "test3", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void editUserEmptyUserNameTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "", "12345", "email7", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void editUserEmptyPasswordTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "gil", "", "email7", "image5",100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void alreadyExistsEmailTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "gil", "1111", "email3", "image5", 100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }

        [TestMethod]
        public void negativeMoneyTest()
        {
            object obj = sl.editUserProfile(db.getUserByName("test0").id, "gil", "1111", "email100", "image5", -100);
            Assert.IsInstanceOfType(obj, typeof(ReturnMessage));
            Assert.IsFalse(((ReturnMessage)obj).success);
        }
    }
}
