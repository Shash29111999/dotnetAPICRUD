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
        private readonly ILogger<TodoServices> _logger;
        private readonly IMapper _mapper;

        public UsersServices(TodoDbContext context, ILogger<TodoServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
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
                throw new Exception(" No Todo items found");
            }
            return users;
        }

        Task<User> IUsersService.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task IUsersService.UpdateUserAsync(Guid id, CreateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
