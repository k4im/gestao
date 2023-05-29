namespace autenticacao.service.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; }
        public bool FlagDesativado { get; set; }
    
        public void desativarUsuario()
        {
            this.FlagDesativado = true;
        }
    }
}