namespace estoque.service.Repository
{
    public interface IRepoEstoque
    {
        Task<Produto> buscarProdutoId(int? id);
        Task<Response<Produto>> buscarProdutos(int pagina, float resultado);

        Task<bool> adicionarProduto(Produto model);

        Task<bool> removerProduto(int? id);

        Task<bool> atualizarProduto(int? id, Produto model);
        Task atualizarEstoque(ProjetoDTO model);
    }
}