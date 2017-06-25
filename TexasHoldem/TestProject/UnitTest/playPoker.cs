using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;
using System.Collections.Generic;
using Backend.User;
using SL;
using ApplicationFacade;
using PeL;

namespace TestProject.UnitTest
{
    
    [TestClass]
    public class PlayPoker
    {
        private SLInterface sl;
        private IPeL db;
        private GameCenter center;
        private TexasHoldemGame game;

        [TestCleanup]
        public void Cleanup()
        {
            for (int i = 0; i < 4; i++)
                db.deleteUser(db.getUserByName("test" + i).id);
            center.shutDown();
        }

        [TestInitialize]
        public void SetUp()
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


            var userList = new List<SystemUser>
            {
                db.getUserByName("test0"),
                db.getUserByName("test1"),
                db.getUserByName("test2"),
                db.getUserByName("test3")
                //new SystemUser("Hadas", "email0", "image0", 1000),
                //new SystemUser("Gili", "email1", "image1", 0),
                //new SystemUser("Or", "email2", "image2", 700),
                //new SystemUser("Aviv", "email3", "image3", 1500)
            };

            center = GameCenter.getGameCenter();

            ////set users ranks.
            //userList[0].rank = 10;
            //userList[1].rank = 15;
            //userList[2].rank = 20;
            //userList[3].rank = 25;

            //for (int i = 0; i < 4; i++)
            //{
            //    userList[i].id = i;
            //    center.loggedInUsers.Add(userList[i]);
            //    //center.login(userList[i].name, userList[i].password);
            //}

            sl = new SLImpl();
            center = GameCenter.getGameCenter();

            game = (TexasHoldemGame) sl.createGame(0, "No_limit", 1000, 30, 50, 10, 2, 10, true, false);
            game.gameId = 1;

            game.players[1] = new Player(userList[1].id, userList[1].name, 50, userList[1].rank, new byte[0]);
            game.players[2] = new Player(userList[2].id, userList[2].name, 50, userList[2].rank, new byte[0]);
            
            for (int i = 0; i < 3; i++)
            {
                game.players[i].playerState = Player.PlayerState.in_round;
            }

            game.currentBlindBet = 20;

        }

        //[TestMethod]
        //public void TestDealCards()
        //{
        //    game.dealCards();
        //    int index = game.nextToSeat(game.currentDealer);

        //    for (int i = 0; i < 3; i++)
        //    {
        //        Console.Out.WriteLine(game.players[index].systemUserID);
        //        Console.Out.WriteLine(game.players[index].playerCards[0].ToString());
        //        Console.Out.WriteLine(game.players[index].playerCards[1].ToString());
        //        index = game.nextToSeat(index);
        //        Assert.AreEqual(game.players[index].playerCards.Count, 2, "The player " + i + " didnt received 2 cards");
        //    }
        //    game.currentDealer = 2;
        //}

        //[TestMethod]
        //public void TestSmallBlindFirst()
        //{
        //    game.currentDealer = 0;
        //    Assert.IsTrue(game.getNextPlayer(game.currentDealer) == 1);
        //}

        //[TestMethod]
        //public void TestSmallBlindLast()
        //{
        //    game.currentDealer = 2;
        //    Assert.IsTrue(game.getNextPlayer(game.currentDealer) == 0);
        //}

        //[TestMethod]
        //public void TestSmallAndBigBlindBets()
        //{
        //    game.betBlinds();
        //    Assert.AreEqual(game.tempPot, 30, "The blindes bet are not correct, tempPot = " + game.tempPot);
        //}

        //[TestMethod]
        //public void TestRoyalFlush()
        //{
        //    List<Card> fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.club, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkRoyalFlush(fullHand), true, "Rank does't match, need to be RoyalFlush");

        //    fullHand = new List<Card>();

        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreNotEqual(game.checkRoyalFlush(fullHand), true, "Rank not to be RoyalFlush");
        //}


        //[TestMethod]
        //public void TestStraightFlush()
        //{
        //    List<Card> fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 6));
        //    fullHand.Add(new Card(Card.cardType.club, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 5));
        //    fullHand.Add(new Card(Card.cardType.club, 7));
        //    fullHand.Add(new Card(Card.cardType.club, 8));
        //    Assert.AreEqual(game.checkStraightFlush(fullHand), 8, "Rank does't match, need to be StraightFlush");

        //    fullHand = new List<Card>();

        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkStraightFlush(fullHand), -1, "Rank not to be StraightFlush");
        //}

        //[TestMethod]
        //public void TestFourOfAKind()
        //{
        //    List<Card> fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.heart, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 4));
        //    fullHand.Add(new Card(Card.cardType.spade, 4));
        //    fullHand.Add(new Card(Card.cardType.diamond, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkFourOfAKind(fullHand), 4, "Rank does't match, need to be four of a kind");

        //    fullHand = new List<Card>();

        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkFourOfAKind(fullHand), -1, "Rank not to be four of a kind");
        //}

        //[TestMethod]
        //public void TestFullHouse()
        //{
        //    List<Card> fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.heart, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 4));
        //    fullHand.Add(new Card(Card.cardType.spade, 4));
        //    fullHand.Add(new Card(Card.cardType.diamond, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkFullHouse(fullHand, 3), 4, "Rank does't match, need to be full house");

        //    fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.heart, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 4));
        //    fullHand.Add(new Card(Card.cardType.spade, 4));
        //    fullHand.Add(new Card(Card.cardType.diamond, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkFullHouse(fullHand, 2), 2, "Rank does't match, need to be full house");

        //    fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkFullHouse(fullHand, 2), -1, "Rank not to be full house");
        //}

        //[TestMethod]
        //public void TestStraight()
        //{
        //    List<Card> fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 7));
        //    fullHand.Add(new Card(Card.cardType.club, 8));
        //    fullHand.Add(new Card(Card.cardType.heart, 4));
        //    fullHand.Add(new Card(Card.cardType.club, 9));
        //    fullHand.Add(new Card(Card.cardType.spade, 10));
        //    fullHand.Add(new Card(Card.cardType.diamond, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkStraight(fullHand), 11, "Rank does't match, need to be straight");

        //    fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 13));
        //    Assert.AreEqual(game.checkStraight(fullHand), 1, "Rank not to be straight");


        //    fullHand = new List<Card>();
        //    fullHand.Add(new Card(Card.cardType.club, 1));
        //    fullHand.Add(new Card(Card.cardType.club, 2));
        //    fullHand.Add(new Card(Card.cardType.club, 3));
        //    fullHand.Add(new Card(Card.cardType.diamond, 10));
        //    fullHand.Add(new Card(Card.cardType.club, 11));
        //    fullHand.Add(new Card(Card.cardType.club, 12));
        //    fullHand.Add(new Card(Card.cardType.club, 7));
        //    Assert.AreEqual(game.checkStraight(fullHand), -1, "Rank not to be straight");
        //}
    }
}
