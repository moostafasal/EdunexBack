using AutoMapper;
using EduNexBL.IRepository;
using EduNexBL.Repository;
using EduNexDB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private Lazy<IExam> _examRepo;
        private Lazy<IStudent> _studentRepo;
        private Lazy<IStudentExam> _studentExamRepo;
        private Lazy<ICourse> _courseRepo;
        private Lazy<ILecture> _lectureRepo;
        private Lazy<IVideo> _videoRepo;
        private Lazy<IAttachment> _attachmentRepo;
        private Lazy<IWallet> _walletRepo;
        private Lazy<ITransaction> _transactionrepo;
        public IExam ExamRepo => _examRepo.Value;
        public IStudent StudentRepo => _studentRepo.Value;
        public IStudentExam StudentExamRepo => _studentExamRepo.Value;
        public ICourse CourseRepo => _courseRepo.Value;
        public ILecture LectureRepo => _lectureRepo.Value;
        public IVideo VideoRepo => _videoRepo.Value;
        public IAttachment AttachmentRepo => _attachmentRepo.Value;
        public IWallet WalletRepo => _walletRepo.Value;
        public ITransaction TransactionRepo => _transactionrepo.Value;
        public UnitOfWork(EduNexContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _examRepo = new Lazy<IExam>(() => new ExamRepo(_context));
            _studentRepo = new Lazy<IStudent>(() => new StudentRepo(_context));
            _studentExamRepo = new Lazy<IStudentExam>(() => new StudentExamRepo(_context));
            _courseRepo = new Lazy<ICourse>(() => new CourseRepo(_context, _mapper, _configuration));
            _lectureRepo = new Lazy<ILecture>(() => new LectureRepo(_context));
            _videoRepo = new Lazy<IVideo>(() => new VideoRepo(_context));
            _attachmentRepo = new Lazy<IAttachment>(() => new AttachmentRepo(_context));
            _walletRepo = new Lazy<IWallet>(() => new WalletRepo(_context));
            _transactionrepo = new Lazy<ITransaction>(() => new TransactionRepo(_context));
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
