using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using note_backend.Models;
using note_backend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace note_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        private readonly JWTTokenGenerator jWTTokenGenerator;
         
        public LiveController(JWTTokenGenerator jWTTokenGenerator)
        {
            this.jWTTokenGenerator = jWTTokenGenerator;
        }

        [HttpGet("stream-token")]
        public IActionResult GetStreamToken(string streamKey)
        {
            var token = jWTTokenGenerator.GenerateToken(streamKey);
            return Ok(new { token });
        }


        [HttpGet("on-pulish")]
        public IActionResult onPublish([FromBody] string streamKey, [FromBody] string token)
        {
            var tokenHanlder = new JwtSecurityTokenHandler();
             var principal = tokenHanlder.ValidateToken(token,new TokenValidationParameters{
                ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = _configuration["Jwt:Issuer"],
            //ValidAudience = _configuration["Jwt:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])) },out_);



            return Ok();
        }
    }
}
