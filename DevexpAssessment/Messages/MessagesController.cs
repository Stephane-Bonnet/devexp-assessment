using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DevexpAssessment.Messages
{
    public class MessagesController
    {
        private readonly HttpClient _httpClient;
        private const string _endpoint = "/messages";
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<MessagesController>();
        }

        public async Task SendAsync(SendMessageRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<GetAllMessagesResponse?> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(_endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetAllMessagesResponse>();
        }

        public async Task<Message?> GetByIdAsync(string messageId)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/{messageId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Message>();
        }
    }
}
