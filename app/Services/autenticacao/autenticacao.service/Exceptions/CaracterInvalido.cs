using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autenticacao.service.Exceptions
{
    public class CaracterInvalido : Exception
    {
        public CaracterInvalido(string message) : base(message)
        {
        }
    }
}