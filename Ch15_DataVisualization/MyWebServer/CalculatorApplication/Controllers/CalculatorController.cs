namespace MyWebServer.CalculatorApplication.Controllers
{
    using System.Globalization;
    using Server.Http.Contracts;

    public class CalculatorController : BaseController
    {

        public IHttpResponse Calculate()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"\calculator");
        }

        internal IHttpResponse Calculate(IHttpRequest request)
        {
            const string firstNum = "firstNum";
            const string secondNum = "secondNum";
            const string operation = "operation";

            var formData = request.FormData;

            this.ViewData["result"] = string.Empty;

            if (formData.ContainsKey(firstNum) && formData.ContainsKey(secondNum) && formData.ContainsKey(operation))
            {
                double numOne = double.Parse(formData[firstNum]);
                double numTwo = double.Parse(formData[secondNum]);
                string operationDecoded = formData[operation].Trim();

                switch (operationDecoded)
                {
                    case "+":
                        this.ViewData["result"] = (numOne + numTwo).ToString(CultureInfo.InvariantCulture);
                        break;
                    case "-":
                        this.ViewData["result"] = (numOne - numTwo).ToString(CultureInfo.InvariantCulture);
                        break;
                    case "*":
                        this.ViewData["result"] = (numOne * numTwo).ToString(CultureInfo.InvariantCulture);
                        break;
                    case "/":
                        this.ViewData["result"] = (numOne / numTwo).ToString(CultureInfo.InvariantCulture);
                        break;
                    default:
                        this.ViewData["result"] = "Invalid Sign";
                        break;
                }
            }

            if (formData.Count == 0)
            {
                this.ViewData["showResult"] = "none";
            }
            else
            {
                this.ViewData["showResult"] = "block";
            }

            return this.FileViewResponse(@"/calculator");
        }

    }
}