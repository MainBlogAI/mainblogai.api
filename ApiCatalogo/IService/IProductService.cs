using MainBlog.Models;
using MainBlog.Pagination;

namespace MainBlog.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetPaginatedProductListAsync(ProdutosParrameters produtosParameters);
    }
}
