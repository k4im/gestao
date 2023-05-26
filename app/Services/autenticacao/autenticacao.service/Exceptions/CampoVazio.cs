using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autenticacao.service.Exceptions
{
    public class CampoVazio : Exception
    {
        public CampoVazio(string message) : base(message)
        {
        }
    }
}