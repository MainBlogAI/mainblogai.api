using MainBlog.Models;

namespace MainBlog.IRepository;

public interface IBlogRepository : IRepository<Blog>
{
    Task<List<Blog>> GetBlogByUserAsync(string UserId);
}
