using Newtonsoft.Json;
using PredictorAPI.Models.Football;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI
{
    public class FootballPredictor : IPredictorAPI
    {
        private string _host_url;
        private RequestBuilder _requestBuilder;
        public static LineNotify.LineNotify ErrorNotify;

        public void Init(string host_url, string line_notify_host, string line_notify_token)
        {
            this._host_url = host_url;
            _requestBuilder = new RequestBuilder();

            ErrorNotify = new LineNotify.LineNotify();
            ErrorNotify.Init(line_notify_host, line_notify_token);
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
            if (response == null)
                return null;

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