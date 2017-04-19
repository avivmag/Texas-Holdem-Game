using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;

namespace TestProject.UnitTest
{
	[TestClass]
	public class RegisterTest
	{
		BLInterface bl = new BLImpl();

		[TestMethod]
		public void successTest()
		{
			Assert.IsTrue(bl.Register("Scorpion", "GET OVER HERE!!", "scorpy@winnig.tournament.mk", "deadly skull behind a mask").success);
		}

		[TestMethod]
		public void alreadyRegisteredTest()
		{
			bl.Register("John Cena", "STFU!!", "cena@winnig.tournament.wwe", "an empty image");
			Assert.IsFalse(bl.Register("John Cena", "You can\'t see me!", "cena@winnig.again.tournament.wwe", "another empty image").success);
		}

		[TestMethod]
		public void emptyUserNameTest()
		{
			Assert.IsFalse(bl.Register("", "P@SSW0RD", "gmail@gmail.com", "none").success);
		}

		[TestMethod]
		public void emptyPasswordTest()
		{
			Assert.IsFalse(bl.Register("NeverPassworded", "", "no@password.com", "ying yang photo").success);
		}

		[TestMethod]
		public void alreadyExistsEmailTest()
		{
			Assert.IsFalse(bl.Register("NeverPassworded", "", "no@password.com", "ying yang photo").success);
		}
	}

}
