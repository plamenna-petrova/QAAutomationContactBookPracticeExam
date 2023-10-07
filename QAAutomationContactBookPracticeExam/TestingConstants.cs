using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.WebClientSeleniumTests
{
    public class TestingConstants
    {
        public const string BaseWebUrl = "https://contactbook.evgenidimitrov0.repl.co/";

        public const string BaseAPIUrl = "https://contactbook.evgenidimitrov0.repl.co/api";

        public const string WebExampleContactComments = "James Bond is a British secret agent working for MI6 under the codename 007.";

        public const string APIExampleContactComments = "Thomas Shelby is the leader of the Birmingham criminal gang, the Peaky Blinders and the patriarch of the Shelby Family.";

        public const string EmptyFirstNameErrorMessage = "Error: First name cannot be empty!";

        public const string EmptyLastNameErrorMessage = "Error: Last name cannot be empty!";

        public const string InvalidEmailErrorMessage = "Error: Invalid email!";

        public const string BadRequestFirstNameErrorMessage = "First name cannot be empty!";

        public const string BadRequestLastNameErrorMessage = "Last name cannot be empty!";

        public const string BadRequestInvalidEmailErrorMessage = "Invalid email!";

        public const string CreatedContactSuccessMessage = "Contact added.";

        public const string DeletedContactSuccessMessage = "Contact deleted: ";
    }
}
