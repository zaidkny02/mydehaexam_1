using deha_exam_quanlykhoahoc.Models;
using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }
        public Lesson? Lesson { get; set; }
        public int LessonID { get; set; }
        public User? User { get; set; }
        public string UserID { get; set; }
    }

    public class CommentRequest
    {
        public string? Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }
        public Lesson? Lesson { get; set; }
        public int LessonID { get; set; }
        public User? User { get; set; }
        public string UserID { get; set; }
    }
}
