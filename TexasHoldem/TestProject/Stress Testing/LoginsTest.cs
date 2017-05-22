using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Game;
using CLClient;
using System;

namespace TestProject.UnitTest
{
    [TestClass]
    public class LoginsTest
    {

        [TestMethod]
        public void loginTest()
        {
            int manyUsers = 1;
            for (int i = 0; i < manyUsers; i++)
            {
                Console.WriteLine(Convert.ToString(i));
                CommClient.Register(Convert.ToString(i), Convert.ToString(i), Convert.ToString(i), "");
            }

            //for (int i = 0; i < manyUsers; i++)
            //{
            //    CommClient.Login(Convert.ToString(i), Convert.ToString(i));
            //}
        }
    }
}