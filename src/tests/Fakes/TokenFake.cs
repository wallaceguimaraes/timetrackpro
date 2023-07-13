using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace tests.Fakes
{
    public class TokenFake
    {
        public string Token { get; set; }
        public TokenFake()
        {
            var claims = new[]
            {
                new Claim(ApiClaimTypes.UserId, "1"),
                new Claim(ClaimTypes.Email, "teste@mail.com"),
                new Claim(ApiClaimTypes.Salt, "d2de614740c24985b7194ba7f095e5a9")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("50D2C5353238D86B047079FCD2F79DA79F55CE79ABFC333EAA2E9D3072211BBD"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "6144F75E2",
                audience: "B706E107AFE8CB7",
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            Token = new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}