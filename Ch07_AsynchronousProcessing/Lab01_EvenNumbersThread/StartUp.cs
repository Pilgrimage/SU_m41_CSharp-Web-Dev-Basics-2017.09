namespace Lab01_EvenNumbersThread
{
    using System;
    using System.Linq;
    using System.Threading;

    public class StartUp
    {
        public static void Main()
        {
            // Extended task - with two asynchronous threads for printing even numbers

            Console.Write("Please enter two integers separated by a space : ");
            int[] border = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int startNum = border.Min();
            int endNum = border.Max();

            Thread thread = new Thread(  () => PrintEvenNumbers(startNum, endNum));

            // It's also possible to use Lambda Expression

            Thread threadSecond = new Thread(() =>
            {
                int startNumSecond = startNum + 1000;
                int endNumSecond = endNum + 1000;

                for (int i = startNumSecond; i <= endNumSecond; i++)
                {
                    if (i % 2 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            thread.Start();
            threadSecond.Start();
            Console.WriteLine("Waitinig for threads to finish work...");

            thread.Join();
            threadSecond.Join();
            Console.WriteLine("Threads finishing work.");
        }

        private static void PrintEvenNumbers(int startNum, int endNum)
        {
            for (int i = startNum; i <= endNum; i++)
            {
                if (i%2==0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}
