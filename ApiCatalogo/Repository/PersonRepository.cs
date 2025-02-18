using MainBlog.Context;
using MainBlog.IRepository;
using MainBlog.Models;

namespace MainBlog.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context)
        {
        }
    }
}
