namespace MyWebServer.CalculatorApplication.Infrastructire
{
    using System.Collections.Generic;
    using System.Linq;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.IO;
    using CalculatorApplication.Views;

    public class Controller
    {
        public const string DefaultPath = @"CalculatorApplication\Resources\{0}.html";

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

            if (this.ViewData!=null && this.ViewData.Any())
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