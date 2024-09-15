using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappins;
using ApiCatalogo.Repository;
using ApiCatalogo.Repository.IRepository;
using AutoMapper;
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
                cfg.AddProfile(new ProdutoDTOMappingProfile());
            });
            mapper = config.CreateMapper();
            var context = new AppDbContext(dbContextOptions);
            unitOfWork = new UnitOfWork(context);
        }
    }
}
