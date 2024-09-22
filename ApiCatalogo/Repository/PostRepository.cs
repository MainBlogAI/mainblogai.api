using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.Models;

namespace MainBlog.Repository
{
    public class PostRepository : Repository<Posts>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }
    }
}
