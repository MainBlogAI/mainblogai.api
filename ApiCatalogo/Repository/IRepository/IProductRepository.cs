using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repository.IRepository;

public interface IProductRepository : IRepository<Product>
{
     Task<IEnumerable<Product>> GetPaginatedProductListAsync(ProdutosParrameters produtosParameters);
}
