using FBC.Basit.Cari.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FBC.Basit.Cari.Auth
{

    internal class SessionManager
    {
        private static readonly ConcurrentDictionary<string, SessionHolder> sessions = new ConcurrentDictionary<string, SessionHolder>();

        internal SessionHolder GetOrCreateSessionHolder(HttpContext? context)
        {
            string? sessionId = SessionHolder.GenerateSessionIdFromContext(context);
            if (!string.IsNullOrEmpty(sessionId))
            {
                if (!sessions.ContainsKey(sessionId))
                {
                    var sessionHolder = new SessionHolder(sessionId);
                    sessions[sessionId] = sessionHolder;
                    return sessionHolder;
                }
                else
                {
                    if (sessions.TryGetValue(sessionId, out SessionHolder? sessionHolder))
                    {
                        if (sessionHolder != null)
                        {
                            return sessionHolder;
                        }
                    }
                }
            }
            //throw new InvalidOperationException($"SESSION_NOT_FOUND_OR_CREATED ({sessionId})");
            return null;
        }
    }



    internal class SessionHolder : IDisposable
    {
        //private const string COOKIE_ERROR = "Bu uygulama çerezleri (cookies) kullanmaktadır. Eğer gizli (private) modda iseniz lütfen normal moda dönünüz, eğer çerezler kapalı ise lütfen açınız. Veya sayfayı yenileyerek tekrar deneyiniz.";

        public string? SessionId { get; }
        public DateTime Created { get; }
        public DateTime LastActionDate { get; private set; }
        public event EventHandler<SysUser?> OnSessionStateChanged;
        public SysUser? User { get; private set; }

        private void UpdateSessionState()
        {
            OnSessionStateChanged?.Invoke(this, User);
        }

        public void setUser(SysUser user)
        {
            User = user;
            UpdateSessionState();
        }
        public void HadAction() => LastActionDate = DateTime.Now;

        public static string? GenerateSessionIdFromContext(HttpContext? context)
        {
            string? id = null;
            if (context != null && context.Request != null && context.Request.Cookies != null && context.Request.Cookies.Any())
            {
                var c = context.Request.Cookies;
                if (c.TryGetValue("fbc-bw-id", out string? fbcid))
                {
                    if (!string.IsNullOrEmpty(fbcid))
                    {

                        id = fbcid;

                        //add ip addr too when only cookie is working
                        try
                        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            String ip = context.Connection.RemoteIpAddress.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                            if (!string.IsNullOrEmpty(ip))
                            {
                                id += ip;
                            }
                            //Console.WriteLine("ip:" + ip);
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        Console.WriteLine($"FBC WARNING: TryGetValue is failed");

                    }
                }
                else
                {
                    Console.WriteLine($"FBC WARNING: fbcid is null or empty");
                }
            }
            else
            {
                if (context == null)
                {
                    Console.WriteLine($"FBC WARNING: Context is null");
                }
                else if (context.Request == null)
                {
                    Console.WriteLine($"FBC WARNING: Request is null");
                }
                else if (context.Request.Cookies == null)
                {
                    Console.WriteLine($"FBC WARNING: Cookies is null");
                }
                else if (!context.Request.Cookies.Any())
                {
                    Console.WriteLine($"FBC WARNING:Cookies is empty");
                }

            }

            return id;
        }

        public void Dispose()
        {
            this.User = null;
            UpdateSessionState();
        }

        public SessionHolder(string sessionId)
        {
            Created = LastActionDate = DateTime.Now;
            this.SessionId = sessionId;
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
    public class SessionedAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static SessionManager mgr;

        #region just for test something
        private static int providerIdcounter = 0;
        private SessionHolder sessionHolder;

        public int Id { get; private set; }
        private void genId()
        {
            if (providerIdcounter < int.MaxValue) providerIdcounter++; else providerIdcounter = 1;
            this.Id = providerIdcounter;
        }
        #endregion
        static SessionedAuthenticationStateProvider()
        {
            mgr = new SessionManager();
        }
        public SessionedAuthenticationStateProvider(IHttpContextAccessor context)
        {
            genId();
#pragma warning disable CS8601 // Possible null reference assignment.

#pragma warning restore CS8601 // Possible null reference assignment.
            this.sessionHolder = mgr.GetOrCreateSessionHolder(context.HttpContext);
            if (sessionHolder != null)
            {
                sessionHolder.OnSessionStateChanged += (s, e) =>
                {
                    UpdateState();
                };
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
            var lUser = sessionHolder?.User;
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

        public static string getSummary()
        {
            StringBuilder sb = new StringBuilder("Summary");
            //foreach (var key in keys)
            //{
            //    sb.AppendLine($"key: {key}");
            //    if (_users.TryGetValue(key, out var userDataHolder))
            //    {
            //        if (userDataHolder != null)
            //        {
            //            foreach (var item in userDataHolder.Providers)
            //            {
            //                sb.AppendLine($"    providerId: {item.Id}");
            //            }
            //        }
            //    }

            //}
            return sb.ToString();
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


//private const long IDLE_TIME_LIMIT_SECONDS = 60 * 5;
//private const bool ENABLE_SESSION_IDLE_TIME_LIMIT = true;
//private static DateTime lastPerodicalIdleChecked = DateTime.Now;

//private static void PerodicalIdleCheck()
//{
//    if ((DateTime.Now - lastPerodicalIdleChecked).TotalSeconds > 60 * 3)
//    {
//        var dead = _users.Where(x =>

//         x.Value == null
//         || (x.Value != null && (DateTime.Now - x.Value.LastActionDate).TotalSeconds > IDLE_TIME_LIMIT_SECONDS)
//        ).Select(x => x.Key).ToList();

//        if (dead.Any())
//        {
//            dead.ForEach(x => _users.TryRemove(x, out UserDataHolder? mahmutHoca));
//        }
//    }
//}
//private UserDataHolder? getHolder()
//{
//    if (!string.IsNullOrEmpty(aiData.SessionId) && _users.TryGetValue(aiData.SessionId, out var userDataHolder))
//    {
//        return userDataHolder;
//    }
//    return null;
//}
//public SysUser? GetLoggedInUser()
//{
//    var userDataHolder = getHolder();
//    if (userDataHolder != null)
//    {
//        if ((DateTime.Now - userDataHolder.LastActionDate).TotalSeconds < IDLE_TIME_LIMIT_SECONDS && userDataHolder.User != null)
//        {
//            userDataHolder.HadAction();
//            return userDataHolder.User;
//        }
//        else
//        {
//            _users.TryRemove(aiData.SessionId, out userDataHolder);
//        }
//    }
//    return null;
//}