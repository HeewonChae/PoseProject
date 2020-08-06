using LogicCore.Debug;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI
{
    public static class RestClientExtension
    {
        public static T ExecuteEx<T>(this IRestClient client, IRestRequest request)
        {
            IRestResponse response = client.Execute(request);

            int repeatCnt = 1;
            while (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // 10번 시도 모두 실패할 경우
                if (repeatCnt > 10)
                    Dev.Assert(false, $"Fail API Call RepeatCnt: {repeatCnt}");

                Dev.DebugString($"Fail API Call RepeatCnt: {repeatCnt}", ConsoleColor.Red);

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