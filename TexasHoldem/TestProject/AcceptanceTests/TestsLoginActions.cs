using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestProject.AcceptanceTests
{
    [TestClass]
    public class TestsLoginActions :ProjectTest
    {
        string username = "Hadas";
        string usernameWrong = "Gil";
        string password = "1234";
        string statusGame = "Active";
        string gameOver = "notActive";
        string email = "gmail@gmail.com";
        string img = "img";
        string game = "Texas1";
        string game2 = "Texas1";
        string seatsNotAv = "none";
        List<string> activeGames = new List<string>();
        string criteria = "points";


        [TestInitialize]
        public void Initialized()
        {
            //check if there is a user in the system
            Assert.IsTrue(this.isUserExist(username, password));
            Assert.IsTrue(this.isLogin(username));
            Assert.AreEqual(this.login(username, password), this.getUserbyName(username));
            Assert.AreNotEqual(this.login(username, password), null);
        }
        [TestMethod]
        public void TestEditProfile()
        {
            //check if there is a user that can be edited ****for security policy***
            Assert.IsNotNull(this.editProfile(username));
            //check if the user can be updated
            Assert.IsTrue(this.editName(username));
            Assert.IsTrue(this.editImage(img));
            Assert.IsTrue(this.editEmail(email));
            //check wrong input for update 
            Assert.IsFalse(this.editImage(username));
            Assert.IsFalse(this.editEmail(username));
            Assert.IsFalse(this.editName(img));
            //check if the user name is already taken or email
            Assert.AreNotEqual(this.editName(username), this.isUserExist(usernameWrong, password));
            Assert.AreNotEqual(this.editEmail(email), this.getUserbyName(username));

        }

        [TestMethod]
        public void TestCreatGame()
        {
            Assert.AreNotEqual(this.creatGame(game), null);
            //check if perferneces ok
            Assert.IsTrue(this.isGameDefOK(game));
            Assert.IsTrue(this.addPlayerToGame(username, game));
            //check wrong input
            Assert.AreNotEqual(this.creatGame(password), this.creatGame(game));
            Assert.IsFalse(this.addPlayerToGame(password, game));

        }

        [TestMethod]
        public void TestjoinExistingGame()
        {
            Assert.AreEqual(this.selectGametoJoin(game), this.selectGametoJoin(game2));
            //check if the game is exist
            Assert.AreNotEqual(this.selectGametoJoin(game), null);
            //check for not an existing game
            Assert.AreEqual(this.selectGametoJoin(seatsNotAv), null);
            Assert.IsTrue(this.checkAvailibleSeats(game));
            //no availble seats
            Assert.IsFalse(this.checkAvailibleSeats(seatsNotAv));
     
            Assert.IsTrue(this.addPlayerToGame(username, game));

        }
        [TestMethod]
        public void TestSpectateActiveGame()
        {
            //check the game is active
            Assert.IsTrue(this.checkActiveGame(statusGame));
            //check the game is inactive
            Assert.IsFalse(this.checkActiveGame(gameOver));
            //check the game is exist
            Assert.AreNotEqual(this.selectGametoJoin(game), null);
            Assert.AreEqual(this.spectateActiveGame(game), selectGametoJoin(game));
            Assert.AreNotEqual(this.spectateActiveGame(gameOver), selectGametoJoin(game));
        }
        [TestMethod]
        public void TestFindActiveGame()
        {
            //there is at least one game that is active
            Assert.IsTrue(this.checkActiveGame(statusGame));
            Assert.AreEqual(this.findAllActive(), activeGames);
            //zero games when there are noy any active games
            Assert.IsFalse(this.checkActiveGame(gameOver));
            Assert.AreEqual(this.findAllActive(), gameOver);
        }

        [TestMethod]
        public void TestFilterActiveGame()
        {
            //the system was able to find a list with this criteria
            Assert.AreEqual(this.filterByCriteria(criteria), activeGames);
            Assert.AreNotEqual(this.filterByCriteria(criteria), "empty list");
            //check if filter all active games
            Assert.AreNotEqual(this.findAllActive(), this.filterByCriteria(criteria));
        }
    }
}
