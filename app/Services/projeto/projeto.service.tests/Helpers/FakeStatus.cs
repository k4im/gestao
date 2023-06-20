using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projeto.service.Models.ValueObjects;

namespace projeto.service.tests.Helpers
{
    public static class FakeStatus
    {
        public static StatusProjeto factoryStatus() => new StatusProjeto("Producao");
    }
}