using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByName(string nameCategory);
        Task<IEnumerable<Category>> CatCategories();

        Task<PagedList<Category>> GetAllCategoriasAsync(CategoryParameters categoriaParameters);
    }
}
