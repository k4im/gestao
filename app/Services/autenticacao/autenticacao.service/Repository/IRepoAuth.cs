namespace autenticacao.service.Repository
{
    public interface IRepoAuth
    {
        Task<Response<AppUser>> listarUsuarios(int pagina, float resultado);
        Task<ResponseRegistroDTO> registrarUsuario(NovoUsuarioDTO user);
        Task<bool> desativarUsuario(string chave);
        Task<ResponseLoginDTO> logar(LoginDTO loginModel);
        Task logOut();

    }
}