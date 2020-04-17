using MessagePack;
using Newtonsoft.Json;
using WebServiceShare.WebServiceClient;

namespace WebServiceShare.ServiceContext
{
    public class WebRequestContext
    {
        public SerializeType SerializeType { get; set; }
        public WebMethodType MethodType { get; set; }
        public string BaseUrl { get; set; }
        public string ServiceUrl { get; set; }
        public string SegmentGroup { get; set; }
        public object SegmentData { get; set; }
        public string QueryParamGroup { get; set; }
        public object QueryParamData { get; set; }
        public object PostData { get; set; }
        public int AttemptCnt { get; set; }
        public bool NeedEncrypt { get; set; }

        public static implicit operator byte[](WebRequestContext req) => req.PostData as byte[];

        public static implicit operator string(WebRequestContext req) => req.PostData as string;

        public void DataSerialize()
        {
            switch (SerializeType)
            {
                case SerializeType.MessagePack:
                    {
                        if (PostData != null && PostData is byte[])
                            PostData = PostData;

                        if (PostData != null)
                            PostData = MessagePackSerializer.Serialize(PostData);
                    }
                    break;

                case SerializeType.Json:
                    {
                        if (PostData != null && PostData is string)
                            PostData = PostData.ToString();

                        if (PostData != null)
                            PostData = JsonConvert.SerializeObject(PostData);
                    }
                    break;
            }
        }
    }
}