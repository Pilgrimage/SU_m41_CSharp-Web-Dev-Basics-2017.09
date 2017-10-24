namespace SimpleMvc.App.Views.Home
{
    using System.Text;
    using SimpleMvc.Framework.Contracts;

    public class Index : IRenderable
    {
        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h2>Notes App</h2>");
            sb.AppendLine("<a href=\"/users/all\">All Users</a>");
            sb.AppendLine("<a href=\"/users/register\">Register User</a>");

            return sb.ToString();
        }
    }
}
