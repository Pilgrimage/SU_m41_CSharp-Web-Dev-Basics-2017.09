namespace MyWebServer.Server.Http
{
    using System;
    using Contracts;
    using Server.Common;
    using System.Collections.Generic;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> values;


        public HttpSession(string id)
        {
            CommonValidator.ThrowIfNullOrEmpty(id, nameof(id));

            this.Id = id;
            this.values = new Dictionary<string, object>();
        }

        public string Id { get; private set; }


        public void Add(string key, object value)
        {
            CommonValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CommonValidator.ThrowIfNull(value, nameof(value));

            this.values[key] = value;
        }

        public void Clear() => this.values.Clear();

        public bool Contains(string key)
        {
            return this.values.ContainsKey(key);
        }


        public object Get(string key)
        {
            CommonValidator.ThrowIfNull(key, nameof(key));

            if (!this.values.ContainsKey(key))
            {
                return null;
            }

            return this.values[key];
        }

        public T Get<T>(string key) => (T)this.Get(key);
    }
}