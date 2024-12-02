namespace MainBlog.DTOs.Request
{
    public class RequestEmailDTO
    {
        public string Email { get; set; }

        public RequestEmailDTO(string Email)
        {
            this.Email = Email;
        }
    }
}
