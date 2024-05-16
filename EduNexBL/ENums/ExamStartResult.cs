namespace EduNexBL.ENums
{
    public enum ExamStartResult
    {
        Success,
        NotFound,
        NotAvailable,
        InvalidDuration,
        AlreadyStarted,
        ExamEnded
    }
    public enum ExamSubmitResult
    {
        Success,
        NotFound,
        NotAvailable,
        NotStarted,
        ExamNotEnded
    }
}
