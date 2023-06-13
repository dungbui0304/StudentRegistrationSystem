using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Users;
using StudentRegistration.ViewModel.Users;

namespace StudentRegistration.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var token = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(token))
                return BadRequest("Username or password is incorrect");
            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            if (users == null)
                return BadRequest("Cannot load list user");
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Register(request);
            if (!result)
                return BadRequest("Cannot create user");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            if (!result)
                return BadRequest($"Cannot delete user wiht id = {id}");
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return BadRequest($"Cannot find user wiht id = {id}");
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateRequest request)
        {
            var result = await _userService.Update(id, request);
            if (!result)
                return BadRequest($"Cannot update user wiht id = {id}");
            return Ok(result);
        }
    }
}
