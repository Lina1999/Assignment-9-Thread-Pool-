using System;
using System.Collections.Generic;
using System.Threading;

namespace thread_pool
{
    public class Program
    {
        /// <summary>
        /// main function
        /// </summary>
        static void Main()
        {
            List<ManualResetEvent> eventFinished = new List<ManualResetEvent>();
            List<Fibonacci> fibNums = new List<Fibonacci>();
            var tp = new Thread_Pool(100);
            try
            {
                Random r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    eventFinished.Add( new ManualResetEvent(false));
                    int rand = r.Next(25, 45);
                    fibNums.Add(new Fibonacci(rand, eventFinished[i]));
                    tp.QueueUserWorkItem(fibNums[i].CallbackForFib);
                }
            }
            finally
            {
                ((IDisposable)tp).Dispose();
            }
            WaitHandle.WaitAll(eventFinished.ToArray());
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Fibonacci sequence {0}th member is {1}", fibNums[i].Num, fibNums[i].Answer);
            }
        }
    }
}

