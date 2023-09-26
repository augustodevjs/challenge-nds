using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Todo.Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static int? GetUserId(this IHttpContextAccessor? contextAccessor)
    {
        var id = contextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return id == null ? null : int.Parse(id);
    }
}