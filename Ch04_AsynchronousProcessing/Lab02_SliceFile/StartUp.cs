namespace Lab02_SliceFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class StartUp
    {
        public static List<Task> tasks = new List<Task>();

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Please, make your choice:");
                Console.WriteLine("   1.Slice a file");
                Console.WriteLine("   2.Exit");
                Console.WriteLine("     Any key");
                Console.Write("Enter number of desired command: ");
                string inputLine = Console.ReadLine();
                
                if (inputLine=="1")
                {
                    Console.Write("Enter full name for resource to split: ");
                    string videoPath = Console.ReadLine();
                    Console.Write("Enter destination folder for parts: ");
                    string destination = Console.ReadLine();
                    Console.Write("Enter the number of parts: ");
                    int pieces = int.Parse(Console.ReadLine());

                    // For test purpose
                    //string videoPath = "c:/splitSource/webdev.mp4";
                    //string destination = "c:/splitResult/";
                    //int pieces = 10;

                    SliceAsync(videoPath, destination, pieces);

                    Thread.Sleep(50);
                }
                else if (inputLine=="2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Your Any key is \"{inputLine}\"");
                }
                Console.WriteLine();
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("All files are sliced.");
        }


        private static void SliceAsync(string sourceFile, string destinationPath, int parts)
        {
            Task task = Task.Run(() => Slice(sourceFile, destinationPath, parts));
            tasks.Add(task);
        }

        private static void Slice(string sourceFile, string destinationPath, int parts)
        {
            Console.WriteLine("The file is proceed...");

            if (Directory.Exists(destinationPath))
            {
                Directory.Delete(destinationPath, true);
            }

            Directory.CreateDirectory(destinationPath);

            using (FileStream source = new FileStream(sourceFile, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(sourceFile);

                long partLength = (source.Length / parts);
                if (partLength * parts < source.Length)
                {
                    partLength++;
                }

                long currentByte = 0;

                for (int currentPart = 1; currentPart <= parts; currentPart++)
                {
                    string filePath = string.Format($"{destinationPath}/Part-{currentPart}{fileInfo.Extension}");

                    using (FileStream destination = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[4096];

                        while (currentByte <= partLength * currentPart)
                        {
                            int readBytesCount = source.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"File : \"{sourceFile}\" is Sliced.");
        }
    }
}
