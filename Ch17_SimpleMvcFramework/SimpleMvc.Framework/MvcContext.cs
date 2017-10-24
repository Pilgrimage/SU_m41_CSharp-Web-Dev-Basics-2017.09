namespace SimpleMvc.Framework
{
    using System.Reflection;

    public class MvcContext
    {
        // lazy Loading SINGLETON pattern implemented

        private static MvcContext instance;

        private MvcContext()
        {
        }

        // with new sintax:
        public static MvcContext Get => instance == null ? (instance = new MvcContext()) : instance;

        // equivalent to :
        //public static MvcContext Get
        //{
        //    get
        //    {
        //        if (instance==null)
        //        {
        //            instance=new MvcContext();
        //        }

        //        return instance;
        //    }
        //}


        // what is the application/project
        public string AssemblyName { get; set; }

        // where are the controllers
        public string ControllerFolder { get; set; } = "Controllers";

        public string ControllerSuffix { get; set; } = "Controller";

        // where are the Views
        public string ViewsFolder { get; set; } = "Views";

        // where are the Models
        public string ModelsFolder { get; set; } = "Models";
    }
}