

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProdCons
{
    class Producers
    {
        IStorage storage;
        int producersSize;

        public Producers(IStorage storage,int producers)
        {
            this.storage = storage;
            
            this.producersSize = producers;
        }
        public void StartProducers(CancellationToken token)
        {


            Task[] producers = new Task[producersSize];
            
            for (int i = 0; i < producersSize; i++)
            {
                int currentProducer = i;
                producers[currentProducer] = Task.Run(async () =>
                  {
                      
                      await this.LoopAsync(currentProducer);

                  }, token);
            }

        }
        private async Task LoopAsync(int currentProducerId)
        {
            int cycle = 0;
            while (true)
            {
                if (cycle > 5)
                {
                    cycle = 0;
                }
                Sample localSample = new Sample { Id = $"p{currentProducerId}", Value = cycle++ };
                await Task.Delay(1000);
                this.storage.Enqueue(localSample);

            }
        }
    }
}
