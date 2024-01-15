using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class ListAccountViewModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        public string? Email { get; set; }
        public IList<String>? Roles { get; set; }

        public ListAccountViewModel(string id, string? userName, string? fullName, DateTime dob, string? email, IList<string>? roles)
        {
            Id = id;
            UserName = userName;
            FullName = fullName;
            Dob = dob;
            Email = email;
            Roles = roles;
        }
        public ListAccountViewModel() { }
    }
}
