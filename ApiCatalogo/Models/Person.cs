using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainBlog.Models
{
    [Table("Person")]
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf {  get; set; }
        public string Rg { get; set; }
        public string Date { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }


    }
}
