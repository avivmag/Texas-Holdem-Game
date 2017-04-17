using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using Backend.User;
using Backend.Game;
using System.Collections.Generic;

namespace TestProject
{
    //filter active games
    //find active games
    [TestClass]
    public class LeaveGameGameTest
    {

        BLInterface bl = new BLImpl();

        [TestMethod]
        public void LeaveSpectatorSuccessTest()
        {
            TexasHoldemGame game = bl.getGameById(0);
            Spectator spec = new Spectator(0);
            game.joinSpectate(spec);
            game.leaveGame(spec);
            CollectionAssert.AreEqual(game.spectators,new List<Spectator> { });
        }

        [TestMethod]
        public void LeavePlayerSuccessTest()
        {
            TexasHoldemGame game = bl.getGameById(0);
            Player p = new Player(0,100,2);
            game.joinGame(p);
            game.leaveGame(p);
            CollectionAssert.AreEqual(game.players, new List<Player> { });
        }
    }
}
