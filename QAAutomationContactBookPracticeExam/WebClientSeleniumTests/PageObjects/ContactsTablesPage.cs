using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.PageObjects
{
    public class ContactsTablesPage : BasePage
    {
        public ContactsTablesPage(IWebDriver driver)
            : base(driver)
        {

        }

        public virtual List<IWebElement> ContactEntryTables { get;  }

        public List<Contact> GetContactsTablesEntries()
        {
            List<Contact> contactEntries = new List<Contact>();

            foreach (var contactEntryTable in this.ContactEntryTables)
            {
                var firstNameTableData = contactEntryTable.FindElement(By.CssSelector("tbody > tr.fname > td")).Text;
                var lastNameTableData = contactEntryTable.FindElement(By.CssSelector("tbody > tr.lname > td")).Text;
                var emailTableData = contactEntryTable.FindElement(By.CssSelector("tbody > tr.email > td")).Text;
                var phoneTableData = contactEntryTable.FindElement(By.CssSelector("tbody > tr.phone > td")).Text;
                var commentsTableData = contactEntryTable.FindElement(By.CssSelector("tbody > tr.comments > td > div")).Text;
                contactEntries.Add(new Contact 
                {
                    FirstName = firstNameTableData,
                    LastName = lastNameTableData,
                    Email = emailTableData,
                    Phone = phoneTableData,
                    Comments = commentsTableData
                });
            }

            return contactEntries;
        }
    }
}
