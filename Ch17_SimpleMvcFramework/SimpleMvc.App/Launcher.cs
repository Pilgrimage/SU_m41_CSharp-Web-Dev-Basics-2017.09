namespace SimpleMvc.App
{
    using SimpleMvc.Data;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            InitializeDatabase();

            WebServer server = new WebServer(1337, new ControllerRouter());

            MvcEngine.Run(server);
        }

        private static void InitializeDatabase()
        {
            using (var db = new NotesDbContext())
            {
                db.Database.EnsureCreated();
            }
        }

    }
}
