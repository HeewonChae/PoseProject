using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineNotify
{
    public static class RestClientExtension
    {
        public static async void ExecuteExAsync(this IRestClient client, IRestRequest request)
        {
            var response = await client.ExecuteAsync(request, request.Method);
        }

        public static IRestResponse ExecuteEx(this IRestClient client, IRestRequest request)
        {
            return client.Execute(request, request.Method);
        }
    }
}