using System;
using System.Threading;

namespace MutexTest
{
    class Program
    {
        const string MutexId = "Mutex-ID";
        static readonly Mutex SingleInstanceMutex = new Mutex(false, MutexId);

        static void Main(string[] args)
        {
            Console.WriteLine("Trying to get the lock...");
            try
            {
                if (!SingleInstanceMutex.WaitOne(new TimeSpan(0, 0, 0, 30)))
                {
                    Console.WriteLine("Process already running, aborting...");
                    Console.ReadKey(true);
                    return;
                }
            }
            catch (AbandonedMutexException exception)
            {
                Console.WriteLine("Abandoned Mutex dected - a previous process had not released the lock. This should not have happened!");
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Lock received!");

            do
            {
                Console.Write(".");
                Thread.Sleep(100);
                
                if(!Console.KeyAvailable) continue;
                if(Console.ReadKey(true).Key != ConsoleKey.Escape) continue;

                Console.WriteLine("\nAborting...");
                break;
            } while (true);

            SingleInstanceMutex.ReleaseMutex();

            Console.WriteLine("Lock released!");
            Console.ReadKey(true);
        }
    }
}
