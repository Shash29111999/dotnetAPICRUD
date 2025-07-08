using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }
    }
}
