using projeto.service.AsyncComm.Extensions;

namespace projeto.service.AsyncComm
{
    public class MessageBusService : MessagePublisherExtension, IMessageBusService
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
            => enviar(serializarObjeto(evento), routingKey);

        // Metodo privado de envio da mensagem
        void enviar(string evento, string routingKey)
        {
            if (_channel.IsOpen)
            {
                // transformando o json em array de bytes
                var body = Encoding.UTF8.GetBytes(evento);

                // Persistindo a mensagem no broker
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;

                // Realizando o envio para o exchange 
                _channel.BasicPublish(exchange: exchange,
                    routingKey: routingKey,
                    basicProperties: props,
                    body: body);

            }
        }
        string serializarObjeto(Projeto evento)
        {
            var projetoModel = _mapper.Map<Projeto, ProjetoDTO>(evento);
            return JsonConvert.SerializeObject(projetoModel);
        }
        // "Logger" caso de algum erro 
        void RabbitMQFailed(object sender, ShutdownEventArgs e)
            => Console.WriteLine("--> RabbitMQ foi derrubado");
    }
}