using MainBlog.Models;

namespace MainBlog.IService
{
    public interface IPersonService
    {
        Task<Person> CreatePersonAsync(Person person);
        Task<Person> GetPersonByUserIdAsync(string userId);
    }
}
