using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MainBlog.DTOs.Request
{
    public class BlogResponseDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }
    }
}
