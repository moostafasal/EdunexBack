namespace EduNexAPI.Controllers
{
    internal class StudentDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ParentPhoneNumber { get; set; }
        public string Religion { get; set; }
        public int? LevelId { get; set; }
        public string LevelName { get; set; }
    }
}