using System.ComponentModel.DataAnnotations;

namespace TechnicalServiceTask.Models
{
    public class userLoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
