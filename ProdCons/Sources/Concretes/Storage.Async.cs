using System;
using System.Collections.Generic;
using System.Text;

namespace ProdCons
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    partial class Storage
    {
        /// <summary>
        
        /// </summary>
        public class Async : IStorage
        {
            private object @lock = new object();

            Queue<TaskCompletionSource<Sample>> tcsQueue = new Queue<TaskCompletionSource<Sample>>();
            Queue<Sample> bufferQueue = new Queue<Sample>();

            public static Async Create(int? size = null)
            {
                return new Async(size);
            }

            private Async(int? size)
            {
                if (size.HasValue)
                {
                    this.bufferQueue = new Queue<Sample>(size.Value);
                }
                else
                    this.bufferQueue = new Queue<Sample>();
            }

            public void Enqueue(Sample value)
            {
                lock (this.@lock)
                {
                    this.bufferQueue.Enqueue(value);
                    if (this.tcsQueue.Count > 0)
                    {
                        var tcs = this.tcsQueue.Dequeue();
                        tcs.SetResult(value);
                    }
                }
            }

            public Task<Sample> DequeueAsync()
            {
                lock (@lock)
                {

                    if (!this.bufferQueue.TryDequeue(out Sample rez))
                    {
                        TaskCompletionSource<Sample> tcs = new TaskCompletionSource<Sample>();

                        this.tcsQueue.Enqueue(tcs);

                        return tcs.Task;
                    }
                    return Task.FromResult(rez);

                }


            }
        }


    }


}
