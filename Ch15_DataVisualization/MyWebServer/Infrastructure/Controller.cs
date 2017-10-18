namespace MyWebServer.Infrastructure
{
    using System.IO;
    using Server.Http.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Server.Enums;
    using Server.Http.Response;
    
    public abstract class Controller
    {
        public const string DefaultPath = @"{0}\Resources\{1}.html";
        public const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["anonymousDisplay"] = "none",      // trigger for navbar for GUEST
                ["authDisplay"] = "flex",           // trigger for navbar for LOGGED
                ["showError"] = "none",             // trigger for Error messages
                ["adminButtons"] = "hidden"              // triger for admin buttons
            };
        }

        
        // directory of current application
        protected abstract string ApplicationDirectory { get; }

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

        protected IHttpResponse RedirectResponse(string route) => new RedirectResponse(route);


        protected void ShowError(string errorMessage)
        {
            this.ViewData["showError"] = "block";
            this.ViewData["error"] = errorMessage;
        }


        // This is part of Custom Validation Attributes in EF Core
        protected bool ValidateModel(object model)
        {
            ValidationContext context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (ValidationResult result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        this.ShowError(result.ErrorMessage);
                        return false;
                    }
                }
            }

            return true;
        }

        // 
        private string ProcessFileHtml(string fileName)
        {
            string layoutHtml = File.ReadAllText(string.Format(DefaultPath, this.ApplicationDirectory, "layout"));

            string fileHtml = File.ReadAllText(string.Format(DefaultPath, this.ApplicationDirectory, fileName));

            string result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }

    }
}