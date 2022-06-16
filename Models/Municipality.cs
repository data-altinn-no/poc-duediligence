using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bransjekartlegging.Models
{
    public record Municipality
    {
        [JsonPropertyName("kommunenummer")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("kommunenavn")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("kommunenavnNorsk")]
        public string NameNorwegian { get; set; } = string.Empty;
    }
}
