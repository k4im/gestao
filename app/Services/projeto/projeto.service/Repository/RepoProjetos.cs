namespace projeto.service.Repository
{
    public class RepoProjetos : IRepoProjetos
    {
        //Delegates e Eventos a serem disparados
        public delegate void aoCriarProjetoEventHandler(Projeto model);
        public delegate void aoRealizarOperacaoEventHandler(string message);
        public event aoRealizarOperacaoEventHandler aoRealizarOperacao;
        public event aoCriarProjetoEventHandler aocriarProjeto;

        IMessageBusService _messageBroker;

        public RepoProjetos(IMessageBusService messageBroker)
        {
            _messageBroker = messageBroker;
            aocriarProjeto += async (Projeto model) => { await RepoProdutosDisponiveis.atualizarTabelaProdutosDisponiveis(model); };
            aocriarProjeto += messageBroker.enviarProjeto;
        }

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
                    if (await db.ProdutosEmEstoque.FirstOrDefaultAsync(x => x.Id == model.ProdutoUtilizado) == null)
                    {
                        throw new Exception($"Enterrompido criação do projeto pois produto com [id] - [{model.ProdutoUtilizado}] não existe!");
                    }
                    db.Projetos.Add(model);
                    await db.SaveChangesAsync();
                    aocriarProjeto(model);
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