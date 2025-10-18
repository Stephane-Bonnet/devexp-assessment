using DevexpAssessment.Exception;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DevexpAssessment.Contacts
{
    public class ContactsController
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly HttpClient _httpClient;
        private const string _endpoint = "/contacts";

        public ContactsController(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<ContactsController>();
        }

        public async Task<Contact?> CreateAsync(CreateContactRequest newContact)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_endpoint, newContact);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error creating contact: {errorMessage} (status code: {response.StatusCode})");
                }
                return await response.Content.ReadFromJsonAsync<Contact>();
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error creating contact");
                throw new DevexpAssessmentException(ex);
            }
        }

        public async Task<GetAllContactsResponse> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error while getting all contacts: {errorMessage} (status code: {response.StatusCode})");
                }
                return await response.Content.ReadFromJsonAsync<GetAllContactsResponse>();
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting all contacts");
                throw new DevexpAssessmentException(ex);
            }
        }

        public async Task<Contact?> GetAsync(string contactId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/{contactId}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error while getting single contact: {errorMessage} (status code: {response.StatusCode})");
                }
                return await response.Content.ReadFromJsonAsync<Contact>();
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting contact");
                throw new DevexpAssessmentException(ex);
            }
        }

        public async Task<Contact?> UpdateAsync(string contactId, UpdateContactRequest updateContact)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"{_endpoint}/{contactId}", updateContact);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error while updating contact: {errorMessage} (status code: {response.StatusCode})");
                }
                return await response.Content.ReadFromJsonAsync<Contact>();
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating contact");
                throw new DevexpAssessmentException(ex);
            }
        }

        public async Task DeleteAsync(string contactId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_endpoint}/{contactId}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new DevexpAssessmentException($"Error while deleting contact: {errorMessage} (status code: {response.StatusCode})");
                }
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact");
                throw new DevexpAssessmentException(ex);
            }
        }
    }
}
