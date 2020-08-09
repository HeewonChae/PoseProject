using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LineNotify
{
    internal class RequestBuilder
    {
        private readonly string _token;

        public RequestBuilder(string token)
        {
            _token = token;
        }

        public IRestRequest Build(string url, RestSharp.Method methodType, object urlSegment = null, object urlParameter = null)
        {
            var newRequest = new RestRequest(url, methodType);
            newRequest.AddHeader("Authorization", $"Bearer {_token}");

            if (urlSegment != null)
            {
                PropertyInfo[] segments = urlSegment.GetType().GetProperties();
                foreach (var segment in segments)
                {
                    newRequest.AddUrlSegment(segment.Name, segment.GetValue(urlSegment));
                }
            }

            if (urlParameter != null)
            {
                PropertyInfo[] parameters = urlParameter.GetType().GetProperties();
                foreach (var parameter in parameters)
                {
                    newRequest.AddParameter(parameter.Name, parameter.GetValue(urlParameter));
                }
            }

            return newRequest;
        }
    }
}