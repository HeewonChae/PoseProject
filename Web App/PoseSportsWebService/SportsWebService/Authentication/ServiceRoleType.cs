namespace SportsWebService.Authentication

{
    public enum ServiceRoleType
    {
        _NONE_,
        User = 1 << 0,
        VIP = 1 << 15,
        Admin = 1 << 30,
    }
}