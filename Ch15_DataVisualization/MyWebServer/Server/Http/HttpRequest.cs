namespace MyWebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Contracts;
    using Enums;
    using Common;
    using Exceptions;

    public class HttpRequest : IHttpRequest
    {
        private readonly string requestText;

        public HttpRequest(string requestText)
        {
            CommonValidator.ThrowIfNullOrEmpty(requestText, nameof(requestText));

            this.requestText = requestText;

            this.FormData = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestText);
        }

        
        // Form Data – data passed in by a POST-request form
        public IDictionary<string, string> FormData { get; private set; }

        // Header Collection – for collecting all headers
        public IHttpHeaderCollection Headers { get; private set; }

        // Cookie Collection – for collecting all cookies
        public IHttpCookieCollection Cookies { get; private set; }
        
        // Path – the part of the URL after the “/” sign
        public string Path { get; private set; }

        // Request Method – GET, POST, etc.
        public HttpRequestMethod Method { get; private set; }

        // URL – the URL the user enters
        public string Url { get; private set; }

        // URL Parameters – host, path, etc.
        public IDictionary<string, string> UrlParameters { get; private set; }

        public IHttpSession Session { get; set; }

        
        // METHODS !

        private void ParseRequest(string requestText)
        {
            string[] requestLines = requestText.Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            string[] requestLine = requestLines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();
            }


            this.Method = this.ParseRequestMethod(requestLine.First());
            this.Url = requestLine[1];
            this.Path = this.ParsePath(this.Url);

            this.ParseHeaders(requestLines);
            this.ParseCookies();
            this.ParseParameters();
            this.ParseFormData(requestLines.Last());

            this.SetSession();
        }

        public void AddUrlParameter(string key, string value)
        {
            CommonValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CommonValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        private HttpRequestMethod ParseRequestMethod(string method)
        {
            HttpRequestMethod parsedMethod;

            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return parsedMethod;
        }
        
        private string ParsePath(string url)
        {
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        
        private void ParseHeaders(string[] requestLines)
        {
            // Get the index of empty line
            int emptyLineAfterHeadersIndex = Array.IndexOf(requestLines, string.Empty);

            // Cycling all header lines
            for (int i = 1; i < emptyLineAfterHeadersIndex; i++)
            {
                string currentLine = requestLines[i];       // Host: localhost:8230
                string[] headerParts = currentLine.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                string headerKey = headerParts[0];
                string headerValue = headerParts[1].Trim();

                HttpHeader header = new HttpHeader(headerKey, headerValue);

                this.Headers.Add(header);
            }

            // Check for header "Host"
            if (!this.Headers.ContainsKey(HttpHeader.Host))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains('?'))
            {
                // There is no query string
                return;
            }

            // eliminate substring before query
            string query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).Last();
            // Eliminate fragment
            // string query2 = query.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries).First();

            this.ParseQuery(query, this.UrlParameters);
        }
        
        private void ParseQuery(string queryString, IDictionary<string, string> dict)
        {
            if (!queryString.Contains('='))
            {
                // There is no valid query (kvp)
                return;
            }

            string[] queryPairs = queryString.Split(new[] { '&' });

            foreach (string queryPair in queryPairs)
            {
                string[] queryKvp = queryPair.Split(new[] { '=' });

                if (queryKvp.Length != 2)
                {
                    // There is no valid query
                    return;
                }

                string queryKey = WebUtility.UrlDecode(queryKvp[0]);
                string queryValue = WebUtility.UrlDecode(queryKvp[1]);

                dict.Add(queryKey, queryValue);
            }
        }
        
        private void ParseFormData(string formDataLine)
        {
            if (this.Method == HttpRequestMethod.Get)
            {
                return;
            }

            this.ParseQuery(formDataLine, this.FormData);
        }
        
        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                ICollection<HttpHeader> allCookies = this.Headers.GetHeader(HttpHeader.Cookie);

                foreach (HttpHeader cookie in allCookies)
                {
                    if (!cookie.Value.Contains('='))
                    {
                        return;
                    }

                    List<string> cookieParts = cookie
                        .Value
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (!cookieParts.Any())
                    {
                        continue;
                    }

                    foreach (string cookiePart in cookieParts)
                    {
                        string[] cookieKeyValuePair = cookiePart.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValuePair.Length == 2)
                        {
                            string key = cookieKeyValuePair[0].Trim();
                            string value = cookieKeyValuePair[1].Trim();

                            this.Cookies.Add(new HttpCookie(key, value, false));
                        }
                    }
                }
            }
        }
        
        private void SetSession()
        {
            // Cookie: SID=seflashfasklfj
            if (this.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                HttpCookie cookie = this.Cookies.GetCookie(SessionStore.SessionCookieKey);
                string sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }


        public override string ToString() => this.requestText;
    }
}