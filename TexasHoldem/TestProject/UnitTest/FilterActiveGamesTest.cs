using Microsoft.VisualStudio.TestTools.UnitTesting;
using SL;
using Backend.Game;
using System.Collections.Generic;
using Backend.User;
using Backend.Game.DecoratorPreferences;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;
using ApplicationFacade;
using PeL;

namespace TestProject
{
    [TestClass]
    public class FilterActiveGamesTest
    {
        SLInterface sl;
        private IPeL db;
        GameCenter center;
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

            for (int i=0; i<gamesList.Count; i++)
            {
                gamesList[i].gameId = i;
                center.TexasHoldemGames.Add(gamesList[i]);
            }

            sl = new SLImpl();
        }

        [TestMethod]
        public void filterActiveGamesByPlayerNameTwoGamesFailsTest()
        {
            Assert.AreEqual(center.filterActiveGamesByPlayerName("Shaked").Count, 0);
        }

        [TestMethod]
        public void filterActiveGamesByPotSizeTest()
        {
            var user2 = db.getUserById(db.getUserByName("test0").id);
            sl.GetGameForPlayers(user2.id, 3);

            sl.GetGameForPlayers(user2.id, 0);

            Assert.AreEqual(center.filterActiveGamesByPotSize(0).Count, 8);
        }

        [TestMethod]
        public void filterActiveGamesByPotSizeFailsTest()
        {
            var user2 = db.getUserById(db.getUserByName("test0").id);
            sl.GetGameForPlayers(user2.id, 3);

            sl.GetGameForPlayers(user2.id, 0);

            Assert.AreEqual(center.filterActiveGamesByPotSize(100).Count, 8);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesTest()
        {
            MustPreferences mustPref = new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit, 0,
                                                           new BuyInPolicyDecPref(100, new StartingAmountChipsCedPref(500,
                                                           new MinBetDecPref(20, new MinPlayersDecPref(2,
                                                           new MaxPlayersDecPref(9, null)))))), true);
            
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(mustPref).Count, 1);
        }

        [TestMethod]
        public void filterActiveGamesByFewPreferencesTest()
        {
            MustPreferences mustPref = new MustPreferences(new BuyInPolicyDecPref(100,null), true);

            Assert.AreEqual(center.filterActiveGamesByGamePreferences(mustPref).Count, 2);
        }

        [TestMethod]
        public void filterActiveGamesByBuyInPolicyPreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null, null, 100, null,null,null,null,true,false).Count, 2);
        }

        [TestMethod]
        public void filterActiveGamesByMaxPlayersPreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null, null, null, null, null, null, 9, true, false).Count, 1);
        }

        [TestMethod]
        public void filterActiveGamesByMinPlayersAndPolicyPreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null,null, 100, null, null, 2, null, true, false).Count, 2);
        }

        [TestMethod]
        public void filterActiveGamesByOnlyMustPreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null, null, null, null, null, null, null, true, false).Count, 2);
        }

        [TestMethod]
        public void filterActiveGamesByOnlyMustNoSpectatePreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null, null, null, null, null, null, null, false, false).Count, 4);
        }

        [TestMethod]
        public void filterActiveGamesBySomePreferencesTest()
        {
            Assert.AreEqual(center.filterActiveGamesByGamePreferences(null, null, 100, null, null, 5, 9, false, false).Count, 0);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesThreeGamesTest()
        {
            MustPreferences mustPref = new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit, 0,
                                                           new BuyInPolicyDecPref(100, new StartingAmountChipsCedPref(500,
                                                           new MinBetDecPref(20, new MinPlayersDecPref(2,
                                                           new MaxPlayersDecPref(2, null)))))), false);


            Assert.AreEqual(center.filterActiveGamesByGamePreferences(mustPref).Count, 3);
        }

        [TestMethod]
        public void filterActiveGamesByPreferencesFailsTest()
        {
            MustPreferences mustPref = new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit, 0,
                                                           new BuyInPolicyDecPref(1000000, new StartingAmountChipsCedPref(500,
                                                           new MinBetDecPref(20, new MinPlayersDecPref(2,
                                                           new MaxPlayersDecPref(2, null)))))), false);

            Assert.AreEqual(center.filterActiveGamesByGamePreferences(mustPref).Count, 0);
        }
    }
}
