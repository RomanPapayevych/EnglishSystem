using EnglishSystem.Application.Common;
using EnglishSystem.Application.DTOs;
using EnglishSystem.Application.Interfaces;
using EnglishSystem.Domain.Entities;
using EnglishSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnglishSystem.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;
        public TeacherService(ApplicationDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        public async Task<OperationResult> AssignTeacherAsync(int groupId, int teacherId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Group not found",
                    Errors = new List<string>()
                };
            }
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            if (teacher == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Teacher not found",
                    Errors = new List<string>()
                };
            }    
            group.TeacherId = teacher.Id;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Teacher choosed group successfully"
            };
        }
        public async Task<IEnumerable<GroupDTO>> TeacherGroupsAsync(int teacherId)
        {
            var groups = await _context.Groups.Where(t => t.TeacherId == teacherId).Include(g => g.Schedule).Include(g => g.EnglishLevel).Include(g => g.Teacher).ToListAsync();
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
        public async Task<OperationResult> RemoveTeacherFromGroup(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Group not found",
                    Errors = new List<string>()
                };
            }
            if (group.TeacherId == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "This group does not have an assigned teacher.",
                    Errors = new List<string>()
                };
            }
            group.TeacherId = null;
            group.Teacher = null;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Teacher has been successfully removed from the group.",
            };
        }

        public async Task<List<ApplicationUser>> GetStudentsByGroupId(int groupId)
        {
            var group = await _context.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            return group.Students!.ToList();
        }

        public async Task<Lesson> AddLessonToScheduleAsync(int groupId, DateTime date, string? topic, string? description)
        {
            var group = await _context.Groups.Include(s => s.Schedule).ThenInclude(s => s.Lessons).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            var schedule = group.Schedule;
            if (schedule == null)
            {
                throw new Exception("Schedule not found for the given group");
            }
            var lesson = new Lesson
            {
                LessonDateTime = date,
                Topic = topic,
                Description = description,
                ScheduleId = schedule.Id
            };
            schedule.Lessons!.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson> UpdateLesson(int lessonId, DateTime date, string? topic, string? description)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null)
            {
                throw new Exception("Lesson not found");
            }
            lesson!.LessonDateTime = date;
            lesson.Topic = topic;
            lesson.Description = description;
            _context.Update(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Homework> AddHomeworkToLessonAsync(int lessonId, string content)
        {
            var lesson = await _context.Lessons.Include(l => l.HomeworkAssignments).FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null)
            {
                throw new Exception("Lesson not found");
            }
            var homework = new Homework
            {
                Content = content,
                CreatedAt = DateTime.Now,
                Status = Domain.Enum.HomeworkStatus.Active,
                LessonId = lessonId
            };
            lesson.HomeworkAssignments.Add(homework);
            await _context.SaveChangesAsync();
            return homework;
        }

        public async Task<OperationResult> DeleteLessonAsync(int lessonId)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
            if(lesson == null)
            {
                return new OperationResult
                {
                    Succeeded = false,
                    Message = "Lesson not found",
                    Errors = new List<string>()
                };
            }
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return new OperationResult
            {
                Succeeded = true,
                Message = "Lesson deleted successfully"
            };
        }
        public async Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _context.Groups.Include(g => g.Schedule).Include(g => g.EnglishLevel).Include(g => g.Teacher).ToListAsync();
            var availableGroups = groups.Where(group => group.Teacher == null).Select(group => new GroupDTO
            {
                Id = group.Id,
                Name = group.Name!,
                StartTime = group.StartTime,
                EndTime = group.EndTime,
                StartTimeOfLesson = group.Schedule!.StartTime,
                EndTimeOfLesson = group.Schedule.EndTime,
                EnglishLevelId = group.EnglishLevelId,
                EnglishLevel = group.EnglishLevel?.Level,
                Teacher = group.Teacher != null ? new { group.Teacher.Id, group.Teacher.FirstName, group.Teacher.LastName } : null,
                DaysOfWeek = group.Schedule.DaysOfWeek?.Select(day => day.ToString()).ToList() ?? new List<string>()
            }).ToList();

            return availableGroups;
        }

        public async Task<List<NormalLessonWithHomeworkDTO>> GetLessonsOfGroup(int groupId)
        {
            var group = await _context.Groups.Include(g => g.Schedule).ThenInclude(s => s.Lessons)!.ThenInclude(l => l.HomeworkAssignments).FirstOrDefaultAsync(g => g.Id == groupId);
            if (group == null)
            {
                throw new Exception("Group not found");
            }
            
            var lesson = group!.Schedule.Lessons?.Select(l => new NormalLessonWithHomeworkDTO{
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

        public async Task<Homework> DeleteHomework(int homeworkId)
        {
            var homework = await _context.Homework.FindAsync(homeworkId);
            if(homework == null)
            {
                throw new Exception("Homework not found");
            }
            _context.Homework.Remove(homework);
            await _context.SaveChangesAsync();
            return homework;
        }

        public async Task<Homework> UpdateHomework(int homeworkId, string content)
        {
            var homework = await _context.Homework.FindAsync(homeworkId);
            if (homework == null)
            {
                throw new Exception("Homework not found");
            }
            homework.Content = content;
            _context.Homework.Update(homework);
            await _context.SaveChangesAsync();
            return homework;
        }
        
    }
}
