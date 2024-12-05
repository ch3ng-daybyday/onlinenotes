using ApiResponseCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using note_backend.Models;
using System.Net;

namespace note_backend.Services
{
    public class RegistrationFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache _memoryCache;


        public RegistrationFilter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress != null)
            {
                var requestTimesKey = $"OnlineNote_request_time_{ipAddress}";
                if (_memoryCache.TryGetValue(requestTimesKey, out int resCount))
                {
                    if (resCount > 5)
                    {
                        var captchakey = $"OnlineNote_captcha_{ipAddress}";
                        context.ActionArguments.TryGetValue("user", out Object? req);
                        //string captchaValue = "";
                        _memoryCache.TryGetValue(captchakey, out string captchaValue);
                        if (captchaValue != null && ((UserDTO)req).Captchakey.Equals(captchaValue))
                        {
                            _memoryCache.Remove(captchakey);//
                            _memoryCache.Remove(requestTimesKey);
                            await next();
                        }
                        else
                        {
                            //验证码
                            context.Result = new OkObjectResult(new ResponseBase()
                            {
                                Status = "success",
                                Message = "需要验证码验证",
                                Meta = new ApiResponseMetadata()
                                {
                                    Captcha = new CaptchaMetadata()
                                    {
                                        Enabled = true
                                    }
                                }
                            });
                            return;
                        }
                    }
                    else
                    {
                        resCount = resCount + 1;
                        _memoryCache.Set(requestTimesKey, resCount, TimeSpan.FromMinutes(15));
                    }
                }
                else
                {
                    _memoryCache.Set(requestTimesKey, 1, TimeSpan.FromMinutes(15));
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ResponseBase()
                {
                    Status = "fail",
                    Message = "IP地址不明确",
                });
                return;
            }
            await next();

        }
    }
}
