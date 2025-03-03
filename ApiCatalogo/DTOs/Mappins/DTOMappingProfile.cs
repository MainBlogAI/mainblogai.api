using MainBlog.Models;
using AutoMapper;
using MainBlog.DTOs.Request;
using MainBlog.DTOs.Response;
using MainBlog.DTOs.AuthenticationsDTO;

namespace MainBlog.DTOs.Mappins
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTOUpdateRequest>().ReverseMap();
            CreateMap<Product, ProdutoDTOUpdateResponse>().ReverseMap();
            CreateMap<Blog, BlogRequestDTO>().ReverseMap();
            CreateMap<BlogResponseDTO, Blog>().ReverseMap();
            CreateMap<Posts, PostPageResponseDTO>().ReverseMap();
            CreateMap<Posts, PostAllResponseDTO>().ReverseMap();
            CreateMap<Posts, PostCreateRequestDTO>().ReverseMap();
            CreateMap<Person, PersonRequestDTO>().ReverseMap();
            CreateMap<Person, PersonCreateResponse>().ReverseMap();
        }
    }
}
