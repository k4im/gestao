using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace estoque.service.Worker
{
    public class RabbitMQBackground : IHostedService
    {
        IServiceProvider _provider;
        Timer _timer;
        public RabbitMQBackground(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(escutarFila, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        public void escutarFila(object state)
        {
            using (var scope = _provider.CreateScope())
            {
                IMessageConsumer consumer = scope.ServiceProvider.GetService<IMessageConsumer>();
                consumer.verificarFila();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Parando de escutar fila...");
            return Task.CompletedTask;
        }
    }
}