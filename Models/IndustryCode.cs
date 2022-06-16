using System.Text.Json.Serialization;

namespace bransjekartlegging.Models
{
    public record IndustryCode
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("parentCode")]
        public string ParentCode { get; set; } = string.Empty;

        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("presentationName")]
        public string PresentationName { get; set; } = string.Empty;
    }
}
