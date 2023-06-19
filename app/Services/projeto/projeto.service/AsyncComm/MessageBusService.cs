namespace projeto.service.AsyncComm
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusService(IConfiguration config)
        {
            _config = config;

            // Definindo a ConnectionFactory, passando o hostname, user, pwd, port
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ"],
                UserName = _config["RabbitMQUser"],
                Password = _config["RabbitMQPwd"],
                Port = int.Parse(_config["RabbitPort"])
            };

            try
            {
                //Criando a conexão ao broker
                _connection = factory.CreateConnection();

                // Criando o modelo da conexão
                _channel = _connection.CreateModel();

                // Definindo a fila no RabbitMQ
                _channel.QueueDeclare(queue: "projetos", durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo o Exchange no RabbitMQ
                _channel.ExchangeDeclare(exchange: "projeto-trigger",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false);

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "projetos",
                    exchange: "projeto-trigger",
                    routingKey: "");


                _connection.ConnectionShutdown += RabbitMQFailed;

                Console.WriteLine("--> Conectado ao Message Bus");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Não foi possivel se conectar com o Message Bus: {e.Message}");
            }
        }

        // Metodo de publicação de um novo projeto contendo todos os dados do projeto
        public void publishNewProjeto(ProjetoDTO evento)
        {
            var projeto = evento;
            var message = JsonConvert.SerializeObject(projeto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, enviando mensagem...");
                SendMessage(message);

            }

            Console.WriteLine("--> RabbitMQ Connection Closed...");
        }

        // Metodo privado de envio da mensagem
        private void SendMessage(string evento)
        {
            // transformando o json em array de bytes
            var body = Encoding.UTF8.GetBytes(evento);

            // Persistindo a mensagem no broker
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            // Realizando o envio para o exchange 
            _channel.BasicPublish(exchange: "projeto-trigger",
                routingKey: "",
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
    }
}
