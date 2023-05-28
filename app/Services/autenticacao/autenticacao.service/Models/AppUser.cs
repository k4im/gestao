namespace autenticacao.service.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; }
        public bool Flag { get; set; }
    }
}