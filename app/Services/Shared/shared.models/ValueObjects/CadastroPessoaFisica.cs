using System.Text.RegularExpressions;

namespace shared.models.ValueObjects
{
    public class CadastroPessoaFisica
    {
        protected CadastroPessoaFisica()
        {}
        
        public CadastroPessoaFisica(string cpf)
        {
            Cpf = ValidarCpf(cpf);
        }

        public string Cpf { get;}

        public string ValidarCpf(string cpf)
        {
            if (!Regex.IsMatch(cpf, "^[0-9]*$")) throw new Exception("Insira apenas os numeros do cpf");
            if(string.IsNullOrEmpty(cpf)) throw new Exception("O cpf não pode estar nulo!");
            if(cpf.Length < 11) throw new Exception("O cpf deve conter no minimo 11 caracteres!");
            return cpf.Trim();
        }
    }
}