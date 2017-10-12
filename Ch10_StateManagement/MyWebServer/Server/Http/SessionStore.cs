namespace MyWebServer.Server.Http
{
    using System.Collections.Concurrent;

    public static class SessionStore
    {
        // We need to keep connection :  Session Id -> HttpSession

        public const string SessionCookieKey = "MY_SID";

        // this structure allow safety asynchronous access
        private static readonly ConcurrentDictionary<string, HttpSession> sessions = new ConcurrentDictionary<string, HttpSession>();


        public static HttpSession Get(string id)
        {
            return sessions.GetOrAdd(id, _ => new HttpSession(id));
        }
        

    }
}