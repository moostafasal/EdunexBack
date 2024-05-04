using CloudinaryDotNet.Actions;
using EduNexBL.Base;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class LectureRepo : Repository<Lecture>, ILecture
    {
        private readonly EduNexContext _context; 
        public LectureRepo(EduNexContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Lecture>> GetLecturesByCourseId(int courseId)
        {
            return await _context.Lectures
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Lecture?> GetFullLectureById(int id)
        {
            return await _context.Lectures.Include(l => l.Attachments).Include(l => l.Videos).Include(l => l.Exams).FirstOrDefaultAsync(l => l.Id == id); 
              
        }

        public async Task AddAttachment(AttachmentFile attachment)
        {
            var lecture = await _context.Lectures.FindAsync(attachment.LectureId);
            lecture.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
        }
        public async Task AddVideo(EduNexDB.Entites.Video video)
        {
            var lecture = await _context.Lectures.FindAsync(video.LectureId);
            lecture.Videos.Add(video);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAssignment(int id)
        {
            var lecture = _context.Lectures.FirstOrDefault(l => l.Id == id);

            return lecture != null && lecture.Exams.Any(exam => exam.Type == "Assignment");
        }

        public async Task<bool> HasPreExam(int id)
        {
            var lecture = _context.Lectures.FirstOrDefault(l => l.Id == id);

            return lecture != null && lecture.Exams.Any(exam => exam.Type == "PreExam");
        }
    }
}
