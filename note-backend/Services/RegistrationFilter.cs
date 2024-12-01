using ApiResponseCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

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
                var cachekey = $"OnlineNote_{ipAddress}";
                if (_memoryCache.TryGetValue(cachekey, out int resCount))
                {
                    _memoryCache.Set(cachekey, resCount, TimeSpan.FromMinutes(15));
                    if (resCount > 6)
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
                    else
                    {
                        resCount = resCount + 1;
                    }
                }
                else
                {
                    _memoryCache.Set(cachekey, 1, TimeSpan.FromMinutes(15));
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
