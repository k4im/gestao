namespace projeto.service.Repository
{
    public class RepoProjetos : IRepoProjetos
    {

        public async Task<bool> AtualizarStatus(StatusProjeto model, int? id)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var projeto = await db.Projetos.FirstOrDefaultAsync(x => x.Id == id);
                    projeto.AtualizarStatus(model);
                    await db.SaveChangesAsync();
                    return true;
                }

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

        public async Task<Projeto> BuscarPorId(int? id)
        {
            using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
            {
                var item = await db.Projetos.FirstOrDefaultAsync(x => x.Id == id);
                return item;
            }

        }

        public async Task<Response<Projeto>> BuscarProdutos(int pagina, float resultadoPorPagina)
        {
            using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
            {
                var ResultadoPorPagina = resultadoPorPagina;
                var projetos = await db.Projetos.ToListAsync();
                var TotalDePaginas = Math.Ceiling(projetos.Count() / ResultadoPorPagina);
                var projetosPaginados = projetos.Skip((pagina - 1) * (int)ResultadoPorPagina).Take((int)ResultadoPorPagina).ToList();

                return new Response<Projeto>(projetosPaginados, pagina, (int)TotalDePaginas);
            }
        }

        public async Task<bool> CriarProjeto(Projeto model)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    db.Projetos.Add(model);
                    await db.SaveChangesAsync();
                    return true;
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeletarProjeto(int? id)
        {
            try
            {
                using (var db = new DataContext(new DbContextOptionsBuilder().UseInMemoryDatabase("Data").Options))
                {
                    var item = await BuscarPorId(id);
                    db.Projetos.Remove(item);
                    await db.SaveChangesAsync();
                    return true;
                }

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
    }
}