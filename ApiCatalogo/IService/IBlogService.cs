using MainBlog.Models;
using MainBlog.Pagination;

namespace MainBlog.IService
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogAsync();

        Task<Blog> PostBlogAsync(Blog blog);

        Task<Blog> GetByIdAsync(int id);

        Task<List<Blog>> GetBlogByUserAsync(string UserId);
    }
}
