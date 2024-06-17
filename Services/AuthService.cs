using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services;

public class AuthService(SignInManager<User> signInManager,
                         UserManager<User> userManager,
                         IRepositoryWrapper repository,
                         IValidator<LoginRequest> loginValidator,
                         IOptions<TokenSettings> tokenSettings)
{
    public async Task<ServiceResult<AuthorizedUserDTO>> LoginAsync(LoginRequest request,
                                                                   CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await loginValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new FluentValidationErrorServiceResult<AuthorizedUserDTO>(validationResult);
        }

        SignInResult result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

        if (result.Succeeded)
        {
            AuthorizedUserDTO token = await CreateTokenAsync(request.Username, cancellationToken);
            return new SuccessServiceResult<AuthorizedUserDTO>(token);
        }

        return new UnauthorizedServiceResult<AuthorizedUserDTO>();
    }

    private async Task<AuthorizedUserDTO> CreateTokenAsync(string username,
                                                           CancellationToken cancellationToken)
    {
        User user = (await userManager.FindByNameAsync(username))!;

        string name = user.UserName!;
        if (user.DriverId is not null)
        {
            name = (await repository.Driver
                        .GetNameAsync(user.DriverId.Value, cancellationToken))!;
        }

        IList<Claim> claims = await userManager.GetClaimsAsync(user);

        string epochNow = EpochTime.GetIntDate(DateTime.UtcNow).ToString();

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, name));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, epochNow));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, epochNow));

        IList<string> userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(tokenSettings.Value.Key!);

        SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = tokenSettings.Value.Issuer,
            Audience = tokenSettings.Value.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(tokenSettings.Value.ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return new(name, tokenHandler.WriteToken(token));
    }
}
