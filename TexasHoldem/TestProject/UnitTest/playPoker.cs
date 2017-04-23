using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;

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
            game.joinGame(p1);
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
                Console.Out.WriteLine(game.players[index].id);
                Console.Out.WriteLine(game.players[index].playerCards[0].ToString());
                Console.Out.WriteLine(game.players[index].playerCards[1].ToString());
                index = game.nextToSeat(index);
                Assert.AreEqual(game.players[index].playerCards.Count, 2, "The player didnt received 2 cards");
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
        public void TestFullHandRank()
        {
            game.dealCards();

            for (int i = 0; i < 3; i++)
            {
                game.flop.Add(game.deck.Top());
            }

            game.turn = game.deck.Top();
            game.river = game.deck.Top();
            
            Assert.AreEqual(game.checkHandRank(game.players[0]), TexasHoldemGame.HandsRanks.FourOfAKind, "Rank does't match");
        }


        [TestCleanup]
        public void TearDown()
        {
            game.leaveGame(p1);
            game.leaveGame(p2);
            game.leaveGame(p3);
        }
    }
}
