using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("CreateLevel")]
        public async Task<IActionResult> CreateEnglishLevel(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name is blank");
            }
            var result = await _adminService.CreateLevelAsync(name);
            if (!result.Succeeded)
            {
                return BadRequest("Level is already exists");
            }
            return Ok(result);
        }

        [HttpGet("AllLevels")]
        public async Task<IActionResult> GetAllLevelsAsync()
        {
            var result = await _adminService.GetAllLevelsAsync();
            return Ok(result);
        }

        [HttpGet("EnglishLevelById")]
        public async Task<IActionResult> GetAllLevelsAsync(int levelId)
        {
            var result = await _adminService.GetLevelByIdAsync(levelId);
            return Ok(result);
        }

        [HttpDelete("DeleteEnglishLevel/{levelId}")]
        public async Task<IActionResult> DeleteEnglishLevel(int levelId)
        {
            var result = await _adminService.DeleteLevelAsync(levelId);
            return Ok(result);
        }

        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO model)
        {
            var result = await _adminService.CreateGroupAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest("Create schedule refused :(");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteGroup/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var result = await _adminService.DeleteGroupAsync(groupId);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid id");
            }
            return Ok(result);
        }
        [HttpGet("AvaibleGroups")]
        public async Task<IActionResult> AllGroups()
        {
            var result = await _adminService.GetAllGroupsAsync();
            return Ok(result);
        }

    }
}
