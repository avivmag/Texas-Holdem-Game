using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.User;
using Backend.Game;
using Database;

namespace TestProject.AcceptanceTests
{
    [TestClass]
    public class TestsLoginActions :ProjectTest
    {
        IDB db;
        TexasHoldemGame gameSpectate;
        TexasHoldemGame gameCantSpectate;

        [TestInitialize]
        public void SetUp()
        {
            db = new DBImpl();

            bool objUser = db.RegisterUser("Hadas", "1234", "shidlhad", "1");
            Assert.IsTrue(objUser);

            bool objOther = db.RegisterUser("Gil", "1234", "gilabadi89", "1");
            Assert.IsTrue(objOther);

            object objGame = creatGame(db.getUserByName("Hadas").id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(objGame);
            Assert.IsInstanceOfType(objGame, typeof(TexasHoldemGame));
            gameSpectate = (TexasHoldemGame)objGame;

            object objGame2 = creatGame(db.getUserByName("Hadas").id, "Limit", 1000, 1000, 1000, 100, 2, 9, false, false);
            Assert.IsNotNull(objGame2);
            Assert.IsInstanceOfType(objGame2, typeof(TexasHoldemGame));
            gameCantSpectate = (TexasHoldemGame)objGame2;
        }
        [TestMethod]
        public void TestEditProfileUserName()
        {
            editProfile(db.getUserByName("Hadas").id, "shid", "1234", "shidlhad", "1", 100);
            Assert.AreEqual(db.getUserByName("shid").name,"shid");
            editProfile(db.getUserByName("shid").id, "Hadas", "1234", "shidlhad", "1", 100);
        }

        [TestMethod]
        public void TestEditProfilePassword()
        {
            editProfile(db.getUserByName("Hadas").id, "Hadas", "12345", "shidlhad", "1", 100);
            logout(db.getUserByName("Hadas").id);
            object objUser = login("Hadas","12345");
            Assert.IsInstanceOfType(objUser,typeof(SystemUser));
            logout(db.getUserByName("Hadas").id);
        }

        [TestMethod]
        public void TestEditProfileEmail()
        {
            editProfile(db.getUserByName("Hadas").id, "hadas", "1234", "shidl", "1", 100);
            Assert.AreEqual(db.getUserByName("Hadas").email,"shidl");
        }

        [TestMethod]
        public void TestEditProfilePicture()
        {
            editProfile(db.getUserByName("Hadas").id, "Hadas", "1234", "shidl", "123", 100);
            Assert.IsNotNull(db.getUserByName("Hadas").userImage,"123");
        }

        [TestMethod]
        public void TestEditProfileUserNameExists()
        {
            editProfile(db.getUserByName("Hadas").id, "Gil", "1234", "shidlhad", "1", 100);
            Assert.AreEqual(db.getUserByName("Hadas").name,"Hadas");
        }

        [TestMethod]
        public void TestCreatGame()
        {
            object obj = creatGame(db.getUserByName("Hadas").id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false);
            Assert.IsNotNull(creatGame(db.getUserByName("Hadas").id, "Limit", 1000, 1000, 1000, 100, 2, 9, true, false));
            Assert.IsInstanceOfType(obj, typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestjoinExistingGame()
        {
            editProfile(db.getUserByName("Hadas").id,"Hadas","123123", "shidlhas","1", 100000);
            Assert.IsNotNull(addPlayerToGame(db.getUserByName("Hadas").id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameTwice()
        {
            editProfile(db.getUserByName("Hadas").id, "Hadas", "123123", "shidlhas", "1", 100000);
            Assert.IsNotNull(addPlayerToGame(db.getUserByName("Hadas").id, gameSpectate.gameId, 1));
            Assert.IsNull(addPlayerToGame(db.getUserByName("Hadas").id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoMoney()
        {
            Assert.IsNull(addPlayerToGame(db.getUserByName("Hadas").id, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoUser()
        {
            Assert.IsNull(addPlayerToGame(0, gameSpectate.gameId, 1));
        }

        [TestMethod]
        public void TestjoinExistingGameNoGame()
        {
            Assert.IsNull(addPlayerToGame(db.getUserByName("Hadas").id, 1000, 1));
        }

        [TestMethod]
        public void TestSpectateActiveGame()
        {
            object ans = spectateActiveGame(db.getUserByName("Hadas").id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
        }

        [TestMethod]
        public void TestSpectateActiveGameCantSpectate()
        {
            Assert.IsNull(spectateActiveGame(db.getUserByName("Hadas").id, gameCantSpectate.gameId));
        }

        [TestMethod]
        public void TestSpectateActiveGameCantSpectateTwice()
        {
            object ans = spectateActiveGame(db.getUserByName("Hadas").id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
            Assert.IsNull(spectateActiveGame(db.getUserByName("Hadas").id, gameSpectate.gameId));
        }

        [TestMethod]
        public void TestSpectateActiveGameTwoSpectators()
        {
            object ans = spectateActiveGame(db.getUserByName("Hadas").id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
            ans = spectateActiveGame(db.getUserByName("Gil").id, gameSpectate.gameId);
            Assert.IsNotNull(ans);
            Assert.IsInstanceOfType(ans, typeof(TexasHoldemGame));
        }

        [TestCleanup]
        public void TearDown()
        {
            removeGame(gameSpectate.gameId);
            removeGame(gameCantSpectate.gameId);
            db.deleteUser(db.getUserByName("Hadas").id);
            db.deleteUser(db.getUserByName("Gil").id);
        }
    }
}
