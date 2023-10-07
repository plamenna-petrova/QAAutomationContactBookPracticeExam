using NUnit.Framework;
using QAAutomationContactBook.WebClientSeleniumTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.Tests
{
    [TestFixture]
    public class TestsContactsPage : TestsBase
    {
        [Test]
        [Category("Contacts Page")]
        public void Test_FirstContactHasFirstNameElon() 
        {
            var contactsPage = new ContactsPage(Driver.driver);
            contactsPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(contactsPage.IsOpened());
                Assert.AreEqual("View Contacts", contactsPage.GetPageHeadingText());
            });

            var firstContactEntry = contactsPage.GetContactsTablesEntries().First();

            Assert.AreEqual("Elon", firstContactEntry.FirstName);
        }
    }
}
