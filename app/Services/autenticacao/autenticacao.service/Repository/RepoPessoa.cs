namespace autenticacao.service.Repository
{
    public class RepoPessoa : IRepoPessoa
    {
        readonly DataContext _db;
        readonly Logger.Logger _logger;
        public RepoPessoa(DataContext db)
        {
            _db = db;
        }

        public RepoPessoa(DataContext db, Logger.Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> atualiarPessoa(int id, Pessoa pessoa)
        {
            try
            {
                var item = await buscarPessoaId(id);
                item = pessoa;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Pessoa> atualizarEndereco(int id, Endereco model)
        {
            try
            {
                var pessoa = await buscarPessoaId(id);
                pessoa.mudarEndereco(model);
                await _db.SaveChangesAsync();
                var pessoaAtualizada = await buscarPessoaId(id);
                return pessoaAtualizada;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Não foi possivel atualizar o dado!");
            }
        }

        public async Task<Pessoa> atualizarTelefone(int id, Telefone model)
        {
            try
            {
                var pessoa = await buscarPessoaId(id);
                pessoa.mudarTelefone(model);
                await _db.SaveChangesAsync();
                return await buscarPessoaId(id);
            }
            catch (Exception e)
            {
                _logger.logarFatal($"Não foi possivel realizar a operação: {e.Message}");
                throw new Exception("Não foi possivel atualizar o dado!");
            }
        }

        public async Task<Pessoa> buscarPessoaId(int id)
        {
            var pessoa = await _db.Pessoas.FirstOrDefaultAsync(x => x.Id == id);
            return pessoa;
        }

        public async Task<Response<Pessoa>> buscarPessoas(int pagina, float resultado)
        {
            var resultadoPaginas = resultado;
            var pessoas = await _db.Pessoas.ToListAsync();
            var totalDePaginas = Math.Ceiling(pessoas.Count() / resultadoPaginas);
            var pessoasPaginada = pessoas.Skip((pagina - 1) * (int)resultadoPaginas).Take((int)resultadoPaginas).ToList();
            var paginasTotal = (int)totalDePaginas;
            return new Response<Pessoa>(pessoasPaginada, pagina, paginasTotal);
        }

        public async Task<bool> criarPessoa(Pessoa pessoa)
        {
            try
            {
                _db.Pessoas.Add(pessoa);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.logarFatal($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }

        }

        public async Task<bool> deletarPessoa(int id)
        {
            try
            {
                var pessoa = await _db.Pessoas.FirstOrDefaultAsync(x => x.Id == id);
                _db.Remove(pessoa);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.logarFatal($"Não foi possivel realizar a operação: {e.Message}");
                return false;
            }
        }
    }
}