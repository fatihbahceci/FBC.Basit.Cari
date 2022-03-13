using FBC.Basit.Cari.DBModels;

namespace FBC.Basit.Cari.Auth
{
//    public class FBCSessionedAuth
//    {
//        private FBCSessionHolder sessionHolder;
//        public event EventHandler OnAuthStateChanged;
//        public FBCSessionedAuth(IHttpContextAccessor context)
//        {
//            this.sessionHolder = FBCSessionManager.GetOrCreateSessionHolder(context.HttpContext);
//            if (sessionHolder != null)
//            {
//                sessionHolder.OnSessionStateChanged += (s, e) => { UpdateState(); };
//            }
//        }

//        public void UpdateState()
//        {
//            OnAuthStateChanged?.Invoke(this, EventArgs.Empty);
//        }

//        public Task<SysUser?> GetCurrentUserAsync()
//        {
//            return Task.FromResult(sessionHolder?.getUser());
//        }

//        public SysUser? CurrentUser { get => sessionHolder?.getUser(); }

//        public bool CanEditData => CurrentUser?.IsCanEditData == true;
//        public bool IsAdmin => CurrentUser?.IsAdmin == true;

//        public int? RelatedCariId => CurrentUser?.CariKartId;

//        public bool Login(string userName, string password)
//        {

//            if (sessionHolder != null)
//            {
//                using (var db = new DB())
//                {
//                    var user = db.Users.Where(x => x.SysUserName == userName && SysUser.ToMD5(password) == x.SysUserPassword).FirstOrDefault();
//                    if (user != null)
//                    {
//                        sessionHolder.setUser(user);
//                        //UpdateState();
//                        return true;
//                    }
//                    else
//                    {
//                        //UpdateState();
//                        return false;
//                    }
//                }
//            }
//            //UpdateState();
//            throw new InvalidOperationException("SESSION_NOT_FOUND_OR_CREATED");
//        }

//        public void Logout()
//        {

//            if (sessionHolder != null)
//            {
//                sessionHolder.setUser(null);
//            }
//            //UpdateState();
//        }

//        public bool LoggedIn { get => CurrentUser != null; }
//    }

   
}

/*
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-6.0
    /// </summary>
    public class FBCSessionedAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public static readonly object HttpContextItemsMiddlewareKey = new();

        public FBCSessionedAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, FBCSessionedAuth auth, NavigationManager NM)
        {

            if (
                httpContext.Request.Path.ToString().ToLower().StartsWith("/login") ||
                httpContext.Request.Path.ToString().ToLower().StartsWith("/_blazor") 

                || auth.LoggedIn)
            {

                await _next(httpContext);
            }
            else
            {
                httpContext.Response.Redirect("/login");

//                //NM.NavigateTo("/login", true);
//                //httpContext.Response.Redirect("/login");
//                //await _next(httpContext);
//                const string content = @"
//<meta http-equiv=""refresh"" content=""0;url=/login?r=meta"" />


                //<!--
                //<meta http-equiv=""refresh"" content=""0; url=/login?r=meta"" />
                //<script>
                //// Simulate a mouse click:
                //window.location.href = ""/login?r=href"";

                //// Simulate an HTTP redirect:
                //window.location.replace(""/mogin?r=replace"");
                //</script>
                //-->
                //";
                //                var headers = new Dictionary<string, string>() {
                //                    { "Content-Type", "text/html"},
                //                    {"Content-Length",""+ content.Length}
                //                };
                //                httpContext.Response.StatusCode = 200;
                //                httpContext.Response.Headers.Clear();
                //                httpContext.Response.Headers.Add("Content-Type", "text/html");
                //                httpContext.Response.Headers.Add("Content-Length", "" + content.Length);
                //                httpContext.Response.WriteAsync(
                //                    //"HTTP/1.0 200 OK\r\n" +
                //                    //string.Join("\r\n", headers.Select(x => string.Format("{0}: {1}", x.Key, x.Value)))+
                //                    //"\r\n\r\n" + content);
                //                    content);

            }
        }
    }

    public static class HFBCSessionedAuthMiddlewareExtension
    {
        public static IApplicationBuilder
            UseFBCSessionedAuthMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<FBCSessionedAuthMiddleware>();
        }
    } 
 
 */