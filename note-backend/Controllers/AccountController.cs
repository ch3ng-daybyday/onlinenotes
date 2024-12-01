using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.Memory;
using note_backend.Models;
using note_backend.Services;
using System.Net;

namespace note_backend.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserServices _userServices;
        //private readonly IMemoryCache _memoryCache;
        public AccountController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        //[ServiceFilter(typeof(RegistrationService))]
        public async Task<ActionResult> GenerateGaptha()
        {
            string contentType = "image/png";
            return File(_userServices.CreateCaptchaBindIp(), contentType);
        }
        [ServiceFilter(typeof(RegistrationFilter))]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserDTO user)
        {

            var result = await _userServices.Register(user.user, user.PassWord);
            //if (result.)
            if (result.Succeeded)
            {
                (bool, string) loginRes = await
                    _userServices.useLogin(user.user.UserName, user.PassWord, user.user.Email);
                //_memoryCache.Remove(cachekey);
                return Created("", new ApiResponseCommon.ResponseBase
                {
                    Status = "success",
                    Message = "注册成功",
                    Token = loginRes.Item2
                });
            }
            else
            {
                return Ok(new ApiResponseCommon.ResponseBase
                {
                    Status = "fail",
                    //success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

        }
        [HttpPost]
        public async Task<ActionResult> Login(UserDTO user)
        {
            (bool, string) result = await _userServices.useLogin(user.user.UserName, user.PassWord, user.user.Email);
                return Ok(new ApiResponseCommon.ResponseBase
                {
                    Status = "success",
                    Meta = new ApiResponseCommon.ApiResponseMetadata
                    {
                        User = new ApiResponseCommon.UserVerificationMetadata
                        {

                            Verified = result.Item1,
                        }
                    },
                    Token = result.Item2
                });
        }
    }
}
