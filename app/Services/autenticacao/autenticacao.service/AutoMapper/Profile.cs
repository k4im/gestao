namespace autenticacao.service.AutoMapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<RefreshToken, RefreshTokenTable>();
            CreateMap<RefreshTokenTable, RefreshToken>();
        }
    }
}