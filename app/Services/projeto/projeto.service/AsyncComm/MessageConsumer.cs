using RabbitMQ.Client.Events;

namespace projeto.service.AsyncComm
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IRepoProdutosDisponiveis _repo;

        public MessageConsumer(IConfiguration config, IRepoProdutosDisponiveis repo)
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

                //declarando exchange
                _channel.ExchangeDeclare(exchange: "produtos/api.estoque",
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false);

                // Declarando a fila 
                _channel.QueueDeclare(queue: "produtos.disponiveis",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);


                // Declarando a fila 
                _channel.QueueDeclare(queue: "produtos.disponiveis.deletados",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo o balanceamento da fila para uma mensagem por vez.
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "produtos.disponiveis",
                    exchange: "produtos/api.estoque",
                    routingKey: "produtos.disponiveis.produto.adicionado");

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "produtos.disponiveis.deletados",
                    exchange: "produtos/api.estoque",
                    routingKey: "produtos.disponiveis.produto.deletado");


                _connection.ConnectionShutdown += RabbitMQFailed;

                Console.WriteLine("--> Conectado ao Message Bus");
                Console.WriteLine("--> Conectado ao a queue");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e.Message}");
            }
        }

        public void verificarFila()
        {
            if (_channel.MessageCount("produtos.disponiveis") != 0) consumirProdutosDisponiveis(_channel);
            if (_channel.MessageCount("produtos.disponiveis.deletados") != 0) consumirProdutosDeletados(_channel);
        }

        private void consumirProdutosDisponiveis(IModel channel)
        {
            // Definindo um consumidor
            var consumer = new EventingBasicConsumer(channel);

            // seta o EventSlim
            // var msgsRecievedGate = new ManualResetEventSlim(false);

            // Definindo o que o consumidor recebe
            consumer.Received += (model, ea) =>
            {
                try
                {
                    // transformando o body em um array
                    byte[] body = ea.Body.ToArray();

                    // transformando o body em string
                    var message = Encoding.UTF8.GetString(body);
                    var projeto = JsonConvert.DeserializeObject<ProdutosDisponiveis>(message);

                    // Estará realizando a operação de adicição dos projetos no banco de dados
                    for (int i = 0; i <= channel.MessageCount("produtos.disponiveis"); i++)
                    {
                        _repo.adicionarProdutos(projeto);
                    }
                    // seta o valor no EventSlim
                    // msgsRecievedGate.Set();
                    Console.WriteLine("--> Messagem tratada");
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                }
                catch (Exception e)
                {
                    channel.BasicNack(ea.DeliveryTag,
                    multiple: false,
                    requeue: true);
                    Console.WriteLine(e);
                }




            };
            // Consome o evento
            channel.BasicConsume(queue: "produtos.disponiveis",
                         autoAck: false,
             consumer: consumer);

            // Espera pelo valor setado
            // msgsRecievedGate.Wait();
            // retorna o valor tratado
            // return listaDeProdutos;


        }
        private void consumirProdutosDeletados(IModel channel)
        {
            // Definindo um consumidor
            var consumer = new EventingBasicConsumer(channel);

            // seta o EventSlim
            // var msgsRecievedGate = new ManualResetEventSlim(false);

            // Definindo o que o consumidor recebe
            consumer.Received += (model, ea) =>
            {
                try
                {
                    // transformando o body em um array
                    byte[] body = ea.Body.ToArray();

                    // transformando o body em string
                    var message = Encoding.UTF8.GetString(body);
                    var produto = JsonConvert.DeserializeObject<ProdutosDisponiveis>(message);

                    // Estará realizando a operação de adicição dos projetos no banco de dados
                    for (int i = 0; i <= channel.MessageCount("produtos.disponiveis"); i++)
                    {
                        _repo.removerProdutos(produto.Id);
                    }
                    // seta o valor no EventSlim
                    // msgsRecievedGate.Set();
                    Console.WriteLine("--> Messagem tratada");
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                }
                catch (Exception e)
                {
                    channel.BasicNack(ea.DeliveryTag,
                    multiple: false,
                    requeue: true);
                    Console.WriteLine(e);
                }




            };
            // Consome o evento
            channel.BasicConsume(queue: "produtos.disponiveis.deletados",
                         autoAck: false,
             consumer: consumer);

            Dispose();

        }


        // Metodo para fechar a conexão com o broker 
        private void Dispose()
        {
            Console.WriteLine("Message Bus disposed");

            // Verifica se a conexão está aberta e fecha
            if (_channel.IsOpen)
            {
                _channel.Close();
            }
        }

        private void RabbitMQFailed(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e}");
        }

    }

}