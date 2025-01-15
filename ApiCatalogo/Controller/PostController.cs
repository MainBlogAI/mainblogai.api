using AutoMapper;
using MainBlog.DTOs.Response;
using MainBlog.IRepository;
using MainBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _unitOfWork.PostService.GetAllPostsAsync();
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
            var newPost = await _unitOfWork.PostService.PostCreateAsync(post);
            if(newPost != null)
                await _unitOfWork.Commit();
           
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
            var postById = await _unitOfWork.PostService.GetPostByIdAsync(id);
            var result = _mapper.Map<PostPageResponseDTO>(postById);
            return Ok(result);
        }
    }
}
