namespace projeto.service.Repository
{
    public interface IRepoProdutosDisponiveis
    {
        Task<ProdutosDisponiveis> buscarTodosProdutos();

        Task<bool> removerProdutos(int id);
        Task<bool> adicionarProdutos(ProdutosDisponiveis model);

        Task<bool> atualizarProdutos(int id, ProdutosDisponiveis model);
    }
}