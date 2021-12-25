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

    class UserDataHolder
    {
        public DateTime Created { get; }
        public DateTime LastActionDate { get; private set; }
        public SysUser User { get; }

        public UserDataHolder(SysUser user)
        {
            Created = LastActionDate = DateTime.Now;
            User = user;
        }

        public void HadAction() => LastActionDate = DateTime.Now;
    }

    class SessionAIUser
    {
        public string? UserId { get; }
        //public string? UserCreatedDate { get; }
        //public string? SessionId { get; }
        //public string? SessionCreatedDate { get; }
        //public string? SessionUpdatedDate { get; }

        public SessionAIUser(HttpContext? context)
        {
            if (context != null && context.Request != null && context.Request.Cookies != null && context.Request.Cookies.Any())
            {
                var c = context.Request.Cookies;
                if (c.TryGetValue("fbc-bw-id", out string? fbcid))
                {
                    if (!string.IsNullOrEmpty(fbcid))
                    {

                        this.UserId = fbcid;

                        //add ip addr too when only cookie is working
                        try
                        {
                            String ip = context.Connection.RemoteIpAddress.ToString();
                            if (!string.IsNullOrEmpty(ip))
                            {
                                this.UserId += ip;
                            }
                            //Console.WriteLine("ip:" + ip);
                        }
                        catch
                        {

                        }

                    }
                }


                //if (c.TryGetValue("ai_user", out string? user))
                //{
                //    if (!string.IsNullOrEmpty(user))
                //    {
                //        var ur = user.Split("|");
                //        this.UserId = ur[0];
                //        this.UserCreatedDate = ur[1];
                //    }
                //}

                //if (c.TryGetValue("ai_session", out string? session))
                //{
                //    if (!string.IsNullOrEmpty(session))
                //    {
                //        var ur = session.Split("|");
                //        this.SessionId = ur[0];
                //        this.SessionCreatedDate = ur[1];
                //        this.SessionUpdatedDate = ur[2];
                //    }
                //}
                /*

                            Browser 1:
                               context.Request.Cookies-ai_user: pQnR0|2021-11-13T19:53:47.993Z
                               context.Request.Cookies-ai_session: KkJTu|1640361317311|1640363078131.7


                               context.Request.Cookies-ai_user: pQnR0|2021-11-13T19:53:47.993Z
                               context.Request.Cookies-ai_session: KkJTu|1640361317311|1640363487113.1


                               context.Request.Cookies-ai_user: pQnR0|2021-11-13T19:53:47.993Z
                               context.Request.Cookies-ai_session: KkJTu|1640361317311|1640363551106.6


                            Browser 2
                               context.Request.Cookies-ai_user: d3+nd|2021-12-24T16:26:50.903Z
                               context.Request.Cookies-ai_session: eNdod|1640363210911|1640363296116.7

                               context.Request.Cookies-ai_user: d3+nd|2021-12-24T16:26:50.903Z
                               context.Request.Cookies-ai_session: eNdod|1640363210911|1640363327643.7

                               context.Request.Cookies-ai_user: d3+nd|2021-12-24T16:26:50.903Z
                               context.Request.Cookies-ai_session: eNdod|1640363210911|1640363365322.7

                            */
            }
        }


    }

    public class UserSessionManager
    {
        private static ConcurrentDictionary<string, UserDataHolder> _users;
        private static DateTime lastPerodicalIdleChecked = DateTime.Now;
        private const long IDLE_TIME_LIMIT_SECONDS = 60 * 5;
        private HttpContext? context;
        private SessionAIUser aiData;
        private const string COOKIE_ERROR = "Bu uygulama çerezleri (cookies) kullanmaktadır. Eğer gizli (private) modda iseniz lütfen normal moda dönünüz, eğer çerezler kapalı ise lütfen açınız. Veya sayfayı yenileyerek tekrar deneyiniz.";

        static UserSessionManager()
        {
            _users = new ConcurrentDictionary<string, UserDataHolder>();
        }
        private static void PerodicalIdleCheck()
        {
            if ((DateTime.Now - lastPerodicalIdleChecked).TotalSeconds > 60 * 3)
            {
                var dead = _users.Where(x =>

                 x.Value == null
                 || (x.Value != null && (DateTime.Now - x.Value.LastActionDate).TotalSeconds > IDLE_TIME_LIMIT_SECONDS)
                ).Select(x => x.Key).ToList();

                if (dead.Any())
                {
                    dead.ForEach(x => _users.TryRemove(x, out UserDataHolder? mahmutHoca));
                }
            }
        }

        public UserSessionManager(IHttpContextAccessor contextAccessor)
        {
            this.context = contextAccessor.HttpContext;
            aiData = new SessionAIUser(this.context);
        }

        public SysUser? GetLoggedInUser()
        {
            PerodicalIdleCheck();
            if (!string.IsNullOrEmpty(aiData.UserId) && _users.TryGetValue(aiData.UserId, out var userDataHolder))
            {
                if (userDataHolder != null)
                {
                    if ((DateTime.Now - userDataHolder.LastActionDate).TotalSeconds < IDLE_TIME_LIMIT_SECONDS && userDataHolder.User != null)
                    {
                        userDataHolder.HadAction();
                        return userDataHolder.User;
                    }
                    else
                    {
                        _users.TryRemove(aiData.UserId, out userDataHolder);
                    }
                }
            }

            return null;
        }

        public bool Login(string userName, string password)
        {
            if (!string.IsNullOrEmpty(aiData.UserId))
            {
                using (var db = new DB())
                {
                    var user = db.Users.Where(x => x.SysUserName == userName && SysUser.ToMD5(password) == x.SysUserPassword).FirstOrDefault();
                    if (user != null)
                    {
                        _users[aiData.UserId] = new UserDataHolder(user);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            throw new Exception(COOKIE_ERROR);
        }

        public void Logout()
        {

            if (!string.IsNullOrEmpty(aiData.UserId))
            {
                _users.TryRemove(aiData.UserId, out var userDataHolder);
            }
            else
            {
                throw new Exception(COOKIE_ERROR);

            }

        }

    }




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
        private HttpContext? context;
        private UserSessionManager userMgr;
        public CustomAuthStateProvider(IHttpContextAccessor context, UserSessionManager userMgr)
        {
            this.context = context.HttpContext;
            this.userMgr = userMgr;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var lUser = userMgr.GetLoggedInUser();
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
                var identity = new ClaimsIdentity(claims, "Database uleyn");
                var user = new ClaimsPrincipal(identity);

                return Task.FromResult(new AuthenticationState(user));
            }
            else
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }





        }
    }


    /*
     
 var kelle = new Dictionary<string, string>();
            kelle["context.TraceIdentifier"] = context.TraceIdentifier;
            //kelle[""] = context.Request.Cookies
            kelle["context.Connection.Id"] = context.Connection.Id;
            kelle["context.Connection.RemoteIpAddress"] = context.Connection.RemoteIpAddress?.ToString();
            kelle["context.Connection.RemotePort"] = context.Connection.RemotePort + "";
            kelle["context.Connection.LocalIpAddress"] = context.Connection.LocalIpAddress?.ToString();
            kelle["context.Connection.LocalPort"] = context.Connection.LocalPort + "";
            foreach (var item in context.Items)
            {
                kelle["context.Items-" + item.Key?.ToString()] = item.Value?.ToString();
            }

            foreach (var item in context.Request.Cookies)
            {
                kelle["context.Request.Cookies-" + item.Key?.ToString()] = item.Value?.ToString();
            }



            //if (context.Session != null)
            //{
            //    kelle["context.Session"] = context.Session?.Id;
            //    foreach (var item in context.Session.Keys)
            //    {
            //        try
            //        {

            //            kelle["context.Session.Keys-" + item] = context.Session.GetString(item);
            //        }
            //        catch (Exception exc)
            //        {

            //            kelle["context.Session.Keys-" + item] = "Error: " + exc.Message;
            //        }

            //    }
            //}

            //var claims = kelle.Select(item => new Claim(ClaimTypes.Role, item.Key + "=" + item.Value)).ToList();
            var claims = kelle.Select(item => new Claim(item.Key, item.Value)).ToList();     

            claims.Add(new Claim(ClaimTypes.Name, "Mok"));
            claims.Add(new Claim(ClaimTypes.Surname, "Kafa"));
            claims.Add(new Claim(ClaimTypes.Role, "A"));
            var identity = new ClaimsIdentity(claims, "Fake authentication type");
     */

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


}
