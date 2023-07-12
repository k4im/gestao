
using AutoMapper;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace estoque.service.AssynComm
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;
        IMapper _mapper;
        public MessagePublisher(IConfiguration config, IMapper mapper)
        {
            _config = config;

            // Definindo a ConnectionFactory, passando o hostname, user, pwd, port
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ"],
                UserName = _config["RabbitMQUser"],
                Password = _config["RabbitMQPwd"],
                Port = Convert.ToInt32(_config["RabbitPort"])
            };

            try
            {
                //Criando a conexão ao broker
                _connection = factory.CreateConnection();

                // Criando o modelo da conexão
                _channel = _connection.CreateModel();

                // Definindo a fila no RabbitMQ
                _channel.QueueDeclare(queue: "produtos.disponiveis", durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo a fila no RabbitMQ
                _channel.QueueDeclare(queue: "produtos.disponiveis.atualizados", durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo a fila no RabbitMQ
                _channel.QueueDeclare(queue: "produtos.disponiveis.deletados", durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo o Exchange no RabbitMQ
                _channel.ExchangeDeclare(exchange: "produtos/api.estoque",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "produtos.disponiveis",
                    exchange: "produtos/api.estoque",
                    routingKey: "produtos.disponiveis.produto.adicionado");

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "produtos.disponiveis.atualizados",
                    exchange: "produtos/api.estoque",
                    routingKey: "produtos.disponiveis.produto.atualizado");

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "produtos.disponiveis.deletados",
                    exchange: "produtos/api.estoque",
                    routingKey: "produtos.disponiveis.produto.deletado");


                _connection.ConnectionShutdown += RabbitMQFailed;

                Console.WriteLine("--> Conectado ao Message Bus");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Não foi possivel se conectar com o Message Bus: {e.Message}");
            }
            _mapper = mapper;
        }

        // Metodo de publicação de um novo projeto contendo todos os dados do projeto
        public void publicarProduto(Produto produto)
        {
            var produtoModel = _mapper.Map<Produto, ProdutoDisponivel>(produto);
            var message = JsonConvert.SerializeObject(produtoModel);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, enviando mensagem...");
                enviarProdutoAdicionado(message);

            }
        }

        // Metodo privado de envio da mensagem
        private void enviarProdutoAdicionado(string evento)
        {
            // transformando o json em array de bytes
            var body = Encoding.UTF8.GetBytes(evento);

            // Persistindo a mensagem no broker
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            // Realizando o envio para o exchange 
            _channel.BasicPublish(exchange: "produtos/api.estoque",
                routingKey: "produtos.disponiveis.produto.adicionado",
                basicProperties: props,
                body: body);
            Console.WriteLine("--> Mensagem enviado");

        }

        // Metodo estará enviando apenas as mensagens que foram atualizadas
        private void enviarProdutoAtualizado(string evento)
        {
            // transformando o json em array de bytes
            var body = Encoding.UTF8.GetBytes(evento);

            // Persistindo a mensagem no broker
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            // Realizando o envio para o exchange 
            _channel.BasicPublish(exchange: "produtos/api.estoque",
                routingKey: "produtos.disponiveis.produto.atualizado",
                basicProperties: props,
                body: body);
            Console.WriteLine("--> Mensagem enviado");

        }

        // Metodo para fechar a conexão com o broker 
        private void Dispose()
        {
            Console.WriteLine("Message Bus disposed");

            // Verifica se a conexão está aberta e fecha
            if (_channel.IsOpen)
            {
                _channel.Dispose();
            }
        }

        // "Logger" caso de algum erro 
        public void RabbitMQFailed(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ foi derrubado");
        }

        public void atualizarProduto(Produto produto)
        {
            var produtoModel = _mapper.Map<Produto, ProdutoDisponivel>(produto);
            produtoModel.Id = 1;
            var message = JsonConvert.SerializeObject(produtoModel);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, enviando mensagem...");
                enviarProdutoAtualizado(message);

            }
        }
    }

}