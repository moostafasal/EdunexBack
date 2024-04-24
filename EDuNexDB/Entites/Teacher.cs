namespace EduNexDB.Entites
{
    public class Teacher:ApplicationUser
    {

        public string? ProfilePhoto { get; set; }

        public string? Description { get; set; }
        public string? FacebookAccount { get; set; }
    }
}
