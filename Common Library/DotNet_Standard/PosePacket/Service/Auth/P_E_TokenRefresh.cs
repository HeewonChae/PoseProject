namespace PosePacket.Service.Auth
{
    public class O_TokenRefresh
    {
        public string PoseToken { get; set; }
        public long TokenExpireIn { get; set; }
    }
}