using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    public class AsyncFactory
    {
        public static Task<int> GetIntAsync(CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<int>();

            if (token.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            var timer = new System.Timers.Timer(3000);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                tcs.TrySetResult(10);
                timer.Dispose();
            };

            if (token.CanBeCanceled)
            {
                token.Register(() => {
                    tcs.TrySetCanceled();
                    timer.Dispose();
                });
            }

            timer.Start();
            return tcs.Task;
        }
    }
}
