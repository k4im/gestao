namespace projeto.service.Worker
{
    public class RabbitConsumer : BackgroundService
    {
        IServiceProvider _provider;

        public RabbitConsumer(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = _provider.CreateScope())
                    {
                        IMessageConsumer messager = scope.ServiceProvider.GetService<IMessageConsumer>();
                        messager.verificarFila();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Não foi possivel se conecetar ao RabbitMQ no Worker: {e.Message}");
                }
            }
            await Task.Delay(8000, stoppingToken);
        }
    }
}