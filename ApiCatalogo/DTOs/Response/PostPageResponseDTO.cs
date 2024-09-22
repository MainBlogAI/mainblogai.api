using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.DTOs.Response
{
    public class PostPageResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }
        public int BlogId { get; set; }
    }
}
