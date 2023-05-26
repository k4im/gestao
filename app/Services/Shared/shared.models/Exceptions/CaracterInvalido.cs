using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shared.models.Exceptions
{
    public class CaracterInvalido : Exception
    {
        public CaracterInvalido(string message) : base(message)
        {
        }
    }
}