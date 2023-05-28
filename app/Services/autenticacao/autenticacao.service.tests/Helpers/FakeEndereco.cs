namespace autenticacao.service.tests.Helpers
{
    public class FakeEndereco
    {
        public static Endereco factoryFakeEndereco()
        {
            return new Endereco("Cidade", "Bairro", "rua", "15000", 0);
        }
    }
}