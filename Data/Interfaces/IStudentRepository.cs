namespace CollegeApp_API.Data.Interfaces
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus); 
    }
}
