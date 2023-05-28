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
            return  new CadastroPessoaFisica("012096332587");
        }
    }
}