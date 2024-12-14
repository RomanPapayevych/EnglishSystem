using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using EnglishSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public StudentService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> ChooseGroupAsync(int userId, int groupId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.EnglishLevelId == user.EnglishLevelId);
            if (group == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Group not available for this English level" });
            }
            user.GroupId = groupId;
            _context.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to assign group" });
        }
        public async Task<List<Group>> GetAvailableGroupsAsync(int englishLevelId)
        {
            var groups = await _context.Groups.Where(g => g.EnglishLevelId == englishLevelId).ToListAsync();
            return groups;
        }
        public async Task<IdentityResult> SetEnglishLevelAsync(int userId, int englishLevelId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }
            var level = await _context.EnglishLevel.FirstOrDefaultAsync(l => l.Id == englishLevelId);
            if (level == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "English level not found" });
            }
            user.EnglishLevelId = englishLevelId;
            _context.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to set English level" });
        }
        public async Task<OperationResult> GetOutFromGroup(int groupId, int studentId)
        {
            var group = await _context.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message ="Group not found",
                    Errors = new List<string>()
                };
            }
            var student = group.Students!.FirstOrDefault(g => g.Id == studentId);
            if (student == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Student not found",
                    Errors = new List<string>()
                };
            }
            group.Students?.Remove(student);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "You came out successfully",
            };
        }

        public async Task<LessonWithHomeworkDTO> GetLessonsWithHomeworkAsync(int studentId, DateTime lessonDate)
        {
            var student = await _context.Users.Include(u => u.Group).ThenInclude(g => g!.Schedule).ThenInclude(g => g.Lessons).FirstOrDefaultAsync(u => u.Id == studentId);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            var lesson = await _context.Lessons.Include(l => l.HomeworkAssignments).Include(l => l.Schedule).Where(l => l.Schedule.Group.Students!.Any(s => s.Id == studentId) && l.LessonDateTime.Date == lessonDate.Date).FirstOrDefaultAsync();
            if (lesson == null)
            {
                throw new Exception("Lesson not found on this date");
            }
            var lessonWithHomeworkDto = new LessonWithHomeworkDTO
            {
                LessonId = lesson.Id,
                LessonDateTime = lesson.LessonDateTime,
                Topic = lesson.Topic,
                Description = lesson.Description,
                Homework = lesson.HomeworkAssignments.Select(h => new HomeworkDTO
                {
                    Content = h.Content!,
                }).ToList()
            };

            return lessonWithHomeworkDto;

        }
    }
}
