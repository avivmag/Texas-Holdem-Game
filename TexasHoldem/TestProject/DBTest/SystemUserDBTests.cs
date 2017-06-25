using PeL;
using Backend.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestProject.DBTest
{
    [TestClass]
    public class SystemUserDBTests
    {
        IPeL db;

        [TestInitialize]
        public void SetUp()
        {
            db = new PeLImpl();
        }

        [TestMethod]
        public void addUserInDB()
        {
            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, null));
            }
        }

        [TestMethod]
        public void editUserInDB()
        {
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
            }
            Assert.IsTrue(db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false));
        }

        [TestMethod]
        public void EditUserLeaderBoardsDB()
        {
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
            }

            Assert.IsTrue(db.EditUserLeaderBoardsById(db.getUserByName("test0").id, null, null));
            Assert.IsTrue(db.EditUserLeaderBoardsById(db.getUserByName("test1").id, 0, 100));
            Assert.IsTrue(db.EditUserLeaderBoardsById(db.getUserByName("test2").id, 5, 30));
            Assert.IsTrue(db.EditUserLeaderBoardsById(db.getUserByName("test3").id, 999, 999));
        }

        [TestMethod]
        public void getUserByIdDB()
        {
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
            }
            Assert.AreEqual(db.getUserById(db.Login("test0","0")).id,db.getUserByEmail("email0").id);
            Assert.AreNotEqual(db.getUserById(db.Login("test1", "1")).id, db.getUserByEmail("email2").id);

        }


        [TestMethod]
        public void getUserByEmailDB()
        {
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
            }
            Assert.AreEqual(db.getUserByName("test0").id, db.getUserByEmail("email0").id);
            Assert.AreNotEqual(db.getUserByName("test2").id, db.getUserByEmail("email3").id);

        }

        [TestMethod]
        public void getAllUsersDB()
        {
            List<SystemUser> usersList = new List<SystemUser>();
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
                usersList.Add(db.getUserByName("test" + i));
            }
            CollectionAssert.AreEqual(db.getAllSystemUsers(),usersList);
        }


        [TestMethod]
        public void removeUserInDB()
        {
            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, null));
            
            Assert.IsTrue(db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false));

            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.deleteUser(db.getUserByName("test" + i).id));

            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, null));
        }
        
        
        [TestCleanup]
        public void Cleanup()
        {
            for (int i = 0; i < 4; i++)
                if (db.getUserByName("test" + i)!=null)
                    db.deleteUser(db.getUserByName("test" + i).id);
        }
    }
}
