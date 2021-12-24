using FBC.Basit.Cari.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;

namespace FBC.Basit.Cari.Auth
{

    //public static class KelleMW
    //{
    //    public static void UseFBC(this IApplicationBuilder app)
    //    {
    //        app.Use(async (context, next) =>
    //        {
    //            var cultureQuery = context.Request.Query["culture"];
    //            // Call the next delegate/middleware in the pipeline
    //            await next();
    //        });
    //    }
    //}


    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#authenticationstateprovider-service
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#implement-a-custom-authenticationstateprovider
    /// https://www.indie-dev.at/2020/04/06/custom-authentication-with-asp-net-core-3-1-blazor-server-side/
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0#session-state
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-6.0&pivots=server#where-to-persist-state
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private HttpContext context;
        private static ConcurrentDictionary<string, SysUser> map = new ConcurrentDictionary<string, SysUser>();
        public const string TOKEN_KEY = "sdjasjkdhsajkdhsad";
        public CustomAuthStateProvider(HttpContext context)
        {
            this.context = context;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string sabri = "";
            //var sabri = context.GetAsync<string>(TOKEN_KEY).Result.Value;
            context.User
            
            if (!string.IsNullOrEmpty(sabri))
            {

                var identity = new ClaimsIdentity(new[]
                   {
                    new Claim(ClaimTypes.Name, sabri), //It will ger user infos from our custom database. (No MS's Auth Database)
                    new Claim(ClaimTypes.Role, "A")
                }, "Fake authentication type");

                var user = new ClaimsPrincipal(identity);

                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {

                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            }




        }
    }

    public class HttpContextItemsMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly object HttpContextItemsMiddlewareKey = new();

        public HttpContextItemsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items[HttpContextItemsMiddlewareKey] = "K-9";

            await _next(httpContext);
        }
    }

    public static class HttpContextItemsMiddlewareExtensions
    {
        public static IApplicationBuilder
            UseHttpContextItemsMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpContextItemsMiddleware>();
        }
    }


}
