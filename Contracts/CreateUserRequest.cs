using System.ComponentModel.DataAnnotations;

namespace TodoAPICS.Contracts
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(20)]
        public string? email { get; set; }

        [Required]
        public string? password { get; set; }
    }
}
