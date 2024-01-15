using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<ClassDetail>? ClassDetail { get; set; }
        
        public string AuthorID { get; set; }

        public User? Author { get; set; }
    }
}
