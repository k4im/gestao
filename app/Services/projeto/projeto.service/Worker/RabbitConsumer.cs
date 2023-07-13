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
                await Task.Delay(8000, stoppingToken);
            }
        }
    }
}