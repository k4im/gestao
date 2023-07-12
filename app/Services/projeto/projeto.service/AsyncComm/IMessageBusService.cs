namespace projeto.service.AsyncComm
{
    public interface IMessageBusService
    {
        void enviarProjeto(ProjetoDTO evento);
    }
}