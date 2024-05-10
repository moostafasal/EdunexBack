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
        public ICourse CourseRepo { get; }
        public ILecture LectureRepo { get; }
        public IVideo VideoRepo {get; }
        public IAttachment AttachmentRepo { get; }
        public IWallet WalletRepo { get; }
        public ITransaction TransactionRepo { get; }

        void Commit();
    }
}
