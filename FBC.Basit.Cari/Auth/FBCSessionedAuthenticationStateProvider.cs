using FBC.Basit.Cari.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FBC.Basit.Cari.Auth
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#authenticationstateprovider-service
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#implement-a-custom-authenticationstateprovider
    /// https://www.indie-dev.at/2020/04/06/custom-authentication-with-asp-net-core-3-1-blazor-server-side/
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0#session-state
    /// https://docs.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-6.0&pivots=server#where-to-persist-state
    /// </summary>
    public class FBCSessionedAuthenticationStateProvider : AuthenticationStateProvider
    {
        private FBCSessionHolder sessionHolder;
        public FBCSessionedAuthenticationStateProvider(IHttpContextAccessor context)
        {
            this.sessionHolder = FBCSessionManager.GetOrCreateSessionHolder(context.HttpContext);
            if (sessionHolder != null)
            {
                sessionHolder.OnSessionStateChanged += (s, e) => { UpdateState(); };
            }
        }

        public void UpdateState()
        {
            // If AuthorizedModel model contains a Jwt token or whatever which you 
            // save in the 
            // local storage, then add it back as a parameter to the AuthenticateUser
            // and place here the logic to save it in the local storage
            // After which call NotifyAuthenticationStateChanged method like this.
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var lUser = sessionHolder?.getUser();
            if (lUser != null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Sid, lUser.SysUserName));
                claims.Add(new Claim(ClaimTypes.Name, lUser.Name));
                claims.Add(new Claim(ClaimTypes.Surname, lUser.Surname));
                if (lUser.IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                }
                if (lUser.IsCanEditData)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "CanEditData"));
                }
                if (lUser.CariKartId != null)
                {
                    claims.Add(new Claim("CariKartId", "" + lUser.CariKartId));

                }
                var identity = new ClaimsIdentity(claims, "Database uleyn");
                var user = new ClaimsPrincipal(identity);

                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
        }

        public bool Login(string userName, string password)
        {

            if (sessionHolder != null)
            {
                using (var db = new DB())
                {
                    var user = db.Users.Where(x => x.SysUserName == userName && SysUser.ToMD5(password) == x.SysUserPassword).FirstOrDefault();
                    if (user != null)
                    {
                        sessionHolder.setUser(user);
                        //UpdateState();
                        return true;
                    }
                    else
                    {
                        //UpdateState();
                        return false;
                    }
                }
            }
            //UpdateState();
            throw new InvalidOperationException("SESSION_NOT_FOUND_OR_CREATED");
        }

        public void Logout()
        {

            if (sessionHolder != null)
            {
                sessionHolder.setUser(null);
            }
            //UpdateState();
        }
    }
}

//public static class TestMW
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

//public class HttpContextItemsMiddleware
//{
//    private readonly RequestDelegate _next;
//    public static readonly object HttpContextItemsMiddlewareKey = new();

//    public HttpContextItemsMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext httpContext)
//    {
//        httpContext.Items[HttpContextItemsMiddlewareKey] = "K-9";

//        await _next(httpContext);
//    }
//}

//public static class HttpContextItemsMiddlewareExtensions
//{
//    public static IApplicationBuilder
//        UseHttpContextItemsMiddleware(this IApplicationBuilder app)
//    {
//        return app.UseMiddleware<HttpContextItemsMiddleware>();
//    }
//}
//}

