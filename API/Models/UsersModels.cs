using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CreateUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserName { get; set; }
    }

    public class CreateUserResponse
    {
        public string UserId { get; set; }
    }
}
