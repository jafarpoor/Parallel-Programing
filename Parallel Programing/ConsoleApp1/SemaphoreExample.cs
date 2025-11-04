using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppParallelPrograming
{
    public static class SemaphoreExample
    {
        private static Semaphore semaphore = new Semaphore(2, 2); // 2 thread همزمان

       public static void Job()
        {
            for (int i = 1; i <= 5; i++)
            {
                Thread thread = new Thread(DoWork);
                thread.Start(i);
            }
        }

        static void DoWork(object id)
        {
            Console.WriteLine($"Thread {id} در انتظار ورود...");

            // ورود به بخش بحرانی
            semaphore.WaitOne();

            try
            {
                Console.WriteLine($"Thread {id} وارد شد! - {DateTime.Now:T}");
                Thread.Sleep(2000); // شبیه‌سازی کار
            }
            finally
            {
                // خروج از بخش بحرانی
                semaphore.Release();
                Console.WriteLine($"Thread {id} خارج شد.");
            }
        }

    }
}
