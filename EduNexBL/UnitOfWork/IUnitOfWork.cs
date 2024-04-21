using EduNexBL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IExam ExamRepo { get; }
        public IStudent StudentRepo { get; }
        public IStudentExam StudentExamRepo { get; } 
        
        void Commit();
    }
}
