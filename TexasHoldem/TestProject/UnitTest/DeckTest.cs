using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;
using System.Diagnostics;
using System.Threading;

namespace TestProject.UnitTest
{
    [TestClass]
    public class DeckTest
    {

        [TestMethod]
        public void TestShuffle()
        {

            Deck deck1 = new Deck();

            Deck deck2 = new Deck();
            deck1.Shuffle();
            deck2.Shuffle();

            Assert.IsFalse(deck1.Top().Equals(deck2.Top()), "The top cards of the decks is the same");
        }
    }
}