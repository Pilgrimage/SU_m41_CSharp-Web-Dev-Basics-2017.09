namespace MyWebServer.Server.Handlers.Contracts
{
    using MyWebServer.Server.Http.Contracts;

    public interface IRequestHandler
    {
        IHttpResponse Handle(IHttpContext httpContext);
    }
}