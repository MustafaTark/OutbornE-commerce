using System.Security.Claims;

namespace OutbornE_commerce.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserIdAPI(this ClaimsPrincipal user) => user.FindFirst("uid")!.Value;
        public static string GetUserFullName(this ClaimsPrincipal user) => user.FindFirst("uid")!.Value;
        //user.FindFirst("uid")!.Value;
        //user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}
