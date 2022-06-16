using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bransjekartlegging.Models{
    public class SearchQuery {
        public List<string> IndustryCodes { get; set; } = new();
        public List<string> Municipalities { get; set; } = new();
    }
}

