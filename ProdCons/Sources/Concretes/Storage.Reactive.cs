using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ProdCons
{
    partial class Storage
    {
        public class Reactive : IObservable<Sample>, IStorage
        {
            private object @lock = new object();

            private Queue<Sample> queue;
            private List<IObserver<Sample>> observers = new List<IObserver<Sample>>();

            public static Reactive Create(int? size = null)
            {
                return new Reactive(size);
            }

            public Reactive(int? size)
            {
                if (size.HasValue)
                {
                    this.queue = new Queue<Sample>(size.Value);
                }
                else
                    this.queue = new Queue<Sample>();
            }

            public void Enqueue(Sample value)
            {
                lock (this.@lock)
                {
                    this.queue.Enqueue(value);

                }

                foreach (var obs in observers)
                {

                    if (!this.queue.TryDequeue(out Sample rez))
                    {
                        return;
                    }
                    obs.OnNext(rez);
                }

            }


            public void FinishProducing()
            {
                foreach (var obs in observers)
                {
                    obs.OnCompleted();
                }
            }

            public IDisposable Subscribe(IObserver<Sample> observer)
            {
                if (!this.observers.Contains(observer))
                {
                    this.observers.Add(observer);
                }
                return new Unsubscriber(observers, observer);
            }
            public class Unsubscriber : IDisposable
            {
                private List<IObserver<Sample>> observers;
                private IObserver<Sample> observer;

                public Unsubscriber(List<IObserver<Sample>> _observers, IObserver<Sample> _observer)
                {
                    this.observer = _observer;
                    this.observers = _observers;
                }
                public void Dispose()
                {
                    if (this.observer != null)
                    {
                        this.observers.Remove(observer);
                    }
                }
            }
        }

    }
}
