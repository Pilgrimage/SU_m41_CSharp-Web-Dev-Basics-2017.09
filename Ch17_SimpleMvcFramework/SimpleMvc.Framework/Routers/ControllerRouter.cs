namespace SimpleMvc.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Helpers;
    using WebServer.Contracts;
    using WebServer.Exceptions;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;
    using SimpleMvc.Framework.Contracts;
    using WebServer.Enums;

    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> getParameters;
        private IDictionary<string, string> postParameters;
        private string requestMethod;
        private object controllerInstance;
        private string controllerName;
        private string actionName;
        private object[] methodParameters;


        public IHttpResponse Handle(IHttpRequest request)
        {
            this.getParameters = new Dictionary<string, string>(request.UrlParameters);
            this.postParameters = new Dictionary<string, string>(request.FormData);
            this.requestMethod = request.Method.ToString().ToUpper();

            this.PrepareControllerAndActionNames(request);

            MethodInfo methodInfo = this.GetActionForExecution();
            if (methodInfo == null)
            {
                return new NotFoundResponse();
            }

            //  Prepare MethodParameters
            this.PrepareMethodParameters(methodInfo);

            var actionResult = (IInvocable)methodInfo.Invoke(
                this.GetControllerInstance(),
                this.methodParameters);

            var content = actionResult.Invoke();

            return new ContentResponse(HttpStatusCode.Ok, content);
        }






        private void PrepareControllerAndActionNames(IHttpRequest request)
        {
            string[] pathParts = request.Path.Split(new[] { '/', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length < 2)
            {
                return;
            }

            this.controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllerSuffix}";
            this.actionName = pathParts[1].Capitalize();

        }


        private MethodInfo GetActionForExecution()
        {
            foreach (MethodInfo method in this.GetSuitableMethods())
            {
                var httpMethodAttributes = method
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                // if no attributes & request is GET
                if (!httpMethodAttributes.Any() && this.requestMethod == "GET")
                {
                    return method;
                }

                foreach (HttpMethodAttribute httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(this.requestMethod))
                    {
                        return method;
                    }
                }

            }

            return null;
        }


        private IEnumerable<MethodInfo> GetSuitableMethods()
        {
            var controller = this.GetControllerInstance();

            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(m => m.Name == this.actionName);
        }


        private object GetControllerInstance()
        {
            // commented, because he remembers an instance of another controller
            //if (this.controllerInstance != null)
            //{
            //    return this.controllerInstance;
            //}

            string controllerFullQualifiedName = string.Format(
                "{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllerFolder,
                this.controllerName);

            Type controllerType = Type.GetType(controllerFullQualifiedName);

            if (controllerType == null)
            {
                return null;
            }

            this.controllerInstance = (Controller)Activator.CreateInstance(controllerType);

            return this.controllerInstance;
        }



        private void PrepareMethodParameters(MethodInfo methodInfo)
        {
            // This method will fill Get- and PostParameters Dictionaries

            ParameterInfo[] parameters = methodInfo.GetParameters();

            this.methodParameters = new object[parameters.Length];


            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameter = parameters[i];

                if (parameter.ParameterType.IsPrimitive || parameter.ParameterType == typeof(string))
                {
                    // PRIMITIVE TYPE

                    // take the Name of parameter
                    string getParameterValue = string.Empty;
                    //var getParameterValue = this.getParameters[parameter.Name];
                    if (this.getParameters.ContainsKey(parameter.Name))
                    {
                        getParameterValue = this.getParameters[parameter.Name];
                        // change the Type (from string) to expected type
                        var value = Convert.ChangeType(getParameterValue, parameter.ParameterType);

                        this.methodParameters[i] = value;

                    }
                }
                else
                {
                    // model TYPE

                    // take the tipe of Model
                    var modelType = parameter.ParameterType;
                    // create instance
                    var modelInstance = Activator.CreateInstance(modelType);

                    // take all properties
                    var modelProperties = modelType.GetProperties();

                    foreach (PropertyInfo modelProperty in modelProperties)
                    {
                        if (this.postParameters.ContainsKey(modelProperty.Name))
                        {
                            var postParameterValue = this.postParameters[modelProperty.Name];
                            //var postParameterValue = this.postParameters.FirstOrDefault(x=>x.Key==modelProperty.Name);
                            var value = Convert.ChangeType(postParameterValue, modelProperty.PropertyType);

                            modelProperty.SetValue(modelInstance, value);

                        }
                    }

                    this.methodParameters[i] = Convert.ChangeType(modelInstance, modelType);
                }

            }
        }

    }
}