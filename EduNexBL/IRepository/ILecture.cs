using EduNexBL.Base;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface ILecture : IRepository<Lecture>
    {
        Task<IEnumerable<Lecture>> GetLecturesByCourseId(int courseId);
        Task<Lecture?> GetFullLectureById(int id);
        Task AddVideo(Video video);
        Task AddAttachment(AttachmentFile attachment);
        Task<bool> HasPreExam(int id);
        Task<bool> HasAssignment(int id);
    }
}
