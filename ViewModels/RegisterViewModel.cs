using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
       
        public string Email { get; set; }
        public string? FullName { get; set; }
        [DisplayName("Date of birth")]
        public DateTime Dob { get; set; }
    }
}
