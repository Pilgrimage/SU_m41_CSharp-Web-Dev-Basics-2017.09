namespace SimpleMvc.Framework.ViewEngine.Generic
{
    using System;
    using SimpleMvc.Framework.Contracts.Generic;

    public class ActionResult<TModel> : IActionResult<TModel>
    {
        public ActionResult(string viewFullQualifiedName, TModel model)
        {
            //this.Action = (IRenderable<TModel>)Activator.CreateInstance(Type.GetType(viewFullQualifiedName));

            //// prefered
            this.Action = Activator.CreateInstance(Type.GetType(viewFullQualifiedName)) as IRenderable<TModel>;
            if (this.Action == null)
            {
                throw new InvalidOperationException("The given view does not implement IRenderable<TModel>.");
            }

            // set the Model of the Action
            this.Action.Model = model;
        }



        public IRenderable<TModel> Action { get; set; }

        public string Invoke()
        {
            return this.Action.Render();
        }
    }
}