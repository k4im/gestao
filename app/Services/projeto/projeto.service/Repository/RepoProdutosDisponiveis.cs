namespace projeto.service.Repository
{
    public class RepoProdutosDisponiveis : IRepoProdutosDisponiveis
    {
        public async Task<bool> adicionarProdutos(ProdutosDisponiveis model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    db.ProdutosEmEstoque.Add(model);
                    await db.SaveChangesAsync();
                    return true;
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public async Task<bool> atualizarProdutos(int id, ProdutosDisponiveis model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var produto = await db.ProdutosEmEstoque.FirstOrDefaultAsync(x => x.Id == id);
                    produto.Nome = model.Nome;
                    produto.Quantidade = model.Quantidade;
                    await db.SaveChangesAsync();
                    return true;
                };

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel estar realizando a operação, a mesma já foi realizada por um outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel estar realizando a operação: {e.Message}");
                return false;
            }
        }

        public async Task<List<ProdutosDisponiveis>> buscarTodosProdutos()
        {
            using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
            {
                return await db.ProdutosEmEstoque.ToListAsync();
            };
        }

        public async Task<bool> removerProdutos(int id)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var item = await db.ProdutosEmEstoque.FirstOrDefaultAsync(x => x.Id == id);
                    db.ProdutosEmEstoque.Remove(item);
                    await db.SaveChangesAsync();
                    return true;
                };

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.Write($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public static async Task atualizarTabelaProdutosDisponiveis(Projeto model)
        {
            using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
            {
                var produto = await db.ProdutosEmEstoque.FirstOrDefaultAsync(x => x.Id == model.ProdutoUtilizado);
                produto.Quantidade -= model.QuantidadeUtilizado;
                await db.SaveChangesAsync();
            }
        }
    }
}