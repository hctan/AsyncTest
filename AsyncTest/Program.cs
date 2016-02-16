using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(1000);

            AsyncFactory.GetIntAsync(cts.Token).ContinueWith((task) => {
                //We get the response.
                //So dispose the CancellationTokenSource
                //so that it is not going to signal.
                cts.Dispose();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine(task.Result);
                }
                else if (task.Status == TaskStatus.Canceled)
                {
                    Console.WriteLine("The task has been canceled.");
                }
                else
                {
                    Console.WriteLine("An error has been occurred. Details:");
                    Console.WriteLine(task.Exception.InnerException.Message);
                }
            });

            Console.ReadLine();
        }
    }
}
