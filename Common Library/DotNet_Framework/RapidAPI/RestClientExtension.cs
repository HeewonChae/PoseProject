using LogicCore.Debug;
using LogicCore.Utility;
using LogicCore.Utility.ThirdPartyLog;
using Newtonsoft.Json;
using RapidAPI.Models.Football;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI
{
    public static class RestClientExtension
    {
        public static T ExecuteEx<T>(this IRestClient client, IRestRequest request)
        {
            IRestResponse response = client.Execute(request);

            int repeatCnt = 1;
            while (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (repeatCnt % 3 == 0)
                {
                    Log4Net.WriteLog($"Fail API Call URL:{client.BaseUrl}/{request.Resource} RepeatCnt: {repeatCnt}", Log4Net.Level.ERROR);
                }

                response = client.Execute(request);

                repeatCnt++;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(response.Content, settings);
        }
    }
}