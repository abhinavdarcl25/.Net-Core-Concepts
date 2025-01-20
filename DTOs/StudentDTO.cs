using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp_API.DTOs
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please provide valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }
        //[Range(18,24)]
        //public int age { get; set; }

        //public string password { get; set; }

        //[Compare(nameof(password))] or [Compare("password")]
        //public string confirmPassword { get; set; }
        public DateTime DOB { get; set; }
    }
}
