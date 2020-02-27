using Newtonsoft.Json;

namespace WebServiceShare.ServiceContext
{
	public class RequestContext
	{
		public WebConfig.WebMethodType MethodType { get; set; }
		public string ServiceUrl { get; set; }
		public string SegmentGroup { get; set; }
		public object SegmentData { get; set; }
		public string QueryParamGroup { get; set; }
		public object QueryParamData { get; set; }
		public object PostData { get; set; }
		public int AttemptCnt { get; set; }

		public string PostDataJsonSerialize()
		{
			if (SegmentData != null)
				return JsonConvert.SerializeObject(PostData);

			return null;
		}
	}
}