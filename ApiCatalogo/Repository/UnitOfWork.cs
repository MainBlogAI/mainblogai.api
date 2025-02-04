using MainBlog.Context;
using MainBlog.IRepository;

namespace MainBlog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            IBlogRepository blogRepository,
            IPostRepository postRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _blogRepository = blogRepository;
            _postRepository = postRepository;
        }

        public IProductRepository ProductRepository => _productRepository;
        public IBlogRepository BlogRepository => _blogRepository;
        public IPostRepository PostRepository => _postRepository;

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
