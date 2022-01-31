using System.Collections.Concurrent;

namespace FBC.Basit.Cari.Auth
{
    internal class FBCSessionManager
    {
        private static readonly ConcurrentDictionary<string, FBCSessionHolder> sessions = new ConcurrentDictionary<string, FBCSessionHolder>();

        internal FBCSessionHolder GetOrCreateSessionHolder(HttpContext? context)
        {
            string? sessionId = FBCSessionHolder.GenerateSessionIdFromContext(context);
            if (!string.IsNullOrEmpty(sessionId))
            {
                if (!sessions.ContainsKey(sessionId))
                {
                    var sessionHolder = new FBCSessionHolder(sessionId);
                    sessions[sessionId] = sessionHolder;
                    return sessionHolder;
                }
                else
                {
                    if (sessions.TryGetValue(sessionId, out FBCSessionHolder? sessionHolder))
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