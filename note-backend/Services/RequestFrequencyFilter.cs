using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;

namespace note_backend.Services
{
    public class RequestFrequencyFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache _memoryCache;

        public RequestFrequencyFilter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string? ipaddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipaddress == null)
            {
                context.Result = new BadRequestObjectResult(
                new
                {
                    message = "IP address not found.",
                });
                return;
            }
            else
            {
                var cacheKey = $"TimeOffset_" + ipaddress;

                var nowTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _memoryCache.TryGetValue<long>(cacheKey, out long cacheValue);
                if (nowTime - cacheValue < 1)
                {
                    context.Result = new BadRequestObjectResult(
                 new
                 {
                     message = "IP address not found.",
                 });
                    return;
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
