using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using autenticacao.service.Exceptions;

namespace autenticacao.service.Models.ValueObjects
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

            VerificarCampos(cidade, bairro, rua, cep, numero);
            Cidade = cidade;
            Bairro = bairro;
            Rua = rua;
            Cep = cep;
            Numero = numero;
        }

       
        public void VerificarCampos(string cidade, string bairro, string cep, string rua, int numero)
        {
            VerificarCidade(cidade);
            VerificarBairro(bairro);
            VerificarRua(rua);
            VerificarCep(cep);
        }

        public void VerificarCidade(string cidade)
        {
            if(string.IsNullOrEmpty(cidade)) throw new CampoVazio("A cidade não pode estar vazia!");
            if(!Regex.IsMatch(cidade, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("A cidade não pode conter caracteres especiais");

        }
        
        public void VerificarBairro(string bairro)
        {
            if(string.IsNullOrEmpty(bairro)) throw new CampoVazio("O bairro não pode estar vazio!");
            if(!Regex.IsMatch(bairro, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("O bairro não pode conter caracteres especiais");

        }
        public void VerificarCep(string cep)
        {
            if(string.IsNullOrEmpty(cep)) throw new CampoVazio("O cep não pode estar vazio!");
            if(!Regex.IsMatch(cep, @"^[a-zA-Z]+$")) throw new CaracterInvalido("O cep não pode conter caracteres especiais");
            
        }
        public void VerificarRua(string rua)
        {
            if(string.IsNullOrEmpty(rua)) throw new CampoVazio("A rua não pode estar vazia!");
            // if (!Regex.IsMatch(rua, @"$[\\p{L}\\s]+$")) throw new CaracterInvalido("A rua não pode conter caracteres especiais");
        }
    }
}