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
        private readonly IPersonRepository _personRepository;

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            IBlogRepository blogRepository,
            IPostRepository postRepository,
            IPersonRepository personRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _blogRepository = blogRepository;
            _postRepository = postRepository;
            _personRepository = personRepository;
        }

        public IProductRepository ProductRepository => _productRepository;
        public IBlogRepository BlogRepository => _blogRepository;
        public IPostRepository PostRepository => _postRepository;
        public IPersonRepository PersonRepository => _personRepository;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            await _context.DisposeAsync();
        }
    }
}
