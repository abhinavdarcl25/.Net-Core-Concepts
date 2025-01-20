using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp_API.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.DepartmentName).IsRequired().HasMaxLength(20);
            builder.Property(n => n.Description).IsRequired(false).HasMaxLength(100);

            //default data
            builder.HasData(new List<Department>()
            {
                new Department
                {
                    Id = 1,
                    DepartmentName = "CSE",
                    Description = "Computer Science Engineering Department"
                },
                new Department
                {
                    Id = 2,
                    DepartmentName = "ME",
                    Description = "Mechanical Engineering Department"
                }
            });
        }
    }
}
