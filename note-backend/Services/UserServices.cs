using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using note_backend.Models;
using CreateCaptcha;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using System.Drawing.Imaging;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
namespace note_backend.Services
{
    public class UserServices
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMemoryCache memoryCache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string? ipAddress;
        private readonly VerificationCode verificationCode;
        private readonly IOptionsSnapshot<JWTOption> jwtOption;
        public UserServices(UserManager<User> userManager, RoleManager<Role> roleManager, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, VerificationCode verificationCode, IOptionsSnapshot<JWTOption> optionsSnapshot)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.memoryCache = memoryCache;
            this.httpContextAccessor = httpContextAccessor;
            ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            this.verificationCode = verificationCode;
            this.jwtOption = optionsSnapshot;
        }

        public async Task<IdentityResult> Register(User user, string password)
        {
            IdentityResult result = string.IsNullOrEmpty(password) ?
               await userManager.CreateAsync(user) :
               await userManager.CreateAsync(user, password);
            return result;
        }
        //public Task<bool> CheckCaptcha(string captchaKey)
        //{
        //var cachekey = $"OnlineNote_captcha_{ipAddress}";

        ////if (_memoryCache.TryGetValue(cachekey, out string captcha)) { }
        //memoryCache.TryGetValue(cachekey, out string? captcha);
        //if (captcha == captchaKey)
        //return Task.FromResult(true);
        //else
        //return Task.FromResult(false);
        ////}
        public (byte[], string) CreateCaptchaKey()
        {
            CaptchaResult cp = this.verificationCode.getVerificationCodeByte(5, true);
            return (cp.ImageBytes, cp.VerificationCode);
        }

        public byte[] CreateCaptchaBindIp()
        {
            var gaptha = CreateCaptchaKey();
            var cachekey = $"OnlineNote_captcha_{ipAddress}";
            memoryCache.Set(cachekey, gaptha.Item2, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return gaptha.Item1;
        }

        public async Task<(bool, string)> useLogin(string? userName, string passWord, string? email)
        {
            User user = null;
            string loginMsg = "";
            if (!string.IsNullOrEmpty(userName))
            {
                user = userManager.FindByNameAsync(userName!).Result!;
            }
            else if (!string.IsNullOrEmpty(email))
            {
                user = userManager.FindByEmailAsync(email!).Result!;
            }
            else
            {
                //return loginMsg;
            }
            var success = await userManager.CheckPasswordAsync(user!, passWord);
            if (success)
            {
                //创建 jwt信息
                loginMsg = creatJWT(user);
            }
            return (success, loginMsg);
        }
        public string creatJWT(User user)
        {
            var chaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            return buildToken(chaims);
        }

        public string buildToken(List<Claim> claims)
        {
            string key = jwtOption.Value.SigningKey;
            DateTime dateTime = DateTime.Now.AddDays(1);
            byte[] secBytes = Encoding.UTF8.GetBytes(key);
            var secKey = new SymmetricSecurityKey(secBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: dateTime, signingCredentials: credentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }
    }
}
