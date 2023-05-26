namespace autenticacao.service.Repository
{
    public interface IRepoPessoa
    {
        Task<bool> criarPessoa(Pessoa pessoa);
        Task<Pessoa> buscarPessoaId(int id);
        Task<Response<Pessoa>> buscarPessoas(int pagina, float resultado);
        Task<bool> atualiarPessoa(Pessoa pessoa);
        Task<bool> deletarPessoa(int id);
    }
}