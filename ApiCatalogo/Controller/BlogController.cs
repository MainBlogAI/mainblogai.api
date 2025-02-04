using AutoMapper;
using MainBlog.DTOs.Request;
using MainBlog.IService;
using MainBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BlogController(
            IMapper mapper,
            IBlogService blogService)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogs = await _blogService.GetAllBlogAsync();

            if(blogs == null)
                return NotFound();

            return Ok(_mapper.Map<List<BlogResponseDTO>>(blogs));
        }

        [HttpGet("getBlogByUserId/{userId}")]
        public async Task<IActionResult> getBlogByUserId(string userId)
        {
            var blogs = await _blogService.GetBlogByUserAsync(userId);

            if (blogs == null)
                return NotFound();

            return Ok(_mapper.Map<List<BlogResponseDTO>>(blogs));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequestDTO blogDTO)
        {
            var blog = _mapper.Map<Blog>(blogDTO);
            var blogResult = await _blogService.PostBlogAsync(blog);
            var blogResponseDTO = _mapper.Map<BlogResponseDTO>(blogResult);

            return Ok(blogResult);
        }

        [HttpGet("getBlogById/{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            var blog = await _blogService.GetByIdAsync(blogId);
            return Ok(_mapper.Map<BlogResponseDTO>(blog));
        }
    }
}
