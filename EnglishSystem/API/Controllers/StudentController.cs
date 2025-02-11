using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("SetEnglishLevel")]
        public async Task<IActionResult> SetEnglishLevel([FromBody] SetEnglishLevelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _studentService.SetEnglishLevelAsync(model.UserId, model.EnglishLevelId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "English level set successfully" });
        }

        [HttpGet("GetAvailableGroups/{englishLevelId}")]
        public async Task<IActionResult> GetAvailableGroups(int englishLevelId)
        {
            var groups = await _studentService.GetAvailableGroupsAsync(englishLevelId);
            if (groups == null || !groups.Any())
            {
                return NotFound(new { message = "No available groups for this level" });
            }
            return Ok(groups);
        }

        [HttpGet("MyGroup/{userId}")]
        public async Task<IActionResult> MyGroup(int userId)
        {
            var result = await _studentService.MyGroup(userId);
            if(result == null)
            {
                return BadRequest("Student not found in any group");
            }
            return Ok(result);
        }

        [HttpPost("ChooseGroup")]
        public async Task<IActionResult> ChooseGroup([FromBody] ChooseGroupDTO model)
        {
            var result = await _studentService.ChooseGroupAsync(model.UserId, model.GroupId);
            if (!result.Succeeded)
            {   
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "Group chosen successfully" });
        }

        [HttpDelete("{groupId}/GetOut/{studentId}")]
        public async Task<IActionResult> GetOutFromGroup(int groupId, int studentId)
        {
            var result = await _studentService.GetOutFromGroup(groupId, studentId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "You come out successfully" });
        }

        [HttpGet("{groupId}/GetLesson")]
        public async Task<IActionResult> GetLessonsWithHomeworkAsync(int groupId)
        {
            var result = await _studentService.GetLessonsWithHomeworkAsync(groupId);
            return Ok(result);
        }
    }
}
