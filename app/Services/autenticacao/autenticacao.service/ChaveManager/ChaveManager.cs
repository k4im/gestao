namespace autenticacao.service.ChaveManager
{
    public class ChaveManager : IChaveManager
    {
        readonly UserManager<AppUser> _userManager;

        public ChaveManager(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<string> gerarChaveDeAcesso()
        {
            var random = new Random(); 
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, 5).Select(x => pool[random.Next(0, pool.Length)]);
            var chaveRandom = new string(chars.ToArray());

            if(await checarChave(chaveRandom)) 
            {
                var chave = await gerarChaveDeAcesso();
                return chave;
            }     
            return chaveRandom;
        }

        async Task<bool> checarChave(string chave)
        {
            var usuario  = await _userManager.FindByNameAsync(chave);
            if(usuario != null) return true;
            return false;   
        }


    }
}