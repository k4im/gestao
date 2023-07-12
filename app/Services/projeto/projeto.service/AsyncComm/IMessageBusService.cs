namespace projeto.service.AsyncComm
{
    public interface IMessageBusService
    {
        void enviarProjeto(Projeto evento);
    }
}