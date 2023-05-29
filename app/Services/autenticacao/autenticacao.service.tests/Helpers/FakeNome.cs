namespace autenticacao.service.tests.Helpers
{
    public class FakeNome
    {
        public static Nome factoryNome()
        {
            var random = new Random(); 
            const string pool = "abcdefghijklmnopqrstuvwxyz";
            var chars = Enumerable.Range(0, 5).Select(x => pool[random.Next(0, pool.Length)]);
            var randomValue = new string(chars.ToArray());
            return new Nome($"{randomValue}", $"{randomValue}");
        }
    }
}