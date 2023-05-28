using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autenticacao.service.tests.Helpers
{
    public class FakeDbOptions
    {
        public static DbContextOptionsBuilder factoryDbInMemory()
        {
            return new DbContextOptionsBuilder().UseInMemoryDatabase(new Guid().ToString());
        }
    }
}