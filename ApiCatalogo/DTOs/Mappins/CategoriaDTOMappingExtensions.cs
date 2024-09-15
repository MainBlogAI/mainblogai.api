using ApiCatalogo.Models;

namespace ApiCatalogo.DTOs.Mappins
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoryDTO ToCategoriaDTO(this Category categoria)
        {
            if(categoria == null)
            {
                return null;
            }
            return new CategoryDTO
            {
                CategoryId = categoria.CategoryId,
                Name = categoria.Name,
                ImgUrl = categoria.imgUrl
            };
        }
        public static Category? CategoriaDTO(this CategoryDTO categoriaDTO)
        {
            if(categoriaDTO == null)
            {
                return null;
            }
            return new Category
            {
                CategoryId = categoriaDTO.CategoryId,
                Name = categoriaDTO.Name,
                imgUrl = categoriaDTO.ImgUrl
            };
        }
        public static IEnumerable<CategoryDTO> ToCategoriaDTOList(this IEnumerable<Category> categorias)
        {
            if(categorias == null)
            {
                return new List<CategoryDTO>();
            }
            return categorias.Select(c => c.ToCategoriaDTO());
        }
    }
}
