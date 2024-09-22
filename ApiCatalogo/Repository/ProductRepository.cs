using MainBlog.Context;
using MainBlog.Models;
using MainBlog.Pagination;
using MainBlog.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MainBlog.Repository
{
    public class ProductRepository : Repository<Product> ,IProductRepository
    {
        
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
