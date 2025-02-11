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
        public async Task<List<GroupDTO>> GetAvailableGroupsAsync(int englishLevelId)
        {
            var groups = await _context.Groups.Where(g => g.EnglishLevelId == englishLevelId).Include(c => c.EnglishLevel).Include(g => g.Teacher).Include(c => c.Schedule).ToListAsync();
            var result = groups.Select(group => new GroupDTO
            {
                Id = group.Id,
                Name = group.Name!,
                StartTime = group.StartTime,
                EndTime = group.EndTime,
                StartTimeOfLesson = group.Schedule.StartTime,
                EndTimeOfLesson = group.Schedule.EndTime,
                EnglishLevelId = group.EnglishLevelId,
                EnglishLevel = group.EnglishLevel?.Level,
                Teacher = group.Teacher != null ? new { group.Teacher.Id, group.Teacher.FirstName, group.Teacher.LastName } : null,
                DaysOfWeek = group.Schedule.DaysOfWeek?.Select(day => day.ToString()).ToList() ?? new List<string>()
            }).ToList();
            return result;
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

        public async Task<List<NormalLessonWithHomeworkDTO>> GetLessonsWithHomeworkAsync(int groupId)
        {
            //var student = await _context.Users.Include(u => u.Group).ThenInclude(g => g!.Schedule).ThenInclude(g => g.Lessons).FirstOrDefaultAsync(u => u.Id == studentId);
            //if (student == null)
            //{
            //    throw new Exception("Student not found");
            //}
            //var lesson = await _context.Lessons.Include(l => l.HomeworkAssignments).Include(l => l.Schedule).Where(l => l.Schedule.Group.Students!.Any(s => s.Id == studentId)).FirstOrDefaultAsync();
            //if (lesson == null)
            //{
            //    throw new Exception("Lesson not found on this date");
            //}
            //var lessonWithHomeworkDto =  new LessonWithHomeworkDTO
            //{
            //    LessonId = lesson.Id,
            //    LessonDateTime = lesson.LessonDateTime,
            //    Topic = lesson.Topic,
            //    Description = lesson.Description,
            //    Homework = lesson.HomeworkAssignments.Select(h => new HomeworkDTO
            //    {
            //        Id = h.Id,
            //        Content = h.Content!,
            //    }).ToList()
            //};

            //return lessonWithHomeworkDto;
            var group = await _context.Groups.Include(g => g.Schedule).ThenInclude(s => s.Lessons)!.ThenInclude(l => l.HomeworkAssignments).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            var lesson = group!.Schedule.Lessons?.Select(l => new NormalLessonWithHomeworkDTO
            {
                Id = l.Id,
                Date = l.LessonDateTime,
                Topic = l.Topic,
                Description = l.Description,
                Homework = l.HomeworkAssignments.Select(hw => new HomeworkDTO
                {
                    Id = hw.Id,
                    Content = hw.Content!
                }).ToList()
            }).ToList();
            return lesson!;

        }

        public async Task<GroupDTO> MyGroup(int userId)
        {
            var group = await _context.Groups.Where(g => g.Students!.Any(s => s.Id == userId)).Select(g => new GroupDTO
            {
                Id = g.Id,
                Name = g.Name!,
                StartTime = g.Schedule!.StartTime,
                EndTime = g.Schedule.EndTime,
                StartTimeOfLesson = g.Schedule.StartTime,
                EndTimeOfLesson = g.Schedule.EndTime,
                EnglishLevelId = g.EnglishLevelId,
                EnglishLevel = g.EnglishLevel.Level,
                Teacher = g.Teacher != null ? new
                {
                    g.Teacher.Id,
                    g.Teacher.FirstName,
                    g.Teacher.LastName
                } : null,
                DaysOfWeek = g.Schedule.DaysOfWeek!.Select(day => day.ToString()).ToList()
            }).FirstOrDefaultAsync();
            if (group == null)
            {
                return null!;
            }
            return group;
        }
    }
}
