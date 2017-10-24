namespace SimpleMvc.Framework.Controllers
{
    using System.Runtime.CompilerServices;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Contracts.Generic;
    using SimpleMvc.Framework.Helpers;
    using SimpleMvc.Framework.ViewEngine;
    using SimpleMvc.Framework.ViewEngine.Generic;

    public abstract class Controller
    {
        protected IActionResult View([CallerMemberName] string caller="")
        {
            string controllerName = ControllerHelpers.GetControllerName(this);

            string viewFullQualifiedName = ControllerHelpers.GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult(viewFullQualifiedName);
        }


        protected IActionResult View(string controller, string action)
        {
            string viewFullQualifiedName = ControllerHelpers.GetViewFullQualifiedName(controller, action);

            return new ActionResult(viewFullQualifiedName);
        }


        protected IActionResult<TModel> View<TModel>(TModel model, [CallerMemberName] string caller = "")
        {
            string controllerName = ControllerHelpers.GetControllerName(this);

            string viewFullQualifiedName = ControllerHelpers.GetViewFullQualifiedName(controllerName, caller);

            return new ActionResult<TModel>(viewFullQualifiedName, model);
        }


        protected IActionResult<TModel> View<TModel>(TModel model, string controller, string action)
        {

            string viewFullQualifiedName = ControllerHelpers.GetViewFullQualifiedName(controller, action);

            return new ActionResult<TModel>(viewFullQualifiedName, model);

        }

    }
}