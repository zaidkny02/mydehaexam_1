using deha_exam_quanlykhoahoc.Models;
using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<ClassDetail>? ClassDetail { get; set; }

        [Display(Name = "Class's Teacher")]
        public string AuthorID { get; set; }

        public User? Author { get; set; }
    }
    public class ClassRequest
    {
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<ClassDetail>? ClassDetail { get; set; }

        [Display(Name = "Class's Teacher")]
        public string AuthorID { get; set; }

        public User? Author { get; set; }
    }
}
