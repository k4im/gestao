using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projeto.service.Worker
{
    public class RabbitConsumer : IHostedService
    {
        Timer _timer;
        IServiceProvider _provider;

        public RabbitConsumer(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Escutando fila em background...");
            _timer = new Timer(consumirMensagens, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }
        public void consumirMensagens(object state)
        {
            using (IServiceScope scope = _provider.CreateScope())
            {
                IMessageConsumer messager = scope.ServiceProvider.GetService<IMessageConsumer>();
                messager.verificarFila();
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Parando processo em background");
            return Task.CompletedTask;
        }
    }
}