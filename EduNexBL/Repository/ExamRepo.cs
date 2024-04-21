using EduNexBL.Base;
using EduNexBL.DTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.ENums;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class ExamRepo : Repository<Exam>, IExam
    {
        private readonly EduNexContext _context;

        public ExamRepo(EduNexContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam>> GetAllExamsWithQuestions()
        {
            return await _context.Exams
                .Include(exam => exam.Questions)
                    .ThenInclude(question => question.Answers)
                .ToListAsync();
        }

        public async Task<Exam> GetExamByIdWithQuestionsAndAnswers(int examId)
        {
            return await _context.Exams
                .Include(exam => exam.Questions)
                    .ThenInclude(question => question.Answers)
                .FirstOrDefaultAsync(exam => exam.Id == examId);
        }

        public async Task<ExamStartResult> StartExam(string studentId, int examId)
        {
            var exam = await GetExamByIdWithQuestionsAndAnswers(examId);
            if (!IsExamAvailable(exam))
                return ExamStartResult.NotAvailable;

            if (IsStudentStartedExam(studentId, examId))
                return ExamStartResult.AlreadyStarted;

            var studentExam = new StudentExam
            {
                StudentId = studentId,
                ExamId = examId,
                StartTime = DateTime.Now
            };

            _context.StudentExam.Add(studentExam);
            await _context.SaveChangesAsync();

            return ExamStartResult.Success;
        }

        public async Task<ExamSubmitResultWithDetails> SubmitExam(int examId,ExamSubmissionDto examSubmissionDto)
        {
            var result = new ExamSubmitResultWithDetails();
            result.SubmitResult = ValidateExamSubmission(examId,examSubmissionDto);

            if (result.SubmitResult != ExamSubmitResult.Success)
                return result;

            await SaveSubmissionToDB(examId, examSubmissionDto);
            EndExam(examSubmissionDto.StudentId, examId);

            var exam = await GetExamByIdWithQuestionsAndAnswers(examId);
            if (exam == null)
            {
                result.SubmitResult = ExamSubmitResult.NotFound;
                return result;
            }

            result.ExamName = exam.Title;
            result.ExamType = exam.Type;
            result.ExamGrade = await CalcTotalExamGrade(examId);
            result.StudentGrade = CalculateStudentGrade(examSubmissionDto);

            result.StudentAnswersWithCorrectAnswers = GetStudentAnswersWithCorrectAnswers(examSubmissionDto);

            await UpdateStudentExamScore(examSubmissionDto.StudentId, examId, result.StudentGrade);

            return result;
        }

        private async Task SaveSubmissionToDB(int examId,ExamSubmissionDto examSubmissionDto)
        {
            var submissions = MapExamSubmissionToEntities(examId,examSubmissionDto);
            await _context.StudentsAnswersSubmissions.AddRangeAsync(submissions);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentExamScore(string studentId, int examId, int studentGrade)
        {
            var studentExam = await _context.StudentExam.FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == examId);

            if (studentExam != null)
            {
                studentExam.Score = studentGrade;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Student exam record not found.");
            }
        }

        private async Task<int> CalcTotalExamGrade(int examId)
        {
            var exam = await GetExamByIdWithQuestionsAndAnswers(examId);
            return exam.Questions.Sum(question => question.Points);
        }

        private ExamSubmitResult ValidateExamSubmission(int examId, ExamSubmissionDto examSubmissionDto)
        {
            if (!IsStudentStartedExam(examSubmissionDto.StudentId, examId))
                return ExamSubmitResult.NotStarted;

            // Add more validation logic if needed

            return ExamSubmitResult.Success;
        }

        private void EndExam(string studentId, int examId)
        {
            var studentExam = _context.StudentExam.FirstOrDefault(se => se.StudentId == studentId && se.ExamId == examId);
            if (studentExam != null)
            {
                studentExam.EndTime = DateTime.Now;
                _context.SaveChanges();
            }
        }


        private bool IsStudentStartedExam(string studentId, int examId)
        {
            var studentExam = _context.StudentExam
                .FirstOrDefault(se => se.StudentId == studentId && se.ExamId == examId && se.StartTime != null);

            return studentExam != null;
        }

        private List<StudentsAnswersSubmissions> MapExamSubmissionToEntities(int examId,ExamSubmissionDto examSubmissionDto)
        {
            var submissions = new List<StudentsAnswersSubmissions>();

            foreach (var answer in examSubmissionDto.Answers)
            {
                foreach (var selectedAnswerId in answer.SelectedAnswersIds)
                {
                    var submission = new StudentsAnswersSubmissions
                    {
                        StudentId = examSubmissionDto.StudentId,
                        ExamId = examId,
                        QuestionId = answer.QuestionId,
                        AnswerId = selectedAnswerId
                    };
                    submissions.Add(submission);
                }
            }

            return submissions;
        }

        private int CalculateStudentGrade(ExamSubmissionDto examSubmissionDto)
        {
            int studentGrade = 0;

            foreach (var submittedQuestion in examSubmissionDto.Answers)
            {
                if (IsCorrectAnswer(submittedQuestion))
                {
                    var question = _context.Questions.SingleOrDefault(q => q.Id == submittedQuestion.QuestionId);
                    studentGrade += question.Points;
                }
            }

            return studentGrade;
        }

        private List<QuestionWithAnswersAndState> GetStudentAnswersWithCorrectAnswers(ExamSubmissionDto examSubmissionDto)
        {
            var studentAnswersWithCorrectAnswers = new List<QuestionWithAnswersAndState>();

            foreach (var submittedQuestion in examSubmissionDto.Answers)
            {
                var question = _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefault(q => q.Id == submittedQuestion.QuestionId);

                if (question == null)
                    continue;

                var questionWithAnswersAndState = new QuestionWithAnswersAndState
                {
                    QuestionId = question.Id,
                    QuestionHeader = question.Header,
                    QuestionType = question.Type,
                    AnswerChoices = question.Answers.Select(answer => new AnswerChoice
                    {
                        AnswerId = answer.Id,
                        AnswerHeader = answer.Header
                    }).ToList(),
                    StudentAnswerIds = submittedQuestion.SelectedAnswersIds,
                    CorrectAnswerIds = GetCorrectAnswerIdsForQuestion(question.Id),
                    IsCorrect = IsCorrectAnswer(submittedQuestion)
                };

                studentAnswersWithCorrectAnswers.Add(questionWithAnswersAndState);
            }

            return studentAnswersWithCorrectAnswers;
        }

        private List<int> GetCorrectAnswerIdsForQuestion(int questionId)
        {
            return _context.Answers
                .Where(answer => answer.QuestionId == questionId && answer.IsCorrect)
                .Select(answer => answer.Id)
                .ToList();
        }

        private bool IsCorrectAnswer(SubmittedQuestionDto submittedQuestionDto)
        {
            var submittedAnswerIds = submittedQuestionDto.SelectedAnswersIds;
            var correctAnswerIds = GetCorrectAnswerIdsForQuestion(submittedQuestionDto.QuestionId);

            if (submittedAnswerIds == null || submittedAnswerIds.Count != correctAnswerIds.Count)
                return false;

            submittedAnswerIds.Sort();
            correctAnswerIds.Sort();

            for (int i = 0; i < submittedAnswerIds.Count; i++)
            {
                if (submittedAnswerIds[i] != correctAnswerIds[i])
                    return false;
            }

            return true;
        }

        private bool IsExamAvailable(Exam exam)
        {
            var currentTime = DateTime.Now;
            return currentTime >= exam.StartDateTime && currentTime <= exam.EndDateTime;
        }
    }
}
