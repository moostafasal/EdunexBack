using AutoMapper;
using EduNexBL.DTOs;
using EduNexBL.DTOs.AuthDtos;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.DTOs.PaymentDTOs;
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
            CreateMap<VideoDTO, Video>();
            CreateMap<Video, VideoDTO>();
            CreateMap<Wallet, WalletDTO>();
            CreateMap<WalletDTO, Wallet>();
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<Teacher, TeacherDto>()
             .ForMember(dest => dest.gender, opt => opt.MapFrom(src => src.gender.ToString()))
             .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));


            CreateMap<Course, CourseMainData>()
               .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => src.CourseType.ToString()))
               .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Subject.Level.LevelName))
               .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
               .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.Teacher.ProfilePhoto))
               .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.FirstName + ' ' + src.Teacher.LastName));
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
