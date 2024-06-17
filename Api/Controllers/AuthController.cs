using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromServices] AuthService userService, [FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        ServiceResult result = await userService.LoginAsync(request, cancellationToken);
        return HandleServiceResult(result);
    }
}
