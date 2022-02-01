using System.Collections.Concurrent;

namespace FBC.Basit.Cari.Auth
{
    internal static class FBCSessionManager
    {
        public const int CIRCUITLESS_USER_DATA_TIMEOUT_SECONDS = 60;
        public const int CIRCUITLESS_GARBAGE_COLLECTOR_TIMEOUT_SECONDS = 120;
        private static readonly ConcurrentDictionary<string, FBCSessionHolder> sessions;
        private static System.Timers.Timer gc_timer;
        static FBCSessionManager()
        {
            sessions = new ConcurrentDictionary<string, FBCSessionHolder>();
            gc_timer = new System.Timers.Timer(CIRCUITLESS_GARBAGE_COLLECTOR_TIMEOUT_SECONDS * 1000);
            gc_timer.Elapsed += (s, e) =>
            {
                sessions.Values.ToList().ForEach(session =>
                {
                    if (session != null)
                    {
                        if (!session.HasCircuits && (DateTime.Now - session.LastActionDate).TotalSeconds > CIRCUITLESS_GARBAGE_COLLECTOR_TIMEOUT_SECONDS)
                        {
                            sessions.TryRemove(session.SessionId, out var hede);
                        }
                    }
                });
            };
            gc_timer.Start();
        }
        public static Dictionary<string, Dictionary<string, string?>> GetSummary()
        {
            return sessions.Values.ToList().Select(x => x.GetSummary()).ToDictionary(x => x.Key, x => x.Value);
        }
        public static FBCTreeNode<KeyValuePair<string, string>> GetSummaryAsNode()
        {
            var items = sessions.Values.ToList().Select(x => x.GetSummaryAsNode()).ToList();
            var node = new FBCTreeNode<KeyValuePair<string, string>>(new KeyValuePair<string, string>("Summary", null));
            node.AddChilds(items);
            return node;

        }
        internal static FBCSessionHolder GetOrCreateSessionHolder(HttpContext? context)
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
                            sessionHolder.HadAction();
                            return sessionHolder;
                        }
                    }
                }
            }
            //throw new InvalidOperationException($"SESSION_NOT_FOUND_OR_CREATED ({sessionId})");
            return null;
        }

        internal static void CirciuitClosed(string circuitId)
        {
            var holder = FBCSessionManager.GetOrCreateSessionHolder(new HttpContextAccessor().HttpContext);
            if (holder != null)
            {
                holder.RemoveCircuit(circuitId);
            }
        }

        internal static void CirciuitOpened(string circuitId)
        {
            var holder = FBCSessionManager.GetOrCreateSessionHolder(new HttpContextAccessor().HttpContext);
            if (holder != null)
            {
                holder.AddCircuit(circuitId);
            }
        }
    }
}