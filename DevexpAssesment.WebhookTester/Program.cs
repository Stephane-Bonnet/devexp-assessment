using DevexpAssessment;
using DevexpAssessment.Contacts;
using DevexpAssessment.Messages;

namespace DevexpAssesment.WebhookTester
{
    internal class Program
    {
        private const string ApiUrl = "http://localhost:3000";

        static async Task Main()
        {
            var client = new DevexpClient(ApiUrl);
            await client.Auth.Authenticate("there-is-no-key");

            var contactsResponse = await client.Contacts.GetAll(0, 8);
            foreach (var contact in contactsResponse.Contacts)
            {
                await client.Contacts.Delete(contact.Id);
            }

            var contact1 = await client.Contacts.Create(new CreateContactRequest()
            {
                Name = "contact1",
                Phone = "+33147424911"
            });
            var contact2 = await client.Contacts.Create(new CreateContactRequest()
            {
                Name = "contact2",
                Phone = "+33686570466"
            });

            await client.Messages.Send(new SendMessageRequest()
            {
                From = contact1.Id,
                Content = "Hello from contact1",
                To = new Recipient() { ContactId = contact2.Id }
            });

            await client.Contacts.Delete(contact1.Id);
            await client.Contacts.Delete(contact2.Id);
        }
    }
}
