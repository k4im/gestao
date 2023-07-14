using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace estoque.service.AssynComm
{
    public class MessageConsumer : IMessageConsumer
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

                // Declarando a fila para eventos que foram adicionados
                _channel.QueueDeclare(queue: "atualizar.estoque",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo o balanceamento da fila para uma mensagem por vez.
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // Linkando a fila de eventos atualizados ao exchange
                _channel.QueueBind(queue: "atualizar.estoque",
                    exchange: "projeto.adicionado/api.projetos",
                    routingKey: "projeto.atualizar.estoque");


                _connection.ConnectionShutdown += RabbitMQFailed;
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e.Message}");
            }
        }

        public void verificarFila()
        {
            if (_channel.MessageCount("atualizar.estoque") != 0) consumirProdutosDisponiveis(_channel);
            // if (_channel.MessageCount("produtos.disponiveis.deletados") != 0) consumirProdutosDeletados(_channel);
        }

        private void consumirProdutosDisponiveis(IModel channel)
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


        private void RabbitMQFailed(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> Não foi possivel se conectar ao Message Bus: {e}");
        }

    }
}