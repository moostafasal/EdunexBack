using EduNexBL.ENums;

namespace EduNexBL.DTOs
{
    public class ExamSubmitResultWithDetails
    {
        public ExamSubmitResult SubmitResult { get; internal set; }
        public string ExamName { get; internal set; }
        public string ExamType { get; internal set; }
        public int ExamGrade { get; internal set; }
        public int StudentGrade { get; internal set; }
        public List<QuestionWithAnswersAndState> StudentAnswersWithCorrectAnswers { get; internal set; }
    }
}