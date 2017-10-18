namespace MyWebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Enums;
    using Contracts;
    using Server.Handlers;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            // get anonymousPaths from the application
            this.AnonymousPaths = new List<string>(appRouteConfig.AnonymousPaths);

            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            // Extract all request Methods in HttpRequestMethod
            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (HttpRequestMethod method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeRouteConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;

        public ICollection<string> AnonymousPaths { get; private set; }


        private void InitializeRouteConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                HttpRequestMethod requestMethod = registeredRoute.Key;
                var routesWithHandlers = registeredRoute.Value;

                foreach (var routeWithHandler in routesWithHandlers)
                {
                    string route = routeWithHandler.Key;
                    RequestHandler handler = routeWithHandler.Value;

                    List<string> parameters = new List<string>();

                    string parsedRouteRegex = this.ParseRoute(route, parameters);

                    RoutingContext routingContext = new RoutingContext(handler, parameters);

                    this.routes[requestMethod].Add(parsedRouteRegex, routingContext);
                }
            }
        }

        // Return valide regex
        private string ParseRoute(string route, List<string> parameters)
        {
            //  /user/{(?<name>[a-z]+)}   ->   ^/users/(?<name>[a-z]+)$  -  name

            if (route == "/")
            {
                return "^/$";
            }

            StringBuilder result = new StringBuilder();

            result.Append("^/");

            string[] tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(tokens, parameters, result);

            return result.ToString();
        }

        private void ParseTokens(string[] tokens, List<string> parameters, StringBuilder result)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                // behind of last token we put "$", otherwise "/"
                string end = (i == tokens.Length - 1) ? "$" : "/";
                string currentToken = tokens[i];

                if (!currentToken.StartsWith('{') && !currentToken.EndsWith('}'))
                {
                    result.Append($"{currentToken}{end}");
                    continue;
                }

                Regex parameterRegex = new Regex("<\\w+>");
                Match parameterMatch = parameterRegex.Match(currentToken);

                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in \"{currentToken}\" is not valid.");
                }

                string match = parameterMatch.Value;
                // <name>   ->   name
                string parameter = match.Substring(1, match.Length - 2);

                parameters.Add(parameter);

                //  { (?< name >[a - z] +)}   ->   (?<name>[a-z]+)/  or (?<name>[a-z]+){end}
                string currentTokenWithoutCurlyBrackets = currentToken.Substring(1, currentToken.Length - 2);

                result.Append($"{currentTokenWithoutCurlyBrackets}{end}");
            }
        }

    }
}