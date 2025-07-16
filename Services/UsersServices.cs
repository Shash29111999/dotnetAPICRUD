using AutoMapper;

using Microsoft.EntityFrameworkCore;
using TodoAPI.AppDataContext;
using TodoAPI.Models;
using TodoAPI.Services;
using TodoAPICS.Contracts;
using TodoAPICS.Interfaces;

namespace TodoAPICS.Services
{
    public class UsersServices : IUsersService
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<UsersServices> _logger;
        private readonly IMapper _mapper;

        public UsersServices(TodoDbContext context, ILogger<UsersServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("Inside Create User");
            try
            {
                var user = _mapper.Map<User>(request);
              
                _context.UserAPI.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Todo item.");
                throw new Exception("An error occurred while creating the Todo item.");
            }
        }

        Task<Todo> IUsersService.DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.UserAPI.ToListAsync();
            if (users == null)
            {
                throw new Exception("No Todo items found");
            }
            return users;
        }

        public async Task<UserDetailsResponse> GetByIdAsync(Guid id)
        {
            var user = await _context.UserAPI.FindAsync(id);
            var userMapped = _mapper.Map<UserDetailsResponse>(user);

            if (userMapped == null)
            {
                throw new Exception("No Todo items found for ID: " + id);
            }

            return userMapped;
        }

        Task IUsersService.UpdateUserAsync(Guid id, CreateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
