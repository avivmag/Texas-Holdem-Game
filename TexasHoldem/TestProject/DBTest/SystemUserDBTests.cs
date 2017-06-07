using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.DBTest
{
    [TestClass]
    public class SystemUserDBTests
    {
        IDB db;

        [TestInitialize]
        public void SetUp()
        {
            db = new DBImpl();
        }

        [TestMethod]
        public void addUserInDB()
        {
            for (int i = 0; i < 4; i++)
            {
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i));
            }
        }

        [TestMethod]
        public void editUserInDB()
        {
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i);
            }
            Assert.IsTrue(db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false));
        }

        [TestMethod]
        public void removeUserInDB()
        {
            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i));
            
            Assert.IsTrue(db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false));

            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.deleteUser(db.getUserByName("test" + i).id));
        }

        [TestMethod]
        public void removeAllUsersInDB()
        {
            for (int i = 0; i < 4; i++)
                Assert.IsTrue(db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i));

            Assert.IsTrue(db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false));
            Assert.IsTrue(db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false));
            
            Assert.IsTrue(db.deleteUsers());
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
