using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests
{
    public class Driver
    {
        public static IWebDriver driver;

        public static void InitializeDriver() 
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
    }
}
