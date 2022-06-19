namespace bransjekartlegging.Models
{
    public record SearchResult {
        public string OrganizationNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Municipality { get; set; } = string.Empty;
        public List<IndustryCode> IndustryCodes { get; set; } = new();
        public string Type { get; set; } = string.Empty;
        public int Employees { get; set; }
        public (DateTime, DateTime)? AccountingPeriod { get; set; }
        public long Revenue { get; set; }
        public long Expenses { get; set; }
        public long Profit { get; set; }
    }
}