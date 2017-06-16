using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Backend.User;
using System.Collections.Generic;
using Moq;
//using DAL;
using Backend.Game;
using ApplicationFacade;
using Backend.Game.DecoratorPreferences;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;
using Backend;
using System;
using Database;

namespace TestProject
{
    [TestClass]
    public class JoinActiveGameTest
    {
        private SLInterface sl;
        private IDB db;
        private GameCenter center = GameCenter.getGameCenter();
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
            db = new DBImpl();
            for (int i = 0; i < 4; i++)
            {
                db.RegisterUser("test" + i, "" + i, "email" + i, "userImage" + i);
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
            };

            center = GameCenter.getGameCenter();
            
            for (int i = 0; i<10; i++)
            {
                db.EditUserById(db.getUserByName("test0").id, null, null, null, null, null, null, true);
                db.EditUserById(db.getUserByName("test1").id, null, null, null, null, null, null, true);
                db.EditUserById(db.getUserByName("test2").id, null, null, null, null, null, null, true);
                db.EditUserById(db.getUserByName("test3").id, null, null, null, null, null, null, true);
            }
            

            //set users ranks.
            int j = 10;
            for (int i = 0; i < 4; i++)
            {
                db.EditUserById(db.getUserByName("test0").id, null, null, null, null, null, j, false);
                j += 5;
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
                                                                    new MaxPlayersDecPref (9,null) ))))),true),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[0],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (9,null) ))))),false),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),true),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                //league games
                new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2])),
                new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank),
                                                                    userIdDeltaRank => db.EditUserById(userIdDeltaRank[0], null, null, null, null, null, userIdDeltaRank[1], false),
                                                                    userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2]))
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
                center.TexasHoldemGames.Add(gamesList[i]);
            }

            //Mock<DALInterface> dalMock = new Mock<DALInterface>();
            //dalMock.Setup(x => x.getAllUsers()).Returns(userList);
            //dalMock.Setup(x => x.getUserById(It.IsAny<int>())).Returns((int i) => userList[i]);
            //dalMock.Setup(x => x.getGameById(It.IsAny<int>())).Returns((int i) => gamesList.Find(g => (g.gameId == i)));
            //dalMock.Setup(x => x.getAllGames()).Returns(gamesList);
            sl = new SLImpl();
        }

        [TestMethod]
        public void joinSuccessTest()
        {
            object m = sl.GetGameForPlayers(db.getUserByName("test0").id, 4);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            Assert.AreEqual(((TexasHoldemGame)m).gameId,4);
        }

        [TestMethod]
        public void joinSuccessLeagueGameTest()
        {
            object m = sl.GetGameForPlayers(db.getUserByName("test2").id, 7);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            Assert.AreEqual(((TexasHoldemGame)m).gameId,7);
        }

        [TestMethod]
        public void joinFailsLeagueGameTest()
        {
            object m = sl.GetGameForPlayers(db.getUserByName("test0").id, 7);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            m = sl.joinGame(db.getUserByName("test0").id, 7, 0);
            //Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailesNoSeatsTest()
        {
            sl.GetGameForPlayers(db.getUserByName("test2").id, 3);
            sl.joinGame(db.getUserByName("test2").id, 3, 0);
            sl.GetGameForPlayers(db.getUserByName("test3").id, 3);
            sl.joinGame(db.getUserByName("test3").id, 3, 1);
            sl.GetGameForPlayers(db.getUserByName("test0").id, 3);
            object m = sl.joinGame(db.getUserByName("test0").id, 3, 0);

            Assert.AreEqual(m,null);
        }

        [TestMethod]
        public void joinFailesNoMoneyTest()
        {
            object m = sl.GetGameForPlayers(db.getUserByName("test1").id, 1);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsAlreadyPlayTest()
        {
            object userObj = db.getUserByName("test0");
            Assert.IsInstanceOfType(userObj, typeof(SystemUser));
            SystemUser user = (SystemUser)userObj;

            object m = sl.GetGameForPlayers(user.id, 3);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            m = sl.joinGame(user.id, 3, 0);

            Assert.IsInstanceOfType(m, typeof(TexasHoldemGame));

            m = sl.GetGameForPlayers(user.id, 3);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsAlreadySpectatingTest()
        {
            sl.spectateActiveGame(db.getUserByName("test0").id, 2);

            object m = sl.GetGameForPlayers(db.getUserByName("test0").id, 2);

            Assert.AreEqual(m,null);
        }

        [TestMethod]
        public void joinFailsGameNoExistsTest()
        {
            object m = sl.GetGameForPlayers(db.getUserByName("test0").id, 1000);

            Assert.AreEqual(m, null);
        }

        [TestMethod]
        public void joinFailsUserDontExistsTest()
        {
            object m = sl.GetGameForPlayers(700000, 1000);

            Assert.AreEqual(m, null);
        }
    }
}
