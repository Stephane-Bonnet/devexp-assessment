namespace DevexpAssessment.IntegrationTests
{
    [TestClass]
    public sealed class IntegrationTester
    {
        private const string ApiUrl = "http://localhost:3000";

        [TestMethod]
        public async Task IntegrationTest()
        {
            var client = new DevexpClient(ApiUrl);
            await client.Auth.Authenticate("there-is-no-key");

            var contactsResponse = await client.Contacts.GetAll(0, 8);
            foreach (var contact in contactsResponse.Contacts)
            {
                await client.Contacts.Delete(contact.Id);
            }

            var contact1 = await client.Contacts.Create(new Contacts.CreateContactRequest()
            {
                Name = "contact1",
                Phone = "+33147424911"
            });
            var contact2 = await client.Contacts.Create(new Contacts.CreateContactRequest()
            {
                Name = "contact2",
                Phone = "+33686570466"
            });
            var contact3 = await client.Contacts.Create(new Contacts.CreateContactRequest()
            {
                Name = "contact3",
                Phone = "+33686570467"
            });

            var updateRequest = new Contacts.UpdateContactRequest()
            {
                Name = "updatedContact1",
                Phone = "+33147424912"
            };
            var updateContact1 = await client.Contacts.Update(contact1.Id, updateRequest);
            Assert.AreEqual(updateContact1.Name, updateRequest.Name);
            Assert.AreEqual(updateContact1.Phone, updateRequest.Phone);

            var getContact1 = await client.Contacts.Get(updateContact1.Id);
            Assert.AreEqual(getContact1.Name, updateContact1.Name);
            Assert.AreEqual(getContact1.Phone, updateContact1.Phone);

            await client.Contacts.Delete(contact3.Id);
            contactsResponse = await client.Contacts.GetAll(0, 8);
            Assert.IsFalse(contactsResponse.Contacts.Any(c => c.Id == contact3.Id));

            await client.Messages.Send(new Messages.SendMessageRequest()
            {
                From = contact1.Id,
                Content = "Hello from contact1",
                To = new Messages.Recipient() { ContactId = contact2.Id }
            });
            await client.Messages.Send(new Messages.SendMessageRequest()
            {
                From = contact2.Id,
                Content = "Reply from contact2",
                To = new Messages.Recipient() { ContactId = contact1.Id }
            });

            var messagesResponse = await client.Messages.GetAll(0, 30);
            Assert.IsTrue(messagesResponse.Messages.Count >= 2);

            var firstMessage = await client.Messages.Get(messagesResponse.Messages[0].Id);

            await client.Contacts.Delete(updateContact1.Id);
            await client.Contacts.Delete(contact2.Id);
        }
    }
}
