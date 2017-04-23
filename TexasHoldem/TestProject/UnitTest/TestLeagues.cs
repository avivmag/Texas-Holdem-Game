using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.Game;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestLeagues
    {
        BLInterface bl = new BLImpl();
        
        [TestMethod]
        public void TestAddAndRemoveNewLeague()
        {
            var addMessage = bl.addLeague(3000, 4000, "Test League for experienced Players.");

            Assert.IsTrue(addMessage.success);

            var leagueToRemove = bl.getLeagueByName("Test League for experienced Players.");

            var removeMessage = bl.removeLeague(leagueToRemove.leagueId);

            Assert.IsTrue(removeMessage.success);

        }

        [TestMethod]
        public void TestAddNewLeagueWithSameName()
        {
            bl.addLeague(30, 40, "Test League for experienced Players.");
            var duplicateRemoveMessage = bl.addLeague(300, 400, "Test League for experienced Players.");

            Assert.IsFalse(duplicateRemoveMessage.success);
        }

        [TestMethod]
        public void TestRemoveLeagueThatDoesntExists()
        {
            var notExistsRemovalMessage = bl.removeLeague(new Guid());

            Assert.IsFalse(notExistsRemovalMessage.success);
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
