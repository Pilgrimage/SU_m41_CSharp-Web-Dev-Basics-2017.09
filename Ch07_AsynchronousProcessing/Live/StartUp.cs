namespace Live
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static void Main()
        {
            List<int> numbers = Enumerable.Range(1, 20000).ToList();


            //// Thread - Example [00:28:29]

            //Thread threadOdd = new Thread(() => SumOddNumbers(numbers));
            //Thread threadEven = new Thread(() => SumEvenNumbers(numbers));

            //threadOdd.Start();
            //threadEven.Start();

            //Console.WriteLine("What should I do?");
            //while (true)
            //{
            //    string command = Console.ReadLine();
            //    if (command=="exit")
            //    {
            //        Console.WriteLine("Console input is OFF!");
            //        break;
            //    }
            //}

            //threadOdd.Join();
            //threadEven.Join();


            //****************************
            //// Thread Race Conditions - Example [00:34:30]

            // Four threads are created. Then we have BOOM!
            //for (int i = 0; i < 4; i++)
            //{
            //    new Thread(() => RaceCondition(numbers)).Start();               // Fail
            //    new Thread(() => RaceConditionAvoided(numbers)).Start();        // Work
            //}


            //****************************
            // Exception Handling [00:48:00]

            //Thread thread3 = new Thread(() => DoWork());
            //thread3.Start();
            // or
            //new Thread(DoWork).Start();


            //****************************
            // Flip the pictures

            //var currentDirectory = Directory.GetCurrentDirectory();
            //var directoryInfo = new DirectoryInfo(currentDirectory + "\\Images");

            //var files = directoryInfo.GetFiles();

            //string resultDir = "ImagesFlip";

            //if (Directory.Exists(resultDir))
            //{
            //    // Delete folder and files inside
            //    Directory.Delete(resultDir, true);
            //}

            //Directory.CreateDirectory(resultDir);

            //var tasks = new List<Task>();

            //foreach (var file in files)
            //{
            //    Task task = Task.Run(() =>
            //    {
            //        var image = Image.FromFile(file.FullName);
            //        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //        image.Save($"{resultDir}\\Flipped_{file.Name}");

            //        Console.WriteLine($"{file.Name} processed...");
            //    });

            //    tasks.Add(task);
            //}

            //Task.WaitAll(tasks.ToArray());

            //Console.WriteLine("Finished!");


            //****************************

            //var secondTask = Task.Run(() =>
            //{
            //    return 100;
            //});

            //Console.WriteLine(secondTask.Result);                   // Stops until get Result. Sometime doesn't work
            //var result = secondTask.GetAwaiter().GetResult();       // Always works fine


            //****************************
            // async & await

            // Za da ne swyrshi prejdevremenno
            //Task
            //    .Run(async () =>
            //    {
            //        await DownloadFileAsync();
            //    })
            //    .GetAwaiter()
            //    .GetResult();               // Taka shte izchaka vsichko jivo! i niama da ni svyrshi Main metoda!



            //****************************
            // Built-in Async Methods

            //Task.Run(async () =>
            //{
            //    HttpClient httpClient = new HttpClient();
            //    string result = await httpClient.GetStringAsync("http://softuni.bg");

            //    Console.WriteLine(result);
            //})
            //.GetAwaiter()
            //.GetResult();


            //Task.Run(async () =>
            //{
            //    await GetHeaders("http://softuni.bg");
            //})
            //.GetAwaiter()
            //.GetResult();


            //Task.Run(async () =>
            //    {
            //        await PostRequest("http://softuni.bg");
            //    })
            //    .GetAwaiter()
            //    .GetResult();


            //string text = "Hello world!";
            //byte[] textToBytes = Encoding.UTF8.GetBytes(text);           // Convert string to byte[]
            //string bytesTotext = Encoding.UTF8.GetString(textToBytes);   // Convert byte[] to string


            //Task
            //    .Run(async () =>
            //    {
            //        using (var reader = new StreamReader("index.html"))
            //        {
            //            while (true)
            //            {
            //                var line = await reader.ReadLineAsync();

            //                if (line==null)
            //                {
            //                    break;
            //                }

            //                Console.WriteLine(line);
            //            }
            //        }
            //    })
            //    .GetAwaiter()
            //    .GetResult();


            //****************************
            // TCP Networking



        }


        private static void DoWork()
        {
            try
            {
                throw new InvalidOperationException("Boom!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Now catch it !");
            }
        }

        private static async void DoWork2()
        {
            var tasks = new List<Task>();
            var results = new List<bool>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    bool result = await SlowMethod();
                    results.Add(result);
                }));
            }

            //Task.WaitAll(tasks.ToArray());            // This block!
            await Task.WhenAll(tasks.ToArray());        // This don't block !

            Console.WriteLine("Finished!");
        }


        private static async Task DownloadFileAsync()
        {
            var webClient = new WebClient();

            Console.WriteLine("Downloadig...");

            await webClient.DownloadFileTaskAsync(
                "https://softuni.bg/trainings/1736/c-sharp-web-development-basics-september-2017/", "index.html");

            Console.WriteLine("Finished!");
        }

        private static async Task<bool> SlowMethod()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Result!");

            return true;
        }

        private static async Task GetAsync(string uri = "https://httpbin.org/get")
        {
            using (HttpClient client = new HttpClient())
            {
                using (var r = await client.GetAsync(new Uri(uri)))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
            }

        }

        private static async Task GetHeaders(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                var headers = response.Headers;
                foreach (var header in headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);
            }
        }

        private static async Task PostRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var contentToSent = new StringContent("Hello Karakondjul!");
                var response = await httpClient.PostAsync(url, contentToSent);

                if (response.IsSuccessStatusCode)
                {
                    var headers = response.Headers;
                    foreach (var header in headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }

            }
        }

        private static async Task<string> ReadFileAsync(string filename)
        {
            // Read a file without blocking the console interface

            byte[] result;
            Console.WriteLine("Reading...");
            using (FileStream reader = File.Open(filename, FileMode.Open))
            {
                result = new byte[reader.Length];
                await reader.ReadAsync(result, 0, (int)reader.Length);
                Console.WriteLine("File readed");
            }
            return System.Text.Encoding.UTF8.GetString(result);
        }

        private static async Task SaveFile(string filename, byte[] data)
        {
            using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                int length = data.Length;
                await stream.WriteAsync(data, 0, length);
            }
        }

        private static async Task DownloadFileAsync(string url, string fileName)
        {
            // Download a file without blocking the console interface
            Console.WriteLine("Downloading...");
            string FileUrl = "";

            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(FileUrl);
                byte[] responseAsBytes = Encoding.UTF8.GetBytes(response);

                Console.WriteLine("File downloaded");
                Task writeFile = SaveFile(fileName, responseAsBytes);
                writeFile.Wait();

                // Open/Start the downloaded file
                Process.Start(fileName);
            }
        }

        private static async void GetWebsiteHtmlAsync(string websiteUrl)
        {
            // Download the HTML of given website without blocking the console interface

            HttpClient client = new HttpClient();

            var websiteHtml = await client.GetStringAsync(websiteUrl);
            Console.WriteLine("Downloaded html");
        }


        private static async Task ServerListen()
        {
            // Provides methods that listen for and accept incoming connection requests

            int port = 8000;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ipAddress, port);
            server.Start();

            // Establish а connection and read request
        }



        private static void SumOddNumbers(List<int> numbers)
        {
            long result = 0;

            foreach (int number in numbers)
            {
                if (number % 2 != 0)
                {
                    result += number;
                }
                else
                {
                    Thread.Sleep(1);
                }
            }

            Console.WriteLine($"Odd sum = {result}");
        }

        private static void SumEvenNumbers(List<int> numbers)
        {
            long result = 0;

            foreach (int number in numbers)
            {
                if (number % 2 == 0)
                {
                    result += number;
                }
                else
                {
                    Thread.Sleep(1);
                }
            }

            Console.WriteLine($"Even sum = {result}");
        }

        private static void RaceCondition(List<int> numbers)
        {
            while (numbers.Count > 0)
            {
                numbers.RemoveAt(numbers.Count - 1);
            }
        }

        private static void RaceConditionAvoided(List<int> numbers)
        {
            while (numbers.Count > 0)
            {
                lock (numbers)
                {
                    if (numbers.Count == 0)
                    {
                        break;
                    }

                    numbers.RemoveAt(numbers.Count - 1);
                }
            }
        }

    }
}
