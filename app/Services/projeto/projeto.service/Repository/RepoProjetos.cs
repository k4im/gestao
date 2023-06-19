namespace projeto.service.Repository
{
    public class RepoProjetos : IRepoProjetos
    {
        public Task<bool> AtualizarStatus(StatusProjeto model, int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Projeto> BuscarPorId(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Projeto>> BuscarProdutos(int pagina, float resultadoPorPagina)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CriarProjeto(Projeto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletarProjeto(int? id)
        {
            throw new NotImplementedException();
        }
    }
}