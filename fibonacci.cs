using System.Threading;

namespace thread_pool
{
    /// <summary>
    /// Class for calculating Fibonacci result for given number
    /// </summary>
    class Fibonacci
    {
        /// <summary>
        /// Number of member needed
        /// </summary>
        private int num;

        /// <summary>
        /// Event notifier for threads
        /// </summary>
        private ManualResetEvent eventFinished;

        /// <summary>
        /// Result
        /// </summary>
        private int ans;

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Fibonacci() { }

        /// <summary>
        /// Consrtuctor with parameters
        /// </summary>
        /// <param name="number"></param>
        /// <param name="evFin"></param>
        public Fibonacci(int number, ManualResetEvent evFin)
        {
            num = number;
            eventFinished = evFin;
        }

        /// <summary>
        /// Getter or num
        /// </summary>
        public int Num
        {
            get
            {
                return num;
            }
        }

        /// <summary>
        /// Getter for result
        /// </summary>
        public int Answer
        {
            get
            {
                return ans;
            }
        }

        /// <summary>
        /// Callback function for thread pool
        /// </summary>
        public void CallbackForFib()
        {
            ans = fib(num);
            eventFinished.Set();
        }

        /// <summary>
        /// calculating Fibonacci sequence result 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int fib(int num)
        {
            if (num == 1)
                return 0;
            if (num == 2)
                return 1;
            return fib(num - 2) + fib(num - 1);
        }
    }
}
