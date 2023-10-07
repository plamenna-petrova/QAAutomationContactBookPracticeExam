using System;
using System.Collections.Generic;
using System.Text;

namespace QAAutomationContactBook.APITests
{
    public class ContactResponseModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateCreated { get; set; }

        public string Comments { get; set; }
    }
}
