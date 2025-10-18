using DevexpAssessment.Authentification;
using DevexpAssessment.Contacts;
using DevexpAssessment.Messages;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DevexpAssessment
{
    public interface IDevexpClient
    {
        public AuthenticationController Auth { get; }
        public ContactsController Contacts { get; }
        public MessagesController Messages { get; }
    }

    public class DevexpClient : IDevexpClient
    {
        private readonly HttpClient _httpClient;
        
        private readonly AuthenticationController _auth;
        private readonly ContactsController _contacts;
        private readonly MessagesController _messages;

        public AuthenticationController Auth => _auth;
        public ContactsController Contacts => _contacts;
        public MessagesController Messages => _messages;
        
        
        public DevexpClient(string apiUrl = "http://localhost:3000")
        {
            _httpClient = CreateHttpClient(apiUrl);
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
            });
            _auth = new AuthenticationController(_httpClient, loggerFactory);
            _contacts = new ContactsController(_httpClient, loggerFactory);
            _messages = new MessagesController(_httpClient, loggerFactory);  
        }

        private static HttpClient CreateHttpClient(string apiUrl)
        {
            return new HttpClient(BuildHttpHandler()) { BaseAddress = new Uri(apiUrl) };
        }

        private static ResilienceHandler BuildHttpHandler()
        {
            var retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new HttpRetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 3
                })
                .Build();

            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15)
            };
            return new ResilienceHandler(retryPipeline)
            {
                InnerHandler = socketHandler
            };
        }
    }
}
