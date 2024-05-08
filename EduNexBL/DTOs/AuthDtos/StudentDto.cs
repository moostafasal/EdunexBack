using EduNexDB.Entites;
using System.ComponentModel.DataAnnotations;

public class StudentDto1
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ParentPhoneNumber { get; set; }
    public string Religion { get; set; }
    public string Gender { get; set; }
    public int? LevelId { get; set; }
    public string LevelName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}
