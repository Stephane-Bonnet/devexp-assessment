using System.Text.Json.Serialization;

namespace DevexpAssessment.Messages
{
    public class Message
    {
        [JsonPropertyName("from")]
        public required string From { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("deliveredAt")]
        public DateTime? DeliveredAt { get; set; }

        [JsonPropertyName("to")]
        public required string To { get; set; }
    }

    public class GetAllMessagesResponse
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = [];

        [JsonPropertyName("data")]
        public Data Data { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("quantityPerPage")]
        public int QuantityPerPage { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("contacts")]
        public Dictionary<string, MessageContact> Contacts { get; set; } = [];
    }

    public class MessageContact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }

    public class SendMessageRequest
    {
        [JsonPropertyName("from")]
        public required string From { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [JsonPropertyName("to")]
        public required Recipient To { get; set; }
    }

    public class Recipient
    {
        [JsonPropertyName("id")]
        public required string ContactId { get; set; }
    }
}
