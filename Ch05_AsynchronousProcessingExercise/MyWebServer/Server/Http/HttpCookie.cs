namespace MyWebServer.Server.Http
{
    using System;
    using Server.Common;

    public class HttpCookie
    {
        // expires - count of days
        public HttpCookie(string key, string value, int expires = 3)
        {
            CommonValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CommonValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;

            this.Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = 3)
            : this(key, value, expires)
        {
            this.IsNew = isNew;
        }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; private set; } = true;

        public override string ToString() 
            => $"{this.Key}={this.Value}; Expires={this.Expires.ToLongTimeString()}";

    }
}