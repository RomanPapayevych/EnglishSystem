using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EnglishSystem.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IdentityResult> SetEnglishLevelAsync(int userId, int englishLevelId);
        Task<IdentityResult> ChooseGroupAsync(int userId, int groupId);
        Task<List<Group>> GetAvailableGroupsAsync(int englishLevelId);
        Task<OperationResult> GetOutFromGroup(int groupId, int studentId);
        Task<LessonWithHomeworkDTO> GetLessonsWithHomeworkAsync(int studentI0d, DateTime lessonDate);
    }
}
