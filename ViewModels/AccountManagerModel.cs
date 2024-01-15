using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class AccountManagerModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string? FullName { get; set; }
        [DisplayName("Date of birth")]
        public DateTime Dob { get; set; }
    }
}
