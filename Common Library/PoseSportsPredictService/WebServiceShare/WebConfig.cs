using Flurl;
using System;
using System.Collections.Generic;

namespace WebServiceShare
{
	public static class WebConfig
	{
		public enum WebMethodType
		{
			GET,
			POST,
		}

		public static string ServiceBaseUrl { get; } = "http://192.168.0.157:8888/";
	}
}
