using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Backend.User;
using System.Collections.Generic;
using Moq;
using DAL;
using Backend.Game;
using ApplicationFacade;
using Backend.Game.DecoratorPreferences;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;
using Backend;
using System;

namespace TestProject
{
    [TestClass]
    public class JoinActiveGameTest
    {
        private SLInterface sl;
        private GameCenter center = GameCenter.getGameCenter();
        [TestInitialize]
        public void SetUp()
        {

            var userList = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };

            //set users ranks.
            int j = 10;
            for (int i = 0; i < 4; i++)
            {
                userList[i].rank = j;
                userList[i].newPlayer = false;
                j += 5;
                userList[i].id = i;
                center.loggedInUsers.Add(userList[i]);
                //center.login(userList[i].name, userList[i].password);
            }

            //set the leagues
            center.maintainLeagues(userList);

            //get the league of user 3
            League l = center.getUserLeague(userList[3]);

            //setting the games
            //pref order: mustpref(spectate,league)->game type , buy in policy, starting chips, minimal bet, minimum players, maximum players.
            var gamesList = new List<TexasHoldemGame>
            {
                //regular games
                new TexasHoldemGame(userList[0],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (9,null) ))))),true)),
                new TexasHoldemGame(userList[0],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (9,null) ))))),false)),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),true)),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                //league games
                new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank)),
                new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank))
                //new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                //new TexasHoldemGame(userList[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                //new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                //new TexasHoldemGame(userList[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                //new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
                //new TexasHoldemGame(userList[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
            };

            for (int i = 0; i < gamesList.Count; i++)
            {
                gamesList[i].gameId = i;
                center.texasHoldemGames.Add(gamesList[i]);
            }

            Mock<DALInterface> dalMock = new Mock<DALInterface>();
            dalMock.Setup(x => x.getAllUsers()).Returns(userList);
            dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList.Find(g => (g.gameId == i)));
            dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
            sl = new SLImpl();
        }

        [TestMethod]
        public void joinSuccessTest()
        {
            object m = sl.joinActiveGame(0, 4);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            Assert.AreEqual(((TexasHoldemGame)m).gameId,4);
        }

        [TestMethod]
        public void joinSuccessLeagueGameTest()
        {
            object m = sl.joinActiveGame(2, 7);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            Assert.AreEqual(((TexasHoldemGame)m).gameId,7);
        }

        [TestMethod]
        public void joinFailsLeagueGameTest()
        {
            object m = sl.joinActiveGame(0, 7);

            Assert.IsInstanceOfType(m, typeof(ReturnMessage));

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailesNoSeatsTest()
        {
            sl.joinActiveGame(2, 3);

            sl.joinActiveGame(3, 3);

            object m = sl.joinActiveGame(0, 3);

            Assert.AreEqual(m,null);
        }

        [TestMethod]
        public void joinFailesNoMoneyTest()
        {
            object m = sl.joinActiveGame(1, 1);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsAlreadyPlayTest()
        {
            sl.joinActiveGame(0, 3);

            object m = sl.joinActiveGame(0, 3);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsAlreadySpectatingTest()
        {
            sl.spectateActiveGame(0, 2);

            object m = sl.joinActiveGame(0, 2);

            Assert.AreEqual(m,null);
        }

        [TestMethod]
        public void joinFailsGameNoExistsTest()
        {
            object m = sl.joinActiveGame(0, 1000);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsUserDontExistsTest()
        {
            object m = sl.joinActiveGame(70, 1000);

            Assert.AreEqual(m, null);
        }

        [TestCleanup]
        public void TearDown()
        {
            center.shutDown();
        }
    }
}
