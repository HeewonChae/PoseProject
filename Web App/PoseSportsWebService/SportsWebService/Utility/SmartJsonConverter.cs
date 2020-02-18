using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Utility
{
	public static class SmartJsonConverter
	{
		public static TOut JsonDeserialize<TOut>(this string jsonString)
		{
			TOut result = default;

			if (string.IsNullOrEmpty(jsonString))
				return result;

			try
			{
				result = JsonConvert.DeserializeObject<TOut>(jsonString);
			}
			catch(Exception)
			{
				ErrorHandler.OccurException(System.Net.HttpStatusCode.BadRequest);
			}

			return result;
		}
	}
}