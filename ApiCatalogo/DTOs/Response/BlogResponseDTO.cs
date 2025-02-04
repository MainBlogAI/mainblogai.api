using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MainBlog.Models;
using MainBlog.DTOs.Response;

namespace MainBlog.DTOs.Request
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public List<PostPageResponseDTO> Posts { get; set; }
    }
}
