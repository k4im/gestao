namespace estoque.service.AssynComm.Extensions
{
    public class MessageConsumerExtension
    {
        public string filaAtualizarEstoque = "atualizar.estoque";
        public string exchange = "projeto.adicionado/api.projetos";
        public string routingKeyEstoque = "projeto.atualizar.estoque";
        public void criarFilas(IModel channel)
        {
            // Declarando a fila para eventos que foram adicionados
            channel.QueueDeclare(queue: filaAtualizarEstoque,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Definindo o balanceamento da fila para uma mensagem por vez.
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            // Linkando a fila de eventos atualizados ao exchange
            channel.QueueBind(queue: filaAtualizarEstoque,
                exchange: exchange,
                routingKey: routingKeyEstoque);
        }
    }
}