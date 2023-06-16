namespace estoque.service.Repository
{
    public class RepoEstoque : IRepoEstoque
    {
        DataContext _db;

        public RepoEstoque(DataContext db)
        {
            _db = db;
        }

        public async Task<bool> adicionarProduto(Produto model)
        {
            try
            {
                _db.Produtos.Add(model);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizada por outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public async Task<bool> atualizarProduto(int? id, Produto model)
        {
            try
            {
                var produto = await buscarProdutoId(id);
                produto = model;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizada por outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public async Task<Produto> buscarProdutoId(int? id)
        {
            try
            {
                var produto = await _db.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto == null) return null;
                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<Produto>> buscarProdutos(int pagina, float resultado)
        {
            var resultadoPaginas = resultado;
            var pessoas = await _db.Produtos.ToListAsync();
            var totalDePaginas = Math.Ceiling(pessoas.Count() / resultadoPaginas);
            var produtosPaginados = pessoas.Skip((pagina - 1) * (int)resultadoPaginas).Take((int)resultadoPaginas).ToList();
            var paginasTotal = (int)totalDePaginas;
            return new Response<Produto>(produtosPaginados, pagina, paginasTotal);
        }

        public async Task<bool> removerProduto(int? id)
        {
            try
            {
                var produto = await _db.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto == null) return false;
                _db.Remove(produto);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não é possivel realizar esta operação, a mesma já foi realizada");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }
    }
}