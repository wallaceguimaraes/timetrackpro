using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Authorization;
// using api.Authorization;
// using api.Extensions.Http;
// using api.Filters;
// using api.Models.ResultModel.Successes;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using api.ResultModel.Successes.Authentication;
// using api.Models.ViewModel;
// using api.ResultModel.Successes.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("/api/v{n}")]
    public class AuthenticationController : Controller
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly AuthOptions _options;

        public AuthenticationController(UserAuthentication userAuthentication,
                                        IOptions<AuthOptions> options
                                        )
        {
            _userAuthentication = userAuthentication;
            _options = options.Value;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Signin([FromBody] CredentialModel model)
        {
            if (!await _userAuthentication.SignIn(model.Login, model.Password))
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ApiClaimTypes.UserId, _userAuthentication.User.Id.ToString()),
                new Claim(ClaimTypes.Email, _userAuthentication.User.Email),
                new Claim(ApiClaimTypes.Salt, _userAuthentication.User.Salt)
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

            return new TokenJson(new JwtSecurityTokenHandler().WriteToken(token), _userAuthentication.User);
        }

        // [HttpGet("whoami"),Auth]
        // public Task<IActionResult> WhoAmI([FromBody] CredentialModel model)
        // {
        //     var whoAmI = HttpContext.WhoAmI();

        //     return new WhoAmIJson(whoAmI);
        // }

    }
}