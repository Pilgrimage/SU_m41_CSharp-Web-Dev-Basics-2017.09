namespace MyWebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Server.Common;
    using Server.Http.Contracts;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }


        public void Add(HttpCookie cookie)
        {
            CommonValidator.ThrowIfNull(cookie, nameof(cookie));

            this.cookies[cookie.Key] = cookie;
        }

        public void Add(string key, string value)
        {
            CommonValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CommonValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Add(new HttpCookie(key, value));
        }


        public bool ContainsKey(string key)
        {
            CommonValidator.ThrowIfNull(key, nameof(key));

            return this.cookies.ContainsKey(key);
        }


        public HttpCookie GetCookie(string key)
        {
            CommonValidator.ThrowIfNull(key, nameof(key));

            if (!this.cookies.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key \"{key}\" is not present in the cookies collection.");
            }

            return this.cookies[key];
        }


        public IEnumerator<HttpCookie> GetEnumerator() => this.cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.cookies.Values.GetEnumerator();

    }
}