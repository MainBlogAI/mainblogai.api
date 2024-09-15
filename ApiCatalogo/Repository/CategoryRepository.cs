using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext context) :base(context) {}

        public async Task<PagedList<Category>> GetAllCategoriasAsync(CategoryParameters categoryParams)
        {
            var category = await GetAllAsync();
            var categoryQuery = category
                .OrderBy(c => c.CategoryId)
                .AsQueryable();
            var sortCategories = PagedList<Category>.ToPagedList(categoryQuery, categoryParams.PageNumber, categoryParams.PageSize);
            return sortCategories;
        }

        public async Task<Category> GetCategoryByName(string nameCategory)
        {
            return await _context.Category.FirstOrDefaultAsync(c => c.Name == nameCategory);
        }

        public async Task<IEnumerable<Category>> CatCategories()
        {
            return await _context.Category
                .Include(c => c.Product)  // Inclui os produtos relacionados a cada categoria
                .Where(c => c.CategoryId <= 5)  // Filtra as categorias cujo CategoriaId é menor ou igual a 5
                .AsNoTracking()  // Indica que as entidades retornadas não precisam ser rastreadas
                .ToListAsync();  // Converte a consulta em uma lista assíncrona       
        }
    }
}
