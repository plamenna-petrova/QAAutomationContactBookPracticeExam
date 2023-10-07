using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.PageObjects
{
    public class BasePage
    {
        public readonly IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public virtual string PageUrl { get; }

        public IWebElement PageHeading => driver.FindElement(By.CssSelector("body > header > h1"));

        public IWebElement HomePageLink => driver.FindElement(By.XPath("/html/body/aside/ul/li[1]/a"));

        public IWebElement ContactsPageLink => driver.FindElement(By.XPath("/html/body/aside/ul/li[2]/a"));

        public IWebElement CreatePageLink => driver.FindElement(By.XPath("/html/body/aside/ul/li[3]/a"));

        public IWebElement SearchPageLink => driver.FindElement(By.XPath("/html/body/aside/ul/li[4]/a"));

        public void Open() 
        {
            driver.Navigate().GoToUrl(this.PageUrl);
        }

        public bool IsOpened() 
        {
            return driver.Url == this.PageUrl;
        }

        public string GetPageHeadingText() 
        {
            return PageHeading.Text;
        }
    }
}
