namespace autenticacao.service.tests.Helpers
{
    public class FakeTelefone
    {
        public static Telefone factoryTelefone()
        {
            return new Telefone("55", "49", "5959595");
        }
    }
}