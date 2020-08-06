using Newtonsoft.Json;
using PredictorAPI.Models.Football;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI
{
    public class FootballPredictor : IPredictorAPI
    {
        private string _host_url;
        private RequestBuilder _requestBuilder;

        public void Init(string host_url)
        {
            this._host_url = host_url;
            _requestBuilder = new RequestBuilder();
        }

        public FootballPrediction PredictFixture(int fixture_id)
        {
            var apiClient = new RestClient(_host_url);

            var request = _requestBuilder.Build(
                "football/Predictor",
                Method.GET,
                null,
                new { fixture_id });

            var response = apiClient.ExecuteEx<string>(request);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<FootballPrediction>(response, settings);
        }
    }
}