using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class Tests : ProjectTest    {

        string username = "Hadas";
        string usernameWrong = "Gil";
        string password = "1234";
        string statusGame = "Active";
        string gameOver = "notActive";
        string game = "Texas1";
        string game2 = "Texas1";
        List<string> activeGames = new List<string>();
        string highLeague = "league #1";
        int points = 100;
        int points2 = 0;
        string lowLeague = "league #10";


        [TestMethod]
        public void TestRegister()
        {
            //User registared correctly
            Assert.AreEqual(this.register(username, password), this.getUserbyName(username));
            //check if User already registered
            Assert.IsTrue(this.isUserExist(username,password));
            Assert.IsFalse(this.isUserExist(usernameWrong, password));
            //User enter wrong input
            Assert.AreNotEqual(this.register(username, password), this.register(usernameWrong, password));
            Assert.AreNotEqual(this.register(username, password), this.register("238///", "...."));
        }

        [TestMethod]
        public void TestLogin()
        {
            //Hadas login
            Assert.AreEqual(this.login(username, password),this.getUserbyName(username));
            //Hadas not equal to another user
            Assert.AreNotEqual(this.login(username, password), this.getUserbyName(usernameWrong));
            //Wrong input
            Assert.AreNotEqual(this.login("1234", password), this.getUserbyName(username));
            //Hadas is already exist in the system
            Assert.IsTrue(this.isUserExist(username, password));
            //Gil didn't login yet
            Assert.IsFalse(this.isUserExist(usernameWrong, password));

        }

        [TestMethod]
        public void TestLogout()
        {
            //If it is an active game than user can logout
            Assert.IsTrue(this.checkActiveGame(statusGame));
            Assert.IsTrue(this.logoutUser(statusGame, username));
            //can't logout
            Assert.IsFalse(this.checkActiveGame(gameOver));

            
        }


        [TestMethod]
        public void TestLeaveGame()
        {
            //user can exit game
            Assert.IsTrue(this.checkActiveGame(statusGame));
            Assert.IsTrue(this.isLogin(username));
            Assert.IsTrue(this.exitGame(game));
            Assert.IsTrue(this.checkAvailibleSeats(game));
            Assert.IsTrue(this.removeUserFromGame(username, game) >= 0);
            //user can't exit game
            Assert.IsFalse(this.removeUserFromGame(username, gameOver) < 0);
            Assert.IsFalse(this.checkActiveGame(gameOver));
            Assert.IsFalse(this.isLogin(usernameWrong));
            Assert.IsFalse(this.exitGame(game));
        }

        [TestMethod]
        public void TestReplayGame()
        {
            //the game is not active and exist in the system
            Assert.IsFalse(this.checkActiveGame(gameOver));
            Assert.AreNotEqual(this.selectGameToReplay(gameOver), null);
            Assert.IsTrue(this.checkActiveGame(statusGame));

        }
        
        [TestMethod]
        public void TestSaveTurns()
        {
            //check if the user is watching a replay
            Assert.IsTrue(this.isWatchingReplay(game));
            //save success
            Assert.IsTrue(this.saveTurn(game));
            //already saved the turn
            Assert.IsFalse(this.saveTurn(game2));
            //check if the replay can be saved
            Assert.IsTrue(this.saveTurn((string)this.selectGameToReplay(statusGame)));
            //check if the turn can be replay
            Assert.IsTrue(this.saveTurn(game) && this.exitGame(game));
        }


        [TestMethod]
        public void TestStoreGame()
        {
            Assert.IsTrue(this.checkActiveGame(statusGame));
            Assert.IsTrue(this.isLogin(username));
            Assert.IsTrue(this.storeGameData());

        }

        [TestMethod]
        public void TestMaintainLeagus()
        {
            //check if the user added to correct league
            Assert.IsTrue(this.isGameOver(game, username));
            Assert.AreEqual(this.joinLeaguePerPoints(points), highLeague);
            Assert.AreNotEqual(this.joinLeaguePerPoints(points), lowLeague);
            //check if new user register is added to lower league
            Assert.AreEqual(this.register(username, password), this.getUserbyName(username));
            Assert.AreEqual(this.joinLeaguePerPoints(points2), lowLeague);
            //check if league list deleted
            Assert.AreNotEqual(this.joinLeaguePerPoints(points), null);
        }

       




    }
}
