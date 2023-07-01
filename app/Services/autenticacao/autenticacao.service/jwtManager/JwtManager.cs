namespace autenticacao.service.jwtManager
{
    public class jwtManager : IjwtManager
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IConfiguration _config;

        public jwtManager(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<string> criarAccessToken(string chaveDeAcesso)
        {
            var usuario = await _userManager.FindByNameAsync(chaveDeAcesso);

            var claims = gerarClaims(usuario);

            var secretKey = _config["Jwt:SecretKey"];
            var keyByte = Encoding.UTF8.GetBytes(secretKey);

            var key = new SymmetricSecurityKey(keyByte);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds,
                audience: _config["Jwt:Audience"],
                issuer: _config["Jwt:Issuer"]
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public List<Claim> gerarClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("email", user.Email),
                new Claim("role", user.Role),
                new Claim("key", _config["Jwt:Key"])
            };

            return claims; ;
        }
    }
}