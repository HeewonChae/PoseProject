using LogicCore.Debug;
using LogicCore.Utility.ThirdPartyLog;
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
#if DEBUG
                // 10번 시도 모두 실패할 경우
                if (repeatCnt > 10)
                    Dev.Assert(false, $"Fail API Call RepeatCnt: {repeatCnt}");
#endif

#if LINE_NOTIFY
                if (repeatCnt % 3 == 0)
                {
                    FootballPredictor.ErrorNotify.SendMessage($"Fail PredictorAPI Call BaseHost: {client.BaseHost}, RepeatCnt: {repeatCnt}");
                }
#endif
                Log4Net.WriteLog($"Fail PredictorAPI Call RepeatCnt: {repeatCnt}", Log4Net.Level.ERROR);

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