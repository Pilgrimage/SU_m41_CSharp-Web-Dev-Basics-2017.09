﻿namespace MyWebServer.Server.Http
{
    using System;
    using System.Collections;
    using Contracts;
    using Common;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, ICollection<HttpHeader>> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, ICollection<HttpHeader>>();
        }


        public void Add(HttpHeader header)
        {
            CommonValidator.ThrowIfNull(header, nameof(header));

            string headerKey = header.Key;

            if (!this.headers.ContainsKey(headerKey))
            {
                this.headers[headerKey] = new List<HttpHeader>();
            }

            // if key exist, rewrite it with new value!
            this.headers[headerKey].Add(header);
        }

        public void Add(string key, string value)
        {
            CommonValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CommonValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Add(new HttpHeader(key, value));
        }


        public bool ContainsKey(string key)
        {
            CommonValidator.ThrowIfNull(key, nameof(key));

            return this.headers.ContainsKey(key);
        }

        
        public ICollection<HttpHeader> GetHeader(string key)
        {
            CommonValidator.ThrowIfNull(key, nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key \"{key}\" is not present in the headers collection.");
            }

            return this.headers[key];
        }


        public IEnumerator<ICollection<HttpHeader>> GetEnumerator() => this.headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.headers.Values.GetEnumerator();
        

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var header in this.headers)
            {
                string headerKey = header.Key;

                foreach (HttpHeader headerValue in header.Value)
                {
                    result.AppendLine($"{headerKey}: {headerValue.Value}");
                }
            }

            return result.ToString();
        }

    }
}