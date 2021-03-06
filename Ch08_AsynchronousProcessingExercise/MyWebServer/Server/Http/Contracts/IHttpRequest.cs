﻿namespace MyWebServer.Server.Http.Contracts
{
    using System.Collections.Generic;
    using Enums;

    public interface IHttpRequest
    {
        IDictionary<string,string> FormData { get; }

        IHttpHeaderCollection Headers { get; }
        IHttpCookieCollection Cookies { get; }

        string Path { get; }

        IDictionary<string,string> QueryParameters { get; }

        HttpRequestMethod Method { get; }

        string Url { get; }

        IDictionary<string,string> UrlParameters { get; }

        void AddUrlParameter(string key, string value);

        IHttpSession Session { get; set; }

    }
}