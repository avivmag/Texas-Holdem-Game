using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;
using System.Collections.Generic;

namespace TestProject.UnitTest
{
    [TestClass]
    public class PlayPoker
    {
        private Player p1;
        private Player p2;
        private Player p3;
        private GamePreferences gp;
        private TexasHoldemGame game;

        [TestInitialize]
        public void SetUp()
        {
            p1 = new Player(1, 20, 20);
            p2 = new Player(2, 30, 30);
            p3 = new Player(3, 40, 40);
            gp = new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 30, 50, 10, 2, 10, true);
            game = new TexasHoldemGame(1, gp);
            game.joinGame(p2);
            game.joinGame(p3);

            for (int i = 0; i < 3; i++)
            {
                game.players[i].playerState = Player.PlayerState.in_round;
            }

            game.currentBlindBet = 20;

        }

        [TestMethod]
        public void TestDealCards()
        {
            game.dealCards();
            int index = game.nextToSeat(game.currentDealer);

            for (int i = 0; i < 3; i++)
            {
                Console.Out.WriteLine(game.players[index].systemUserID);
                Console.Out.WriteLine(game.players[index].playerCards[0].ToString());
                Console.Out.WriteLine(game.players[index].playerCards[1].ToString());
                index = game.nextToSeat(index);
                Assert.AreEqual(game.players[index].playerCards.Count, 2, "The player " + i + " didnt received 2 cards");
            }
            game.currentDealer = 2;
        }

        [TestMethod]
        public void TestSmallBlindFirst()
        {
            game.currentDealer = 0;
            Assert.IsTrue(game.setSmallBlind() == 1);
        }

        [TestMethod]
        public void TestSmallBlindLast()
        {
            game.currentDealer = 2;
            Assert.IsTrue(game.setSmallBlind() == 0);
        }

        [TestMethod]
        public void TestSmallAndBigBlindBets()
        {
            game.betBlinds();
            Assert.AreEqual(game.tempPot, 30, "The blindes bet are not correct, tempPot = " + game.tempPot);
        }

        [TestMethod]
        public void TestActionsCheck()
        {
            int index = game.nextToSeat(game.currentBig);
            for (int i = game.nextToSeat(game.currentBig); i < game.GamePreferences.MaxPlayers; i++)
            {
                if (game.players[i] != null && game.players[i].playerState == Player.PlayerState.in_round)
                {
                    game.chooseBetAction(game.players[i], TexasHoldemGame.BetAction.check, 0);
                    index = game.nextToSeat(index);
                }
            }
            Assert.AreEqual(game.tempPot, 0, "Not everybody check, tempPot = " + game.tempPot);
        }


        [TestMethod]
        public void TestActionsBet()
        {
            int index = game.nextToSeat(game.currentBig);
            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.bet, 20);
                index = game.nextToSeat(index);
            }

            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.call, 0);
                index = game.nextToSeat(index);
            }

            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.call, 0);
                index = game.nextToSeat(index);
            }

            Assert.AreEqual(game.tempPot, 60, "Not everybody call or bet, tempPot = " + game.tempPot);
        }

        [TestMethod]
        public void TestActionsRaise()
        {
            int index = game.nextToSeat(game.currentBig);
            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.bet, 20);
                index = game.nextToSeat(index);
            }

            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.raise, 10);
                index = game.nextToSeat(index);
            }

            if (game.players[index] != null && game.players[index].playerState == Player.PlayerState.in_round)
            {
                game.chooseBetAction(game.players[index], TexasHoldemGame.BetAction.raise, 20);
                index = game.nextToSeat(index);
            }

            Assert.AreEqual(game.tempPot, 90, "Not everybody raise the exact money, tempPot = " + game.tempPot);
        }

        [TestMethod]
        public void TestRoyalFlush()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.club, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkRoyalFlush(fullHand), true, "Rank does't match, need to be RoyalFlush");

            fullHand = new List<Card>();

            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreNotEqual(game.checkRoyalFlush(fullHand), true, "Rank not to be RoyalFlush");
        }


        [TestMethod]
        public void TestStraightFlush()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 6));
            fullHand.Add(new Card(Card.cardType.club, 4));
            fullHand.Add(new Card(Card.cardType.club, 5));
            fullHand.Add(new Card(Card.cardType.club, 7));
            fullHand.Add(new Card(Card.cardType.club, 8));
            Assert.AreEqual(game.checkStraightFlush(fullHand), 8, "Rank does't match, need to be StraightFlush");

            fullHand = new List<Card>();

            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkStraightFlush(fullHand), -1, "Rank not to be StraightFlush");
        }

        [TestMethod]
        public void TestFourOfAKind()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.heart, 4));
            fullHand.Add(new Card(Card.cardType.club, 4));
            fullHand.Add(new Card(Card.cardType.spade, 4));
            fullHand.Add(new Card(Card.cardType.diamond, 4));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkFourOfAKind(fullHand), 4, "Rank does't match, need to be four of a kind");

            fullHand = new List<Card>();

            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkFourOfAKind(fullHand), -1, "Rank not to be four of a kind");
        }

        [TestMethod]
        public void TestFullHouse()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.heart, 4));
            fullHand.Add(new Card(Card.cardType.club, 4));
            fullHand.Add(new Card(Card.cardType.spade, 4));
            fullHand.Add(new Card(Card.cardType.diamond, 2));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkFullHouse(fullHand, 3), 4, "Rank does't match, need to be full house");

            fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.heart, 4));
            fullHand.Add(new Card(Card.cardType.club, 4));
            fullHand.Add(new Card(Card.cardType.spade, 4));
            fullHand.Add(new Card(Card.cardType.diamond, 2));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkFullHouse(fullHand, 2), 2, "Rank does't match, need to be full house");

            fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkFullHouse(fullHand, 2), -1, "Rank not to be full house");
        }

        [TestMethod]
        public void TestStraight()
        {
            List<Card> fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 7));
            fullHand.Add(new Card(Card.cardType.club, 8));
            fullHand.Add(new Card(Card.cardType.heart, 4));
            fullHand.Add(new Card(Card.cardType.club, 9));
            fullHand.Add(new Card(Card.cardType.spade, 10));
            fullHand.Add(new Card(Card.cardType.diamond, 11));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkStraight(fullHand), 11, "Rank does't match, need to be straight");

            fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 13));
            Assert.AreEqual(game.checkStraight(fullHand), 1 , "Rank not to be straight");


            fullHand = new List<Card>();
            fullHand.Add(new Card(Card.cardType.club, 1));
            fullHand.Add(new Card(Card.cardType.club, 2));
            fullHand.Add(new Card(Card.cardType.club, 3));
            fullHand.Add(new Card(Card.cardType.diamond, 10));
            fullHand.Add(new Card(Card.cardType.club, 11));
            fullHand.Add(new Card(Card.cardType.club, 12));
            fullHand.Add(new Card(Card.cardType.club, 7));
            Assert.AreEqual(game.checkStraight(fullHand), -1, "Rank not to be straight");
        }


        [TestCleanup]
        public void TearDown()
        {
            game.leaveGamePlayer(p1);
            game.leaveGamePlayer(p2);
            game.leaveGamePlayer(p3);
        }
    }
}
