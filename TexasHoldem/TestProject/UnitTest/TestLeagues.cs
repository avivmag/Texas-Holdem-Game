using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Moq;
using Backend.User;
using System.Collections.Generic;
using ApplicationFacade;
using PeL;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestLeagues
    {
        GameCenter center;
        Random rnd = new Random();
        SLInterface sl = new SLImpl();
        private IPeL db;

        [TestCleanup]
        public void Cleanup()
        {
            for (int i = 0; i < 4; i++)
                db.deleteUser(db.getUserByName("test" + i).id);
            center.shutDown();
        }

        [TestInitialize]
        public void setUp()
        {
            db = new PeLImpl();
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, null);
            }
            db.EditUserById(db.getUserByName("test0").id, null, null, null, null, 1000, 10, false);
            db.EditUserById(db.getUserByName("test1").id, null, null, null, null, 0, 15, false);
            db.EditUserById(db.getUserByName("test2").id, null, null, null, null, 700, 20, false);
            db.EditUserById(db.getUserByName("test3").id, null, null, null, null, 1500, 25, false);


            var userDummies = new List<SystemUser>
            {
                db.getUserByName("test0"),
                db.getUserByName("test1"),
                db.getUserByName("test2"),
                db.getUserByName("test3")
            };

            Random rnd = new Random();
            foreach(SystemUser u in userDummies)
            {
                u.rank = rnd.Next(0, 999999);
            }
            center = GameCenter.getGameCenter();
            center.maintainLeagues(userDummies);
            sl = new SLImpl();
            
        }
        [TestMethod]
        public void TestMaintainLeagueCount()
        {
            Assert.IsTrue(center.leagues.Count==2);
        }

        [TestMethod]
        public void TestMaintainLeagueNumPlayers()
        {
            Assert.IsTrue(center.leagues[0].Users.Count - center.leagues[1].Users.Count <= 1);
        }

        [TestMethod]
        public void TestMaintainLeagueMaxRank()
        {
            Assert.IsTrue(center.leagues[0].maxRank > center.leagues[1].maxRank);
        }

        [TestMethod]
        public void TestMaintainLeagueMinRank()
        {
            Assert.IsTrue(center.leagues[0].minRank >= center.leagues[1].maxRank);
        }

        [TestMethod]
        public void TestMaintainSmallUsers()
        {
            List<SystemUser> allUsers = new List<SystemUser>();
            for (int i = 0; i < 9; i++)
            {
                SystemUser u = new SystemUser("", "", "", 100, rnd.Next(0, 999999));
                allUsers.Add(u);
            }
            center.maintainLeagues(allUsers);
            Assert.IsTrue(center.leagues.Count == 3);

            for (int i = 0; i < center.leagues.Count; i++)
            {
                Assert.IsTrue(center.leagues[i].maxRank >= center.leagues[i].minRank);
            }

            for (int i = 0; i < center.leagues.Count - 1; i++)
            {
                Assert.IsTrue(center.leagues[i].minRank >= center.leagues[i + 1].maxRank);
            }

            for (int i = 0; i < center.leagues.Count; i++)
            {
                for (int j = i; j < center.leagues.Count; j++)
                {
                    Assert.IsTrue(center.leagues[i].Users.Count == center.leagues[j].Users.Count + 1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count - 1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count);
                }
            }
        }

        [TestMethod]
        public void TestMaintainMediumUsers()
        {
            List<SystemUser> allUsers = new List<SystemUser>();
            for (int i = 0; i < 50; i++)
            {
                SystemUser u = new SystemUser("", "", "", 100, rnd.Next(0, 999999));
                allUsers.Add(u);
            }
            center.maintainLeagues(allUsers);
            Assert.IsTrue(center.leagues.Count == 8);

            for (int i = 0; i < center.leagues.Count; i++)
            {
                Assert.IsTrue(center.leagues[i].maxRank >= center.leagues[i].minRank);
            }

            for (int i = 0; i < center.leagues.Count - 1; i++)
            {
                Assert.IsTrue(center.leagues[i].minRank >= center.leagues[i + 1].maxRank);
            }

            for (int i = 0; i < center.leagues.Count; i++)
            {
                for (int j = i; j < center.leagues.Count; j++)
                {
                    Assert.IsTrue(center.leagues[i].Users.Count == center.leagues[j].Users.Count + 1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count - 1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count);
                }
            }
        }

        [TestMethod]
        public void TestMaintainBigUsers()
        {
            List<SystemUser> allUsers = new List<SystemUser>();
            for(int i = 0; i<128; i++)
            {
                SystemUser u = new SystemUser("", "", "", 100, rnd.Next(0, 999999));
                allUsers.Add(u);
            }
            center.maintainLeagues(allUsers);
            Assert.IsTrue(center.leagues.Count == 12);

            for (int i=0; i<center.leagues.Count; i++)
            {
                Assert.IsTrue(center.leagues[i].maxRank >= center.leagues[i].minRank);
            }

            for (int i = 0; i < center.leagues.Count-1; i++)
            {
                Assert.IsTrue(center.leagues[i].minRank >= center.leagues[i+1].maxRank);
            }

            for (int i = 0; i < center.leagues.Count; i++)
            {
                for(int j = i; j<center.leagues.Count; j++)
                {
                    Assert.IsTrue(center.leagues[i].Users.Count == center.leagues[j].Users.Count +1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count - 1 ||
                        center.leagues[i].Users.Count == center.leagues[j].Users.Count);
                }
            }
        }
    }
}
