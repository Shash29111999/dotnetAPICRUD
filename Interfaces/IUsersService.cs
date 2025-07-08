using TodoAPI.Contracts;
using TodoAPI.Models;
using TodoAPICS.Contracts;

namespace TodoAPICS.Interfaces
{
    public interface IUsersService
    {

        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task CreateUserAsync(CreateUserRequest request);
        Task UpdateUserAsync(Guid id, CreateUserRequest request);
        Task<Todo> DeleteUserAsync(Guid id);

    }
}
