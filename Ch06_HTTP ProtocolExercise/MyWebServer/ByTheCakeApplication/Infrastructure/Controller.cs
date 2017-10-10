namespace MyWebServer.ByTheCakeApplication.Infrastructure
{
    using System;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ByTheCakeApplication.Views;

    public abstract class Controller
    {
        public const string DefaultPath = @"ByTheCakeApplication\Resources\{0}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block"
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }


        protected IHttpResponse FileViewResponse(string fileName)
        {
            string result = this.ProcessFileHtml(fileName);

            if (this.ViewData.Any())
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
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));

            string fileHtml = File.ReadAllText(string.Format(DefaultPath, fileName));

            string result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}