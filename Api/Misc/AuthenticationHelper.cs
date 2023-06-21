using System.Security.Claims;
using Database.Enums;

namespace Api.Misc;

public static class AuthenticationHelper
{
    public static bool IsAuthenticated(this HttpContext context)
    {
        var identity = context.User.Identity;
        return identity is { IsAuthenticated: true };
    }

    public static bool CheckRole(this HttpContext context, Roles role)
    {
        return context.User.Claims.FirstOrDefault(x =>
            x.Type == ClaimTypes.Role && x.Value == role.ToString()) != null;
    }
    
    public static Roles GetRole(this HttpContext context)
    {
        var roleClaim = context.User.Claims.FirstOrDefault(x =>
            x.Type == ClaimTypes.Role);

        return Enum.Parse<Roles>(roleClaim!.Value);
    }

    public static string? GetUsername(this HttpContext context)
    {
        return context.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value;
    }
    
}