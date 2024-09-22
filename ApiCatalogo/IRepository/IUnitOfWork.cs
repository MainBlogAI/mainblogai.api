using MainBlog.IService;

namespace MainBlog.IRepository;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    IProductService ProductService { get; }

    IBlogService BlogService { get; }

    IPostService PostService { get; }
    void Commit();
}
