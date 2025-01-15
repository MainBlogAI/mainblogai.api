using MainBlog.Context;
using MainBlog.DTOs.Mappins;
using MainBlog.Repository;
using AutoMapper;
using MainBlog.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class UnitTestController
    {
        public IUnitOfWork unitOfWork;
        public IMapper mapper;
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "server=localhost;port=3307;database=CatalogoDB;user=root;password=S3cure!P4ssw0rd#2024";
    
        static UnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseMySQL(connectionString).Options;
        }

        public UnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMappingProfile());
            });
            mapper = config.CreateMapper();
            var context = new AppDbContext(dbContextOptions);
        }
    }
}
