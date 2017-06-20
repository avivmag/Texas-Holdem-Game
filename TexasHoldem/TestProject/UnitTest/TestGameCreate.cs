using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Backend.Game;
//using DAL;
using Moq;
using Backend.User;
using Backend;
using System.Collections.Generic;
using ApplicationFacade;
using Backend.Game.DecoratorPreferences;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;
using Database;

namespace TestProject.UnitTest
{
    [TestClass]
    public class TestGameCreate
    {
        private SLInterface sl;
        private IDB db;
        private GameCenter center;

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
            };

            for (int i = 0; i < gamesList.Count; i++)
            {
                gamesList[i].gameId = i;
                center.TexasHoldemGames.Add(gamesList[i]);
            }

            sl = new SLImpl();
        }

        [TestMethod]
        public void TestCreateGame()
        {
            object objUser = db.getUserByName("test0");
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            SystemUser user = (SystemUser)objUser;

            object game = sl.createGame(user.id, "Limit", 1000, 100, 15000, 120, 4, 8, true, false);
            Assert.IsInstanceOfType(game,typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestCreateGameWithNonExistantUser()
        {
            object game = sl.createGame(40, "Limit", 100, 100, 15000, 120, 4, 8, true, false);

            Assert.IsNotInstanceOfType(game, typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestCreateGameWithLimit()
        {
            object objUser = db.getUserByName("test0");
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            SystemUser user = (SystemUser)objUser;

            object objGame = sl.createGame(user.id, "Limit", 1000, 100, 15000, 120, 4, 8, true, false);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            TexasHoldemGame game = (TexasHoldemGame)objGame;

            Assert.IsTrue(game.gamePreferences.isContain(new MustPreferences(new GamePolicyDecPref(GameTypePolicy.Limit,1000,null),true)));
        }

        [TestMethod]
        public void TestCreateGameWithNoLimit()
        {
            object objUser = db.getUserByName("test0");
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            SystemUser user = (SystemUser)objUser;

            object objGame = sl.createGame(user.id, "No_Limit", 1000, 100, 15000, 120, 4, 8, true, false);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            TexasHoldemGame game = (TexasHoldemGame)objGame;

            Assert.IsTrue(game.gamePreferences.isContain(new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit, 1000, null), true)));
        }

        [TestMethod]
        public void TestCreateGameWithNoLimitNone()
        {
            object objUser = db.getUserByName("test0");
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            SystemUser user = (SystemUser)objUser;

            object objGame = sl.createGame(user.id, "none", 1000, 100, 15000, 120, 4, 8, true, false);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            TexasHoldemGame game = (TexasHoldemGame)objGame;

            Assert.IsTrue(game.gamePreferences.isContain(new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit, 1000, null), true)));
        }

        [TestMethod]
        public void TestCreateGameWithPotLimit()
        {
            object objUser = db.getUserByName("test0");
            Assert.IsInstanceOfType(objUser, typeof(SystemUser));
            SystemUser user = (SystemUser)objUser;

            object objGame = sl.createGame(user.id, "Pot_Limit", 1000, 100, 15000, 120, 4, 8, true, false);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            TexasHoldemGame game = (TexasHoldemGame)objGame;

            Assert.IsTrue(game.gamePreferences.isContain(new MustPreferences(new GamePolicyDecPref(GameTypePolicy.Pot_Limit, 1000, null), true)));
        }
    }
}
