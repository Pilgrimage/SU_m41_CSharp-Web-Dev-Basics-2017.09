namespace MyWebServer.Server
{
    using System;
    using System.Collections.Generic;

    public class Model
    {
        private readonly IDictionary<string, object> objects;

        public Model()
        {
            this.objects=new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get => this.objects[key];
            set => this.objects[key] = value;
        }

    public void Add(string key, object value)
        {
            try
            {
                this.objects.Add(key, value);
                //this.objects[key] = value;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("In Model.cs we cannot add object to Dictionary!");
            }
        }

    }
}