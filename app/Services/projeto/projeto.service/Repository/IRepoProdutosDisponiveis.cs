namespace projeto.service.Repository
{
    public interface IRepoProdutosDisponiveis
    {
        Task<List<ProdutosDisponiveis>> buscarTodosProdutos();

        Task<bool> removerProdutos(int id);
        Task<bool> adicionarProdutos(ProdutosDisponiveis model);

        Task<bool> atualizarProdutos(int id, ProdutosDisponiveis model);
    }
}