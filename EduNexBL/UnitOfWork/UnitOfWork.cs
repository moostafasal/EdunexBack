using EduNexBL.IRepository;
using EduNexBL.Repository;
using EduNexDB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace EduNexBL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EduNexContext _context;
        private Lazy<IExam> _examRepo;
        private Lazy<IStudent> _studentRepo;
        private Lazy<IStudentExam> _studentExamRepo;

        public IExam ExamRepo => _examRepo.Value;
        public IStudent StudentRepo => _studentRepo.Value;
        public IStudentExam StudentExamRepo => _studentExamRepo.Value;

        public UnitOfWork(EduNexContext context)
        {
            _context = context;
            _examRepo = new Lazy<IExam>(() => new ExamRepo(_context));
            _studentRepo = new Lazy<IStudent>(() => new StudentRepo(_context));
            _studentExamRepo = new Lazy<IStudentExam>(() => new StudentExamRepo(_context));

            
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
