using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api;

public class CurrentUserModelBinder(IHttpContextAccessor contextAccessor)
{
    public CurrentUser? BindModel()
    {
        CurrentUser? user = null;

        if (contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Guid id = new(contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);
            string name = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)!.Value;

            user = new(id, name);
        }

        return user;
    }
}
