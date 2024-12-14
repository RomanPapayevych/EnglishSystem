using EnglishSystem.Application.Common;
using EnglishSystem.Domain.Entities;


namespace EnglishSystem.Application.Interfaces
{
    public interface ITeacherService
    {
        Task<OperationResult> AssignTeacherAsync(int groupId, int teacherId);
        Task<IEnumerable<Group>> TeacherGroupsAsync(int teacherId);
        Task<OperationResult> RemoveTeacherFromGroup(int groupId);
        Task<List<ApplicationUser>> GetStudentsByGroupId(int groupId);
        //----------------------
        Task<Lesson> AddLessonToScheduleAsync(int scheduleId, DateTime date, string? topic, string? description);
        Task<Homework> AddHomeworkToLessonAsync(int lessonId, string content);
        Task<OperationResult> DeleteLessonAsync(int lessonId);
    }
}
