using System.Text.Json.Serialization;

namespace DevexpAssessment.Contacts
{
    public class Contact
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("phone")]
        public required string Phone { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Phone: {Phone}";
        }
    }

    public class GetAllContactsResponse
    {
        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; } = [];

        // was going to create a common "PagedResponse" common class, but field names were different between Contacts and Messages

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }

    public class CreateContactRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("phone")]
        public required string Phone { get; set; }
    }

    public class UpdateContactRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("phone")]
        public required string Phone { get; set; }
    }
}
