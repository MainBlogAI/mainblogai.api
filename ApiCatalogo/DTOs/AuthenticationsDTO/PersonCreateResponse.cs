using MainBlog.Models;

namespace MainBlog.DTOs.AuthenticationsDTO
{
    public class PersonCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Date { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
