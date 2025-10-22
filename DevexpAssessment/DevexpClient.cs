using DevexpAssessment.Authentification;
using DevexpAssessment.Contacts;
using DevexpAssessment.Messages;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;

namespace DevexpAssessment
{
    public interface IDevexpClient
    {
        /// <summary>
        ///     Manages authentication operations
        /// </summary>
        public AuthenticationController Auth { get; }
        /// <summary>
        ///     Create, update, delete and retrieve contacts
        /// </summary>
        public ContactsController Contacts { get; }
        /// <summary>
        ///     Send and retrieve messages
        /// </summary>
        public MessagesController Messages { get; }
    }

    public class DevexpClient : IDevexpClient
    {
        private readonly HttpClient _httpClient;
        
        private readonly AuthenticationController _auth;
        private readonly ContactsController _contacts;
        private readonly MessagesController _messages;

        /// <inheritdoc />
        public AuthenticationController Auth => _auth;
        /// <inheritdoc />
        public ContactsController Contacts => _contacts;
        /// <inheritdoc />
        public MessagesController Messages => _messages;

        
        public DevexpClient(string apiUrl, ILoggerFactory? loggerFactory = null) // Could add on Options class for handling all SDK options (url, logger, logLevel,etc..)
        {
            _httpClient = CreateHttpClient(apiUrl);

            if (loggerFactory == null)
            {
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            }

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
            // must be careful with retry strategy to avoid duplicating non-idempotent requests
            var retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new HttpRetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 3
                })
                .Build();
            // Configure SocketsHttpHandler to recycle connections periodically
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
