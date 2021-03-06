﻿namespace MyWebServer.Server.Handlers
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Contracts;
    using Http.Contracts;
    using Common;
    using Enums;
    using Http.Response;
    using Server.Http;
    using Routing.Contracts;
    using System;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            CommonValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.serverRouteConfig = serverRouteConfig;
        }
        

        public IHttpResponse Handle(IHttpContext context)
        {
            try
            {
                // Check if user is authenticated
                //string[] anonymousPaths = new[] { "/login", "/register" };

                //if (context.Request.Path == null || 
                //    (!anonymousPaths.Contains(context.Request.Path) &&
                //     !context.Request.Session.Contains(SessionStore.CurrentUserKey)))
                //{
                //    return new RedirectResponse(anonymousPaths.First());
                //}

                string[] anonymousPaths = new[] { "/login", "/register" };

                if (!anonymousPaths.Contains(context.Request.Path) &&
                    (context.Request.Session == null || !context.Request.Session.Contains(SessionStore.CurrentUserKey)))
                {
                    return new RedirectResponse(anonymousPaths.First());
                }


                HttpRequestMethod requestMethod = context.Request.Method;
                string requestPath = context.Request.Path;
                var registeredRoutes = this.serverRouteConfig.Routes[requestMethod];

                foreach (var registeredRoute in registeredRoutes)
                {
                    // will return   ^/users/(?<name>[a-z]+)$
                    string routePattern = registeredRoute.Key;
                    IRoutingContext routingContext = registeredRoute.Value;

                    Regex routeRegex = new Regex(routePattern);
                    Match match = routeRegex.Match(requestPath);

                    if (!match.Success)
                    {
                        continue;
                    }

                    //  ^/users/(?<name>[a-z]+)$   ->  name
                    var parameters = routingContext.Parameters;

                    // extract value for <name>
                    foreach (string parameter in parameters)
                    {
                        // if we have named group in the regex
                        string parameterValue = match.Groups[parameter].Value;
                        context.Request.AddUrlParameter(parameter, parameterValue);
                    }

                    return routingContext.Handler.Handle(context);
                }
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }

            return new NotFoundResponse();
        }

    }
}