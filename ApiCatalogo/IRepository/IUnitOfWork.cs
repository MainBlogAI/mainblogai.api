namespace MainBlog.IRepository;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    IBlogRepository BlogRepository { get; }

    IPostRepository PostRepository { get; }

    IPersonRepository PersonRepository { get; }

    Task Commit();
    Task RollBack();
}
