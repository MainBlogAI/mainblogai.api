using AutoMapper;
using MainBlog.DTOs.Request;
using MainBlog.IRepository;
using MainBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogs = await _unitOfWork.BlogService.GetAllBlogAsync();

            if(blogs == null)
                return NotFound();

            return Ok(_mapper.Map<List<BlogResponseDTO>>(blogs));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequestDTO blogDTO)
        {
            var blog = _mapper.Map<Blog>(blogDTO);
            var blogResult = _mapper.Map<BlogResponseDTO>(await _unitOfWork.BlogService.PostBlogAsync(blog));

            if(blogResult != null)
                _unitOfWork.Commit();

            return Ok(blogResult);
        }

        [HttpGet("getBlogById")]
        public async Task<IActionResult> GetBlogById([FromQuery]string userId)
        {
            var blog = await _unitOfWork.BlogService.GetByIdAsync(userId);
            return Ok(_mapper.Map<BlogResponseDTO>(blog));
        }
    }
}
