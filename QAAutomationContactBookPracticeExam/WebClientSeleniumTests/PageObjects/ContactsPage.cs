using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.PageObjects
{
    public class ContactsPage : ContactsTablesPage
    {
        public ContactsPage(IWebDriver driver)
            : base(driver)
        {

        }

        public override string PageUrl => TestingConstants.BaseWebUrl + "contacts";

        public override List<IWebElement> ContactEntryTables => driver.FindElements(By.XPath("/html/body/main/div/a/table")).ToList();
    }
}
