using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainBlog.Models
{
    [Table("Address")]
    public class Address
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }

        [JsonIgnore]
        public Person Person { get; set; }
    }
}
