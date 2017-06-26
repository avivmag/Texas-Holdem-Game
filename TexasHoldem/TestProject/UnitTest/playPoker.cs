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
            };

            center = GameCenter.getGameCenter();

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

    }
}
