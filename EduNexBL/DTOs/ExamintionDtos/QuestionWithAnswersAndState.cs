namespace EduNexBL.DTOs
{
    public class QuestionWithAnswersAndState
    {
        public int QuestionId { get; internal set; }
        public string QuestionHeader { get; internal set; }
        public string QuestionType { get; internal set; }
        public object AnswerChoices { get; internal set; }
        public List<int?> StudentAnswerIds { get; internal set; }
        public List<int> CorrectAnswerIds { get; internal set; }
        public bool IsCorrect { get; internal set; }
    }
}