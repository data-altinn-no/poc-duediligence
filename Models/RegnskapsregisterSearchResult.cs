using System.Text.Json.Serialization;

namespace bransjekartlegging.Models
{
    public class RegnskapsregisterSearchResult
    {
        [JsonPropertyName("id")]
        public decimal Id { get; set; }

        [JsonPropertyName("journalnr")]
        public string Journalnr { get; set; } = string.Empty;

        [JsonPropertyName("regnskapstype")]
        public string Regnskapstype { get; set; } = string.Empty;

        [JsonPropertyName("virksomhet")]
        public Virksomhet Virksomhet { get; set; } = new();

        [JsonPropertyName("regnskapsperiode")]
        public Regnskapsperiode Regnskapsperiode { get; set; } = new();

        [JsonPropertyName("valuta")]
        public string Valuta { get; set; } = string.Empty;

        [JsonPropertyName("avviklingsregnskap")]
        public bool Avviklingsregnskap { get; set; } = new();

        [JsonPropertyName("oppstillingsplan")]
        public string Oppstillingsplan { get; set; } = string.Empty;

        [JsonPropertyName("revisjon")]
        public Revisjon Revisjon { get; set; } = new();

        [JsonPropertyName("regnkapsprinsipper")]
        public Regnkapsprinsipper Regnkapsprinsipper { get; set; } = new();

        [JsonPropertyName("egenkapitalGjeld")]
        public EgenkapitalGjeld EgenkapitalGjeld { get; set; } = new();

        [JsonPropertyName("eiendeler")]
        public Eiendeler Eiendeler { get; set; } = new();

        [JsonPropertyName("resultatregnskapResultat")]
        public ResultatregnskapResultat ResultatregnskapResultat { get; set; } = new();
    }

    public class EgenkapitalGjeld
    {
        [JsonPropertyName("sumEgenkapitalGjeld")]
        public decimal SumEgenkapitalGjeld { get; set; }

        [JsonPropertyName("egenkapital")]
        public Egenkapital Egenkapital { get; set; } = new();

        [JsonPropertyName("gjeldOversikt")]
        public GjeldOversikt GjeldOversikt { get; set; } = new();
    }

    public class Egenkapital
    {
        [JsonPropertyName("sumEgenkapital")]
        public decimal SumEgenkapital { get; set; }

        [JsonPropertyName("opptjentEgenkapital")]
        public OpptjentEgenkapital OpptjentEgenkapital { get; set; } = new();

        [JsonPropertyName("innskuttEgenkapital")]
        public InnskuttEgenkapital InnskuttEgenkapital { get; set; } = new();
    }

    public class InnskuttEgenkapital
    {
        [JsonPropertyName("sumInnskuttEgenkaptial")]
        public decimal SumInnskuttEgenkaptial { get; set; } = new();
    }

    public class OpptjentEgenkapital
    {
        [JsonPropertyName("sumOpptjentEgenkapital")]
        public decimal SumOpptjentEgenkapital { get; set; } = new();
    }

    public class GjeldOversikt
    {
        [JsonPropertyName("sumGjeld")]
        public decimal SumGjeld { get; set; }

        [JsonPropertyName("kortsiktigGjeld")]
        public KortsiktigGjeld KortsiktigGjeld { get; set; } = new();

        [JsonPropertyName("langsiktigGjeld")]
        public LangsiktigGjeld LangsiktigGjeld { get; set; } = new();
    }

    public class KortsiktigGjeld
    {
        [JsonPropertyName("sumKortsiktigGjeld")]
        public decimal SumKortsiktigGjeld { get; set; }
    }

    public class LangsiktigGjeld
    {
        [JsonPropertyName("sumLangsiktigGjeld")]
        public decimal SumLangsiktigGjeld { get; set; }
    }

    public class Eiendeler
    {
        [JsonPropertyName("sumVarer")]
        public decimal SumVarer { get; set; }

        [JsonPropertyName("sumFordringer")]
        public decimal SumFordringer { get; set; }

        [JsonPropertyName("sumInvesteringer")]
        public decimal SumInvesteringer { get; set; }

        [JsonPropertyName("sumBankinnskuddOgKontanter")]
        public decimal SumBankinnskuddOgKontanter { get; set; }

