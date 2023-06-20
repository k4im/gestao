using projeto.service.Models;
using projeto.service.Models.ValueObjects;

namespace projeto.service.tests.Helpers
{
    public class FakeProjeto
    {
        public static Projeto factoryProjeto()
        {
            var status = factoryProjetoStatus();
            var chapa = factoryChapaUtilizada();
            return new Projeto("Projeto FAKE", status, DateTime.UtcNow, DateTime.UtcNow.AddHours(1), chapa, "asdasd", 5, 155.5);
        }

        static StatusProjeto factoryProjetoStatus() => new StatusProjeto("Producao");

        static ChapaUtilizada factoryChapaUtilizada() => new ChapaUtilizada("Chapa branca");
    }
}