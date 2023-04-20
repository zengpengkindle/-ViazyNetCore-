using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace ViazyNetCore.TaskScheduler
{
    public class CommonInterruptableJob
    {
        private Thread _currentThread;

        public void Execute(IJobExecutionContext context)
        {
            _currentThread = Thread.CurrentThread;
            try
            {
                //TODO:编写你的任务代码
            }
            finally
            {
                _currentThread = null;
            }
        }

        public void Interrupt()
        {
            if (_currentThread != null)
            {
                _currentThread.Abort();
                _currentThread = null;
            }
        }
    }
}
