using MainBlog.Models;

namespace MainBlog.IService
{
    public interface IPostService
    {
        Task<IEnumerable<Posts>> GetAllPostsAsync();

        Task<Posts> PostCreateAsync(Posts posts);

        Task<Posts> GetPostByIdAsync(int id);
    }
}
