using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Tutorial.Models
{
	public class CountryInfo
	{
        public class Currency
        {
            [JsonProperty("code")]
            public string Code { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("symbol")]
            public string Symbol { get; set; }
        }

        public class Language
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("nativeName")]
            public string NativeName { get; set; }
        }

        public class RegionalBloc
        {
            [JsonProperty("acronym")]
            public string Acronym { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("topLevelDomain")]
        public List<string> TopLevelDomain { get; set; }
        [JsonProperty("callingCodes")]
        public List<string> CallingCodes { get; set; }
        [JsonProperty("capital")]
        public string Capital { get; set; }
        [JsonProperty("altSpellings")]
        public List<string> AltSpellings { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("subregion")]
        public string Subregion { get; set; }
        [JsonProperty("population")]
        public int Population { get; set; }
        [JsonProperty("latlng")]
        public List<double> Latlng { get; set; }
        [JsonProperty("demonym")]
        public string Demonym { get; set; }
        [JsonProperty("area")]
        public double Area { get; set; }
        [JsonProperty("gini")]
        public double Gini { get; set; }
        [JsonProperty("timezones")]
        public List<string> Timezones { get; set; }
        [JsonProperty("borders")]
        public List<string> Borders { get; set; }
        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
        [JsonProperty("numericCode")]
        public string NumericCode { get; set; }
        [JsonProperty("currencies")]
        public List<Currency> Currencies { get; set; }
        [JsonProperty("languages")]
        public List<Language> Languages { get; set; }
        [JsonProperty("flag")]
        public string Flag { get; set; }
        [JsonProperty("regionalBlocs")]
        public List<RegionalBloc> RegionalBlocs { get; set; }
        [JsonProperty("cioc")]
        public string Cioc { get; set; }
    }
}
