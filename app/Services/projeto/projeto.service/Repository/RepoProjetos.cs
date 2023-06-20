namespace projeto.service.Repository
{
    public class RepoProjetos : IRepoProjetos
    {
        readonly DataContext _db;

        public RepoProjetos(DataContext db)
        {
            _db = db;
        }

        public async Task<bool> AtualizarStatus(StatusProjeto model, int? id)
        {
            try
            {
                var projeto = await _db.Projetos.FirstOrDefaultAsync(x => x.Id == id);
                projeto.AtualizarStatus(model);
                await _db.SaveChangesAsync();
                return true;
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
            var item = await _db.Projetos.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<Response<Projeto>> BuscarProdutos(int pagina, float resultadoPorPagina)
        {
            var ResultadoPorPagina = resultadoPorPagina;
            var projetos = await _db.Projetos.ToListAsync();
            var TotalDePaginas = Math.Ceiling(projetos.Count() / ResultadoPorPagina);
            var projetosPaginados = projetos.Skip((pagina - 1) * (int)ResultadoPorPagina).Take((int)ResultadoPorPagina).ToList();

            return new Response<Projeto>(projetosPaginados, pagina, (int)TotalDePaginas);
        }

        public async Task<bool> CriarProjeto(Projeto model)
        {
            try
            {
                _db.Projetos.Add(model);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Não foi possivel realizar a operação, a mesma já foi realizado por um outro usuario!");
                return false;
            }
            catch (Exception e)
            {
                throw new Exception($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeletarProjeto(int? id)
        {
            try
            {
                var item = await BuscarPorId(id);
                _db.Projetos.Remove(item);
                await _db.SaveChangesAsync();
                return true;
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