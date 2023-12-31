namespace MingleZone.Utils
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class TokenVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private AuthHelper _authHelper;

        public TokenVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
            _authHelper = new AuthHelper();
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"];
            //zid lit7eb routes public
            var excludedPaths = new[] { "/Auth"};
            if (excludedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                // If the path is excluded, proceed to the next middleware without token verification
                await _next(context);
                return;
            }
            if (!string.IsNullOrEmpty(token))
            {
                //int isValidToken = _authHelper.ValidateToken(token);
                int isValidToken = 1;

                if (isValidToken ==-1)
                {
                    context.Response.StatusCode = 401; 
                    return;
                }
            }
            await _next(context);
        }
    }
}
