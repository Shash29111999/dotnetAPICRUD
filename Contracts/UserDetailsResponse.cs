

namespace TodoAPICS.Contracts
{
    public class UserDetailsResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}