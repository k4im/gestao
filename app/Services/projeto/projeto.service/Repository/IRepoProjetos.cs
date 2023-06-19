
namespace projeto.service.Repository
{
    public interface IRepoProjetos
    {
        Task<Response<Projeto>> BuscarProdutos(int pagina, float resultadoPorPagina);

        Task<bool> CriarProjeto(Projeto model);

        Task<bool> AtualizarStatus(StatusProjeto model, int? id);

        Task<bool> DeletarProjeto(int? id);
        Task<Projeto> BuscarPorId(int? id);
    }
}