namespace projeto.service.AsyncComm
{
    public interface IMessageBusService
    {
        void publishNewProjeto(ProjetoDTO evento);
    }
}