using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainBlog.Models
{

    [Table("Blog")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }

        public List<Posts> Posts { get; set; }
    }
}
