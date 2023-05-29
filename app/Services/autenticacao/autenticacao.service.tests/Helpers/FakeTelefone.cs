namespace autenticacao.service.tests.Helpers
{
    public class FakeTelefone
    {
        public static Telefone factoryTelefone()
        {
            var random = new Random(); 
            const string pool = "1234567890";
            var chars = Enumerable.Range(0, 2).Select(x => pool[random.Next(0, pool.Length)]);
            var randomValue = new string(chars.ToArray());
            return new Telefone($"{randomValue}", $"{randomValue}", "5959595");
        }
    }
}