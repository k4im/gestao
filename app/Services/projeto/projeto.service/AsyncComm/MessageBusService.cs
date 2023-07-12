namespace projeto.service.AsyncComm
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _config;
        private IConnection _connection;
        private IModel _channel;
        IMapper _mapper;
        public MessageBusService(IConfiguration config, IMapper mapper)
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
                _channel.QueueDeclare(queue: "atualizar.estoque", durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Definindo o Exchange no RabbitMQ
                _channel.ExchangeDeclare(exchange: "projeto.adicionado/api.projetos",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

                // Linkando a fila ao exchange
                _channel.QueueBind(queue: "atualizar.estoque",
                    exchange: "projeto.adicionado/api.projetos",
                    routingKey: "projeto.atualizar.estoque");


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
        public void enviarProjeto(Projeto evento)
        {
            // Realizando apontamento para outra variavel e
            // convertendo o objeto em JSON
            var projetoModel = _mapper.Map<Projeto, ProjetoDTO>(evento);
            var message = JsonConvert.SerializeObject(projetoModel);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> Enviando mensagem para o RabbitMQ...");
                enviar(message);

            }
        }

        // Metodo privado de envio da mensagem
        private void enviar(string evento)
        {
            // transformando o json em array de bytes
            var body = Encoding.UTF8.GetBytes(evento);

            // Persistindo a mensagem no broker
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            // Realizando o envio para o exchange 
            _channel.BasicPublish(exchange: "projeto.adicionado/api.projetos",
                routingKey: "projeto.atualizar.estoque",
                basicProperties: props,
                body: body);
        }
        // "Logger" caso de algum erro 
        public void RabbitMQFailed(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ foi derrubado");
        }
    }
}
