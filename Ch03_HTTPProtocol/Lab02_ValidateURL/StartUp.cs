namespace Lab02_ValidateURL
{
    using System;
    using System.Net;

    public class StartUp
    {
        public static void Main()
        {
            string urlEncoded = Console.ReadLine();
            string urlDecoded = WebUtility.UrlDecode(urlEncoded);

            try
            {
                Uri urlParsed = new Uri(urlDecoded);
                string protocol = urlParsed.Scheme;
                string host = urlParsed.Host;
                int port = urlParsed.Port;
                string path = urlParsed.AbsolutePath;
                string query = urlParsed.Query;
                string fragment = urlParsed.Fragment;

                if (string.IsNullOrEmpty(protocol) || 
                    string.IsNullOrEmpty(host) || 
                    string.IsNullOrEmpty(path) ||
                    (protocol == "http" && port != 80) ||
                    (protocol == "https" && port != 443))
                {
                    throw new ArgumentException();
                }

                Console.WriteLine($"Protocol: {protocol}");
                Console.WriteLine($"Host: {host}");
                Console.WriteLine($"Port: {port}");
                Console.WriteLine($"Path: {path}");

                if (string.IsNullOrEmpty(query))
                {
                    Console.WriteLine($"Query: {urlParsed.Query.Substring(1, urlParsed.Query.Length - 1)}");
                }

                if (string.IsNullOrEmpty(fragment))
                {
                    Console.WriteLine($"Fragment: {urlParsed.Fragment.Substring(1, urlParsed.Fragment.Length - 1)}");
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Invalid URL");
            }
            
        }
    }
}
