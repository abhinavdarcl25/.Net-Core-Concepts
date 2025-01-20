using CollegeApp_API.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_API.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } //table 1
        public DbSet<Department> Departments { get; set; } //table 2

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
        }
    }
}
