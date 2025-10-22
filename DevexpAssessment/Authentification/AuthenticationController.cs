using Microsoft.Extensions.Logging;

namespace DevexpAssessment.Authentification
{
    public class AuthenticationController
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<AuthenticationController>();
        }

        public ValueTask Authenticate(string secretKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", secretKey);
            // Optionally, you could add a method to verify the authentication by making a test request to the server.
            return ValueTask.CompletedTask;
        }
    }
}
