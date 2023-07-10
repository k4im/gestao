using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace projeto.service.AsyncComm
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public MessageConsumer(IConfiguration config)
        {
            _config = config;
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

        public List<ProdutosDisponiveis> consumeMessage()
        {
            var listaDeProdutos = consumirProdutosDisponiveis(_channel);
            return listaDeProdutos;
        }

        private List<ProdutosDisponiveis> consumirProdutosDisponiveis(IModel channel)
        {
            // Cria uma lista de produtos que foram enviados para a queue
            var listaDeProdutos = new List<ProdutosDisponiveis>();
            if (channel.MessageCount("produtos.disponiveis") == 0) return listaDeProdutos;
            // Definindo um consumidor
            var consumer = new EventingBasicConsumer(channel);

            // seta o EventSlim
            var msgsRecievedGate = new ManualResetEventSlim(false);

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

                    // Repassa o valor da mensagem para a var
                    for (int i = 0; i <= channel.MessageCount("projetos"); i++)
                    {
                        listaDeProdutos.Add(projeto);

                    }
                    // seta o valor no EventSlim
                    msgsRecievedGate.Set();
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
            msgsRecievedGate.Wait();
            // retorna o valor tratado
            Dispose();
            return listaDeProdutos;


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