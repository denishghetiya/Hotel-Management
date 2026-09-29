using Microsoft.AspNetCore.Builder;

namespace HotelManagement.Middleware
{
    public static class NoCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseNoCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NoCacheMiddleware>();
        }
    }
}
