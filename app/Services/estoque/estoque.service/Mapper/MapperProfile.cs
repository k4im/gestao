using AutoMapper;

namespace estoque.service.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Produto, ProdutoDisponivel>();
        }
    }
}