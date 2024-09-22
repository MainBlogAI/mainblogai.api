using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Models;

namespace MainBlog.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Posts> PostCreateAsync(Posts posts)
        {
            return await _postRepository.CreateAsync(posts);
        }

        public async Task<IEnumerable<Posts>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Posts> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetAsync(p => p.Id == id);
            if (post == null)
                throw new Exception("O Post não foi encontrado");
            return post;
        }
    }
}
