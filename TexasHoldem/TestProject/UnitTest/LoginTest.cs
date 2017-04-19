using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;

namespace TestProject.UnitTest
{
	[TestClass]
	public class LoginTest
	{
		BLInterface bl = new BLImpl();

		[TestMethod]
		public void successTest()
		{
			bl.Register("Ron wisely", "abra kadbras", "ginger@for.life", "nickleback photo");
			Assert.IsTrue(bl.Login("Ron wisely", "abra kadbras").success);
		}

		[TestMethod]
		public void alreadyLoggedInTest()
		{
			bl.Register("crash bandicoot", "green boxes are the worst", "crash@bash.bugabugabuga", "hedgehog photo");
			bl.Login("crash bandicoot", "green boxes are the worst");
			Assert.IsFalse(bl.Login("crash bandicoot", "green boxes are the worst").success);
		}

		[TestMethod]
		public void notRegisteredTest()
		{
			bl.Login("a worm from worms world party", "for a donkey, for a donkey, my kindom for a donkey");
			Assert.IsFalse(bl.Login("a worm from worms world party", "for a donkey, for a donkey, my kindom for a donkey").success);
		}

		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(bl.Login("", "Transparancy is overwhelming").success);
		}

		[TestMethod]
		public void emptyPasswordTest()
		{
			Assert.IsFalse(bl.Login("sandro the living dead", "").success);
		}
	}

}
