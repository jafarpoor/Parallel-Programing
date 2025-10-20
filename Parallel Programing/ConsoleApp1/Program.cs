using ConsoleAppParallelPrograming;
using ConsoleAppParallelPrograming.ClassException;
using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // --- Example1 ---
        //Example.opration1();
        //Example.opration2();
        //Console.WriteLine("End Main");

        // // --- Example2 ---
        //await Task.WhenAll(Example.opration3(), Example.opration4());
        // Console.WriteLine("End Main2");


        // // --- Example3 ---
        // Parallel.Invoke(()=>Example.opration5() , ()=>Example.opration6());

        // --- Example4 ---
        //Example.Run();

        // --- Example5 ---
        //int number1 = await Example.ReturnNumber();
        // Console.WriteLine(number1);

        // Run WithOut awit
        //Example.ReturnNumber();
        //Console.WriteLine("End Main");


        // --- Example6 ---
        //Example.RunContinueWith();


        // --- Example7 ---
        //Example.TestException();

        // --- Example8 ---
        Example.TestCancleToken();


        // --- Example9 ---
        Task T1 = Task.Factory.StartNew(Example.PrintStar);
        Task T2 = T1.ContinueWith(a => Example.PrintPlus());

        Task.WaitAll(new Task[] { T1, T2 });

        // --- Example 10 ---
        Example.TestPlinq();


        Console.ReadKey();
    }
}