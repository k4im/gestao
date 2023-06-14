using shared.models;

namespace estoque.service.Repository
{
    public interface IRepoEstoque
    {
        Task<Object> buscarProdutoId(int id);
        Task<Response<Object>> buscarProdutos(int pagina, float resultado);

        bool adicionarProduto(Object model);

        bool removerProduto(int id);

        bool atualizarProduto(int id, Object model);
    }
}