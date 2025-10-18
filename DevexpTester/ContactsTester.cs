using DevexpAssessment;

namespace DevexpTester
{
    [TestClass]
    public sealed class ContactsTester
    {
        private string _contactId;

        [TestMethod]
        public async Task TestContactWorkflow()
        {
            //await CreateContact();
            await GetAllContacts();
            //await GetContactById();
            //await UpdateContactById();
            //await DeleteContactById();
        }

        public async Task CreateContact()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");
            var contact = await client.Contacts.CreateAsync(new DevexpAssessment.Contacts.CreateContactRequest()
            {
                Name = "titi",
                Phone = "+33147424910"
            });
            Console.WriteLine($"Created contact: {contact}");
            _contactId = contact.Id;
        }

        public async Task GetAllContacts()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");
            var contactsResponse = await client.Contacts.GetAllAsync();
            Console.WriteLine($"Contacts: {string.Join(',', contactsResponse.Contacts.Select(contact => contact.ToString()))}");

            foreach (var contact in contactsResponse.Contacts)
            {
                await client.Contacts.DeleteAsync(contact.Id);
            }
        }

        public async Task GetContactById()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");
            var contact = await client.Contacts.GetAsync(_contactId);
            Console.WriteLine($"Contact: {contact}");
        }

        public async Task UpdateContactById()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");
            var contact = await client.Contacts.UpdateAsync(_contactId, new DevexpAssessment.Contacts.UpdateContactRequest() { Name = "toto2", Phone = "+33147424911" });
            Console.WriteLine($"Contact: {contact}");
        }

        public async Task DeleteContactById()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");
            await client.Contacts.DeleteAsync(_contactId);
            Console.WriteLine($"Deleted contact {_contactId}");
        }
    }
}
