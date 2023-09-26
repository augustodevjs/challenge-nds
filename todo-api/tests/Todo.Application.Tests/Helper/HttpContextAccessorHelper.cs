using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Todo.Application.Tests.Helper;

public static class HttpContextAccessorHelper
{
    public static void SetupHttpContextWithClaims(Mock<IHttpContextAccessor> httpContextAccessorMock, IEnumerable<Claim> claims)
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims))
        };
        httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
    }
}