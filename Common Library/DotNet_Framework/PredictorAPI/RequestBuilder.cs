using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI
{
    internal class RequestBuilder
    {
        public IRestRequest Build(string url, RestSharp.Method methodType, object urlSegment = null, object urlParameter = null)
        {
            var newRequest = new RestRequest(url, methodType);

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