using projeto.service.Models;
using projeto.service.Models.ValueObjects;

namespace projeto.service.tests.Helpers
{
    public class FakeProjeto
    {
        public static Projeto factoryProjeto()
        {
            var status = factoryProjetoStatus();
            return new Projeto("Projeto FAKE", status, DateTime.UtcNow, DateTime.UtcNow.AddHours(1), 1, 5, "asd", 155.5);
        }

        static StatusProjeto factoryProjetoStatus() => new StatusProjeto("Rodando");

    }
}