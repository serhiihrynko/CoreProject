﻿using System.ComponentModel.DataAnnotations;


namespace API.Models.Users
{
    public class CreateUserModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
