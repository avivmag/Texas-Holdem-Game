using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using DAL;
using Moq;
using Backend.User;
using System.Collections.Generic;
using ApplicationFacade;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestLeagues
    {
        GameCenter center;
        Random rnd = new Random();
        SLInterface sl = new SLImpl();

        [TestInitialize]
        public void setUp()
        {
            //var leagues = new System.Collections.Generic.List<League>();
            //leagues.Add(new League(0, 1000, "Starter League"));
            //leagues.Add(new League(1000, 2000, "Experienced League"));
            var userDummies = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };
            Random rnd = new Random();
            foreach(SystemUser u in userDummies)
            {
                u.rank = rnd.Next(0, 999999);
            }
            center = GameCenter.getGameCenter();
            center.maintainLeagues(userDummies);
            Mock <DALInterface> dalMock = new Mock<DALInterface>();
            //dalMock.Setup(x => x.addLeague(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new ReturnMessage(true,null));
            //dalMock.Setup(x => x.getAllLeagues()).Returns(leagues);
            //dalMock.Setup(x => x.removeLeague(It.IsAny<Guid>())).Returns(new ReturnMessage(true,null));
            //dalMock.Setup(x => x.setLeagueCriteria(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>())).Returns(new ReturnMessage(true, null));
            //dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns(userDummies[0]);
            //dalMock.Setup(x => x.getHighestUserId()).Returns(userDummies[3].id);
            //this.sl = new SLImpl(dalMock.Object);
            sl = new SLImpl();
            
        }
        [TestMethod]
        public void TestMaintainLeagueCount()
        {
            //var addMessage = sl.addLeague(3000, 4000, "Test League for experienced Players.");
            Assert.IsTrue(center.leagues.Count==2);
        }

        [TestMethod]
        public void TestMaintainLeagueNumPlayers()
        {
            //var leagueToRemove = sl.getLeagueByName("Experienced League");

            //var removeMessage = sl.removeLeague(leagueToRemove);

            //Assert.IsTrue(removeMessage.success);
            Assert.IsTrue(center.leagues[0].Users.Count - center.leagues[1].Users.Count <= 1);
        }

        [TestMethod]
        public void TestMaintainLeagueMaxRank()
        {

            //var duplicateRemoveMessage = sl.addLeague(300, 400, "Starter League");

            //Assert.IsFalse(duplicateRemoveMessage.success);
            Assert.IsTrue(center.leagues[0].maxRank > center.leagues[1].maxRank);
        }

        [TestMethod]
        public void TestMaintainLeagueMinRank()
        {

            //var duplicateRemoveMessage = sl.addLeague(300, 400, "Starter League");

            //Assert.IsFalse(duplicateRemoveMessage.success);
            Assert.IsTrue(center.leagues[0].minRank >= center.leagues[1].maxRank);
        }

        [TestMethod]
        public void TestMaintainSmallUsers()
        {
            List<SystemUser> allUsers = new List<SystemUser>();
            for (int i = 0; i < 9; i++)
            {
                SystemUser u = new SystemUser("", "", "", "", 100);
                u.rank = rnd.Next(0, 999999);
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
                SystemUser u = new SystemUser("", "", "", "", 100);
                u.rank = rnd.Next(0, 999999);
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
                SystemUser u = new SystemUser("", "", "", "", 100);
                u.rank = rnd.Next(0, 999999);
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
