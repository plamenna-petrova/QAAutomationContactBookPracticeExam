using NUnit.Framework;
using QAAutomationContactBook.WebClientSeleniumTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.Tests
{
    [TestFixture]
    public class TestsCreateContactPage : TestsBase
    {
        [Test]
        [Category("Create Contact Page")]
        [TestCase("", "", "", "", "", TestingConstants.EmptyFirstNameErrorMessage)]
        [TestCase("", "Bond", "jamesbond@gmail.com", "+44 78 5000 007", TestingConstants.WebExampleContactComments, TestingConstants.EmptyFirstNameErrorMessage)]
        [TestCase("James", "", "jamesbond@gmail.com", "+44 78 5000 007", TestingConstants.WebExampleContactComments, TestingConstants.EmptyLastNameErrorMessage)]
        [TestCase("James", "Bond", "", "+44 78 5000 007", TestingConstants.WebExampleContactComments, TestingConstants.InvalidEmailErrorMessage)]
        [TestCase("James", "Bond", "232342", "+44 78 5000 007", TestingConstants.WebExampleContactComments, TestingConstants.InvalidEmailErrorMessage)]
        [TestCase("James", "Bond", "jamesbond", "+44 78 5000 007", TestingConstants.WebExampleContactComments, TestingConstants.InvalidEmailErrorMessage)]
        public void Test_CreateNewContactWithInvalidData(string firstName, string lastName, string email, string phone, string comments, string errorMessage) 
        {
            CreateContactPage createContactPage = new CreateContactPage(Driver.driver);
            createContactPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(createContactPage.IsOpened());
                Assert.AreEqual("Create Contact", createContactPage.GetPageHeadingText());
            });

            Contact invalidContact = new Contact()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Comments = comments
            };

            createContactPage.CreateNewContact(invalidContact);

            Assert.AreEqual(errorMessage, createContactPage.ErrorDiv.Text);
        }

        [Test]
        [Category("Create Contact Page")]
        public void Test_CreateNewContactWithValidData() 
        {
            CreateContactPage createContactPage = new CreateContactPage(Driver.driver);
            createContactPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(createContactPage.IsOpened());
                Assert.AreEqual("Create Contact", createContactPage.GetPageHeadingText());
            });

            Contact validContact = new Contact()
            {
                FirstName = "James",
                LastName = "Bond",
                Email = "jamesbond@gmail.com",
                Phone = "+44 78 5000 007",
                Comments = "James Bond is a British secret agent working for MI6 under the codename 007."
            };

            createContactPage.CreateNewContact(validContact);

            ContactsPage contactsPage = new ContactsPage(Driver.driver);
            contactsPage.Open();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(contactsPage.IsOpened());
                Assert.AreEqual("View Contacts", contactsPage.GetPageHeadingText());
            });

            var newestContactEntry = contactsPage.GetContactsTablesEntries().Last();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(newestContactEntry.FirstName, validContact.FirstName);
                Assert.AreEqual(newestContactEntry.LastName, validContact.LastName);
                Assert.AreEqual(newestContactEntry.Email, validContact.Email);
                Assert.AreEqual(newestContactEntry.Phone.Replace(" ", string.Empty), validContact.Phone.Replace(" ", string.Empty));
                Assert.AreEqual(newestContactEntry.Comments.Replace(" ", string.Empty), validContact.Comments.Replace(" ", string.Empty));
            });
        }
    }
}
