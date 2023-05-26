using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shared.models.Exceptions;

namespace shared.models.ValueObjects
{
    public class Nome
    {
        public Nome(string primeiroNome, string sobreNome)
        {
            PrimeiroNome = validarPrimeiroNome(primeiroNome);
            SobreNome = validarSobreNome(sobreNome);
        }
        protected Nome() {}

        public string PrimeiroNome { get;}
        public string SobreNome { get;}

        public string validarPrimeiroNome(string nome)
        {
            if(string.IsNullOrEmpty(nome)) throw new CampoVazio("O nome não pode estar vazio!");
            if(!Regex.IsMatch(nome, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("O nome não pode conter caracteres especiais");
            return nome;
        }

        public string validarSobreNome(string sobreNome)
        {
            if(string.IsNullOrEmpty(sobreNome)) throw new CampoVazio("O sobrenome não pode estar vazio!");
            if(!Regex.IsMatch(sobreNome, @"^[a-zA-Z ]+$")) throw new CaracterInvalido("O sobrenome não pode conter caracteres especiais");
            return sobreNome.Trim();
        }
    }
}