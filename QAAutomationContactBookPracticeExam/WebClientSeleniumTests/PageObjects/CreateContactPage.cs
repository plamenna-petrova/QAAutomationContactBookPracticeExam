using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests.PageObjects
{
    public class CreateContactPage : BasePage
    {
        public CreateContactPage(IWebDriver driver)
            : base(driver)
        {

        }

        public override string PageUrl => TestingConstants.BaseWebUrl + "contacts/create";

        public IWebElement FirstNameInputField => driver.FindElement(By.CssSelector("input[name='firstName']"));

        public IWebElement LastNameInputField => driver.FindElement(By.CssSelector("input[name='lastName']"));

        public IWebElement EmailInputField => driver.FindElement(By.CssSelector("input[name='email']"));

        public IWebElement PhoneInputField => driver.FindElement(By.CssSelector("input[name='phone']"));

        public IWebElement CommentsTextArea => driver.FindElement(By.CssSelector("textarea[name='comments']"));

        public IWebElement CreateButton => driver.FindElement(By.Id("create"));

        public IWebElement ErrorDiv => driver.FindElement(By.CssSelector("div.err"));

        public void CreateNewContact(Contact contact) 
        {
            FirstNameInputField.Clear();
            LastNameInputField.Clear();
            EmailInputField.Clear();
            PhoneInputField.Clear();
            CommentsTextArea.Clear();

            FirstNameInputField.SendKeys(contact.FirstName);
            LastNameInputField.SendKeys(contact.LastName);
            EmailInputField.SendKeys(contact.Email);
            PhoneInputField.SendKeys(contact.Phone);
            CommentsTextArea.SendKeys(contact.Comments);

            CreateButton.Click();
        }
    }
}
