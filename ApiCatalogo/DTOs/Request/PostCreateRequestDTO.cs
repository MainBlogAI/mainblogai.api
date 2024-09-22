using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.DTOs.Response
{
    public class PostCreateRequestDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string imageUrl { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }
        public int BlogId { get; set; }
    }
}
