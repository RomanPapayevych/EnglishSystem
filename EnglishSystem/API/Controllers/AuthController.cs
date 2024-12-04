using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthController(IUserService userService, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.LoginAsync(loginDTO);
            if (user.Succeeded)
            {
                var tokenString = _userService.GenerateToken(loginDTO);
                return Ok(tokenString);
            }
            return BadRequest("Invalid email or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //bool ifUnique = _userService.isUniqueUser(registerDTO.FirstName!);
            //if (!ifUnique)
            //{
            //    return BadRequest("Username already exist");
            //}
            var register = await _userService.RegisterAsync(registerDTO);
            if (!register.Succeeded)
            {
                return BadRequest("User alread exists");
            }
            return Ok(register);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _signInManager.SignOutAsync();
            return Ok("Logout");
        }
        [HttpDelete("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string name)
        {
            if (ModelState.IsValid)
            {
                var delete = await _userService.DeleteUser(name);
                return Ok(delete);
            }
            return BadRequest("Wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _userService.AssignRoleToUserAsync(request.UserId!, request.RoleName!);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("users-with-roles")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetUsersWithRoles()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something go wrong");
            }
            var getUsers = await _userService.GetUsersWithRoles();
            return Ok(getUsers);
        }
    }
}
