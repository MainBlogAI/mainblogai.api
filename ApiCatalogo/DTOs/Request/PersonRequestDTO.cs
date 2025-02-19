using MainBlog.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBlog.DTOs.Request
{
    public class PersonRequestDTO
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Date { get; set; }
        public string UserId { get; set; }
    }
}
