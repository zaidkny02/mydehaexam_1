using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Introduction { get; set; }
        public string? Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public int ClassID { get; set; }
        public Class? Class { get; set; }
        public ICollection<FileinLesson>? listFile { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public string? NewCommentContent { get; set; } // Property to hold the new comment content

    }
}
