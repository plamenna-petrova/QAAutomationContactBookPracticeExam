using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.PageObjects
{
    public class SearchPage : ContactsTablesPage
    {
        public SearchPage(IWebDriver driver)
            : base(driver)
        {

        }

        public override string PageUrl => TestingConstants.BaseWebUrl + "contacts/search";

        public IWebElement KeywordInputField => driver.FindElement(By.Id("keyword"));

        public IWebElement SearchButton => driver.FindElement(By.Id("search"));

        public override List<IWebElement> ContactEntryTables => driver.FindElements(By.XPath("/html/body/main/div[2]/a/table")).ToList();

        public IWebElement NoContactsFoundDiv => driver.FindElement(By.Id("searchResult"));

        public void SearchForContact(string keyword) 
        {
            KeywordInputField.Clear();
            KeywordInputField.SendKeys(keyword);
            SearchButton.Click();
        }
    }
}
