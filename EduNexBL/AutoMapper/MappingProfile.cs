using AutoMapper;
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
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ExamDto, Exam>()
                        .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
            CreateMap<Exam, ExamDto>();
            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionDto >();
            CreateMap<AnswerDto, Answer>();
            CreateMap<Answer, AnswerDto>();
            CreateMap<RegisterTeacherDto, Teacher>();
        }
    }
}
