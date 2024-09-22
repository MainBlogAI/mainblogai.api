using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Services;
using Microsoft.EntityFrameworkCore;

namespace MainBlog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext _context;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBlogService> _blogService;
        private readonly Lazy<IPostService> _postService;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(_context));
            _productService = new Lazy<IProductService>(() => new ProductService(_productRepository.Value));
            _blogService = new Lazy<IBlogService>(() => new BlogService(new BlogRepository(_context)));
            _postService = new Lazy<IPostService>(() => new PostService(new PostRepository(_context)));
        }

        public IProductRepository ProductRepository => _productRepository.Value;
        public IProductService ProductService => _productService.Value;
        public IBlogService BlogService => _blogService.Value;
        public IPostService PostService => _postService.Value;

        public void Commit()
        {
             _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
