using ApiCatalogo.Models;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mappins
{
    public class ProdutoDTOMappingProfile : Profile
    {
        public ProdutoDTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTOUpdateRequest>().ReverseMap();
            CreateMap<Product, ProdutoDTOUpdateResponse>().ReverseMap();
        }
    }
}
