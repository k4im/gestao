namespace autenticacao.service.Models
{
    public class Pessoa
    {
        protected Pessoa() {}
        public Pessoa(Nome nome, Endereco endereco, Telefone telefone, CadastroPessoaFisica cpf)
        {
            Nome = nome;
            Endereco = endereco;
            Telefone = telefone;
            Cpf = cpf;
        }
        [Key]
        public int Id { get; private set; }
        public Nome Nome { get;}
        public Endereco Endereco { get;}
        public Telefone Telefone { get;}
        public CadastroPessoaFisica Cpf {get;}

    }
}