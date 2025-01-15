using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Services;
using MainBlog.Services.AuthenticationsServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MainBlog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;
        private readonly IPostService _postService;

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            IProductService productService,
            IBlogService blogService,
            IPostService postService)
        {
            _context = context;
            _productRepository = productRepository;
            _productService = productService;
            _blogService = blogService;
            _postService = postService;
        }

        public IProductRepository ProductRepository => _productRepository;
        public IProductService ProductService => _productService;
        public IBlogService BlogService => _blogService;
        public IPostService PostService => _postService;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void RollBack()
        {
            _context.Dispose();
        }
    }
}
