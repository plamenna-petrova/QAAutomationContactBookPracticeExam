using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using QAAutomationContactBook.WebClientSeleniumTests;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAAutomationContactBook.APITests
{
    [TestFixture]
    public class ContactBookAPITests
    {
        private IRestClient client;

        [OneTimeSetUp]
        public void Setup() 
        {
            this.client = new RestClient(TestingConstants.BaseAPIUrl);
            this.client.Timeout = 3000;
        }

        [Test, Order(1)]
        [Category("Get Contacts")]
        public void Test_ThirdContactHasLastNameHathaway() 
        {
            //  Arrange
            IRestRequest request = new RestRequest("/contacts", Method.GET);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);

            List<ContactResponseModel> retrievedContacts = new JsonDeserializer().Deserialize<List<ContactResponseModel>>(response);

            var thirdContactLastName = retrievedContacts.Where(c => c.Id == 3).Select(c => c.LastName).First();

            Assert.AreEqual("Hathaway", thirdContactLastName);
        }

        [Test, Order(2)]
        [Category("Search For Contacts")]
        public void Test_SearchByValidKeyword() 
        {
            // Arrange
            string validKeyword = "dimitar";

            IRestRequest request = new RestRequest($"/contacts/search/{validKeyword}", Method.GET);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);

            List<ContactResponseModel> searchedContacts = new JsonDeserializer().Deserialize<List<ContactResponseModel>>(response);

            var firstContactSearchResult = searchedContacts.First();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Dimitar", firstContactSearchResult.FirstName);
                Assert.AreEqual("Berbatov", firstContactSearchResult.LastName);
            });
        }

        [Test, Order(2)]
        [Category("Search For Contacts")]
        public void Test_SearchByInvalidKeyword() 
        {
            // Arrange
            Random random = new Random();

            int randomNumber = random.Next(1, 100000000);

            string invalidKeyword = $"missing{randomNumber.ToString()}";

            IRestRequest request = new RestRequest($"/contacts/search/{invalidKeyword}", Method.GET);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(200, (int)response.StatusCode);

            List<ContactResponseModel> searchedContacts = new JsonDeserializer().Deserialize<List<ContactResponseModel>>(response);

            Assert.IsTrue(searchedContacts.Count == 0);
        }

        [Test, Order(3)]
        [Category("Create New Contact")]
        [TestCase("", "", "", "", "", TestingConstants.BadRequestFirstNameErrorMessage)]
        [TestCase("", "Shelby", "mrthomasshelby@gmail.com", "+44 79 5001 123", TestingConstants.APIExampleContactComments, TestingConstants.BadRequestFirstNameErrorMessage)]
        [TestCase("Thomas", "", "mrthomasshelby@gmail.com", "+44 79 5001 123", TestingConstants.APIExampleContactComments, TestingConstants.BadRequestLastNameErrorMessage)]
        [TestCase("Thomas", "Shelby", "", "+44 79 5001 123", TestingConstants.APIExampleContactComments, TestingConstants.BadRequestInvalidEmailErrorMessage)]
        [TestCase("Thomas", "Shelby", "231742", "+44 79 5001 123", TestingConstants.APIExampleContactComments, TestingConstants.BadRequestInvalidEmailErrorMessage)]
        [TestCase("Thomas", "Shelby", "tommyshelby", "+44 79 5001 123", TestingConstants.APIExampleContactComments, TestingConstants.BadRequestInvalidEmailErrorMessage)]
        public void Test_CreateNewContactWithInvalidData(string firstName, string lastName, string email, string phone, string comments, string badRequestErrorMessage) 
        {
            // Arrange
            IRestRequest request = new RestRequest("/contacts", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            ContactRequestModel invalidContact = new ContactRequestModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Comments = comments
            };

            string body = JsonConvert.SerializeObject(invalidContact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(400, (int)response.StatusCode);

            JObject errorMessageJSON = (JObject)JsonConvert.DeserializeObject(response.Content);
            string errorMessageText = errorMessageJSON["errMsg"].ToString();

            Assert.AreEqual(badRequestErrorMessage, errorMessageText);
        }

        [Test, Order(3)]
        [Category("Create New Contact")]
        public void Test_CreateNewContactWithValidData() 
        {
            // Arrange
            IRestRequest request = new RestRequest("/contacts", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            ContactRequestModel validContact = new ContactRequestModel()
            {
                FirstName = "Thomas",
                LastName = "Shelby",
                Email = "mrthomasshelby@gmail.com",
                Phone = "+44 79 5001 123",
                Comments = TestingConstants.APIExampleContactComments
            };

            string body = JsonConvert.SerializeObject(validContact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(201, (int)response.StatusCode);

            JObject createdContactJSON = (JObject)JsonConvert.DeserializeObject(response.Content);
            string successMessageText = createdContactJSON["msg"].ToString();

            Assert.AreEqual(TestingConstants.CreatedContactSuccessMessage, successMessageText);

            ContactResponseModel newContact = createdContactJSON["contact"].ToObject<ContactResponseModel>();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(newContact.Id > 0);
                Assert.AreEqual(validContact.FirstName, newContact.FirstName);
                Assert.AreEqual(validContact.LastName, newContact.LastName);
                Assert.AreEqual(validContact.Email, newContact.Email);
                Assert.AreEqual(validContact.Phone, newContact.Phone);
                Assert.AreEqual(validContact.Comments, newContact.Comments);
            });
        }

        [Test, Order(3)]
        [Category("Create New Contact")]
        public void Test_CheckForNonExistingContactAfterSuccessfulCreationAndSubsequentDeletion() 
        {
            // Arrange
            IRestRequest postRequest = new RestRequest("/contacts", Method.POST);
            postRequest.AddHeader("Content-Type", "application/json");

            ContactRequestModel validContact = new ContactRequestModel()
            {
                FirstName = "Thomas",
                LastName = "Shelby",
                Email = "mrthomasshelby@gmail.com",
                Phone = "+44 79 5001 123",
                Comments = TestingConstants.APIExampleContactComments
            };

            string body = JsonConvert.SerializeObject(validContact, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            postRequest.AddParameter("application/json", body, ParameterType.RequestBody);

            // Act
            IRestResponse createdContactResponse = client.Execute(postRequest);

            // Assert
            Assert.AreEqual(201, (int)createdContactResponse.StatusCode);

            JObject createdContactJSON = (JObject)JsonConvert.DeserializeObject(createdContactResponse.Content);
            string successMessageText = createdContactJSON["msg"].ToString();

            Assert.AreEqual(TestingConstants.CreatedContactSuccessMessage, successMessageText);

            ContactResponseModel newContact = createdContactJSON["contact"].ToObject<ContactResponseModel>();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(newContact.Id > 0);
                Assert.AreEqual(validContact.FirstName, newContact.FirstName);
                Assert.AreEqual(validContact.LastName, newContact.LastName);
                Assert.AreEqual(validContact.Email, newContact.Email);
                Assert.AreEqual(validContact.Phone, newContact.Phone);
                Assert.AreEqual(validContact.Comments, newContact.Comments);
            });

            // Arrange
            IRestRequest deleteRequest = new RestRequest($"/contacts/{newContact.Id}", Method.DELETE);
            deleteRequest.AddHeader("Content-Type", "application/json");

            // Act
            IRestResponse deletedContactResponse = client.Execute(deleteRequest);

            // Assert
            Assert.AreEqual(200, (int)deletedContactResponse.StatusCode);

            string deletedContactSuccessMessage = TestingConstants.DeletedContactSuccessMessage + $"{newContact.Id.ToString()}";

            JObject deletedContactMessageJSON = (JObject)JsonConvert.DeserializeObject(deletedContactResponse.Content);
            string deletedContactMessageText = deletedContactMessageJSON["msg"].ToString();

            Assert.AreEqual(deletedContactSuccessMessage, deletedContactMessageText);

            // Arrange
            IRestRequest getRequest = new RestRequest("/contacts", Method.GET);

            // Act
            IRestResponse allContactsResponse = client.Execute(getRequest);

            // Assert
            Assert.AreEqual(200, (int)allContactsResponse.StatusCode);

            List<ContactResponseModel> retrievedContacts = new JsonDeserializer().Deserialize<List<ContactResponseModel>>(allContactsResponse);

            Assert.That(!retrievedContacts.Any(c => c.Id == newContact.Id));
        }
    }
}
