using Microsoft.AspNetCore.Mvc;
using TodoAPI.Contracts;
using TodoAPI.Interface;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoServices _todoServices;

        public TodoController(ITodoServices todoServices)
        {
            _todoServices = todoServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync(CreateTodoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {

                await _todoServices.CreateTodoAsync(request);
                return Ok(new { message = "Blog post successfully created" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var todo = await _todoServices.GetAllAsync();
                if (todo == null || !todo.Any())
                {
                    return Ok(new { message = "No Todo Items  found" });
                }
                return Ok(new { message = "Successfully retrieved all blog posts", data = todo });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving all Tood it posts", error = ex.Message });


            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoItem(Guid id)
        {
            var todo = await _todoServices.GetByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                var todo = await _todoServices.GetByIdAsync(id);
                if (todo == null)
                {
                    return NotFound(new { message = "Todo item not found" });
                }

                await _todoServices.DeleteTodoAsync(id);
                return Ok(new { message = "Todo item successfully deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the Todo item", error = ex.Message });
            }
        }




    }
}