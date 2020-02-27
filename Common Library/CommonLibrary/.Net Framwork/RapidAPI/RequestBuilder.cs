using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI
{
	internal class RequestBuilder
	{
		readonly string _x_rapidapi_host;
		readonly string _x_rapidapi_key;

		public RequestBuilder(string host, string key)
		{
			_x_rapidapi_host = host;
			_x_rapidapi_key = key;
		}

		public IRestRequest Build(string url, RestSharp.Method methodType, object urlSegment = null, object urlParameter = null)
		{
			var newRequest = new RestRequest(url, methodType);
			newRequest.AddHeader("x-rapidapi-host", _x_rapidapi_host);
			newRequest.AddHeader("x-rapidapi-key", _x_rapidapi_key);

			if(urlSegment != null)
			{
				PropertyInfo[] segments = urlSegment.GetType().GetProperties();
				foreach (var segment in segments)
				{
					newRequest.AddUrlSegment(segment.Name, segment.GetValue(urlSegment));
				}
			}

			if(urlParameter != null)
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
