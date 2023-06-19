namespace projeto.service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Projeto, ProjetoDTO>();
        }
    }
}