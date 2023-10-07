using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.Tests
{
    [TestFixture]
    public class TestsBase
    {
        [OneTimeSetUp]
        public void Setup() 
        {
            Driver.InitializeDriver();
            Driver.driver.Navigate().GoToUrl(TestingConstants.BaseWebUrl);
        }

        [OneTimeTearDown]
        public void ShutDown() 
        {
            Driver.driver.Quit();
        }
    }
}
