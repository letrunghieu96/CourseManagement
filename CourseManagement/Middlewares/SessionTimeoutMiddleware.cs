using CourseManagement.Helpers;
using CourseManagement.Models;

namespace CourseManagement.Middlewares
{
    /// <summary>
    /// Session timeout middleware
    /// </summary>
    public class SessionTimeoutMiddleware
    {
        /// <summary>Next</summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public SessionTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>Redirect</returns>
        public async Task Invoke(HttpContext context)
        {
            var userLogin = context.Session.GetObjectFromJson<LoginModel>("UserLogin");
            if (userLogin == null)
            {
                var controllerRoute = context.Request.RouteValues["controller"] ?? string.Empty;
                if (controllerRoute.ToString() != "Index")
                {
                    // If the request is AJAX, return 401 error
                    if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("SessionExpired");
                        return;
                    }

                    // Redirect the user to the Login page when the request is not AJAX
                    var returnUrl = $"{context.Request.Path.Value}";
                    if (returnUrl.Length > 1)
                    {
                        context.Response.Redirect($"/?ReturnUrl={returnUrl}");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
