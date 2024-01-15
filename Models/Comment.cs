using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.Models
{
    public class Comment
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
}
