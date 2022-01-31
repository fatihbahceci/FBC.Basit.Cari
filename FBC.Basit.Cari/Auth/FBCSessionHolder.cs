using FBC.Basit.Cari.DBModels;

namespace FBC.Basit.Cari.Auth
{
    internal class FBCSessionHolder : IDisposable
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

        public FBCSessionHolder(string sessionId)
        {
            Created = LastActionDate = DateTime.Now;
            this.SessionId = sessionId;
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