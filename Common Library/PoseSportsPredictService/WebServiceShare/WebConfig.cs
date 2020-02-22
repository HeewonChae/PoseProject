namespace WebServiceShare
{
	public static class WebConfig
	{
		public enum WebMethodType
		{
			GET,
			POST,
		}

		public static string ServiceBaseUrl { get; set; }
	}
}