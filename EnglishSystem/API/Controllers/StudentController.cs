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
        //[HttpPost("assign-group")]
        //public async Task<IActionResult> AssignGroup([FromBody] AssignGroupDTO model)
        //{
        //    var result = await _studentService.AssignGroupToUserAsync(model.UserId, model.GroupId);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //    return Ok(new { message = "Group assigned successfully" });
        //}

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

        [HttpGet("{studentId}/GetLesson")]
        public async Task<IActionResult> GetLessonsWithHomeworkAsync(int studentId, DateTime lessonDate)
        {
            var result = await _studentService.GetLessonsWithHomeworkAsync(studentId, lessonDate);
            return Ok(result);
        }
    }
}
