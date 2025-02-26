﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainBlog.Models
{
    [Table("Posts")]
    public class Posts
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string imageUrl { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; }
        [ForeignKey("Blog")]
        [JsonIgnore]
        public int BlogId { get; set; }
        [JsonIgnore]
        public Blog Blog { get; set; }
    }
}
