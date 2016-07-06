using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NumberSearcher
{
    internal class SearchThread
    {
        private Thread thread;
        private int interval;
        private string name;
        public event EventHandler<ThreadResultArgs> ReturnHandler;

        public SearchThread(int interval, string name)
        {
            thread = new Thread(Search);
            this.interval = interval;
            this.name = name;
        }

        private void Search()
        {
            Random rnd = new Random();
            while (true)
            {
                Thread.Sleep(interval);
                int resultValue = rnd.Next(0, 100);
                ReturnHandler?.Invoke(this, new ThreadResultArgs(resultValue, name, DateTime.Now.ToString("dd.mm hh:mm:ss")));
            }
        }

        /// <summary>
        /// Start searching
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// Stop searching
        /// </summary>
        public void Stop()
        {
            thread.Abort();
        }
    }
}
