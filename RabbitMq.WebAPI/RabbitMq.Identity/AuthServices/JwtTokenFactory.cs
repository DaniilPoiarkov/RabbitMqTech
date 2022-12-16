using Microsoft.IdentityModel.Tokens;
using RabbitMq.Identity.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RabbitMq.Identity.AuthServices
{
    public class JwtTokenFactory
    {
        private readonly JwtOptions _options;
        public JwtTokenFactory(JwtOptions options)
        {
            _options = options;
        }

        public string GenerateToken(string userEmail, string userName, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim("id", userId.ToString()),
            };

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.Now.AddDays(_options.ValidFor),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
