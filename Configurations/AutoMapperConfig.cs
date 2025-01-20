using AutoMapper;
using CollegeApp_API.Data;
using CollegeApp_API.DTOs;

namespace CollegeApp_API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Student, StudentDTO>();
            //CreateMap<StudentDTO, Student>();

            CreateMap<Student, StudentDTO>().ReverseMap();
        }
    }
}
