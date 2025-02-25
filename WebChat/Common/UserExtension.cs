using System.Security.Claims;

namespace WebChat.Common
{
    public static class UserExtension
    {
        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated ?? false;
        }
        public static string GetFullName(this ClaimsPrincipal user)
        {
            return user.FindFirst("FullName")?.Value ?? string.Empty;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated == true ? int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;
        }

        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Identity?.Name ?? string.Empty;
        }
    }
}
