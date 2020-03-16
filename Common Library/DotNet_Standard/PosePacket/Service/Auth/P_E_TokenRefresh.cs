namespace PosePacket.Service.Auth
{
    public class O_TokenRefresh
    {
        public string PoseToken { get; set; }
        public int TokenExpireIn { get; set; }
    }
}