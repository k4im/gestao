namespace projeto.service.AsyncComm
{
    public interface IMessageConsumer
    {
        List<ProdutosDisponiveis> consumeMessage();
    }
}