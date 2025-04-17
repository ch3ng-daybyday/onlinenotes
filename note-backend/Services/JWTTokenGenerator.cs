using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace note_backend.Services
{
    public class JWTTokenGenerator
    {
        private readonly IConfiguration configuration;

        public JWTTokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string GenerateToken(string streamKey)
        {
            //string securityKey = configuration.GetSection("JWT:SigningKey").Value!;
            var securityKey = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "stream-auth"),
                new Claim("streamKey", streamKey)
            };
            var token = new JwtSecurityToken(
                issuer: configuration[""],
                audience: configuration[""],
                claims: claims,
                expires: DateTime.Now.AddMinutes(configuration.GetValue<double>("JWT:ExpireSeconds")),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
