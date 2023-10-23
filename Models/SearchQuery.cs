using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bransjekartlegging.Models{
    public record SearchQuery
    {
        public string OrganisationNumber { get; set; }

        public bool Append { get; set; } = false;
    }
}

