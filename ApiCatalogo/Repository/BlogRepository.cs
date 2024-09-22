using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.Models;

namespace MainBlog.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
