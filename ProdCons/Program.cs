
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ProdCons
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Storage.Reactive queue = Storage.Reactive.Create();
            Storage.Async aqueue = Storage.Async.Create();
            Producers group = new Producers(aqueue,3);
            CancellationTokenSource source = new CancellationTokenSource();
           // SocketConsumer consumer = new SocketConsumer();
            Task consume = Task.Run(async() =>
            {
                while (true)
                {
                    var data = await aqueue.DequeueAsync();
                    Console.WriteLine($"Received:  id:{data.Id}, value:{data.Value}");
                }
                

            },source.Token);
           // var disposeable=queue.Subscribe(consumer);
            group.StartProducers(source.Token);
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.A)
                {

                    //disposeable.Dispose();
                    source.Cancel();
                    
                    break;
                }
            }
            await consume;
            
           
            
            
            Console.WriteLine("Hello World!");
        }
    }
}
