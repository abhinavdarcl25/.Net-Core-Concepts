using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp_API.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
       public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);  
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.Name).IsRequired();
            builder.Property(n => n.Name).HasMaxLength(50);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(50);

            //default data
            builder.HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "Ram",
                    Email = "ram@example.com",
                    Address = "Patna",
                    DOB = new DateTime(2001,12,25)
                },
                new Student
                {
                    Id = 2,
                    Name = "Mohan",
                    Email = "mohan@example.com",
                    Address = "Patna",
                    DOB = new DateTime(1995,7,3)
                }
            });

            //Foreign Key Configuration
            builder.HasOne(n => n.Department)
                .WithMany(n => n.Students)
                .HasForeignKey(n => n.DepartmentId)
                .HasConstraintName("FK_Students_Department");
        }
    }
}
