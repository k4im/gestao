using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace estoque.service.Worker
{
    public class RabbitMQBackground : BackgroundService
    {
        IServiceProvider _provider;
        Timer _timer;
        public RabbitMQBackground(IServiceProvider provider)
        {
            _provider = provider;
        }


        public void escutarFila(object state)
        {
            using (var scope = _provider.CreateScope())
            {
                try
                {
                    IMessageConsumer consumer = scope.ServiceProvider.GetService<IMessageConsumer>();
                    consumer.verificarFila();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Não foi possivel conectar ao BUS: {e.Message}");
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Escutando fila...");
            _timer = new Timer(escutarFila, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }
    }
}