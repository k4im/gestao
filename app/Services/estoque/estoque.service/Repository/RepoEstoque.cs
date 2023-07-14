namespace estoque.service.Repository
{
    public class RepoEstoque : IRepoEstoque
    {
        readonly IMessagePublisher _publisher;
        delegate void produtoEventHandler(Produto model);
        event produtoEventHandler aoCriarProduto;
        event produtoEventHandler aoDeletarProduto;
        event produtoEventHandler aoAtualizarProduto;

        public RepoEstoque(IMessagePublisher publisher)
        {
            _publisher = publisher;
            aoCriarProduto += _publisher.publicarProduto;
            aoAtualizarProduto += _publisher.atualizarProduto;
            aoDeletarProduto += _publisher.deletarProduto;
        }

        public async Task<bool> adicionarProduto(Produto model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    db.Produtos.Add(model);
                    await db.SaveChangesAsync();
                    aoCriarProduto.Invoke(model);
                    return true;
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizada por outro usuario!");
                return false;
            }
            catch (ArgumentException e)
            {
                // Caso o software tente adicionar um item mais de uma vez 
                // problema identificado com o IHostServiced rodando em background
                Console.WriteLine($"Debug: {e.Message}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação no repo: {e.Message}");
                return false;
            }
        }

        public async Task<bool> atualizarProduto(int? id, Produto model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var produto = await db.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                    produto.atualizarProduto(model);
                    await db.SaveChangesAsync();
                    aoAtualizarProduto.Invoke(produto);
                    return true;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizada por outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação no repo: {e.Message}");
                return false;
            }
        }

        public async Task<Produto> buscarProdutoId(int? id)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var produto = await db.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                    if (produto == null) return null;
                    return produto;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Response<Produto>> buscarProdutos(int pagina, float resultado)
        {
            using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
            {
                var resultadoPaginas = resultado;
                var pessoas = await db.Produtos.ToListAsync();
                var totalDePaginas = Math.Ceiling(pessoas.Count() / resultadoPaginas);
                var produtosPaginados = pessoas.Skip((pagina - 1) * (int)resultadoPaginas).Take((int)resultadoPaginas).ToList();
                var paginasTotal = (int)totalDePaginas;
                return new Response<Produto>(produtosPaginados, pagina, paginasTotal);
            }

        }

        public async Task<bool> removerProduto(int? id)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var produto = await db.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                    if (produto == null) return false;
                    db.Remove(produto);
                    await db.SaveChangesAsync();
                    aoDeletarProduto.Invoke(produto);
                    return true;
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não é possivel realizar esta operação, a mesma já foi realizada");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação no repo: {e.Message}");
                return false;
            }
        }

        public async Task atualizarEstoque(ProjetoDTO model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var produto = await db.Produtos.FirstOrDefaultAsync(x => x.Id == model.ProdutoUtilizado);
                    if (produto == null) Console.WriteLine("Produto nulo");
                    produto.diminuirQuantidade(model.QuantidadeUtilizado);
                    await db.SaveChangesAsync();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não é possivel realizar esta operação, a mesma já foi realizada");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação no repo: {e.Message}");
            }
        }
    }
}