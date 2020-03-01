using Newtonsoft.Json;

namespace WebServiceShare.ServiceContext
{
	public class WebRequestContext
	{
		public WebServiceClient.WebMethodType MethodType { get; set; }
		public string BaseUrl { get; set; }
		public string ServiceUrl { get; set; }
		public string SegmentGroup { get; set; }
		public object SegmentData { get; set; }
		public string QueryParamGroup { get; set; }
		public object QueryParamData { get; set; }
		public object PostData { get; set; }
		public int AttemptCnt { get; set; }

		public string JsonSerialize()
		{
			if (PostData != null && PostData is string)
				return PostData.ToString();

			if (PostData != null)
				return JsonConvert.SerializeObject(PostData);

			return string.Empty;
		}
	}
}