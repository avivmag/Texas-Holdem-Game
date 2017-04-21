using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;

namespace TestProject.UnitTest
{
	[TestClass]
	public class LogOutTest
	{
		BLInterface bl = new BLImpl();

		[TestMethod]
		public void successTest()
		{
			bl.Register("piter pen", "staying young forever", "neverland@never.getting.bigger", "some kid with cape in front of a fan");
			bl.Login("piter pen", "staying young forever");
			Assert.IsTrue(bl.Logout("piter pen").success);
		}

		[TestMethod]
		public void notLoggedInTest()
		{
			Assert.IsFalse(bl.Logout("an outsider").success);
		}

		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(bl.Logout("").success);
		}

		[TestMethod]
		public void logOutTwiceTest()
		{
			bl.Register("rick roll", "never gonna give you up", "never@gonna.let.you.down", "a picture of something completly not related to rick roll");
			bl.Login("rick roll", "never gonna give you up");
			bl.Logout("rick roll");
			Assert.IsFalse(bl.Logout("rick roll").success);
		}
	}
}