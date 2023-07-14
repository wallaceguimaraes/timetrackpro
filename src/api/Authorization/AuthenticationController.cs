using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Authorization;
using api.Models.Interfaces;
using api.Models.ViewModel;
using api.ResultModel.Successes.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/v1")]
    public class AuthenticationController : Controller
    {
        private readonly IUserAuthentication _userAuthentication;
        private readonly AuthOptions _options;

        public AuthenticationController(IUserAuthentication userAuthentication,
                                        IOptions<AuthOptions> options
                                        )
        {
            _userAuthentication = userAuthentication;
            _options = options.Value;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Signin([FromBody] CredentialModel model)
        {
            var (success, user) = await _userAuthentication.SignIn(model.Login, model.Password);

            if (!success)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ApiClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ApiClaimTypes.Salt, user.Salt)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new TokenJson(new JwtSecurityTokenHandler().WriteToken(token), user);
        }

    }
}