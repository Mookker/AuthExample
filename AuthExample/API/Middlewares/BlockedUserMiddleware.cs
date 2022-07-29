using AuthExample.Domain.Exceptions;
using AuthExample.Domain.Interfaces;
using System.Security.Claims;

namespace AuthExample.API.Middlewares
{
    public class BlockedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUsersRepository usersRepository)
        {
            string token = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                var claims = context.User.Claims.ToList();
                var userLogin = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (userLogin == null)
                    throw new ArgumentException("Invalid token exception");
                var user = await usersRepository.GetByLoginAsync(userLogin.Value);
                if (user == null)
                    throw new UserNotFoundException();

                if (user.IsBlocked == true)
                    throw new UserBlockedException();

            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class BlockedUserMiddlewareExtensions
    {
        public static IApplicationBuilder CheckBLockedUser(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlockedUserMiddleware>();
        }
    }
}
