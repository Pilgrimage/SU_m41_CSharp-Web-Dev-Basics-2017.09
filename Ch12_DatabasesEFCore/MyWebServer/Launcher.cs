namespace MyWebServer
{
    using System.Net.Mime;
    using Application;
    using ByTheCakeApplication;
    using CalculatorApplication;
    using LoginFormApplication;
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    public class Launcher
    {
        private WebServer webServer; 
        
        public static void Main()
        {
            new Launcher().Run();
        }
        
        public void Run()
        {
            //IApplication app = new MainApplication();
            var app = new ByTheCakeApp();
            //IApplication app = new CalculatorApp();
            //IApplication app = new LoginFormApp();

            app.InitializeDatabase();

            var appRouteConfig = new AppRouteConfig();
            app.Start(appRouteConfig);

            this.webServer = new WebServer(8230, appRouteConfig);

            this.webServer.Run();
        }
    }
}
