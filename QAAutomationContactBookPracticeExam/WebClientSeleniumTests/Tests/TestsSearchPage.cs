using NUnit.Framework;
using QAAutomationContactBook.WebClientSeleniumTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.Tests
{
    [TestFixture]
    public class TestsSearchPage : TestsBase
    {
        [Test]
        [Category("Search Page")]
        public void Test_SearchByKeyword() 
        {
            SearchPage searchPage = new SearchPage(Driver.driver);
            searchPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(searchPage.IsOpened());
                Assert.AreEqual("Search Contacts", searchPage.GetPageHeadingText());
            });

            string keywordInput = "dimitar";

            searchPage.SearchForContact(keywordInput);

            var firstRetrievedContact = searchPage.GetContactsTablesEntries().First();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Dimitar", firstRetrievedContact.FirstName);
                Assert.AreEqual("Berbatov", firstRetrievedContact.LastName);
            });
        }

        [Test]
        [Category("Search Page")]
        public void Test_SearchByInvalidKeyword() 
        {
            string invalidKeywordInput = "Invalid key word 123";

            SearchPage searchPage = new SearchPage(Driver.driver);
            searchPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(searchPage.IsOpened());
                Assert.AreEqual("Search Contacts", searchPage.GetPageHeadingText());
            });

            searchPage.SearchForContact(invalidKeywordInput);

            Assert.AreEqual("No contacts found.", searchPage.NoContactsFoundDiv.Text);
        }
    }
}
