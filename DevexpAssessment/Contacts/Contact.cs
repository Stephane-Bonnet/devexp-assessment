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
        public int PageNumber { get; set; }
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
