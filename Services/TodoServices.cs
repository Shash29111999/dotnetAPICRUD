using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoAPI.AppDataContext;
using TodoAPI.Contracts;
using TodoAPI.Interface;
using TodoAPI.Models;

namespace TodoAPI.Services
{
    

    public class TodoServices : ITodoServices
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<TodoServices> _logger;
        private readonly IMapper _mapper;

        public TodoServices(TodoDbContext context, ILogger<TodoServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task CreateTodoAsync(CreateTodoRequest request)
        {
            try
            {
                var todo = _mapper.Map<Todo>(request);
                todo.CreatedAt = DateTime.UtcNow;
                _context.TodoAPI.Add(todo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Todo item.");
                throw new Exception("An error occurred while creating the Todo item.");
            }
        }

        public async Task<Todo> DeleteTodoAsync(Guid id)
        {
                var todoItem = await _context.TodoAPI.FindAsync(id);
            if (todoItem == null)
            {
                _logger.LogError( "An error occurred while creating the Todo item.");
                throw new Exception("An error occurred while creating the Todo item.");
            }

            _context.TodoAPI.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

       public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            var todo= await _context.TodoAPI.ToListAsync();
            if (todo == null)
            {
                throw new Exception(" No Todo items found");
            }
            return todo;

        }

        public async Task<Todo> GetByIdAsync(Guid id)
        {
            var todoItem = await _context.TodoAPI.FindAsync(id);

            if (todoItem == null)
            {
                throw new Exception( "No Todo items found for ID: "+id);
            }

            return todoItem;
        }

        public Task UpdateTodoAsync(Guid id, UpdateTodoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}