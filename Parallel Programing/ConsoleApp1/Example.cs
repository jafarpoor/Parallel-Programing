using ConsoleAppParallelPrograming.ClassException;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppParallelPrograming
{
    static internal class Example
    {
        public static int counter = 10;

        // --- Example1 ---

        public static void opration1()
        {
            Console.WriteLine("opration1");
        }
        public static async void opration2()
        {
            Console.WriteLine("opration2");
            await Task.Delay(1000);
            Console.WriteLine("End opration2");
        }


        // --- Example2 ---

        public async static Task opration3()
        {
            Console.WriteLine("opration3");
            await Task.Delay(500);
            Console.WriteLine("End opration3");
        }
        public static async Task opration4()
        {
            Console.WriteLine("opration4");
            await Task.Delay(1000);
            Console.WriteLine("End opration4");
        }


        // --- Example3 ---

        public static void opration5()
        {
            Console.WriteLine("opration5");
            Thread.Sleep(1000);
            Console.WriteLine("End opration5");
        }
        public static void opration6()
        {
            Console.WriteLine("opration6");
            Thread.Sleep(500);
            Console.WriteLine("End opration6");
        }

        // --- Example4 ---
        public static async void Run()
        {
            var stopwatch = Stopwatch.StartNew();
            var tcs = new TaskCompletionSource<bool>();
            Console.WriteLine($"Start : {stopwatch.ElapsedMilliseconds} ms");

            var fnf = Task.Delay(2000).ContinueWith(task => tcs.SetResult(true));
            Console.WriteLine($"Waiting : {stopwatch.ElapsedMilliseconds} ms");

            await tcs.Task;
            Console.WriteLine($"Done : {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Stop();
        }

        // --- Example5 ---
        public static async Task<int> ReturnNumber()
        {
            await Task.Delay(1000);
            Console.WriteLine("Method ReturnNumber");
            return 5;
        }

        // --- Example6 ---
        public static void RunContinueWith()
        {
            var task = Task<string>.Run(() =>
            {
                Thread.Sleep(4000);
                return "Run Method";
            });
            var result = task.ContinueWith((task2) =>
            {
                Console.WriteLine("test ContinueWith");
            });
        }


        // --- Example7 ---
        public static async void TestException()
        {
            try
            {
                var task1 = Task.Run(async () =>
                {
                    await Task.WhenAll(
                        Task.Run(() => throw new CustomException("Child1 faulted")),
                        Task.Run(() => throw new CustomException("Child2 faulted"))
                    );
                });

                await task1;
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.Flatten().InnerExceptions)
                {
                    Console.WriteLine($"Handled: {e.Message}");
                }
            }
        }

        // --- Example8 ---
        public static void TestCancleToken()
        {
            Console.WriteLine("*** Start Main ***");

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task t;
            var tasks = new ConcurrentBag<Task>();

            Console.WriteLine("Press Any Key to begin ...");
            Console.ReadKey(true);
            Console.WriteLine("To terminate the example, press 'c' to cancel and exit");
            Console.WriteLine();

            t = Task.Factory.StartNew(() => LongRunningOperation(1, token), token);
            Console.WriteLine($"Task {t.Id} executing");
            tasks.Add(t);

            char ch = Console.ReadKey().KeyChar;
            if (ch == 'c')
            {
                tokenSource.Cancel();
                Console.WriteLine("Task Cancellation Request");
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException e)
            {
                Console.WriteLine("AggregateException thrown");
                foreach (var v in e.InnerExceptions)
                {
                    if (v is TaskCanceledException)
                    {
                        Console.WriteLine("TaskCanceledException thrown");
                    }
                    else
                    {
                        Console.WriteLine($"Execption {v.GetType().Name} thrown");
                    }

                    Console.WriteLine();
                }
            }
            finally
            {
                tokenSource.Dispose();
            }

            foreach (var task in tasks)
            {
                Console.WriteLine($"Task {task.Id} status is now {task.Status}");
            }

            Console.WriteLine("*** End Main ***");
            Console.ReadKey();
        }
        private static void LongRunningOperation(int taskNum, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine($"Task {taskNum} was cancelled before it got started");
                ct.ThrowIfCancellationRequested();
            }

            int max = 100;
            for (int i = 0; i < max; i++)
            {
                Thread.Sleep(1000);
            }

            if (ct.IsCancellationRequested)
            {
                Console.WriteLine($"Task {taskNum} Cancelled");
                ct.ThrowIfCancellationRequested();
            }

        }


        // --- Example 9 ---
        public static void PrintStar()
        {
            for (counter = 0; counter < 5; counter++)
            {
                Console.WriteLine(" * ");
            }
        }
        public static void PrintPlus()
        {
            for (counter = 0; counter < 5; counter++)
            {
                Console.WriteLine(" + ");
            }
        }

        // --- Example 10 ---
        public static void TestPlinq()
        {
            var source = Enumerable.Range(1, 1000000);
            var sw = new Stopwatch();
            sw.Start();

            var result = source
                .AsParallel()
                .WithDegreeOfParallelism(2)
                .AsOrdered()
                .Where(n => n % 2 == 0)
                .Select(s => s);

            sw.Stop();

            Console.WriteLine($"{result.Count()} even number out of {source.Count()}, in :{sw.ElapsedMilliseconds}");
        }
    }
}
