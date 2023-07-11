namespace projeto.service.Repository
{
    public class RepoProdutosDisponiveis : IRepoProdutosDisponiveis
    {
        readonly DataContext _db;

        public RepoProdutosDisponiveis(DataContext db)
        {
            _db = db;
        }

        public async Task<bool> adicionarProdutos(ProdutosDisponiveis model)
        {
            _db.ProdutosEmEstoque.Add(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> atualizarProdutos(int id, ProdutosDisponiveis model)
        {
            throw new NotImplementedException();
        }

        public Task<ProdutosDisponiveis> buscarTodosProdutos()
        {
            throw new NotImplementedException();
        }

        public Task<bool> removerProdutos(int id)
        {
            throw new NotImplementedException();
        }
    }
}