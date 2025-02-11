using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace EnglishSystem.Application.Interfaces
{
    public interface ITeacherService
    {
        //--------------Teacher----------------
        Task<OperationResult> AssignTeacherAsync(int groupId, int teacherId);
        Task<IEnumerable<GroupDTO>> TeacherGroupsAsync(int teacherId);
        Task<List<GroupDTO>> GetAllGroupsAsync();
        Task<OperationResult> RemoveTeacherFromGroup(int groupId);

        //--------------GetStudents----------------
        Task<List<ApplicationUser>> GetStudentsByGroupId(int groupId);

        //--------------Lesson----------------
        Task<Lesson> AddLessonToScheduleAsync(int groupId, DateTime date, string? topic, string? description);
        Task<Lesson> UpdateLesson(int lessonId, DateTime date, string? topic, string? description);
        Task<List<NormalLessonWithHomeworkDTO>> GetLessonsOfGroup(int groupId);
        Task<OperationResult> DeleteLessonAsync(int lessonId);

        //--------------Homework--------------
        Task<Homework> AddHomeworkToLessonAsync(int lessonId, string content);
        Task<Homework> DeleteHomework(int homeworkId);
        Task<Homework> UpdateHomework(int homeworkId, string content);
    }
}
