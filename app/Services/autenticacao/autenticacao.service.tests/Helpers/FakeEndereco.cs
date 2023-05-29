namespace autenticacao.service.tests.Helpers
{
    public class FakeEndereco
    {
        public static Endereco factoryFakeEndereco()
        {
            var random = new Random(); 
            const string pool = "abcdefghijklmnopqrstuvwxyz";
            var chars = Enumerable.Range(0, 5).Select(x => pool[random.Next(0, pool.Length)]);
            var randomValue = new string(chars.ToArray());
            return new Endereco($"{randomValue}", $"{randomValue}", $"{randomValue}", "15000", 0);
        }
    }
}