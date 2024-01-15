using deha_exam_quanlykhoahoc.Models;
using System.ComponentModel.DataAnnotations;

namespace deha_exam_quanlykhoahoc.ViewModels
{
    public class LessonViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Introduction { get; set; }
        public string? Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public int ClassID { get; set; }
        public Class? Class { get; set; }
        public ICollection<FileinLesson>? listFile { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public string? NewCommentContent { get; set; } // Property to hold the new comment content
        public IList<IFormFile>? lsfile { get; set; }  //this for upload multi file
    }

    public class LessonRequest
    {
        public string? Title { get; set; }
        public string? Introduction { get; set; }
        public string? Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public int ClassID { get; set; }
        public Class? Class { get; set; }
        public ICollection<FileinLesson>? listFile { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public string? NewCommentContent { get; set; } // Property to hold the new comment content
        public IList<IFormFile>? lsfile { get; set; }  //this for upload multi file
    }
}
