using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace thread_pool
{
    /// <summary>
    /// delegate with no return type and no parameters
    /// </summary>
    public delegate void WaitCallback();
    
    /// <summary>
    /// class Thread_Pool(implements IDisposable interface)
    /// </summary>
    public class Thread_Pool : IDisposable
    {
        /// <summary>
        /// list of working threads
        /// </summary>
        private List<Thread> working;

        /// <summary>
        /// queue of given tasks
        /// </summary>
        private ConcurrentQueue<WaitCallback> taskQueue;

        /// <summary>
        /// boolean field which indicates if the queue was disposed
        /// </summary>
        private bool isDisposed;


        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Thread_Pool()
        {
            working = new List<Thread>();
            taskQueue = new ConcurrentQueue<WaitCallback>();
            isDisposed = false;
        }

        /// <summary>
        /// constructor with parameters with given size
        /// </summary>
        /// <param name="n"></param>
        public Thread_Pool(int n):this()
        {
            for (int i = 0; i < n; ++i)
            {
                Thread t = new Thread(Run);
                t.Start();
                working.Add(t);
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// function for processing tasks
        /// </summary>
        private void Run()
        {
            WaitCallback wcb = null;
            lock (taskQueue)
            {
                while (true)
                {
                    if (isDisposed)
                        return;
                    if (taskQueue.Count != 0 && working[0] != null && taskQueue != null
                        && working[0].Equals(Thread.CurrentThread))
                    {
                        taskQueue.TryPeek(out wcb);
                        taskQueue.TryDequeue(out wcb);
                        working.Remove(working[0]);
                        Monitor.PulseAll(taskQueue);
                        break;
                    }
                    if (taskQueue != null)
                        Monitor.Wait(taskQueue);
                }
            }
            wcb();
            wcb = null;
            lock (taskQueue)
            {
                working.Add(Thread.CurrentThread);
            }
        }

        /// <summary>
        /// Disposing queue
        /// </summary>
        public void Dispose()
        {
            lock (taskQueue)
            {
                while (taskQueue.Count != 0)
                    Monitor.Wait(taskQueue);
                Monitor.PulseAll(taskQueue);
                isDisposed = true;
            }
            for (int i = 0; i < working.Count; ++i)
                working[i].Join();
        }

        /// <summary>
        /// adding given task to the queue of tasks
        /// </summary>
        /// <param name="task"></param>
        public void QueueUserWorkItem(WaitCallback task)
        {
            lock(taskQueue)
            {
                taskQueue.Enqueue(task);
                Monitor.PulseAll(taskQueue);
            }
        }
    }
}
