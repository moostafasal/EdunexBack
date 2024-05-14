using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduNexBL.DTOs;
using EduNexBL.DTOs.AuthDtos;
using EduNexDB.Entites;

namespace EduNexBL.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<TeacherDto>> GetTeachersAsync();
        Task<IEnumerable<TeacherDto>> GetApprovedTeachers();

        Task<IEnumerable<TeacherDto>> GetTeachersPendingAsync();
        Task<bool> ApproveTeacherAsync(string id);
        Task<bool> RejectTeacherAsync(string id);
        Task<TeacherDto> GetTeacherByIdAsync(string id);
        Task<StudentDto> GetStudentByIdAsync(string id);
        Task<IEnumerable<UserDto>> SearchUsersAsync(SearchQuery query);
        Task<IEnumerable<StudentDto>> GetStudentsAsync();
        Task UpdateTeachersAboutMe(string id, AboutinfoDto Aboutinfo);
        Task UpdateTeachersAccountNote(string id, AccountNoteDto Aboutinfo);
        Task UpdatePendTeacher(string id, UpdatePendingTeacherDto teacherDto);
        Task UpdateAllTeacherData(string id, UpdateAllTeacherDto TeacherDto);

        Task AddedTeachersPhoto(string id, string path);

        Task DeleteTeacherAsync(string id);
        Task DeleteStudentAsync(string id);



    }
}
