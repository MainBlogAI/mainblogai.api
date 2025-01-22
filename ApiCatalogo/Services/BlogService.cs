using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Models;

namespace MainBlog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<Blog> PostBlogAsync(Blog blog)
        {
           return await _blogRepository.CreateAsync(blog);
        }

        public async Task<IEnumerable<Blog>> GetAllBlogAsync()
        {
            return await _blogRepository.GetAllAsync();
        }

        public async Task<Blog> GetByIdAsync(string id)
        {
            var blog = await _blogRepository.GetAsync(b => b.UserId == id);
            if (blog == null)
            {
                throw new Exception("Blog not found");
            }
            return blog;
        }

        public async Task<List<Blog>> GetBlogByUserAsync(string UserId)
        {
            return await _blogRepository.GetBlogByUserAsync(UserId);
        }
    }
}
