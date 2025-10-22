using DevexpAssessment.Exceptions;
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

        public async Task<Contact?> Create(CreateContactRequest newContact)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task<GetAllContactsResponse?> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var paramsQuery = Tools.QueryBuilder.Build(new Dictionary<string, object?>
                {
                    { "pageNumber", pageIndex.ToString() },
                    { "pageSize", pageSize.ToString() }
                });

                if (Uri.TryCreate(_httpClient.BaseAddress, _endpoint + paramsQuery, out var uri))
                {
                    var response = await _httpClient.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new DevexpAssessmentException($"Error while getting all contacts: {errorMessage} (status code: {response.StatusCode})");
                    }
                    return await response.Content.ReadFromJsonAsync<GetAllContactsResponse>();
                }
                else
                {
                    throw new DevexpAssessmentUnexpectedException("Error while constructing request URI for getting all contacts");
                }  
            }
            catch (DevexpAssessmentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all contacts");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task<Contact?> Get(string contactId)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contact");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task<Contact?> Update(string contactId, UpdateContactRequest updateContact)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }

        public async Task Delete(string contactId)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact");
                throw new DevexpAssessmentUnexpectedException(ex);
            }
        }
    }
}
