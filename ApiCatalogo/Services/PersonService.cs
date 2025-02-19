using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Models;

namespace MainBlog.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Person> CreatePersonAsync(Person person)
        {
            var newPerson = new Person();
            try
            {
                newPerson = await _unitOfWork.PersonRepository.CreateAsync(person);
            } catch(Exception ex){
                throw new Exception(ex.Message, ex);
            }
            finally{
                if (newPerson != null)
                {
                    await _unitOfWork.Commit();
                }
                else
                {
                    await _unitOfWork.RollBack();
                }
            }

            return newPerson;

        }
    }
}
