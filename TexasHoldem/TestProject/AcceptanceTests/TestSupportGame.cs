using System;
using Backend.Game;
using Backend.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.AcceptanceTests
{
    [TestClass]
    public class TestSupportGame : ProjectTest
    {
        SystemUser hadas;
        SystemUser other;
        TexasHoldemGame game;
        int amountToBet = 1000;

        [TestInitialize]
        public void SetUp()
        {
            object objUser = register("hadas", "1", "shidlhad", "1");
            Assert.IsNotNull(objUser);
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            hadas = (SystemUser)objUser;
            hadas.money = 100000;
            editProfile(hadas.id, hadas.name, hadas.password, hadas.email, hadas.userImage, 100000);

            object objOtherUser = register("other", "2", "other", "2");
            Assert.IsNotNull(objOtherUser);
            Assert.IsInstanceOfType(objOtherUser, typeof(SystemUser));
            other = (SystemUser)objOtherUser;
            other.money = 100000;
            editProfile(other.id, other.name, other.password, other.email, other.userImage, 100000);

            object objGame = creatGame(hadas.id,"Limit",amountToBet*2+1,amountToBet+1,100,100,2,9,true,false);
            Assert.IsNotNull(objGame);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            game = (TexasHoldemGame)objGame;
            
            Assert.IsTrue(game.joinGame(other,6).success);
            game.setInitialState();

        }

        //[TestMethod]
        //public void TestBet()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }
        //    Assert.IsNotNull(p);

        //    //enough chips to bet
        //    Assert.IsTrue(canBet(game,hadas,amountToBet));
        //    Assert.IsTrue(game.bet(p, amountToBet).success);
        //}

        //[TestMethod]
        //public void TestCall()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }
                    
        //    Assert.IsNotNull(p);

        //    //enough chips to bet
        //    Assert.IsTrue(canBet(game, hadas, amountToBet));
        //    Assert.IsTrue(game.bet(p, amountToBet).success);

        //    foreach (Player player in game.players)
        //        if (player != null && player.systemUserID != p.systemUserID)
        //        {
        //            p = player;
        //            break;
        //        }

        //    //make the call
        //    Assert.IsTrue(p.Tokens>=amountToBet);
        //    game.call(p);

        //    //Assert that the game and player changed according to the call.
        //    Assert.IsTrue(p.Tokens >= 0);
        //    Assert.IsTrue(game.tempPot == 2000);
        //}


        //[TestMethod]
        //public void TestRaise()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }

        //    Assert.IsNotNull(p);

        //    //enough chips to bet
        //    Assert.IsTrue(canBet(game, hadas, amountToBet));
        //    Assert.IsTrue(game.bet(p, amountToBet).success);

        //    foreach (Player player in game.players)
        //        if (player != null && player.systemUserID != p.systemUserID)
        //        {
        //            p = player;
        //            break;
        //        }

        //    //make the rais
        //    Assert.IsTrue(canRaise(game, other, amountToBet+1));


        //    //Assert that the game and player changed according to the call.
        //    //Assert.IsTrue(p.Tokens == 0);
        //    //Assert.IsTrue(game.tempPot == 2000);

        //}



        //[TestMethod]
        //public void TestFold()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }

        //    Assert.IsNotNull(p);
        //    Assert.IsNull(game.fold(p));
        //    //Assert.IsTrue(game.fold(p).success);
        //}

        //[TestMethod]
        //public void TestFoldAfterBet()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }

        //    Assert.IsNotNull(p);

        //    //enough chips to bet
        //    Assert.IsTrue(canBet(game, hadas, amountToBet));
        //    Assert.IsTrue(game.bet(p, amountToBet).success);

        //    foreach (Player player in game.players)
        //        if (player != null && player.systemUserID != p.systemUserID)
        //        {
        //            p = player;
        //            break;
        //        }
        //    Assert.IsNull(game.fold(p));
        //    //Assert.IsTrue(game.fold(p).success);
        //}

        //[TestMethod]
        //public void TestCheck()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }

        //    Assert.IsNotNull(p);
        //    Assert.IsNull(game.check(p));
        //    //Assert.IsTrue(game.check(p).success);
        //}

        //[TestMethod]
        //public void TestCantCheck()
        //{
        //    Player p = null;
        //    foreach (Player player in game.players)
        //        if (player != null)
        //        {
        //            p = player;
        //            break;
        //        }

        //    Assert.IsNotNull(p);

        //    //enough chips to bet
        //    Assert.IsTrue(canBet(game, hadas, amountToBet));
        //    Assert.IsTrue(game.bet(p, amountToBet).success);

        //    foreach (Player player in game.players)
        //        if (player != null && player.systemUserID != p.systemUserID)
        //        {
        //            p = player;
        //            break;
        //        }
        //    Assert.IsNull(game.check(p));
        //    //Assert.IsFalse(game.fold(p).success);
        //}

        //[TestMethod]
        //public void TestBlindBets()
        //{
        //    game.currentSmall = 0;
        //    game.currentBig = 1;
        //    game.currentBlindBet = 100;
        //    game.betBlinds();
        //    Assert.AreEqual(game.players[game.currentSmall].Tokens ,951);
        //    Assert.AreEqual(game.players[game.currentBig].Tokens, 901);
        //}

        [TestCleanup]
        public void TearDown()
        {
            removeGame(game.gameId);
            logout(hadas.id);
            logout(other.id);
            removeUser(hadas.id);
            removeUser(other.id);
        }
    }
}
