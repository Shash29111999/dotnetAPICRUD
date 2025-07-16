using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPICS.Contracts;
using TodoAPICS.Interfaces;

namespace TodoAPICS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUsersService userServices;

        public UserController(IUsersService userServices)
        {
            this.userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
        }


        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {

                await userServices.CreateUserAsync(request);
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
                var user = await userServices.GetAllAsync();
                if (user == null || !user.Any())
                {
                    return Ok(new { message = "No Todo Items  found" });
                }
                return Ok(new { message = "Successfully retrieved all user data", data = user });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving all Tood it posts", error = ex.Message });


            }   
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailsResponse>> GetTodoItem(Guid id)
        {
            var user = await userServices.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
