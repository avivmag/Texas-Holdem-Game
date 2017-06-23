using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;
using CLClient;
using System;
using System.Threading.Tasks;

namespace TestProject.UnitTest
{
    [TestClass]
    public class LoginsTest
    {
        [TestMethod]
        public void loginTest()
        {
            int manyUsers = 10000;
            for (int i = 0; i < manyUsers; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine(Convert.ToString(i));
                    var sysUser = CommClient.Register(Convert.ToString(i), Convert.ToString(i), Convert.ToString(i), null/*Convert.ToString(i)*/);
                });
            }
        }
    }
}