        [JsonPropertyName("sumEiendeler")]
        public decimal SumEiendeler { get; set; }

        [JsonPropertyName("omloepsmidler")]
        public Omloepsmidler Omloepsmidler { get; set; } = new();

        [JsonPropertyName("anleggsmidler")]
        public Anleggsmidler Anleggsmidler { get; set; } = new();
    }

    public class Anleggsmidler
    {
        [JsonPropertyName("sumAnleggsmidler")]
        public decimal SumAnleggsmidler { get; set; }
    }

    public class Omloepsmidler
    {
        [JsonPropertyName("sumOmloepsmidler")]
        public decimal SumOmloepsmidler { get; set; }
    }

    public class Regnkapsprinsipper
    {
        [JsonPropertyName("smaaForetak")]
        public bool SmaaForetak { get; set; }

        [JsonPropertyName("regnskapsregler")]
        public string Regnskapsregler { get; set; } = string.Empty;
    }

    public class Regnskapsperiode
    {
        [JsonPropertyName("fraDato")]
        public DateTime FraDato { get; set; }

        [JsonPropertyName("tilDato")]
        public DateTime TilDato { get; set; }
    }

    public class ResultatregnskapResultat
    {
        [JsonPropertyName("ordinaertResultatFoerSkattekostnad")]
        public decimal OrdinaertResultatFoerSkattekostnad { get; set; }

        [JsonPropertyName("ordinaertResultatSkattekostnad")]
        public decimal OrdinaertResultatSkattekostnad { get; set; }

        [JsonPropertyName("aarsresultat")]
        public decimal Aarsresultat { get; set; }

        [JsonPropertyName("totalresultat")]
        public decimal Totalresultat { get; set; }

        [JsonPropertyName("finansresultat")]
        public Finansresultat Finansresultat { get; set; } = new();

        [JsonPropertyName("driftsresultat")]
        public Driftsresultat Driftsresultat { get; set; } = new();
    }

    public class Driftsresultat
    {
        [JsonPropertyName("driftsresultat")]
        public decimal DriftsresultatDriftsresultat { get; set; }

        [JsonPropertyName("driftsinntekter")]
        public Driftsinntekter Driftsinntekter { get; set; } = new();

        [JsonPropertyName("driftskostnad")]
        public Driftskostnad Driftskostnad { get; set; } = new();
    }

    public class Driftsinntekter
    {
        [JsonPropertyName("salgsinntekter")]
        public decimal Salgsinntekter { get; set; }

        [JsonPropertyName("sumDriftsinntekter")]
        public decimal SumDriftsinntekter { get; set; }
    }

    public class Driftskostnad
    {
        [JsonPropertyName("loennskostnad")]
        public decimal Loennskostnad { get; set; }

        [JsonPropertyName("sumDriftskostnad")]
        public decimal SumDriftskostnad { get; set; }
    }

    public class Finansresultat
    {
        [JsonPropertyName("nettoFinans")]
        public decimal NettoFinans { get; set; }

        [JsonPropertyName("finansinntekt")]
        public Finansinntekt Finansinntekt { get; set; } = new();

        [JsonPropertyName("finanskostnad")]
        public Finanskostnad Finanskostnad { get; set; } = new();
    }

    public class Finansinntekt
    {
        [JsonPropertyName("sumFinansinntekter")]
        public decimal SumFinansinntekter { get; set; }
    }

    public class Finanskostnad
    {
        [JsonPropertyName("annenRentekostnad")]
        public decimal AnnenRentekostnad { get; set; }

        [JsonPropertyName("sumFinanskostnad")]
        public decimal SumFinanskostnad { get; set; }
    }

    public class Revisjon
    {
        [JsonPropertyName("ikkeRevidertAarsregnskap")]
        public bool IkkeRevidertAarsregnskap { get; set; }

        [JsonPropertyName("fravalgRevisjon")]
        public bool FravalgRevisjon { get; set; }
    }

    public class Virksomhet
    {
        [JsonPropertyName("organisasjonsnummer")]
        public string Organisasjonsnummer { get; set; } = string.Empty;

        [JsonPropertyName("organisasjonsform")]
        public string Organisasjonsform { get; set; } = string.Empty;

        [JsonPropertyName("morselskap")]
        public bool Morselskap { get; set; }
    }
}
