﻿namespace MainBlog.DTOs.Request
{
    public class ResetPasswordRequestDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
