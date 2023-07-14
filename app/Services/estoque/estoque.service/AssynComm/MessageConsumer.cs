namespace estoque.service.AssynComm
{
    public class MessageConsumer : MessageConsumerExtension, IMessageConsumer
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IRepoEstoque _repo;

        public MessageConsumer(IConfiguration config, IRepoEstoque repo)
        {
            _config = config;
            _repo = repo;
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ"],
                Port = int.Parse(_config["RabbitPort"]),
                UserName = _config["RabbitMQUser"],
                Password = _config["RabbitMQPwd"],
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                criarFilas(_channel);
                _connection.ConnectionShutdown += RabbitMQFailed;
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e.Message}");
            }
        }

        public void verificarFila()
            => consumirProdutosDisponiveis(_channel);

        void consumirProdutosDisponiveis(IModel channel)
        {
            if (_channel.MessageCount("atualizar.estoque") != 0)
            {
                // Definindo um consumidor
                var consumer = new EventingBasicConsumer(channel);

                // Definindo o que o consumidor recebe
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        // transformando o body em um array
                        byte[] body = ea.Body.ToArray();

                        // transformando o body em string
                        var message = Encoding.UTF8.GetString(body);
                        var projeto = JsonConvert.DeserializeObject<ProjetoDTO>(message);

                        // Estará realizando a operação de adicição dos projetos no banco de dados
                        for (int i = 0; i <= channel.MessageCount("atualizar.estoque"); i++)
                        {
                            await _repo.atualizarEstoque(projeto);
                        }

                        // seta o valor no EventSlim
                        // msgsRecievedGate.Set();
                        Console.WriteLine("--> Dado consumido da fila[projeto.adicionado]");
                        Console.WriteLine(message);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    }
                    catch (Exception e)
                    {
                        channel.BasicNack(ea.DeliveryTag,
                        multiple: false,
                        requeue: true);
                        Console.WriteLine($"Erro ao consumir mensagem: {e.Message}");
                    }

                };
                // Consome o evento
                channel.BasicConsume(queue: "atualizar.estoque",
                             autoAck: false,
                 consumer: consumer);
            }

        }

        void RabbitMQFailed(object sender, ShutdownEventArgs e)
          => Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e}");

    }
}