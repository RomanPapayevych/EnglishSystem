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

        //--------Teacher-------------

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

        [HttpDelete("{groupId}/RemoveTeacherFromGroup")]
        public async Task<IActionResult> RemoveTeacherFromGroup(int groupId)
        {
            var teacher = await _teacherService.RemoveTeacherFromGroup(groupId);
            return Ok(teacher);
        }

        //--------Students-------------

        [HttpGet("{groupId}/GetStudentsFromGroup")]
        public async Task<IActionResult> GetStudentsFromGroup(int groupId)
        {
            var teacher = await _teacherService.GetStudentsByGroupId(groupId);
            return Ok(teacher);
        }

        //--------Group-------------

        [HttpGet("AvaibleGroups")]
        public async Task<IActionResult> AvaibleGroups()
        {
            var teacher = await _teacherService.GetAllGroupsAsync();
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


        //--------Lesson-------------

        [HttpPost("{groupId}/Lesson")]
        public async Task<IActionResult> AddLesson(int groupId, [FromBody] LessonDTO model)
        {
            var result = await _teacherService.AddLessonToScheduleAsync(groupId, model.Date, model.Topic, model.Description);
            return Ok(result);
        }

        [HttpGet("LessonsOfGroup/{groupId}")]
        public async Task<IActionResult> GetLessonstOfGroup(int groupId)
        {
            var result = await _teacherService.GetLessonsOfGroup(groupId);
            return Ok(result);
        }

        [HttpPut("{lessonId}/UpdateLesson")]
        public async Task<IActionResult> UpdateLesson(int lessonId, [FromBody] LessonDTO model)
        {
            var result = await _teacherService.UpdateLesson(lessonId, model.Date, model.Topic, model.Description);
            return Ok(result);
        }

        [HttpDelete("{lessonId}/RemoveLesson")]
        public async Task<IActionResult> RemoveLesson(int lessonId)
        {
            var result = await _teacherService.DeleteLessonAsync(lessonId);
            return Ok(result);
        }



        //--------Homework-------------

        [HttpPost("{lessonId}/Homework")]
        public async Task<IActionResult> AddHomework(int lessonId, [FromBody] HomeworkDTO model)
        {
            var result = await _teacherService.AddHomeworkToLessonAsync(lessonId, model.Content);
            return Ok(result);
        }

        [HttpDelete("{homeworkId}/RemoveHomework")]
        public async Task<IActionResult> RemoveHomework(int homeworkId)
        {
            var deleteHomework = await _teacherService.DeleteHomework(homeworkId);
            return Ok(deleteHomework);
        }
        [HttpPut("{homeworkId}/UpdateHomework")]
        public async Task<IActionResult> UpdateHomework(int homeworkId, [FromBody] HomeworkDTO model)
        {
            var updateHomework = await _teacherService.UpdateHomework(homeworkId, model.Content);
            return Ok(updateHomework);
        }
    }   
}
