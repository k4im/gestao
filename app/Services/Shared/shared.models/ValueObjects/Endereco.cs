using System.Text.RegularExpressions;
using shared.models.Exceptions;

namespace shared.models.ValueObjects
{
    public class Endereco
    {
         public string Cidade { get; private set; }
        public string Bairro { get; private set; }
        public string Rua { get; private set; }
        public string Cep { get; private set; }
        public int Numero { get; private set; }


        protected Endereco()
        {}
        
        public Endereco(string cidade, string bairro, string rua, string cep, int numero)
        {

            Cidade = VerificarCidade(cidade);
            Bairro = VerificarBairro(bairro);
            Rua = VerificarRua(rua);
            Cep = cep;
            Numero = numero;
        }


        public string VerificarCidade(string cidade)
        {
            if(string.IsNullOrEmpty(cidade)) throw new CampoVazio("A cidade não pode estar vazia!");
            if(!Regex.IsMatch(cidade, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("A cidade não pode conter caracteres especiais");
            return cidade;
        }
        
        public string VerificarBairro(string bairro)
        {
            if(string.IsNullOrEmpty(bairro)) throw new CampoVazio("O bairro não pode estar vazio!");
            if(!Regex.IsMatch(bairro, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("O bairro não pode conter caracteres especiais");
            return bairro;
        }
        public string VerificarCep(string cep)
        {
            if(string.IsNullOrEmpty(cep)) throw new CampoVazio("O cep não pode estar vazio!");
            if(!Regex.IsMatch(cep, @"^[a-zA-Z]+$")) throw new CaracterInvalido("O cep não pode conter caracteres especiais");
            return cep;
        }
        public string VerificarRua(string rua)
        {
            if(string.IsNullOrEmpty(rua)) throw new CampoVazio("A rua não pode estar vazia!");
            // if (!Regex.IsMatch(rua, @"$[\\p{L}\\s]+$")) throw new CaracterInvalido("A rua não pode conter caracteres especiais");
            return rua;
        }
    }
}