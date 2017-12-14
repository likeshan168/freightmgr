namespace Allocation.Modules.Common
{
    public class LoginResponse : AppResponse
    {
        public int TenantId { get; set; }
        public bool IsAdmin { get; set; }
    }
}