using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Models;

namespace MainBlog.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Posts> PostCreateAsync(Posts posts)
        {
            try
            {
                var newPost = await _unitOfWork.PostRepository.CreateAsync(posts);
                if (newPost != null)
                {
                    await _unitOfWork.Commit();
                    return newPost;
                }
                throw new Exception("Post não foi criado");
            }
            catch (Exception ex) {
                throw new Exception("Post não foi criado",ex);
            }
        }

        public async Task<IEnumerable<Posts>> GetAllPostsAsync()
        {
            return await _unitOfWork.PostRepository.GetAllAsync();
        }

        public async Task<Posts> GetPostByIdAsync(int id)
        {
            var post = await _unitOfWork.PostRepository.GetAsync(p => p.Id == id);
            if (post == null)
                throw new Exception("O Post não foi encontrado");
            return post;
        }
    }
}
