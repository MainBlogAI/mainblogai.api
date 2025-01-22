using System.Collections.Generic;
using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace MainBlog.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        protected readonly AppDbContext _context;
        public BlogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetBlogByUserAsync(string UserId)
        {
            var blogs = await _context.Set<Blog>().Where(x => x.UserId == UserId).ToListAsync();
           return blogs;
        }
    }
}
