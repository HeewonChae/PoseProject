using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineNotify
{
    public class LineNotify : ILineNotify
    {
        private string _host_url;
        private RequestBuilder _requestBuilder;

        public void Init(string host_url, string token)
        {
            this._host_url = host_url;
            _requestBuilder = new RequestBuilder(token);
        }

        public void SendMessageAsync(string msg)
        {
            var apiClient = new RestClient(_host_url);

            var request = _requestBuilder.Build(
                "",
                Method.POST,
                null,
                new { message = msg });

            apiClient.ExecuteExAsync(request);
        }

        public void SendMessage(string msg)
        {
            var apiClient = new RestClient(_host_url);

            var request = _requestBuilder.Build(
                "",
                Method.POST,
                null,
                new { message = msg });

            var ret = apiClient.ExecuteEx(request);
        }
    }
}