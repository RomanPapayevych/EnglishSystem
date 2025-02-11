using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishSystem.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IdentityResult> SetEnglishLevelAsync(int userId, int englishLevelId);
        Task<IdentityResult> ChooseGroupAsync(int userId, int groupId);
        Task<List<GroupDTO>> GetAvailableGroupsAsync(int englishLevelId);
        Task<GroupDTO> MyGroup(int userId);
        Task<OperationResult> GetOutFromGroup(int groupId, int studentId);
        Task<List<NormalLessonWithHomeworkDTO>> GetLessonsWithHomeworkAsync(int groupId);
    }
}
