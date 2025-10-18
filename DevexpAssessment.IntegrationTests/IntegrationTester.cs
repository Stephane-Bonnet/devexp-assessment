namespace DevexpAssessment.IntegrationTests
{
    [TestClass]
    public sealed class IntegrationTester
    {
        [TestMethod]
        public async Task IntegrationTest()
        {
            var client = new DevexpClient();
            client.Auth.Authenticate("there-is-no-key");

            var contact1 = await client.Contacts.CreateAsync(new DevexpAssessment.Contacts.CreateContactRequest()
            {
                Name = "contact1",
                Phone = "+33147424911"
            });
            var contact2 = await client.Contacts.CreateAsync(new DevexpAssessment.Contacts.CreateContactRequest()
            {
                Name = "contact2",
                Phone = "+33686570466"
            });
            var contact3 = await client.Contacts.CreateAsync(new DevexpAssessment.Contacts.CreateContactRequest()
            {
                Name = "contact32",
                Phone = "+33686570467"
            });

            var contactsResponse = await client.Contacts.GetAllAsync();
            var updateContact1 = await client.Contacts.UpdateAsync(contact1.Id, new DevexpAssessment.Contacts.UpdateContactRequest()
            {
                Name = "updatedContact1",
                Phone = "+33147424912"
            });
            var getContact1 = await client.Contacts.GetAsync(contact1.Id);
            await client.Contacts.DeleteAsync(contact3.Id);

            await client.Messages.SendAsync(new DevexpAssessment.Messages.SendMessageRequest()
            {
                From = contact1.Id,
                Content = "Hello from contact1",
                To = new DevexpAssessment.Messages.Recipient() { ContactId = contact2.Id }
            });
            await client.Messages.SendAsync(new DevexpAssessment.Messages.SendMessageRequest()
            {
                From = contact2.Id,
                Content = "Reply from contact2",
                To = new DevexpAssessment.Messages.Recipient() { ContactId = contact1.Id }
            });

            var messageResponse = await client.Messages.GetAllAsync();

            var firstMessage = await client.Messages.GetByIdAsync(messageResponse.Messages[0].Id);

            await client.Contacts.DeleteAsync(contact1.Id);
            await client.Contacts.DeleteAsync(contact2.Id);
        }
    }
}
