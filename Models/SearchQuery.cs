using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bransjekartlegging.Models{
    public record SearchQuery {
        public List<string> IndustryCodes { get; set; } = new();
        public List<string> Municipalities { get; set; } = new();
        public int OffsetPage { get; set; } = 0;
        public bool Append { get; set; } = false;
    }
}

