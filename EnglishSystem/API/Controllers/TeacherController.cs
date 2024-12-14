using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IAdminService _adminService;
        public TeacherController(ITeacherService teacherService, IAdminService adminService)
        {
            _teacherService = teacherService;
            _adminService = adminService;
        }

        [HttpPost("AssignTeacherForGroup")]
        public async Task<IActionResult> AssignTeacher([FromBody] AssignTeacherDTO model)
        {
            var teacher = await _teacherService.AssignTeacherAsync(model.GroupId, model.TeacherId);
            return Ok(teacher);
        }
        [HttpGet("teacher/{teacherId}/groups")]
        public async Task<IActionResult> AssignTeacher(int teacherId)
        {
            var teacher = await _teacherService.TeacherGroupsAsync(teacherId);
            return Ok(teacher);
        }
        [HttpPost("{groupId}/RemoveTeacherFromGroup")]
        public async Task<IActionResult> RemoveTeacherFromGroup(int groupId)
        {
            var teacher = await _teacherService.RemoveTeacherFromGroup(groupId);
            return Ok(teacher);
        }
        [HttpGet("{groupId}/GetStudentsFromGroup")]
        public async Task<IActionResult> GetStudentsFromGroup(int groupId)
        {
            var teacher = await _teacherService.GetStudentsByGroupId(groupId);
            return Ok(teacher);
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

        [HttpPost("{scheduleId}/Lesson")]
        public async Task<IActionResult> AddLesson(int scheduleId, [FromBody] LessonDTO model)
        {
            var result = await _teacherService.AddLessonToScheduleAsync(scheduleId, model.Date, model.Topic, model.Description);
            return Ok(result);
        }
        [HttpPost("{lessonId}/Homework")]
        public async Task<IActionResult> AddHomework(int lessonId, [FromBody] HomeworkDTO model)
        {
            var result = await _teacherService.AddHomeworkToLessonAsync(lessonId, model.Content);
            return Ok(result);
        }

        [HttpDelete("{lessonId}/RemoveLesson")]
        public async Task<IActionResult> RemoveLesson(int lessonId)
        {
            var result = await _teacherService.DeleteLessonAsync(lessonId);
            return Ok(result);
        }
    }   
}
