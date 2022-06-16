using System.Text.Json.Serialization;

namespace bransjekartlegging.Models
{
    public record IndustryCodesResult
    {
        [JsonPropertyName("codes")] 
        public List<IndustryCode> IndustryCodes { get; set; } = new();
    }
}
