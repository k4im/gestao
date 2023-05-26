using shared.models.ValueObjects;

namespace autenticacao.service.Models
{
    public class Pessoa
    {
        public Pessoa(Nome nome, Endereco endereco, Telefone telefone, CadastroPessoaFisica cpf)
        {
            Nome = nome;
            Endereco = endereco;
            Telefone = telefone;
            Cpf = cpf;
        }

        public Nome Nome { get;}
        public Endereco Endereco { get;}
        public Telefone Telefone { get;}
        public CadastroPessoaFisica Cpf {get;}

    }
}