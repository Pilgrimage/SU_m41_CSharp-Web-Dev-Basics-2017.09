namespace MyWebServer.LoginFormApplication.Controllers
{
    using System.Net;
    using Server.Http.Contracts;

    public class LoginFormController : BaseController
    {

        public IHttpResponse Login()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"\login");
        }
        
        public IHttpResponse Login(IHttpRequest request)
        {
            const string usrName = "username";
            const string pswrd = "password";

            var formData = request.FormData;

            this.ViewData["result"] = string.Empty;

            if (formData.ContainsKey(usrName) && formData.ContainsKey(pswrd))
            {
                string username = WebUtility.UrlDecode(formData[usrName].Trim());
                string password = WebUtility.UrlDecode(formData[pswrd].Trim());

                this.ViewData["showResult"] = "block";

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    this.ViewData["result"] = $"Hi {username}, your password is {password}";
                }
            }

            return this.FileViewResponse(@"/login");
        }

    }
}