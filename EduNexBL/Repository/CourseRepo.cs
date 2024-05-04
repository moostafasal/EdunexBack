using AutoMapper;
using EduNexBL.Base;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class CourseRepo : Repository<Course>, ICourse
    {
        private readonly IMapper _mapper;
        private readonly EduNexContext _context;

        public CourseRepo(EduNexContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<ICollection<CourseMainData>> GetAllCoursesMainData()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                    .ThenInclude(s => s.Level)
                .ToListAsync();

            return _mapper.Map<List<CourseMainData>>(courses);
        }

        public async Task<CourseDTO?> GetCourseById(int courseId)
        {
            var course = await _context.Courses
                          .Include(c => c.Teacher)
                          .Include(c => c.Subject)
                              .ThenInclude(s => s.Level)
                          .Include(c => c.Lectures)
                              .ThenInclude(l => l.Attachments)
                          .Include(c => c.Lectures)
                              .ThenInclude(l => l.Videos)
                            .Include(c => c.Lectures)
                              .ThenInclude(l => l.Exams)
                          .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return null; // Course with given ID not found

            return MapCourseToCourseDTO(course);
        }

        public CourseDTO MapCourseToCourseDTO(Course course)
        {
            return new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Thumbnail = course.Thumbnail,
                CourseType = course.CourseType.ToString(), // Convert enum to string
                Price = course.Price,
                SubjectName = course.Subject?.SubjectName ?? "", // Assuming Subject has a Name property
                TeacherName = $"{course.Teacher?.FirstName} {course.Teacher?.LastName}", // Assuming Teacher has a Name property
                ProfilePhoto = course.Teacher?.ProfilePhoto ?? "", // Assuming Teacher has a ProfilePhoto property
                LevelName = course.Subject?.Level?.LevelName ?? "", // Assuming Subject has a Level property and Level has a Name property
                LectureList = course.Lectures.Select(MapLectureToLectureDTO).ToList()
            };
        }

        public LectureDto MapLectureToLectureDTO(Lecture lecture)
        {
            return new LectureDto
            {
                Id = lecture.Id,
                LectureTitle = lecture.LectureTitle,
                Price = lecture.Price,
                CourseId = lecture.CourseId,
                Videos = lecture.Videos?.Select(MapVideoToVideoDTO).ToList(),
                Attachments = lecture.Attachments?.Select(MapAttachmentToAttachmentDTO).ToList(),
                PreExam = lecture.Exams?.FirstOrDefault(e => e.Type == "PreExam")?.Id, // Get the first exam with type "PreExam"
                Assignment = lecture.Exams?.FirstOrDefault(a => a.Type == "Assignment")?.Id
            };
        }

        public VideoDTO MapVideoToVideoDTO(Video video)
        {
            return new VideoDTO
            {
                id = video.Id,
                VideoTitle = video.VideoTitle,
                VideoPath = video.VideoPath,
            };
        }

        public AttachmentDto MapAttachmentToAttachmentDTO(AttachmentFile attachment)
        {
            return new AttachmentDto
            {
                Id = attachment.Id,
                AttachmentTitle = attachment.AttachmentTitle,
                AttachmentPath = attachment.AttachmentPath,
            };
        }
    }
}
