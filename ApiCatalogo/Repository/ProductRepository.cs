using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiCatalogo.Repository
{
    public class ProductRepository : Repository<Product> ,IProductRepository
    {
        
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetPaginatedProductListAsync(ProdutosParrameters produtosParameters)
        {
            var produtos = await GetAllAsync();

            var paginatedProductList = produtos
                .OrderBy(p => p.Name)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize);

            return paginatedProductList;
        }
    }
}
