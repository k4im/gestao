namespace autenticacao.service.Logger
{
    public class Logger
    {
        readonly IConfiguration _config;
        readonly Serilog.Core.Logger _logger;
        public Logger(IConfiguration config)
        {
            _config = config;
            _logger = new LoggerConfiguration()
           .WriteTo.Graylog(
               new GraylogSinkOptions
               {
                   HostnameOrAddress = _config["Serilog:Addr"],
                   Port = Convert.ToInt32(_config["Serilog:Port"])

               }
           ).CreateLogger();
        }

        public void logarInfo(string mensagem)
        {
            _logger.Information($"[{DateTime.UtcNow}] - [INFO]: {mensagem}");
        }

        public void logarErro(string mensagem)
        {
            _logger.Error($"[{DateTime.UtcNow}] - [ERRO]: {mensagem}");
        }

        public void logarAviso(string mensagem)
        {
            _logger.Warning($"[{DateTime.UtcNow}] - [AVISO]: {mensagem}");

        }


        public void logarFatal(string mensagem)
        {
            _logger.Fatal($"[{DateTime.UtcNow}] - [FATAL]: {mensagem}");

        }
    }
}