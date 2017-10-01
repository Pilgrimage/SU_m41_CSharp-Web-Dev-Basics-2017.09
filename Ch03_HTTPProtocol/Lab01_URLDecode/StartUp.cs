namespace Lab01_URLDecode
{
    using System;
    using System.Net;

    public class StartUp
    {
        public static void Main()
        {
            string urlEncoded = Console.ReadLine();

            string urlDecoded = WebUtility.UrlDecode(urlEncoded);

            Console.WriteLine(urlDecoded);
        }
    }
}
