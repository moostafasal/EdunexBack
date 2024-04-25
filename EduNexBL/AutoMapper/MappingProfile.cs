using AutoMapper;
using EduNexBL.DTOs;
using EduNexBL.DTOs.AuthDtos;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ExamDto, Exam>()
                        .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
            CreateMap<Exam, ExamDto>();
            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionDto>();
            CreateMap<AnswerDto, Answer>();
            CreateMap<Answer, AnswerDto>();
            CreateMap<RegisterTeacherDto, Teacher>();
            CreateMap<Teacher, TeacherDto>()
             .ForMember(dest => dest.gender, opt => opt.MapFrom(src => src.gender.ToString()))
             .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));
        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
