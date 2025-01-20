using CollegeApp_API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_API.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDBContext _collegeDBContext;
        
        public StudentRepository(CollegeDBContext collegeDBContext) : base(collegeDBContext)
        {
            _collegeDBContext = collegeDBContext;
        }

        public async Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            //logic to return list of students 
            return null;
        }
    }
}
