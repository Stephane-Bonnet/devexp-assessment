using DevexpAssessment.Exceptions;
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

        public async Task Send(SendMessageRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_endpoint, request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error sending message: {errorMessage} (status code: {response.StatusCode})");
                }
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task<GetAllMessagesResponse?> GetAll(int pageIndex = 0, int pageSize = 100)
        {
            try
            {
                var paramsQuery = Tools.QueryBuilder.Build(new Dictionary<string, object?>
                {
                    { "page", pageIndex.ToString() },
                    { "limit", pageSize.ToString() }
                });

                if (Uri.TryCreate(_httpClient.BaseAddress, _endpoint + paramsQuery, out var uri))
                {
                    var response = await _httpClient.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new DevexpAssessmentException($"Error getting messages: {errorMessage} (status code: {response.StatusCode})");
                    }
                    return await response.Content.ReadFromJsonAsync<GetAllMessagesResponse>();
                }
                else
                {
                    throw new System.Exception("Error while constructing request URI for getting all messages");
                }
                    
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task<Message?> Get(string messageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/{messageId}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error getting message: {errorMessage} (status code: {response.StatusCode})");
                }
                return await response.Content.ReadFromJsonAsync<Message>();
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting message");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }
    }
}
