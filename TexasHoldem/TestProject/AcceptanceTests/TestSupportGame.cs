using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.AcceptanceTests
{
    [TestClass]
    public class TestSupportGame : ProjectTest
    {
        string username = "Hadas";
        string usernameWrong = "Gil";
        string password = "1234";
        string statusGame = "Active";
        string statusGame2 = "notActive";
        string email = "gmail@gmail.com";
        string img = "img";
        string game = "Texas1";
        string game2 = "Texas1";
        string seatsNotAv = "none";
        string criteria = "points";
        string highLeague = "league #1";
        int points = 100;
        int points2 = 0;
        string lowLeague = "league #10";
        int amountToBet = 1000;
        string statePlayer = "Player Bet";


        [TestMethod]
        public void TestBet()
        {
            //bet in an existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //enough chips to bet
            Assert.IsTrue(this.bet(amountToBet));
            //bet again in the current round
            Assert.IsFalse(this.bet(amountToBet));
            Assert.IsTrue(this.updatePot(amountToBet));
            Assert.IsTrue(this.updateStatePlayer(statePlayer, amountToBet));
            //not enough chips
            Assert.IsFalse(this.updatePot(34326));

        }

        [TestMethod]
        public void TestRaise()
        {
            //raise in an existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //one player already bet 
            Assert.IsTrue(this.bet(amountToBet) && this.raise(amountToBet));
            Assert.IsTrue(this.updatePot(amountToBet));
            //raise to lower of bet
            Assert.IsFalse(this.raise(0));
            //update to raise game
            Assert.IsTrue(this.updateStatePlayer(statePlayer, amountToBet));
            //not enough chips
            Assert.IsFalse(this.updatePot(10000000));

        }

        [TestMethod]
        public void TestCall()
        {
            //call in an existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //request to call success
            Assert.IsTrue(this.call(amountToBet));
            Assert.IsTrue(this.updatePot(amountToBet));
            Assert.IsTrue(this.updateStatePlayer(statePlayer, amountToBet));
            //call without enough money
            Assert.IsFalse(this.call(0));
            //*NOT SURE IF WE NEED TO IMPLEMENT TEST function FOR ALL-IN*//

        }

        [TestMethod]
        public void TestFold()
        {
            //call in an existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //fold success
            Assert.IsTrue(this.fold());
            // update the amount to be the same as before the fold
            Assert.IsTrue(this.updateStatePlayer(statePlayer, amountToBet));

        }

        [TestMethod]
        public void TestCheck()
        {
            //call in an existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //check success
            Assert.IsTrue(this.check());
            Assert.IsTrue(this.updatePot(amountToBet));
            //if one player already bet
            Assert.IsTrue(this.bet(amountToBet));
            Assert.IsFalse(this.check());

        }

        [TestMethod]
        public void TestBlindBets()
        {
            //bet in existing game
            Assert.IsFalse(this.isGameOver(game, username));
            Assert.IsTrue(this.spectateActiveGame(game));
            //check if can blind bet
            Assert.IsTrue(this.betSmallBlind(amountToBet));
            Assert.IsTrue(this.betBigBlind(amountToBet * 2));
            Assert.IsTrue(this.updatePot(amountToBet + amountToBet * 2));
            //not enough to bet
            Assert.IsFalse(this.betBigBlind(0));
            Assert.IsFalse(this.betSmallBlind(0));

        }

    }
}
