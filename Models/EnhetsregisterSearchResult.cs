using System.Text.Json.Serialization;

namespace bransjekartlegging.Models
{
    public class EnhetsregisterSearchResultHalWrapper
    {
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; } = new();

        [JsonPropertyName("_links")]
        public TemperaturesLinks Links { get; set; } = new();

        [JsonPropertyName("page")]
        public Page Page { get; set; } = new();
    }

    public class Embedded
    {
        [JsonPropertyName("enheter")]
        public List<EnhetsregisterUnit> Enheter { get; set; } = new();
    }

    public record EnhetsregisterUnit
    {
        [JsonPropertyName("organisasjonsnummer")]
        public string Organisasjonsnummer { get; set; } = string.Empty;

        [JsonPropertyName("navn")]
        public string Navn { get; set; } = string.Empty;

        [JsonPropertyName("organisasjonsform")]
        public Organisasjonsform Organisasjonsform { get; set; } = new();

        [JsonPropertyName("postadresse")]
        public Postadresse Postadresse { get; set; } = new();

        [JsonPropertyName("registreringsdatoEnhetsregisteret")]
        public DateTime RegistreringsdatoEnhetsregisteret { get; set; }

        [JsonPropertyName("registrertIMvaregisteret")]
        public bool RegistrertIMvaregisteret { get; set; }

        [JsonPropertyName("naeringskode1")]
        public Naeringskode Naeringskode1 { get; set; } = new();

        [JsonPropertyName("naeringskode2")]
        public Naeringskode? Naeringskode2 { get; set; }

        [JsonPropertyName("naeringskode3")]
        public Naeringskode? Naeringskode3 { get; set; }

        [JsonPropertyName("antallAnsatte")]
        public int AntallAnsatte { get; set; }

        [JsonPropertyName("forretningsadresse")]
        public Forretningsadresse Forretningsadresse { get; set; } = new();

        [JsonPropertyName("stiftelsesdato")]
        public DateTime Stiftelsesdato { get; set; }

        [JsonPropertyName("registrertIForetaksregisteret")]
        public bool RegistrertIForetaksregisteret { get; set; }

        [JsonPropertyName("registrertIStiftelsesregisteret")]
        public bool RegistrertIStiftelsesregisteret { get; set; }

        [JsonPropertyName("registrertIFrivillighetsregisteret")]
        public bool RegistrertIFrivillighetsregisteret { get; set; }

        [JsonPropertyName("konkurs")]
        public bool Konkurs { get; set; }

        [JsonPropertyName("underAvvikling")]
        public bool UnderAvvikling { get; set; }

        [JsonPropertyName("underTvangsavviklingEllerTvangsopplosning")]
        public bool UnderTvangsavviklingEllerTvangsopplosning { get; set; }

        [JsonPropertyName("maalform")]
        public string Maalform { get; set; } = string.Empty;

        [JsonPropertyName("_links")]
        public EnheterLinks Links { get; set; } = new();
    }

    public class Forretningsadresse
    {
        [JsonPropertyName("land")]
        public string Land { get; set; } = string.Empty;

        [JsonPropertyName("landkode")]
        public string Landkode { get; set; } = string.Empty;

        [JsonPropertyName("postnummer")]
        public string Postnummer { get; set; } = string.Empty;

        [JsonPropertyName("poststed")]
        public string Poststed { get; set; } = string.Empty;

        [JsonPropertyName("adresse")]
        public List<string> Adresse { get; set; } = new();

        [JsonPropertyName("kommune")]
        public string Kommune { get; set; } = string.Empty;

        [JsonPropertyName("kommunenummer")]
        public string Kommunenummer { get; set; } = string.Empty;
    }

    public class EnheterLinks
    {
        [JsonPropertyName("self")]
        public UriHref Self { get; set; } = new();
    }

    public class UriHref
    {
        [JsonPropertyName("href")]
        public Uri? Href { get; set; }
    }

    public class Naeringskode
    {
        [JsonPropertyName("beskrivelse")]
        public string Beskrivelse { get; set; } = string.Empty;

        [JsonPropertyName("kode")]
        public string Kode { get; set; } = string.Empty;
    }

    public class Organisasjonsform
    {
        [JsonPropertyName("kode")]
        public string Kode { get; set; } = string.Empty;

        [JsonPropertyName("beskrivelse")]
        public string Beskrivelse { get; set; } = string.Empty;

        [JsonPropertyName("_links")]
        public EnheterLinks Links { get; set; } = new();
    }

    public class Postadresse
    {
        [JsonPropertyName("land")]
        public string Land { get; set; } = string.Empty;

        [JsonPropertyName("landkode")]
        public string Landkode { get; set; } = string.Empty;

        [JsonPropertyName("postnummer")]
        public string Postnummer { get; set; } = string.Empty;

        [JsonPropertyName("poststed")]
        public string Poststed { get; set; } = string.Empty;

        [JsonPropertyName("adresse")]
        public List<string> Adresse { get; set; } = new();

        [JsonPropertyName("kommune")]
        public string Kommune { get; set; } = string.Empty;

        [JsonPropertyName("kommunenummer")]
        public string Kommunenummer { get; set; } = string.Empty;
    }

    public class TemperaturesLinks
    {
        [JsonPropertyName("first")]
        public UriHref UriHref { get; set; } = new();

        [JsonPropertyName("self")]
        public UriHref Self { get; set; } = new();

        [JsonPropertyName("next")]
        public UriHref Next { get; set; } = new();

        [JsonPropertyName("last")]
        public UriHref Last { get; set; } = new();
    }

    public class Page
    {
        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("totalElements")]
        public long TotalElements { get; set; }

        [JsonPropertyName("totalPages")]
        public long TotalPages { get; set; }

        [JsonPropertyName("number")]
        public long Number { get; set; }
    }
}
