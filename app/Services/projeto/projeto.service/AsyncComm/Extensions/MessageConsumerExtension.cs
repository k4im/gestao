namespace projeto.service.AsyncComm.Extensions
{
    public class MessageConsumerExtension
    {
        public string exchange = "produtos/api.estoque";
        public string routingKeyDisponiveis = "produtos.disponiveis.produto.adicionado";
        public string filaProdutosDisponiveis = "produtos.disponiveis";
        public string routingKeyAtualizados = "produtos.disponiveis.produto.atualizado";
        public string filaProdutosDisponiveisAtualizados = "produtos.disponiveis.atualizados";
        public string routingKeyDeletados = "produtos.disponiveis.produto.deletado";
        public string filasProdutosDisponiveisDeletados = "produtos.disponiveis.deletados";
        public void criarFilas(IModel channel)
        {

            // Declarando a fila para eventos que foram adicionados
            channel.QueueDeclare(queue: filaProdutosDisponiveis,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Declarando a fila para eventos que foram atualizados
            channel.QueueDeclare(queue: filaProdutosDisponiveisAtualizados,
                durable: true,
                exclusive: false,
                autoDelete: false);


            // Declarando a fila para eventos que foram deletados
            channel.QueueDeclare(queue: filasProdutosDisponiveisDeletados,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Definindo o balanceamento da fila para uma mensagem por vez.
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            // Linkando a fila de eventos adicionados ao exchange 
            channel.QueueBind(queue: filaProdutosDisponiveis,
                exchange: exchange,
                routingKey: routingKeyDisponiveis);

            // Linkando a fila de eventos atualizados ao exchange 
            channel.QueueBind(queue: filaProdutosDisponiveisAtualizados,
                exchange: exchange,
                routingKey: routingKeyAtualizados);

            // Linkando a fila de eventos deletados ao exchange 
            channel.QueueBind(queue: filasProdutosDisponiveisDeletados,
                exchange: exchange,
                routingKey: routingKeyDeletados);

        }
    }
}