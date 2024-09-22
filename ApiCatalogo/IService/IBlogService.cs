using MainBlog.Models;
using MainBlog.Pagination;

namespace MainBlog.IService
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogAsync();

        Task<Blog> PostBlogAsync(Blog blog);

        Task<Blog> GetByIdAsync(string id);
    }
}
