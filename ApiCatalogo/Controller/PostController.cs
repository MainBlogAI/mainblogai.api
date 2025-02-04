using AutoMapper;
using MainBlog.DTOs.Response;
using MainBlog.IService;
using MainBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(
            IMapper mapper,
            IPostService postService)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            var results = _mapper.Map<IList<PostAllResponseDTO>>(posts);
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> PostCreatePosts(PostCreateRequestDTO postRequest)
        {
            if (postRequest == null)
            {
                return BadRequest("O seu post não pode estar vazio");
            }
            var post = _mapper.Map<Posts>(postRequest);
            var newPost = await _postService.PostCreateAsync(post);
            var result = _mapper.Map<PostPageResponseDTO>(newPost);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O id é invalido");
            }
            var postById = await _postService.GetPostByIdAsync(id);
            var result = _mapper.Map<PostPageResponseDTO>(postById);
            return Ok(result);
        }
    }
}
