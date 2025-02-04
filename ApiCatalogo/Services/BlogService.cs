using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Models;

namespace MainBlog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Blog> PostBlogAsync(Blog blog)
        {
            try
            {
                var blogResult = await _unitOfWork.BlogRepository.CreateAsync(blog);
                if (blogResult != null)
                    await _unitOfWork.Commit();

                return blogResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na criação do post", ex);
            }
        }

        public async Task<IEnumerable<Blog>> GetAllBlogAsync()
        {
            return await _unitOfWork.BlogRepository.GetAllAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetIncludesAsync(b => b.Id == id, b => b.Posts);
            if (blog == null)
            {
                throw new Exception("Blog not found");
            }
            return blog;
        }

        public async Task<List<Blog>> GetBlogByUserAsync(string UserId)
        {
            return await _unitOfWork.BlogRepository.GetBlogByUserAsync(UserId);
        }
    }
}
