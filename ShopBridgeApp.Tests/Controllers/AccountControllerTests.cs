using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridgeApp;
using ShopBridgeApp.Controllers;

namespace ShopBridgeApp.Tests.Controllers
{
    [TestClass]
    class AccountControllerTests
    {
        [TestMethod]
        public void Signup()
        {
            //Arrange

            AccountController accountController = new AccountController();

            //Act
            ViewResult result = accountController.Signup() as ViewResult;

            //Asset
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Login()
        {
            //Arrange            
            AccountController accountController = new AccountController();

            //Act
            ViewResult result = accountController.Login() as ViewResult;

            //Asset
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LogOut()
        {
            //Arrange            
            AccountController accountController = new AccountController();

            //Act
            ViewResult result = accountController.LogOut() as ViewResult;

            //Asset
            Assert.IsNotNull(result);
        }
    }
}
