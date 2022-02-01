using FBC.Basit.Cari.DBModels;
using System.Collections.Concurrent;
using System.Text;

namespace FBC.Basit.Cari.Auth
{
    internal class FBCSessionHolder : IDisposable
    {
        //private const string COOKIE_ERROR = "Bu uygulama çerezleri (cookies) kullanmaktadır. Eğer gizli (private) modda iseniz lütfen normal moda dönünüz, eğer çerezler kapalı ise lütfen açınız. Veya sayfayı yenileyerek tekrar deneyiniz.";
        private ConcurrentDictionary<string, object?> active_circuits;

        private bool IsCircuitlessTimeout()
        {
            return
                !HasCircuits &&
                ((DateTime.Now - LastActionDate).TotalSeconds > FBCSessionManager.CIRCUITLESS_USER_DATA_TIMEOUT_SECONDS);
        }

        private void CheckCircuitlessTimeout()
        {
            if (IsCircuitlessTimeout())
            {
                _user = null;
                UpdateSessionState();
            }
        }
        public bool HasCircuits => active_circuits.Any();

        public void AddCircuit(string circuitId)
        {
            CheckCircuitlessTimeout();
            active_circuits[circuitId] = null;
            LastActionDate = DateTime.Now;//Don't use HadAction() method
        }

        public void RemoveCircuit(string circuitId)
        {
            if (active_circuits.ContainsKey(circuitId))
            {
                active_circuits.TryRemove(circuitId, out object? o);
                //Give a time after circuit closed
                LastActionDate = DateTime.Now;//Don't use HadAction() method
            }
            //CheckIdleTimeout(); //This became unnecessary because of updating the LastActionDate;
        }

        public string? SessionId { get; }
        public DateTime Created { get; }
        public DateTime LastActionDate { get; private set; }
        public event EventHandler<SysUser?> OnSessionStateChanged;
        private SysUser? _user;
        public SysUser? getUser()
        {
            return !IsCircuitlessTimeout() ? this._user : null;
        }

        private void UpdateSessionState()
        {
            OnSessionStateChanged?.Invoke(this, _user);
        }

        public void setUser(SysUser user)
        {
            _user = user;
            UpdateSessionState();
        }
        public void HadAction()
        {
            //Check last action before 
            CheckCircuitlessTimeout();
            LastActionDate = DateTime.Now;
        }

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
                            string? ip = context.Connection?.RemoteIpAddress?.ToString();
                            if (!string.IsNullOrEmpty(ip))
                            {
                                id += "-" + ip;
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
            this._user = null;
            UpdateSessionState();
        }

        internal KeyValuePair<string, Dictionary<string, string?>> GetSummary()
        {
            KeyValuePair<string, Dictionary<string, string?>> r = new KeyValuePair<string, Dictionary<string, string?>>(this.SessionId ?? "NO_SESSION_ID", new Dictionary<string, string?>());
            r.Value["Session Id"] = this.SessionId;
            r.Value["Created"] = Created + "";
            r.Value["LastActionDate"] = LastActionDate + "";
            r.Value["UserName"] = _user == null ? null : _user.SysUserName;
            int idx = 0;
            active_circuits.Keys.ToList().ForEach(x =>
            {
                r.Value["Circuit " + idx++] = x;
            });

            r.Value[""] = "";

            return r;
        }
        private KeyValuePair<string, string> kv(string key, string? value)
        {
            return KeyValuePair.Create(key, value);
        }
        internal FBCTreeNode<KeyValuePair<string, string>> GetSummaryAsNode()
        {
            FBCTreeNode<KeyValuePair<string, string>> r =
                new FBCTreeNode<KeyValuePair<string, string>>(kv("SessionId", this.SessionId ?? "NO_SESSION_ID"));
            r.CreateChild(kv("Created", Created + ""));
            r.CreateChild(kv("LastActionDate", LastActionDate + ""));
            r.CreateChild(kv("UserName", _user == null ? null : _user.SysUserName));
            var cr = r.CreateChild(kv("Circuits", null));
            int idx = 0;
            active_circuits.Keys.ToList().ForEach(x =>
            {
                cr.CreateChild(kv("Circuit " + idx++, x));
            });
            return r;
        }

        public FBCSessionHolder(string sessionId)
        {
            active_circuits = new ConcurrentDictionary<string, object?>();
            Created = LastActionDate = DateTime.Now;
            this.SessionId = sessionId;
        }
    }
}