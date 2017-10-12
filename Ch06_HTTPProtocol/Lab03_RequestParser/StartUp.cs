namespace Lab03_RequestParser
{
    using System;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main()
        {
            Dictionary<string, HashSet<string>> validUrls = new Dictionary<string, HashSet<string>>();

            while (true)
            {
                string inputLine = Console.ReadLine();

                if (inputLine == "END")
                {
                    break;
                }

                string[] urlParts = inputLine.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string path = $"/{urlParts[0]}";
                string method = urlParts[1].ToUpper();

                if (!validUrls.ContainsKey(path))
                {
                    validUrls.Add(path, new HashSet<string> { method });
                }
                else
                {
                    validUrls[path].Add(method);
                }
            }

            string request = Console.ReadLine();
            string[] requestParts = request.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string requestMethod = requestParts[0];
            string requestUrl = requestParts[1];
            string requestProtocol = requestParts[2];

            string responseBody;
            int responseCode;

            if (validUrls.ContainsKey(requestUrl) &&
                validUrls[requestUrl].Contains(requestMethod.ToUpper()))
            {
                responseBody = "OK";
                responseCode = 200;
            }
            else
            {
                responseBody = "NotFound";
                responseCode = 404;
            }

            Console.WriteLine($"{requestProtocol} {responseCode} {responseBody}");
            Console.WriteLine($"Content-Length: {responseBody.Length}");
            Console.WriteLine("Content-Type: text/plain");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(responseBody);
        }
    }
}
