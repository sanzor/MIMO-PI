using System;
using System.Collections.Generic;
using System.Text;

namespace ProdCons
{
    /// <summary>
    /// Console based consumer
    /// </summary>
    class Mock : IObserver<Sample>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Finished");
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Sample value)
        {
            Console.WriteLine($"Received from :{value.Id}\tvalue:{value.Value}");
        }
    }
}
