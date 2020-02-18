using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceShare.ServiceContext
{
	public class RequestContext
	{
		public WebConfig.WebMethodType MethodType { get; set; }
		public string ServiceUrl { get; set; }
		public string SegmentGroup { get; set; }
		public object InputData { get; set; }
		public int AttemptCnt { get; set; }

		public string InputDataJsonSerialize()
		{
			if (InputData != null)
				return JsonConvert.SerializeObject(InputData);

			return null;
		}
	}
}
