using System.ComponentModel.DataAnnotations;

namespace TechnicalServiceTask.Models
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
