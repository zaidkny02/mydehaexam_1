using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.Models
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        [Required]
        public string? FullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime Dob { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ClassDetail>? ClassDetail { get; set; }
    }
}
