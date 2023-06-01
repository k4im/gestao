namespace autenticacao.service.Repository
{
    public interface IRepoPessoa
    {
        Task<bool> criarPessoa(Pessoa pessoa);
        Task<Pessoa> buscarPessoaId(int id);
        Task<Response<Pessoa>> buscarPessoas(int pagina, float resultado);
        Task<bool> atualiarPessoa(int id, Pessoa pessoa);
        Task<Pessoa> atualizarEndereco(int id, Endereco model);
        Task<Pessoa> atualizarTelefone(int id, Telefone model);
        Task<bool> deletarPessoa(int id);
    }
}