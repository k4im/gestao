using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projeto.service.AsyncComm.Extensions
{
    public class MessagePublisherExtension
    {
        public string filaAtualizarEstoque = "atualizar.estoque";
        public string exchange = "projeto.adicionado/api.projetos";
        public string routingKey = "projeto.atualizar.estoque";
        public void criarFilas(IModel channel)
        {

            // Definindo a fila no RabbitMQ
            channel.QueueDeclare(queue: filaAtualizarEstoque, durable: true,
                exclusive: false,
                autoDelete: false);

            // Definindo o Exchange no RabbitMQ
            channel.ExchangeDeclare(exchange: exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

            // Linkando a fila ao exchange
            channel.QueueBind(queue: filaAtualizarEstoque,
                exchange: exchange,
                routingKey: routingKey);
        }
    }
}