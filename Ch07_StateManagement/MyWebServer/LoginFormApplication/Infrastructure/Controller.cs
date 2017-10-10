namespace MyWebServer.LoginFormApplication.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using LoginFormApplication.Views;

    public abstract class Controller
    {
        public const string DefaultPath = @"LoginFormApplication\Resources\{0}.html";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["showResult"] = "none"
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }


        protected IHttpResponse FileViewResponse(string fileName)
        {
            string result = this.ProcessFileHtml(fileName);

            if (this.ViewData != null && this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.OK, new FileView(result));
        }


        private string ProcessFileHtml(string fileName)
        {
            string result = File.ReadAllText(string.Format(DefaultPath, fileName));

            return result;
        }

    }
}