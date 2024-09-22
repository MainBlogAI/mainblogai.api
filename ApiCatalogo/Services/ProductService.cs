using MainBlog.Models;
using MainBlog.Pagination;
using MainBlog.Repository;
using MainBlog.IRepository;
using MainBlog.IService;

namespace MainBlog.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetPaginatedProductListAsync(ProdutosParrameters produtosParameters)
        {

            var produtos = await _productRepository.GetAllAsync();
            var paginatedProductList = produtos.OrderBy(p => p.Name)
            .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            .Take(produtosParameters.PageSize);

            return paginatedProductList;
        }
    }
}
