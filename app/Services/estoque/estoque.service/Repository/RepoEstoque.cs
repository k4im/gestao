using shared.models;

namespace estoque.service.Repository
{
    public class RepoEstoque : IRepoEstoque
    {
        DataContext _db;

        public RepoEstoque(DataContext db)
        {
            _db = db;
        }

        public bool adicionarProduto(object model)
        {
            throw new NotImplementedException();
        }

        public bool atualizarProduto(int id, object model)
        {
            throw new NotImplementedException();
        }

        public async Task<object> buscarProdutoId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<object>> buscarProdutos(int pagina, float resultado)
        {
            throw new NotImplementedException();
        }

        public bool removerProduto(int id)
        {
            throw new NotImplementedException();
        }
    }
}