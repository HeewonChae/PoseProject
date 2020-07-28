namespace WebServiceShare.WebServiceClient
{
    public static class WebConfig
    {
#if RELEASE
        public static int ReTryCount = 3;
#else
        public static int ReTryCount = 1;
#endif
    }
}