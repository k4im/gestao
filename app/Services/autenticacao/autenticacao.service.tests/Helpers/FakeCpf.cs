using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autenticacao.service.tests.Helpers
{
    public class FakeCpf
    {
        public static CadastroPessoaFisica factoryCpf()
        {
            var random = new Random(); 
            const string pool = "1234567890";
            var chars = Enumerable.Range(0, 11).Select(x => pool[random.Next(0, pool.Length)]);
            var randomValue = new string(chars.ToArray());
            return  new CadastroPessoaFisica($"{randomValue}");
        }
    }
}