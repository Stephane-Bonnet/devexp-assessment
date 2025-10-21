using System.Net.Http.Json;
using System.Net;
using DevexpAssessment.Contacts;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using DevexpAssessment.Exception;


namespace DevexpAssessment.UnitTests
{
    [TestClass]
    public class ContactsControllerTests
    {
        [TestMethod]
        public async Task Create_ReturnsContact_WhenSuccess()
        {
            // Arrange
            var expectedContact = new Contact { Id = "1", Name = "Toto", Phone = "+33147424910" };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedContact)
                });
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.url") };
            var loggerFactoryMock = new Mock<ILoggerFactory>();

            var contactsController = new ContactsController(httpClient, loggerFactoryMock.Object);

            var request = new CreateContactRequest { Name = "Toto", Phone = "+33147424910" };

            // Act
            var result = await contactsController.Create(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContact.Id, result.Id);
            Assert.AreEqual(expectedContact.Name, result.Name);
            Assert.AreEqual(expectedContact.Phone, result.Phone);
        }

        [TestMethod]
        public async Task Get_ReturnsContact_WhenNotFound()
        {
            // Arrange
            var expectedContact = new Contact { Id = "1", Name = "Toto", Phone = "+33147424910" };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.url") };
            var loggerFactoryMock = new Mock<ILoggerFactory>();

            var contactsController = new ContactsController(httpClient, loggerFactoryMock.Object);

            // Assert
            await Assert.ThrowsExceptionAsync<DevexpAssessmentException>(async () => await contactsController.Get("contact.Id"));
        }
    }
}
