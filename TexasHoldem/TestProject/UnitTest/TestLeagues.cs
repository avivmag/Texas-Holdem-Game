using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;
using DAL;
using Moq;
using Backend;
using Backend.User;
using System.Collections.Generic;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestLeagues
    {
       
        BLInterface bl = new BLImpl();
        [TestInitialize]
        public void setUp()
        {
            var leagues = new System.Collections.Generic.List<League>();
            leagues.Add(new League(0, 1000, "Starter League"));
            leagues.Add(new League(1000, 2000, "Experienced League"));
            var userDummies = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };
            Mock <DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.addLeague(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new ReturnMessage(true,null));
            dalMock.Setup(x => x.getAllLeagues()).Returns(leagues);
            dalMock.Setup(x => x.removeLeague(It.IsAny<Guid>())).Returns(new ReturnMessage(true,null));
            dalMock.Setup(x => x.setLeagueCriteria(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>())).Returns(new ReturnMessage(true, null));
            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns(userDummies[0]);
            dalMock.Setup(x => x.getHighestUserId()).Returns(userDummies[3].id);
            this.bl = new BLImpl(dalMock.Object);
            
        }
        [TestMethod]
        public void TestAddNewLeague()
        {
            var addMessage = bl.addLeague(3000, 4000, "Test League for experienced Players.");

            Assert.IsTrue(addMessage.success);

        }
        [TestMethod]
        public void RemoveNewLeague()
        { 
            var leagueToRemove = bl.getLeagueByName("Experienced League");

            var removeMessage = bl.removeLeague(leagueToRemove);

            Assert.IsTrue(removeMessage.success);

        }

        [TestMethod]
        public void TestAddNewLeagueWithSameName()
        {
            
            var duplicateRemoveMessage = bl.addLeague(300, 400, "Starter League");

            Assert.IsFalse(duplicateRemoveMessage.success);
        }

        [TestMethod]
        public void AddLeagueWithNegativeMinRank()
        {
            var NegativeRankMessage = bl.addLeague(-5, 100, "test.");

            Assert.IsTrue(NegativeRankMessage.success == false && NegativeRankMessage.description.Contains("invalid minRank"));
        }

        [TestMethod]
        public void AddLeagueWithMaxRankLowerThanMinRank()
        {
            var LowMaxRankMessage = bl.addLeague(2, -2, "test.");

            Assert.IsTrue(LowMaxRankMessage.success == false && LowMaxRankMessage.description.Contains("maxRank has to be bigger than minRank"));
        }

        [TestMethod]
        public void AddLeagueWithMinRankHigherThanMaxRank()
        {
            var HighMinRankMessage = bl.addLeague(2, 1, "test.");

            Assert.IsTrue(HighMinRankMessage.success == false && HighMinRankMessage.description.Contains("maxRank has to be bigger than minRank"));

        }

        [TestMethod]
        public void TestGetLeagueByName()
        {
            var league = bl.getLeagueByName("Starter League");

            Assert.IsTrue(league != null);
        }

        [TestMethod]
        public void TestGetLeagueById()
        {
            var league = bl.getLeagueByName("Starter League");

            var league2 = bl.getLeagueById(league.leagueId);
            Assert.IsTrue(league2 != null && league.leagueId == league2.leagueId);
        }

        [TestMethod]
        public void TestGetNonExistsLeagueById()
        {
            var league = bl.getLeagueById(Guid.NewGuid());

            Assert.IsTrue(league == null);
        }

        [TestMethod]
        public void TestGetNonExistsLeagueByName()
        {
            var league = bl.getLeagueByName(String.Empty);

            Assert.IsTrue(league == null);
        }

        [TestMethod]
        public void TestSetCriteria()
        {
            var highestPlayerId = bl.getUserById(3).id;
            var league = bl.getLeagueByName("Experienced League");
            var setCriteriaMessage = bl.setLeagueCriteria(2100, 3100, "Experienced League", league.leagueId, highestPlayerId);
            Assert.IsTrue(setCriteriaMessage.success);
        }

        [TestMethod]
        public void TestSetCriteriaWithTakenName()
        {
            var highestPlayerId = bl.getUserById(3).id;
            var league = bl.getLeagueByName("Experienced League");
            var setCriteriaMessage = bl.setLeagueCriteria(2000, 3000, "Starter League", league.leagueId, highestPlayerId);
            Assert.IsFalse(setCriteriaMessage.success);
        }

        [TestMethod]
        public void TestSetCriteriaWithBadMinRank()
        {
            var highestPlayerId = bl.getUserById(3).id;
            var league = bl.getLeagueByName("Experienced League");
            var setCriteriaMessage = bl.setLeagueCriteria(-100, 3000, "Experienced League", league.leagueId, highestPlayerId);
            Assert.IsFalse(setCriteriaMessage.success);
        }

        [TestMethod]
        public void TestSetCriteriaWithBadMaxRank()
        {
            var highestPlayerId = bl.getUserById(3).id;
            var league = bl.getLeagueByName("Experienced League");
            var setCriteriaMessage = bl.setLeagueCriteria(2000, -300, "Experienced League", league.leagueId, highestPlayerId);
            Assert.IsFalse(setCriteriaMessage.success);
        }
    }
}
