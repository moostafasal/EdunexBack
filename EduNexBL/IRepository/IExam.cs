using EduNexBL.Base;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.DTOs;
using EduNexBL.ENums;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface IExam : IRepository<Exam>
    {
        Task<IEnumerable<Exam>> GetAllExamsWithQuestions();
        Task<Exam> GetExamByIdWithQuestionsAndAnswers(int examId);
        Task<ExamStartResult> StartExam(string studentId, int examId);
        Task<ExamSubmitResultWithDetails> SubmitExam(int examId, ExamSubmissionDto examSubmissionDto);
        Task<ExamSubmitResultWithDetails> GetExamSubmitResultWithDetails(int examId, string studentId);
        Task<DateTime?> GetExamSubmissionStartTime(string studentId, int examId);
        Task<DateTime?> GetExamSubmissionEndTime(string studentId, int examId);
        Task<StudentExam> GetStudentExamInfo(string studentId, int examId);
        Task<int> GetCourseIdOfExam(int examId);


    }
}
